using UnityEngine;
using UnityEditor;

public class Artefact : MonoBehaviour
{
    [SerializeField]
    private Configuration _config;

    [SerializeField]
    private string _name;

    [SerializeField]
    private GameObject _prefab;
    private GameObject _object;

    public string a_name => _name;

    public static (GameObject, Artefact) CreateArtefact(Configuration config, GameObject prefab, string name)
    {
        var (obj, artefact) = Artefact.Create();
        artefact._config = config;
        artefact._name = name;
        artefact._prefab = prefab;

        return (obj, artefact);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _config.c_gizmos_range);

        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position, _config.c_gizmos_range);


        Gizmos.color = Color.blue;
        var to_f = transform.position + transform.forward * _config.c_gizmos_length;
        Gizmos.DrawLine(transform.position, to_f);

        Gizmos.color = Color.red;
        var to_r = transform.position + transform.right * _config.c_gizmos_length;
        Gizmos.DrawLine(transform.position, to_r);

        Gizmos.color = Color.green;
        var to_u = transform.position + transform.up * _config.c_gizmos_length;
        Gizmos.DrawLine(transform.position, to_u);
    }

    public GameObject CreateArtefact()
    {
        _object = Object.Instantiate(_prefab, transform.position, transform.rotation);
        _object.tag = "Artefact";
        return _object;
    }

    public void RemoveArtefact()
    {
        Object.Destroy(_object);
    }

    [MenuItem("GameObject/Pre Defined Object/Artefact", false, 0)]
    public static (GameObject, Artefact) Create()
    {
        var _object = new GameObject("Artefact");
        var _component = _object.AddComponent<Artefact>();

        return (_object, _component);
    }
}
