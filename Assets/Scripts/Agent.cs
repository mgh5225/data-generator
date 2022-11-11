using UnityEngine;

public class Agent : MonoBehaviour
{
    [SerializeField]
    private Configuration _config;
    [SerializeField]
    private GameObject _points;
    private Point[] _pos;
    private int _next = 0;

    public Configuration a_config => _config;

    void Awake()
    {
        _pos = _points.GetComponentsInChildren<Point>();
    }

    public (Point, bool) NextPosition()
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

    public void ChangeView()
    {
        var vec = new Vector3();
        vec.x = Random.Range(_config.c_min_vertical_angle, _config.c_max_vertical_angle);
        vec.y = Random.Range(_config.c_min_horizontal_angle, _config.c_max_horizontal_angle);

        transform.Rotate(vec, Space.Self);
    }
}
