using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Configuration", menuName = "Create New Configuration")]
public class Configuration : ScriptableObject
{
    [Header("Gizmos")]
    [SerializeField]
    private float _gizmos_range = 1f;
    [SerializeField]
    private float _gizmos_length = 1f;


    [Header("Camera")]
    [SerializeField]
    [Range(-90, 90)]
    private float _min_vertical_angle = 0f;
    [SerializeField]
    [Range(-90, 90)]
    private float _max_vertical_angle = 0f;

    [SerializeField]
    [Range(-90, 90)]
    private float _min_horizontal_angle = 0f;
    [SerializeField]
    [Range(-90, 90)]
    private float _max_horizontal_angle = 0f;

    [SerializeField]
    [Range(1, 100)]
    private int _photos_num = 1;



    public float c_gizmos_range => _gizmos_range;
    public float c_gizmos_length => _gizmos_length;
    public float c_max_vertical_angle => _max_vertical_angle;
    public float c_min_vertical_angle => _min_vertical_angle;
    public float c_max_horizontal_angle => _max_horizontal_angle;
    public float c_min_horizontal_angle => _min_horizontal_angle;
    public float c_photos_num => _photos_num;

    private void OnValidate()
    {
        if (_min_vertical_angle > _max_vertical_angle)
        {
            _min_vertical_angle = _max_vertical_angle;
        }

        if (_max_vertical_angle < _min_vertical_angle)
        {
            _max_vertical_angle = _min_vertical_angle;
        }

        if (_min_horizontal_angle > _max_horizontal_angle)
        {
            _min_horizontal_angle = _max_horizontal_angle;
        }

        if (_max_horizontal_angle < _min_horizontal_angle)
        {
            _max_horizontal_angle = _min_horizontal_angle;
        }
    }
}
