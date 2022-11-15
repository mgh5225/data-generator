using System;
using UnityEngine;


public class Generator : MonoBehaviour
{
    private Agent _agent;
    private int _photos_num = 0;
    private Point _point = null;
    private bool _done = false;
    void Awake()
    {
        _agent = GetComponent<Agent>();
    }

    void Start()
    {
        (_point, _done) = _agent.NextPosition();
        _photos_num = 0;
    }

    void FixedUpdate()
    {
        var _width = _agent.a_config.c_width;
        var _height = _agent.a_config.c_height;

        if (_done)
            UnityEditor.EditorApplication.isPlaying = false;

        if (_photos_num == _agent.a_config.c_photos_num)
        {
            (_point, _done) = _agent.NextPosition();
            _photos_num = 0;
        }
        _agent.ChangeView();
        ScreenshotHandler.TakeScreenshot_Static(_width, _height, String.Format("{0}{1}_default.png", _point.p_name, _photos_num));

        foreach (var _light in _point.p_lights)
        {
            _light.enabled = !_light.enabled;
        }

        if (_point.p_lights.Length > 0)
            ScreenshotHandler.TakeScreenshot_Static(_width, _height, String.Format("{0}{1}_light.png", _point.p_name, _photos_num));

        foreach (var _light in _point.p_lights)
        {
            _light.enabled = !_light.enabled;
        }

        _photos_num++;
    }
}
