using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(Camera))]
public class HorizontalCameraScaler : MonoBehaviour
{
    [SerializeField] private float _targetAspectX = 16.0f;
    [SerializeField] private float _targetAspectY = 9.0f;
    [SerializeField] private float _targetOrthographicSize = 5.0f;

    private Camera _camera;

    private void Awake()
    {
        _camera = GetComponent<Camera>();
    }

    private void Update()
    {
        if (_camera == null)
            _camera = GetComponent<Camera>();

        AdjustCamera();
    }

    private void AdjustCamera()
    {
        float targetRatio = _targetAspectX / _targetAspectY;

        float screenRatio = (float)Screen.width / (float)Screen.height;

        if (screenRatio >= targetRatio)
        {
            float difference = targetRatio / screenRatio;
            _camera.orthographicSize = _targetOrthographicSize * difference;
        }
        else
        {
            float difference = targetRatio / screenRatio;
            _camera.orthographicSize = _targetOrthographicSize * difference;
        }
    }
}