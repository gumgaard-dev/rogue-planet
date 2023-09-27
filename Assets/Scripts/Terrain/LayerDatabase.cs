using UnityEngine;

[CreateAssetMenu(fileName = "db_layer", menuName = "Terrain/Layer Database", order = 1)]
public class LayerDatabase : ScriptableObject
{
    [SerializeField]
    private LayerData[] layerData;

    public LayerData[] GetLayerData() { return layerData; }
}
