using System;
using UnityEngine;

public class Agent : MonoBehaviour
{
    [SerializeField]
    private Configuration _config;
    [SerializeField]
    private GameObject _points;
    private Point[] _pos;
    private int _next = 0;
    private int _photos_num = 0;
    private Point _point = null;
    private Artefact _artefact = null;
    private bool _point_flag = false;
    private bool _artefact_flag = false;

    void Awake()
    {
        _pos = _points.GetComponentsInChildren<Point>();
    }

    void Start()
    {
        (_point, _point_flag) = nextPosition();
        _photos_num = 0;
    }


    private (Point, bool) nextPosition()
    {
        if (_pos.Length > 0)
        {
            var _point = _pos[_next];

            transform.SetPositionAndRotation(_point.transform.position, _point.transform.rotation);

            _next += 1;
            _next %= _pos.Length;

            return (_point, _next == 0);
        }
        return (null, true);
    }

    private void changeCameraView()
    {
        var vec = new Vector3();
        vec.x = UnityEngine.Random.Range(_config.c_min_vertical_angle, _config.c_max_vertical_angle);
        vec.y = UnityEngine.Random.Range(_config.c_min_horizontal_angle, _config.c_max_horizontal_angle);

        transform.Rotate(vec, Space.Self);
    }

    void FixedUpdate()
    {
        if (_point == null)
        {
            UnityEditor.EditorApplication.isPlaying = false;
            return;
        }

        var _width = _config.c_width;
        var _height = _config.c_height;

        if (_photos_num == _config.c_photos_num)
        {
            (_point, _point_flag) = nextPosition();
            _photos_num = 0;
            _artefact = null;
            _artefact_flag = false;
        }


        if (!_artefact)
        {
            changeCameraView();
            ScreenshotHandler.TakeScreenshot_Static(_width, _height, String.Format("{0}{1}_default.png", _point.p_name, _photos_num));

            foreach (var _light in _point.p_lights)
            {
                _light.enabled = !_light.enabled;
            }

            if (_point.p_lights.Length > 0)
                ScreenshotHandler.TakeScreenshot_Static(_width, _height, String.Format("{0}{1}_light.png", _point.p_name, _photos_num));

        }

        (_artefact, _artefact_flag) = _point.GetArtifact();

        if (_artefact)
        {
            _artefact.CreateArtefact();
            ScreenshotHandler.TakeScreenshot_Static(_width, _height, String.Format("{0}{1}_{2}.png", _point.p_name, _photos_num, _artefact.a_name));
            _artefact.RemoveArtefact();
        }

        if (_artefact_flag)
        {
            foreach (var _light in _point.p_lights)
            {
                _light.enabled = !_light.enabled;
            }
            _photos_num++;
        }

        if (_point_flag && _artefact_flag)
            UnityEditor.EditorApplication.isPlaying = false;
    }
}
