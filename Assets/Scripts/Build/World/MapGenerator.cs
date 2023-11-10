using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

namespace Capstone.Build.World
{
    [System.Serializable] public class OrePlacedEvent : UnityEvent<Vector3Int> { }
    [System.Serializable] public class MapGeneratedEvent : UnityEvent { }
    public class MapGenerator : MonoBehaviour
    {
        [Header("Tilemaps")]
        public Tilemap TerrainTilemap;
        public Tilemap OreTilemap;
        public Tilemap BackgroundTilemap;

        [Header("Chunk Settings")]
        public int ChunkWidth = 10;
        public int ChunkHeight = 10;
        private Dictionary<Vector2Int, Chunk> _generatedChunks = new();
        private Dictionary<Vector2Int, Chunk> _loadedChunks = new();

        [Header("Terrain Type Config")]
        public List<TerrainTile> TerrainTileTypes;
        [Header("Ore Type Config")]
        public List<OreTile> OreTileTypes;

        [Header("Events")]
        public MapGeneratedEvent ChunkGeneratedEvent;

        public Dictionary<Vector3Int, TerrainTileData> TerrainTileDataMap = new();

        private Queue<Vector2Int> chunksToLoad = new Queue<Vector2Int>();
        private Queue<Vector2Int> chunksToUnload = new Queue<Vector2Int>();
        private const int chunksToProcessPerFrame = 2; // Adjust this number based on performance

        // class variable used for checking neighbours during ore generation
        private static readonly Vector3Int[] DIRECTIONS = { Vector3Int.up, Vector3Int.down, Vector3Int.left, Vector3Int.right };

        public int LoadRange;

        void Awake()
        {
            if (TerrainTilemap == null || OreTilemap == null || BackgroundTilemap == null) { Debug.Log("MapGenerator: No reference to one or more tilemaps"); }
            if (TerrainTileTypes.Count == 0) { Debug.Log("MapGenerator: No TerrainTile references set in inspector"); }
            if (OreTileTypes.Count == 0) { Debug.Log("MapGenerator: No OreTile references set in inspector"); }
        }

        private void Update()
        {
            ProcessChunkLoading();
        }

        public void GenerateChunkAt(Vector2Int chunkCoord)
        {
            if (!_generatedChunks.TryGetValue(chunkCoord, out var chunk))
            {
                chunk = new Chunk(chunkCoord);
                _generatedChunks[chunkCoord] = chunk;
            }

            if (chunk.IsGenerated)
                return;

            // Run the terrain generation in a separate thread
            Task.Run(() => GenerateTerrainAndOresInBackground(chunk))
                .ContinueWith(task =>
                {
                    // This code runs on the main thread after the background generation is completed
                    if (task.Exception != null)
                    {
                        Debug.LogError("Error during chunk generation: " + task.Exception.InnerException);
                    }
                    else
                    {
                        // Perform any Unity-specific operations here
                        LoadChunkTiles(chunk); // Load the generated tiles
                        chunk.IsGenerated = true; // Mark the chunk as generated
                    }
                }, TaskScheduler.FromCurrentSynchronizationContext()); // Ensure that this runs on the main thread
        }

        private void GenerateTerrainAndOresInBackground(Chunk chunk)
        {
            // Background work
            int startX = chunk.CoordinatesInChunkMap.x * ChunkWidth;
            int startY = chunk.CoordinatesInChunkMap.y * ChunkHeight;
            int endX = startX + ChunkWidth;
            int endY = startY - ChunkHeight;

            chunk.StartTile = new(startX, startY);
            chunk.EndTile = new(endX, endY);

            Dictionary<Vector3Int, TerrainTileData> tempTerrainDataMap = new Dictionary<Vector3Int, TerrainTileData>();

            for (int y = chunk.StartTile.y; y > chunk.EndTile.y; y--)
            {
                for (int x = chunk.StartTile.x; x < chunk.EndTile.x; x++)
                {
                    Vector3Int position = new Vector3Int(x, y, 0);
                    TerrainTileData terrainTileData = GetTerrainTileDataAtDepth(y);
                    tempTerrainDataMap[position] = terrainTileData;
                }
            }

            lock (chunk.TerrainDataMap)
            {
                foreach (var item in tempTerrainDataMap)
                {
                    chunk.TerrainDataMap[item.Key] = item.Value;
                }
            }

            GenerateOresInChunk(chunk); // This can also be moved to background processing if needed
        }


        private void GenerateOresInChunk(Chunk chunk)
        {
            foreach (OreTile oreTile in OreTileTypes)
            {
                List<Vector3Int> orePositions = GenerateOreVeins(oreTile, chunk);
                foreach (Vector3Int position in orePositions)
                {
                    if (chunk.TerrainDataMap.TryGetValue(position, out TerrainTileData terrainData))
                    {
                        terrainData.OreTile = oreTile;
                    }
                }
            }
        }


        private List<Vector3Int> GenerateOreVeins(OreTile ore, Chunk chunk)
        {
            List<Vector3Int> veinPositions = new List<Vector3Int>();
            List<Vector3Int> startingPositions = GenerateStartingPoints(ore, chunk);

            int maxVeinSize = ore.MaxVeinSize;
            System.Random random = new();
            // generate a vein for each starting position
            foreach (var startPoint in startingPositions)
            {
                // calculate the size of the current vein
                float veinSize = random.Next(1, maxVeinSize + 1); // Define MaxVeinSize somewhere

                // grow the vein out until it reaches vein size
                GrowVein(startPoint, SelectRandomDirection(), veinSize, ref veinPositions, chunk);
            }

            return veinPositions;
        }


        private List<Vector3Int> GenerateStartingPoints(OreTile ore, Chunk chunk)
        {
            List<Vector3Int> startingPoints = new();
            Vector3Int position = Vector3Int.zero;
            System.Random random = new();
            for (int y = chunk.StartTile.y; y >= chunk.EndTile.y; y--)
            {
                position.y = y;
                for (int x = chunk.StartTile.x; x < chunk.EndTile.x; x++)
                {
                    position.x = x;
                    
                    chunk.TerrainDataMap.TryGetValue(position, out TerrainTileData currentTerrainData);
                    if (currentTerrainData == null) { continue; }
                    float spawnChance = ore.GetSpawnChanceInTerrain(currentTerrainData.TerrainTileType.TerrainName);

                    if (!startingPoints.Contains(position))
                    {
                        float chance = (float) GetRandomDouble(random, 0, 1);
                        if (chance < spawnChance)
                        {
                            Vector3Int startPoint = new(x, y, 0);
                            startingPoints.Add(startPoint);
                        }
                    }
                }
            }
            return startingPoints;
        }



        void GrowVein(Vector3Int lastPosition, Vector3Int direction, float propagationValue, ref List<Vector3Int> veinPositions, Chunk chunk)
        {
            Vector3Int currentPosition = lastPosition + direction;
            if (propagationValue > 0 && chunk.TerrainDataMap.ContainsKey(currentPosition))
            {
                veinPositions.Add(currentPosition);
                propagationValue -= 1;
                GrowVein(currentPosition, SelectRandomDirection(), propagationValue, ref veinPositions, chunk);
            }
        }


        // Loads/unloads chunks as needed if the player moves
        public void OnPlayerMoved(Vector3 playerPosition)
        {
            Vector2Int currentChunk = GetChunkCoordinate(playerPosition);

            // Define the range around the player within which chunks should be loaded

            // Unload chunks that are outside the load range
            foreach (var loadedChunkCoord in _loadedChunks.Keys)
            {
                if (Mathf.Abs(loadedChunkCoord.x - currentChunk.x) > LoadRange ||
                    Mathf.Abs(loadedChunkCoord.y - currentChunk.y) > LoadRange)
                {
                    if(!chunksToUnload.Contains(loadedChunkCoord))
                    {
                        chunksToUnload.Enqueue(loadedChunkCoord);
                    }
                }
            }


            // Generate surrounding chunks
            for (int x = -LoadRange; x <= LoadRange; x++)
            {
                for (int y = -LoadRange; y <= LoadRange; y++)
                {
                    Vector2Int chunkCoord = new(currentChunk.x + x, currentChunk.y + y);
                    if (!chunksToLoad.Contains(chunkCoord) && !_loadedChunks.ContainsKey(chunkCoord))
                    {
                        chunksToLoad.Enqueue(chunkCoord);
                    }
                }
            }
        }

        private void ProcessChunkLoading()
        {
            int chunksProcessed = 0;

            // Process loading chunks
            while (chunksProcessed < chunksToProcessPerFrame && chunksToLoad.Count > 0)
            {
                Vector2Int chunkCoord = chunksToLoad.Dequeue();
                LoadChunkAt(chunkCoord);
                chunksProcessed++;
            }

            // Process unloading chunks
            while (chunksProcessed < chunksToProcessPerFrame && chunksToUnload.Count > 0)
            {
                Vector2Int chunkCoord = chunksToUnload.Dequeue();
                UnloadChunkAt(chunkCoord);
                chunksProcessed++;
            }
        }


        private Vector2Int GetChunkCoordinate(Vector3 worldPosition)
        {
            int x = Mathf.FloorToInt(worldPosition.x / ChunkWidth);
            int y = Mathf.FloorToInt(worldPosition.y / ChunkHeight);
            return new Vector2Int(x, y);
        }

        private void LoadChunkAt(Vector2Int position)
        {
            if (_loadedChunks.ContainsKey(position)) { return; }

            this._generatedChunks.TryGetValue(position, out Chunk chunk);

            if (chunk == null) { GenerateChunkAt(position); }

            _generatedChunks.TryGetValue(position, out chunk);

            _loadedChunks.Add(position, chunk);

            LoadChunkTiles(chunk);
        }

        private void LoadChunkTiles(Chunk chunk)
        {
            foreach (var entry in chunk.TerrainDataMap)
            {
                Vector3Int tilePos = entry.Key;
                TerrainTileData tileData = entry.Value;
                if (entry.Value != null)
                {
                    PlaceTerrain(tilePos, tileData);
                    if (tileData.HasOre)
                    {
                        PlaceOre(tileData.OreTile, tilePos);
                    }
                }
            }
        }

        public void UnloadChunkAt(Vector2Int chunkCoord)
        {
            if (_loadedChunks.TryGetValue(chunkCoord, out var chunk))
            {
                _loadedChunks.Remove(chunkCoord);

                // Save or process chunk data before unloading
                UpdateChunkData(chunk);

                // Remove tiles from tilemaps within this chunk
                UnloadChunkTiles(chunk);
            }
        }


        private void UnloadChunkTiles(Chunk chunk)
        {
            for (int y = chunk.StartTile.y; y > chunk.EndTile.y; y--)
            {
                for (int x = chunk.StartTile.x; x < chunk.EndTile.x; x++)
                {
                    Vector3Int tilePos = new Vector3Int(x, y, 0);

                    RemoveTilesAt(tilePos);
                }
            }
        }

        private void RemoveTilesAt(Vector3Int tilePos)
        {
            if (TerrainTileDataMap.ContainsKey(tilePos))
            {
                TerrainTileDataMap.Remove(tilePos);
            }

            TerrainTilemap.SetTile(tilePos, null);

            if (OreTilemap.GetTile(tilePos) != null)
            {
                OreTilemap.SetTile(tilePos, null);
            }
            BackgroundTilemap.SetTile(tilePos, null);
        }

        private void UpdateChunkData(Chunk chunk)
        {
            var updates = new List<KeyValuePair<Vector3Int, TerrainTileData>>();

            foreach (var pos in chunk.TerrainDataMap.Keys)
            {
                if (TerrainTileDataMap.ContainsKey(pos))
                {
                    updates.Add(new(pos, TerrainTileDataMap[pos]));
                } else
                {
                    updates.Add(new(pos, null));
                }
            }

            foreach (var update in updates)
            {
                chunk.TerrainDataMap[update.Key] = update.Value;
            }
        }


        private void PlaceTerrain(Vector3Int position, TerrainTileData terrainTileData)
        {
            if (terrainTileData == null) return;

            TerrainTile terrainTile = terrainTileData.TerrainTileType;
            TerrainTilemap.SetTile(position, terrainTile);
            TerrainTileDataMap.Add(position, terrainTileData);
            SetBackground(terrainTile.BackgroundTile, position);
        }

        private void PlaceOre(OreTile oreTile, Vector3Int position)
        {
            if (!TerrainTileDataMap.ContainsKey(position))
            {
                Debug.LogWarning("Trying to place ore on a tile that doesn't exist.");
                return;
            }

            // Update the tile data with ore information
            var tileData = TerrainTileDataMap[position];
            tileData.OreTile = oreTile;

            OreTilemap.SetTile(position, oreTile);
        }

        private void SetBackground(Tile backgroundTile, Vector3Int position)
        {
            BackgroundTilemap.SetTile(position, backgroundTile);
        }

        TerrainTileData GetTerrainTileDataAtDepth(int depth)
        {
            List<TerrainTile> validTiles = new();
            TerrainTileData result = new();
            System.Random random = new();

            foreach (var terrainType in TerrainTileTypes)
            {
                if (depth <= terrainType.minDepth && depth >= terrainType.maxDepth)
                {
                    validTiles.Add(terrainType);
                }
            }

            // one valid tile, return it
            if (validTiles.Count == 1)
            {
                result.TerrainTileType = validTiles[0];

                return result;
            }

            // multiple valid tiles, choose randomly
            else if (validTiles.Count > 0)
            {
                int randomIndex = random.Next(0, validTiles.Count);
                result.TerrainTileType = validTiles[randomIndex];
                return result;
            } 
            else
            {
                // no valid tiles
                return null;
            }


        }

        private double GetRandomDouble(System.Random random, double minValue, double maxValue)
        {
            return random.NextDouble() * (maxValue - minValue) + minValue;
        }

        private static Vector3Int SelectRandomDirection()
        {
            System.Random random = new System.Random();
            int randomIndex = random.Next(0, DIRECTIONS.Length);
            return DIRECTIONS[randomIndex];
        }
    }
}
