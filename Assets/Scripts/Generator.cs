using System;
using UnityEngine;


public class Generator : MonoBehaviour
{
    private Agent _agent;
    private Screenshot _screenshot;
    private Camera _camera;
    private int _photos_num = 0;
    private Point _point = null;
    private bool _done = false;
    void Awake()
    {
        _agent = GetComponent<Agent>();
        _screenshot = GetComponent<Screenshot>();
        _camera = GetComponent<Camera>();
    }

    void Start()
    {
        (_point, _done) = _agent.NextPosition();
        _photos_num = 0;
    }

    void FixedUpdate()
    {
        if (_done)
            UnityEditor.EditorApplication.isPlaying = false;

        if (_photos_num == _agent.a_config.c_photos_num)
        {
            (_point, _done) = _agent.NextPosition();
            _photos_num = 0;
        }
        _agent.ChangeView();
        _screenshot.SaveCameraView(_camera, String.Format("{0}{1}.png", _point.p_name, _photos_num));
        _photos_num++;
    }
}
