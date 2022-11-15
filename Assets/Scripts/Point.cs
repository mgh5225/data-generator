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
    private int _next_a = 0;

    public string p_name => _name;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _config.c_gizmos_range);

        var to = transform.position + transform.forward * _config.c_gizmos_length;

        Gizmos.DrawLine(transform.position, to);
    }

    public (Artefact, bool) GetArtifact()
    {
        if (_artefacts.Length > 0)
        {
            var _artefact = _artefacts[_next_a];


            _next_a += 1;
            _next_a %= _artefacts.Length;

            return (_artefact, _next_a == 0);
        }
        return (null, true);
    }

    public bool ToggleLights()
    {
        foreach (var _light in _lights)
        {
            _light.enabled = !_light.enabled;
        }

        return _lights.Length > 0;
    }


    [MenuItem("GameObject/Pre Defined Object/Point", false, 0)]
    public static void Create()
    {
        var _object = new GameObject("Point");
        var _component = _object.AddComponent<Point>();
    }

}
