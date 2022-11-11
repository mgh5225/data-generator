using UnityEngine;

public class Artefact : MonoBehaviour
{
    [SerializeField]
    private Configuration _config;

    [SerializeField]
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
}
