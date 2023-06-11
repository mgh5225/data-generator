using System.Collections.Generic;
using System;
using UnityEngine;

public class Agent : MonoBehaviour
{
    [SerializeField]
    private Configuration _config;
    [SerializeField]
    private GameObject _points;
    [SerializeField]
    private MeshCollider[] _surfaces;
    [SerializeField]
    private GameObject[] _artefacts;
    private List<Point> _pos;
    private int _next = 0;
    private int _photos_num = 0;
    private Point _point = null;
    private Artefact _artefact = null;
    private bool _point_flag = false;
    private bool _artefact_flag = false;
    private Transform _obstacle;

    void Awake()
    {
        _pos = new List<Point>();
        if (!_config.c_generate_random_points)
            _pos.AddRange(_points.GetComponentsInChildren<Point>());
    }

    void Start()
    {
        if (_config.c_generate_random_points)
            generatePoints();

        (_point, _point_flag) = nextPosition();
        _photos_num = 0;

    }

    void FixedUpdate()
    {
        generateData();
    }


    private void generatePoints()
    {

        foreach (var surface in _surfaces)
        {
            surface.tag = "Surface";

            var meshProperties = RandomPointOnMesh.CalcMeshProperties_Static(surface.sharedMesh, _config.c_artefact_max_angle);

            for (int i = 0; i < _config.c_artefacts_per_surface; i++)
            {
                var artefact_idx = UnityEngine.Random.Range(0, _artefacts.Length);
                var prefab = _artefacts[artefact_idx];

                var (artefact_obj, artefact) = Artefact.CreateArtefact(_config, prefab, prefab.name);

                var (position, normalVec) = RandomPointOnMesh.GetRandomPointOnMesh_Static(surface, meshProperties);

                if (position == null)
                {
                    GameObject.Destroy(artefact_obj);
                    continue;
                }

                artefact_obj.transform.position = (Vector3)position;

                artefact_obj.transform.rotation = Quaternion.FromToRotation(artefact_obj.transform.up, (Vector3)normalVec);

                var random_rotate = UnityEngine.Random.rotation.eulerAngles;
                random_rotate.x = 0;
                random_rotate.z = 0;

                artefact_obj.transform.Rotate(random_rotate, Space.Self);

                var target = artefact_obj.transform.position;

                var (obj, point) = Point.CreatePoint(_config, target.ToString());

                obj.transform.position = target + _config.c_offset;
                obj.transform.LookAt(target);

                point.addArtefact(artefact);

                _pos.Add(point);
            }


        }

    }


    private (Point, bool) nextPosition()
    {
        if (_pos.Count > 0)
        {
            var _point = _pos[_next];

            transform.SetPositionAndRotation(_point.transform.position, _point.transform.rotation);

            _next += 1;
            _next %= _pos.Count;

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

    private void generateData()
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

            if (_config.c_use_lightmaps)
                _point.ToggleLightmaps();
            if (_config.c_use_lights)
                _point.ToggleLights();

            ScreenshotHandler.TakeScreenshot_Static(_width, _height, String.Format("{0}_{1}_default.png", _point.p_name, _photos_num));

            if (_config.c_use_lightmaps || _config.c_use_lights)
            {
                if (_config.c_use_lightmaps)
                    _point.ToggleLightmaps();
                if (_config.c_use_lights)
                    _point.ToggleLights();

                ScreenshotHandler.TakeScreenshot_Static(_width, _height, String.Format("{0}_{1}_light.png", _point.p_name, _photos_num));
            }


        }

        (_artefact, _artefact_flag) = _point.GetArtifact();

        if (_artefact)
        {
            var _artefact_obj = _artefact.CreateArtefact();

            removeObstacle(_artefact_obj.transform);

            ScreenshotHandler.TakeScreenshot_Static(_width, _height, String.Format("{0}_{1}_{2}.png", _point.p_name, _photos_num, _artefact.a_name));
            _artefact.RemoveArtefact();

            fixObstacle();
        }

        if (_artefact_flag)
        {
            if (_config.c_use_lightmaps)
                _point.ToggleLightmaps();
            if (_config.c_use_lights)
                _point.ToggleLights();

            _photos_num++;
        }

        if (_point_flag && _artefact_flag)
            UnityEditor.EditorApplication.isPlaying = false;
    }

    private void removeObstacle(Transform target)
    {
        fixObstacle();

        RaycastHit hit;
        var distance = Vector3.Distance(transform.position, target.position);
        if (Physics.Raycast(transform.position, target.position - transform.position, out hit, distance))
        {
            if (!_config.c_white_tags.Contains(hit.transform.tag))
            {
                _obstacle = hit.transform;
                _obstacle.gameObject.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
            }
        }


    }

    private void fixObstacle()
    {
        if (_obstacle)
        {
            _obstacle.gameObject.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
            _obstacle = null;
        }
    }
}
