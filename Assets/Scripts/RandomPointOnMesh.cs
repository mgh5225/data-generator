using UnityEngine;


public class MeshProperties
{
    public float[] sizes { get; set; }
    public float[] cumulativeSizes { get; set; }
    public bool[] flags { get; set; }
    public float total { get; set; }
}

public class RandomPointOnMesh : MonoBehaviour
{
    private static RandomPointOnMesh _instance;


    void Awake()
    {
        _instance = this;
    }

    private MeshProperties CalcMeshProperties(Mesh mesh, float epsilon = 0)
    {
        var sizes = GetTriSizes(mesh.triangles, mesh.vertices, epsilon);
        var cumulativeSizes = new float[sizes.Length];
        var flags = new bool[sizes.Length];
        float total = 0;

        for (int i = 0; i < sizes.Length; i++)
        {
            total += sizes[i];
            cumulativeSizes[i] = total;
            flags[i] = sizes[i] > 0;
        }

        return new MeshProperties
        {
            sizes = sizes,
            cumulativeSizes = cumulativeSizes,
            total = total,
            flags = flags
        };
    }

    private (Vector3?, Vector3?) GetRandomPointOnMesh(MeshCollider meshCollider, MeshProperties meshProperties)
    {
        var mesh = meshCollider.sharedMesh;

        var sizes = meshProperties.sizes;
        var cumulativeSizes = meshProperties.cumulativeSizes;
        var total = meshProperties.total;
        var flags = meshProperties.flags;

        float randomSample = Random.value * total;
        int triIndex = -1;

        for (int i = 0; i < sizes.Length; i++)
        {
            if (!flags[i])
                continue;

            if (randomSample <= cumulativeSizes[i])
            {
                triIndex = i;
                break;
            }
        }

        if (triIndex == -1)
            return (null, null);

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

    private float[] GetTriSizes(int[] triangles, Vector3[] vertices, float epsilon)
    {
        int triCount = triangles.Length / 3;
        float[] sizes = new float[triCount];
        for (int i = 0; i < triCount; i++)
        {

            Vector3 a = vertices[triangles[i * 3]];
            Vector3 b = vertices[triangles[i * 3 + 1]];
            Vector3 c = vertices[triangles[i * 3 + 2]];

            var normalVec = Vector3.Cross(b - a, c - a);
            var angle = Vector3.Angle(normalVec, Vector3.up);

            if (angle <= epsilon)
                sizes[i] = .5f * normalVec.magnitude;
            else
                sizes[i] = 0;

        }
        return sizes;
    }

    public static MeshProperties CalcMeshProperties_Static(Mesh mesh, float epsilon = 0)
    {
        return _instance.CalcMeshProperties(mesh, epsilon);
    }

    public static (Vector3?, Vector3?) GetRandomPointOnMesh_Static(MeshCollider meshCollider, MeshProperties meshProperties)
    {
        return _instance.GetRandomPointOnMesh(meshCollider, meshProperties);
    }
}