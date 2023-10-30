using Build.Component;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TerrainTile : TileBase
{
    private HealthData _health;
    // Start is called before the first frame update
    public override void RefreshTile(Vector3Int position, ITilemap tilemap)
    {
        base.RefreshTile(position, tilemap);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
