using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.Universal;



#if UNITY_EDITOR

[RequireComponent(typeof(CompositeCollider2D))]
public class ShadowCaster2DCreator : MonoBehaviour
{
    [SerializeField]
    private bool selfShadows = true;
    public GameObject ShadowCasterTemplate;

    public static bool updated = false;
    private bool created = false;

    private int timeSinceLastUpdate = 0;
    private int minTimeBetweenUpdates = 15;
    private GameObject shadowCasterContainerPrimary;
    private GameObject shadowCasterContainerSecondary;
    private bool isPrimaryActive = true;

    private CompositeCollider2D tilemapCollider;

    static readonly FieldInfo meshField = typeof(ShadowCaster2D).GetField("m_Mesh", BindingFlags.NonPublic | BindingFlags.Instance);
    static readonly FieldInfo shapePathField = typeof(ShadowCaster2D).GetField("m_ShapePath", BindingFlags.NonPublic | BindingFlags.Instance);
    static readonly FieldInfo shapePathHashField = typeof(ShadowCaster2D).GetField("m_ShapePathHash", BindingFlags.NonPublic | BindingFlags.Instance);
    static readonly MethodInfo generateShadowMeshMethod = typeof(ShadowCaster2D)
                                    .Assembly
                                    .GetType("UnityEngine.Rendering.Universal.ShadowUtility")
                                    .GetMethod("GenerateShadowMesh", BindingFlags.Public | BindingFlags.Static);


    private void Awake()
    {
        shadowCasterContainerPrimary = new GameObject("ShadowCasterContainerPrimary");
        shadowCasterContainerPrimary.transform.parent = transform;
        shadowCasterContainerSecondary = new GameObject("ShadowCasterContainerSecondary");
        shadowCasterContainerSecondary.transform.parent = transform;
        shadowCasterContainerSecondary.SetActive(false);
    }

    private void FixedUpdate()
    {
        if (updated && timeSinceLastUpdate > minTimeBetweenUpdates)
        {
            Create();
            SwapShadowCasters();
            timeSinceLastUpdate = 0;
            updated = false;
        }
        else
        {
            timeSinceLastUpdate++;
        }
    }
    public void Create()
    {
        GameObject activeContainer = isPrimaryActive ? shadowCasterContainerSecondary : shadowCasterContainerPrimary;
        DestroyOldShadowCasters(activeContainer);
        tilemapCollider = GetComponent<CompositeCollider2D>();

        for (int i = 0; i < tilemapCollider.pathCount; i++)
        {
            Vector2[] pathVertices = new Vector2[tilemapCollider.GetPathPointCount(i)];
            tilemapCollider.GetPath(i, pathVertices);
            GameObject shadowCaster = Instantiate(ShadowCasterTemplate);
            shadowCaster.transform.parent = activeContainer.transform; // Change here
            ShadowCaster2D shadowCasterComponent = shadowCaster.GetComponent<ShadowCaster2D>();
            shadowCasterComponent.castsShadows = true;
            shadowCasterComponent.selfShadows = this.selfShadows;

            Vector3[] testPath = new Vector3[pathVertices.Length];
            for (int j = 0; j < pathVertices.Length; j++)
            {
                testPath[j] = pathVertices[j];
            }

            shapePathField.SetValue(shadowCasterComponent, testPath);
            shapePathHashField.SetValue(shadowCasterComponent, Random.Range(int.MinValue, int.MaxValue));
            meshField.SetValue(shadowCasterComponent, new Mesh());
            generateShadowMeshMethod.Invoke(shadowCasterComponent,
            new object[] { meshField.GetValue(shadowCasterComponent), shapePathField.GetValue(shadowCasterComponent) });
        }
    }

    public void SwapShadowCasters()
    {
        isPrimaryActive = !isPrimaryActive;
        shadowCasterContainerPrimary.SetActive(isPrimaryActive);
        shadowCasterContainerSecondary.SetActive(!isPrimaryActive);
    }
    public void DestroyOldShadowCasters(GameObject container)
    {
        var tempList = container.transform.Cast<Transform>().ToList();
        foreach (var child in tempList)
        {
            DestroyImmediate(child.gameObject);
        }
    }
}

[CustomEditor(typeof(ShadowCaster2DCreator))]
public class ShadowCaster2DTileMapEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Create"))
        {
            var creator = (ShadowCaster2DCreator)target;
            creator.Create();
        }
        EditorGUILayout.EndHorizontal();
    }

}

#endif