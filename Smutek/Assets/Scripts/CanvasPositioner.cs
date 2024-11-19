using UnityEngine;

public class CanvasPositioner : MonoBehaviour
{
    public static CanvasPositioner Instance { get; private set; }
    private Canvas _canvas;
    private Camera _camera;
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            _canvas = FindFirstObjectByType<Canvas>();
            _camera = FindFirstObjectByType<Camera>();
        }
    }
    
    public void RepositionCanvas(Transform targetTransform)
    {
        _canvas.transform.SetParent(targetTransform);
        _canvas.transform.localScale = new Vector3(1, 1, 1);
        _canvas.transform.localPosition = new Vector3(0, 0, 0);
        _canvas.transform.localRotation = Quaternion.Euler(0, 0, 0);
        _canvas.enabled = true;
    }
    
    public void OverlayCanvasOnScreen()
    {
        _canvas.transform.SetParent(_camera.transform);
        _canvas.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        _canvas.transform.localRotation = Quaternion.Euler(0, 0, 0);
        _canvas.transform.localPosition = new Vector3(0, 0, 1);
    }
}
