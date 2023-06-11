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
    private List<string> _white_tags;

    [SerializeField]
    private Vector3 _offset;

    [SerializeField]
    [Range(1, 100)]
    private int _photos_num = 1;

    [SerializeField]
    private int _width = 300;

    [SerializeField]
    private int _height = 300;

    [Header("Main Lightmaps")]
    [SerializeField]
    private Lightmaps _main_lightmaps;

    [Header("Points")]
    [SerializeField]
    private bool _generate_random_points = false;
    [SerializeField]
    private bool _use_lights = false;
    [SerializeField]
    private bool _use_lightmaps = false;
    [SerializeField]
    private int _artefacts_per_surface = 0;
    [SerializeField]
    [Range(0, 180)]
    private float _artefact_max_angle = 0;

    public float c_gizmos_range => _gizmos_range;
    public float c_gizmos_length => _gizmos_length;
    public float c_max_vertical_angle => _max_vertical_angle;
    public float c_min_vertical_angle => _min_vertical_angle;
    public float c_max_horizontal_angle => _max_horizontal_angle;
    public float c_min_horizontal_angle => _min_horizontal_angle;
    public List<string> c_white_tags => _white_tags;
    public Vector3 c_offset => _offset;
    public float c_photos_num => _photos_num;
    public int c_width => _width;
    public int c_height => _height;
    public Lightmaps c_main_lightmaps => _main_lightmaps;
    public bool c_generate_random_points => _generate_random_points;
    public bool c_use_lights => _use_lights;
    public bool c_use_lightmaps => _use_lightmaps;
    public int c_artefacts_per_surface => _artefacts_per_surface;
    public float c_artefact_max_angle => _artefact_max_angle;

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
