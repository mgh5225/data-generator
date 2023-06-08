using UnityEngine;


public class MeshProperties
{
    public float[] sizes { get; set; }
    public float[] cumulativeSizes { get; set; }
    public float total { get; set; }
}

public class RandomPointOnMesh : MonoBehaviour
{
    private static RandomPointOnMesh _instance;


    void Awake()
    {
        _instance = this;
    }

    private MeshProperties CalcMeshProperties(Mesh mesh)
    {
        var sizes = GetTriSizes(mesh.triangles, mesh.vertices);
        var cumulativeSizes = new float[sizes.Length];
        float total = 0;

        for (int i = 0; i < sizes.Length; i++)
        {
            total += sizes[i];
            cumulativeSizes[i] = total;
        }

        return new MeshProperties
        {
            sizes = sizes,
            cumulativeSizes = cumulativeSizes,
            total = total
        };
    }

    private (Vector3?, Vector3?) GetRandomPointOnMesh(MeshCollider meshCollider, MeshProperties meshProperties)
    {
        var mesh = meshCollider.sharedMesh;

        var sizes = meshProperties.sizes;
        var cumulativeSizes = meshProperties.cumulativeSizes;
        var total = meshProperties.total;

        float randomSample = Random.value * total;
        int triIndex = -1;

        for (int i = 0; i < sizes.Length; i++)
        {
            if (randomSample <= cumulativeSizes[i])
            {
                triIndex = i;
                break;
            }
        }

        if (triIndex == -1)
        {
            Debug.LogError("triIndex should never be -1");
            return (null, null);
        }

        Vector3 a = mesh.vertices[mesh.triangles[triIndex * 3]];
        Vector3 b = mesh.vertices[mesh.triangles[triIndex * 3 + 1]];
        Vector3 c = mesh.vertices[mesh.triangles[triIndex * 3 + 2]];

        // Generate random barycentric coordinates
        float r = Random.value;
        float s = Random.value;

        if (r + s >= 1)
        {
            r = 1 - r;
            s = 1 - s;
        }

        // Turn point back to a Vector3
        Vector3 pointOnMesh = a + r * (b - a) + s * (c - a);

        var normalVec = Vector3.Cross(b - a, c - a);
        normalVec /= normalVec.magnitude;

        pointOnMesh = meshCollider.transform.rotation * pointOnMesh;
        pointOnMesh += meshCollider.transform.position;

        return (pointOnMesh, normalVec);
    }

    private float[] GetTriSizes(int[] triangles, Vector3[] vertices)
    {
        int triCount = triangles.Length / 3;
        float[] sizes = new float[triCount];
        for (int i = 0; i < triCount; i++)
        {
            sizes[i] = .5f * Vector3.Cross(
                vertices[triangles[i * 3 + 1]] - vertices[triangles[i * 3]],
                vertices[triangles[i * 3 + 2]] - vertices[triangles[i * 3]]
            ).magnitude;
        }
        return sizes;
    }

    public static MeshProperties CalcMeshProperties_Static(Mesh mesh)
    {
        return _instance.CalcMeshProperties(mesh);
    }

    public static (Vector3?, Vector3?) GetRandomPointOnMesh_Static(MeshCollider meshCollider, MeshProperties meshProperties)
    {
        return _instance.GetRandomPointOnMesh(meshCollider, meshProperties);
    }
}