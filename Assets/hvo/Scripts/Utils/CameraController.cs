using UnityEngine;

public class CameraController
{
    private float m_PanSpeed;
    private float m_MobilePanSpeed;
    private float m_ZoomSpeed;
    private float m_MinZoom;
    private float m_MaxZoom;

    public bool LockCamera { get; set; }

    private float m_LastTapTime = 0f;
    private float m_DoubleTapThreshold = 0.3f;
    private bool m_ZoomedIn = false;

    public CameraController(float panSpeed, float mobilePanSpeed, float zoomSpeed, float minZoom, float maxZoom)
    {
        m_PanSpeed = panSpeed;
        m_MobilePanSpeed = mobilePanSpeed;
        m_ZoomSpeed = zoomSpeed;
        m_MinZoom = minZoom;
        m_MaxZoom = maxZoom;
    }

    public void Update()
    {
        if (LockCamera) return;

        HandlePan();
        HandleZoom();
    }

    private void HandlePan()
    {
        if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;
            Vector2 normalizedDelta = touchDeltaPosition / new Vector2(Screen.width, Screen.height);
            Camera.main.transform.Translate(
                -normalizedDelta.x * m_MobilePanSpeed,
                -normalizedDelta.y * m_MobilePanSpeed,
                0
            );
        }
        else if (Input.touchCount == 0 && Input.GetMouseButton(0))
        {
            Vector2 mouseDeltaPosition = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

            Camera.main.transform.Translate(
                -mouseDeltaPosition.x * Time.deltaTime * m_PanSpeed,
                -mouseDeltaPosition.y * Time.deltaTime * m_PanSpeed,
                0
            );
        }
    }

    private void HandleZoom()
    {
        if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            float currentTime = Time.time;
            if (currentTime - m_LastTapTime < m_DoubleTapThreshold)
            {
                ToggleZoom();
            }
            m_LastTapTime = currentTime;
        }
    }

    private void ToggleZoom()
    {
        if (Camera.main.orthographic)
        {
            Camera.main.orthographicSize = m_ZoomedIn ? m_MaxZoom : m_MinZoom;
        }
        else
        {
            Camera.main.fieldOfView = m_ZoomedIn ? m_MaxZoom : m_MinZoom;
        }
        m_ZoomedIn = !m_ZoomedIn;
    }
}
