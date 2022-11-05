using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{
    [SerializeField]
    private Configuration _config;
    [SerializeField]
    private GameObject _points;
    private Point[] _pos;
    private int next = 0;

    public Configuration a_config => _config;

    void Awake()
    {
        _pos = _points.GetComponentsInChildren<Point>();
    }

    public (Point, bool) NextPosition()
    {
        if (_pos.Length > 0)
        {
            transform.SetPositionAndRotation(_pos[next].transform.position, _pos[next].transform.rotation);

            next += 1;
            next %= _pos.Length;

            return (_pos[next], next == 0);
        }
        return (null, true);
    }

    public void ChangeView()
    {
        var vec = new Vector3();
        vec.x = Random.Range(_config.c_min_vertical_angle, _config.c_max_vertical_angle);
        vec.y = Random.Range(_config.c_min_horizontal_angle, _config.c_max_horizontal_angle);

        transform.Rotate(vec, Space.Self);
    }
}
