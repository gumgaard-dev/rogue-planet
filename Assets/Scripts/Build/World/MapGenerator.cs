using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.Events;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

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
        public UnityEvent MapChanged;

        private Queue<Vector2Int> chunksToLoad = new Queue<Vector2Int>();
        private Queue<Vector2Int> chunksToUnload = new Queue<Vector2Int>();
        private const int chunksToProcessPerFrame = 2; // Adjust this number based on performance

        // class variable used for checking neighbours during ore generation
        private static readonly Vector3Int[] DIRECTIONS = { Vector3Int.up, Vector3Int.down, Vector3Int.left, Vector3Int.right };

        // the radius, in number of chunks, in which chunks should be loaded
        // chunks that enter this radius are loaded, and those that exit are unloaded
        public int ChunkLoadRange = 2;

        // maximum ore veins, per ore, that can be spawned in a single chunk
        public int MaxVeinsPerChunk = 2;

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
            GenerateTerrainAndOresInBackground(chunk);
        }

        private void GenerateTerrainAndOresInBackground(Chunk chunk)
        {
            // Background work
            int startX = chunk.Coordinates.x * ChunkWidth;
            int startY = chunk.Coordinates.y * ChunkHeight;
            int endX = startX + ChunkWidth - 1;
            int endY = startY + ChunkHeight - 1;

            chunk.StartTile = new(startX, startY);
            chunk.EndTile = new(endX, endY);


            Vector3Int position = Vector3Int.zero;


            for (int y = chunk.StartTile.y; y <= chunk.EndTile.y; y++)
            {
                position.y = y;
                for (int x = chunk.StartTile.x; x <= chunk.EndTile.x; x++)
                {
                    position.x = x;
                    MapTileData mapTileData = GetTerrainAtDepth(y);
                    chunk.TileDataDict.Add(position, mapTileData);
                }
            }

            GenerateOresInChunk(chunk);
            
        }

        private void GenerateOresInChunk(Chunk chunk)
        {
            foreach (OreTile oreTile in OreTileTypes)
            {
                List<Vector3Int> orePositions = GenerateOreVeins(oreTile, chunk);
                foreach (Vector3Int position in orePositions)
                {
                    chunk.TileDataDict[position].OreTile = oreTile;
                }
            }
        }

        private List<Vector3Int> GenerateOreVeins(OreTile ore, Chunk chunk)
        {
            List<Vector3Int> veinPositions = new();
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
            for (int y = chunk.StartTile.y; y <= chunk.EndTile.y; y++)
            {
                position.y = y;
                if (startingPoints.Count >= MaxVeinsPerChunk)
                {
                    break;
                }
                for (int x = chunk.StartTile.x; x <= chunk.EndTile.x; x++)
                {
                    position.x = x;

                    // Check if the tile data and terrain tile are not null
                    if (chunk.TileDataDict.TryGetValue(position, out MapTileData currentTerrainData) &&
                        currentTerrainData.TerrainTile != null)
                    {
                        // Make sure starting point isn't already added and TerrainTile.TerrainName is not null
                        if (!startingPoints.Contains(position) && !string.IsNullOrEmpty(currentTerrainData.TerrainTile.TerrainName))
                        {
                            float spawnChance = ore.GetSpawnChanceInTerrain(currentTerrainData.TerrainTile.TerrainName);
                            float chance = (float)GetRandomDouble(random, 0, 1);
                            if (chance < spawnChance)
                            {
                                Vector3Int startPoint = new(x, y, 0);
                                startingPoints.Add(startPoint);
                            }
                        }
                    }
                }
            }
            return startingPoints;
        }


        // grows an ore vein by a single tile
        void GrowVein(Vector3Int lastPosition, Vector3Int direction, float propagationValue, ref List<Vector3Int> veinPositions, Chunk chunk)
        {
            Vector3Int currentPosition = lastPosition + direction;
            if (propagationValue > 0 && chunk.TileDataDict.ContainsKey(currentPosition))
            {
                veinPositions.Add(currentPosition);
                propagationValue -= 1;
                GrowVein(currentPosition, SelectRandomDirection(), propagationValue, ref veinPositions, chunk);
            }
        }


        // Checks if any chunks need to be loaded or unloaded
        public void CullChunks(Vector3 playerPosition)
        {
            Vector2Int currentChunk = GetChunkCoordinate(playerPosition);

            // load any chunks within load range
            for (int x = -ChunkLoadRange; x <= ChunkLoadRange; x++)
            {
                for (int y = -ChunkLoadRange; y <= ChunkLoadRange; y++)
                {
                    Vector2Int chunkCoord = new(currentChunk.x + x, currentChunk.y - y);
                    if (!chunksToLoad.Contains(chunkCoord) && !_loadedChunks.ContainsKey(chunkCoord))
                    {
                        chunksToLoad.Enqueue(chunkCoord);
                    }
                }
            }

            // Unload chunks that are outside the load range
            foreach (var loadedChunkCoord in _loadedChunks.Keys)
            {
                if (Mathf.Abs(loadedChunkCoord.x - currentChunk.x) > ChunkLoadRange ||
                    Mathf.Abs(loadedChunkCoord.y - currentChunk.y) > ChunkLoadRange)
                {
                    if(!chunksToUnload.Contains(loadedChunkCoord))
                    {
                        chunksToUnload.Enqueue(loadedChunkCoord);
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
                MapChanged?.Invoke();
            }

            // Process unloading chunks
            while (chunksProcessed < chunksToProcessPerFrame && chunksToUnload.Count > 0)
            {
                Vector2Int chunkCoord = chunksToUnload.Dequeue();
                UnloadChunkAt(chunkCoord);
                chunksProcessed++;
                MapChanged?.Invoke();
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
            // arrays for batch tile setting
            List<Vector3Int> terrainPositions = new();   
            List<TerrainTile> terrainTiles = new();
            List<Vector3Int> orePositions = new();
            List<OreTile> oreTiles = new();
            List<Vector3Int> backgroundPositions = new();
            List<TileBase> backgroundTiles = new();

            // populate arrays based on chunk data
            foreach (var entry in chunk.TileDataDict)
            {
                Vector3Int tilePos = entry.Key;
                MapTileData tileData = entry.Value;

                if (tileData != null)
                {
                    // add terrain info if exists
                    if (tileData.HasTerrain)
                    {
                        terrainPositions.Add(tilePos);
                        terrainTiles.Add(tileData.TerrainTile); // Assuming TerrainTile is of type TileBase

                        // Add ore tile info if present
                        if (tileData.HasOre)
                        {
                            orePositions.Add(tilePos);
                            oreTiles.Add(tileData.OreTile); // Assuming OreTile is of type TileBase
                        }
                    }

                    // add background
                    backgroundPositions.Add(tilePos);
                    backgroundTiles.Add(tileData.BackgroundTile);
                }
            }

            // Batch set tiles on tilemaps
            if (terrainPositions.Count > 0)
            {
                TerrainTilemap.SetTiles(terrainPositions.ToArray(), terrainTiles.ToArray());
            }

            if (orePositions.Count > 0)
            {
                OreTilemap.SetTiles(orePositions.ToArray(), oreTiles.ToArray());
            }

            if (backgroundPositions.Count > 0)
            {
                BackgroundTilemap.SetTiles(backgroundPositions.ToArray(), backgroundTiles.ToArray());
            }
        }
            
        public void UnloadChunkAt(Vector2Int chunkCoord)
        {
            if (_loadedChunks.TryGetValue(chunkCoord, out var chunk))
            {
                // Save or process chunk data before unloading
                UpdateChunkData(chunk);

                // Remove tiles from tilemaps within this chunk
                UnloadChunkTiles(chunk);

                _loadedChunks.Remove(chunkCoord);
            }
        }

        private void UnloadChunkTiles(Chunk chunk)
        {
            int totalTiles = ChunkWidth * ChunkHeight;
            Vector3Int[] positions = new Vector3Int[totalTiles];
            int index = 0;

            for (int y = chunk.StartTile.y; y <= chunk.EndTile.y; y++)
            {
                for (int x = chunk.StartTile.x; x <= chunk.EndTile.x; x++)
                {
                    positions[index++] = new Vector3Int(x, y, 0);
                }
            }

            TerrainTilemap.SetTiles(positions, new TileBase[totalTiles]);
            OreTilemap.SetTiles(positions, new TileBase[totalTiles]);
            BackgroundTilemap.SetTiles(positions, new TileBase[totalTiles]);
        }

        private void UpdateChunkData(Chunk chunk)
        {
            foreach (var position in chunk.TileDataDict.Keys)
            {  
                TerrainTile terrainAtPosition = TerrainTilemap.GetTile(position) as TerrainTile;
                OreTile oreAtPosition = OreTilemap.GetTile(position) as OreTile;
                chunk.UpdateTileData(position, terrainAtPosition, oreAtPosition);
            }
        }

        MapTileData GetTerrainAtDepth(int depth)
        {
            List<MapTileData> validTiles = new();
            System.Random random = new();

            foreach (var terrainType in TerrainTileTypes)
            {
                if (depth <= terrainType.minDepth && depth >= terrainType.maxDepth)
                {
                    validTiles.Add(terrainType.CreateMapTileData());
                }
            }

            // one valid tile, return it
            if (validTiles.Count == 1)
            {
                return validTiles[0];
            }

            // multiple valid tiles, choose randomly
            else if (validTiles.Count > 0)
            {
                int randomIndex = random.Next(0, validTiles.Count);
                return validTiles[randomIndex];
            } 
            else
            {
                // no valid tiles, return an empty tileData object
                return new MapTileData();
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

        internal Vector3Int GetTerrainAtWorldPosition(Vector3 worldPosition)
        {
            return TerrainTilemap.WorldToCell(worldPosition);
        }

        public void OnTileDestroyedByPlayer(Vector3Int position)
        {
            TerrainTile terrainTile = TerrainTilemap.GetTile(position) as TerrainTile;
            if (terrainTile != null) { 
                TerrainTilemap.SetTile(position, null);

                OreTile oreTile = OreTilemap.GetTile(position) as OreTile;
                if (oreTile != null)
                {
                    ItemSpawner.SpawnOre(oreTile.OreToDrop, TerrainTilemap.CellToWorld(position));
                    OreTilemap.SetTile(position, null);
                }
            }

            MapChanged?.Invoke();
        }

        public MapTileData GetTileDataAt(Vector3Int position)
        {
            // Convert the tilemap position to chunk coordinates
            Vector2Int chunkCoord = GetChunkCoordinateFromTileMapPosition(position);

            // Check if the chunk is loaded
            if (_loadedChunks.TryGetValue(chunkCoord, out Chunk chunk))
            {
                // If the chunk is loaded, try to get the tile data
                if (chunk.TileDataDict.TryGetValue(position, out MapTileData tileData))
                {
                    return tileData;
                }
            }

            // Return null or a default MapTileData if the chunk isn't loaded or the tile data isn't found
            return null;
        }

        private Vector2Int GetChunkCoordinateFromTileMapPosition(Vector3Int position)
        {
            int chunkX = Mathf.FloorToInt((float)position.x / ChunkWidth);
            int chunkY = Mathf.FloorToInt((float)position.y / ChunkHeight);
            return new Vector2Int(chunkX, chunkY);
        }
    }
}
