using UnityEngine;
using UnityEditor;

public class Artefact : MonoBehaviour
{
    [SerializeField]
    private Configuration _config;

    [SerializeField]
    [ContextMenuItem("GameObject/Create Artefact", "Create")]
    private GameObject _prefab;
    private GameObject _object;
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, _config.c_gizmos_range);

        var to = transform.position + transform.forward * _config.c_gizmos_length;

        Gizmos.DrawLine(transform.position, to);
    }

    public void CreateArtefact()
    {
        _object = Object.Instantiate(_prefab, transform.position, transform.rotation);
    }

    public void RemoveArtefact()
    {
        Object.Destroy(_object);
    }

    [MenuItem("GameObject/Pre Defined Object/Artefact", false, 0)]
    public static void Create()
    {
        var _object = new GameObject("Artefact");
        var _component = _object.AddComponent<Artefact>();
    }
}
