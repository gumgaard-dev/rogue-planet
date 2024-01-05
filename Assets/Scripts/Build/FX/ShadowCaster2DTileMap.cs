using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Rendering.Universal;


[RequireComponent(typeof(CompositeCollider2D))]
public class ShadowCaster2DCreator : MonoBehaviour
{
    public GameObject ShadowCasterTemplate;

    private static bool _mapUpdated = false;
    private static bool _mapUpdatedLastFrame = false;

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

    // this is a weird system, but we need to use a combination of late and fixed update in order for this to work properly
    // basically, if we call Create() outside of FixedUpdate(), it results in graphics frames where the shadow caster is non-existent
    // but if we call it in the fixed update, the tilemap collider won't be updated yet.
    // so, we need to use these flags to make sure that Create() gets called in the FixedUpdate on the frame AFTER the map is changed.

    private void LateUpdate()
    {
        if (_mapUpdated)
        {
            _mapUpdated = false;
            _mapUpdatedLastFrame = true;
        }
    }

    private void FixedUpdate()
    {
        if (_mapUpdatedLastFrame)
        {
            Create();
            _mapUpdatedLastFrame = false;
        }
    }

    public void OnMapChanged()
    {
        _mapUpdated = true;
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
            shadowCasterComponent.selfShadows = true;

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

        SwapShadowCasters();
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