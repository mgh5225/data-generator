using System;
using UnityEngine;


public class Generator : MonoBehaviour
{
    private Agent _agent;
    private Screenshot _screenshot;
    private Camera _camera;
    void Awake()
    {
        _agent = GetComponent<Agent>();
        _screenshot = GetComponent<Screenshot>();
        _camera = GetComponent<Camera>();
    }

    void Start()
    {
        while (_agent.NextPosition())
        {
            for (var i = 0; i < _agent.a_config.c_photos_num; i++)
            {
                _agent.ChangeView();
                _screenshot.SaveCameraView(_camera, String.Format("{0}.png", Time.fixedTime));
            }
        }
    }
}
