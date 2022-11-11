using UnityEditor;
using UnityEngine;

public class Point : MonoBehaviour
{
    [SerializeField]
    private Configuration _config;

    [SerializeField]
    private string _name;

    [SerializeField]
    private Light[] _lights;

    [SerializeField]
    private Artefact[] _artefacts;

    public string p_name => _name;
    public Light[] p_lights => _lights;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _config.c_gizmos_range);

        var to = transform.position + transform.forward * _config.c_gizmos_length;

        Gizmos.DrawLine(transform.position, to);
    }

    [MenuItem("GameObject/Pre Defined Object/Point", false, 0)]
    public static void Create()
    {
        var _object = new GameObject("Point");
        var _component = _object.AddComponent<Point>();
    }

}
