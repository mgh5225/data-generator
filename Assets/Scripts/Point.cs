using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point : MonoBehaviour
{
    [SerializeField]
    private Configuration _config;

    [SerializeField]
    private string _name;

    public string p_name => _name;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _config.c_gizmos_range);

        var to = transform.position + transform.forward * _config.c_gizmos_length;

        Gizmos.DrawLine(transform.position, to);
    }

}
