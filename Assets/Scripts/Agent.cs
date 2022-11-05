using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : ConfigManager
{
    private GameObject[] _pos;
    private int next = 0;

    void Awake()
    {
        _pos = GameObject.FindGameObjectsWithTag("Point");
    }

    public bool NextPosition()
    {
        if (_pos.Length > 0)
        {
            transform.SetPositionAndRotation(_pos[next].transform.position, Quaternion.identity);

            next += 1;
            next %= _pos.Length;

            return next == 0;
        }
        return true;
    }

    public void changeView()
    {
        var vec = new Vector3();
        vec.x = Random.Range(_config.c_min_vertical_angle, _config.c_max_vertical_angle);
        vec.y = Random.Range(_config.c_min_horizontal_angle, _config.c_max_horizontal_angle);

        transform.Rotate(vec, Space.Self);
    }
}
