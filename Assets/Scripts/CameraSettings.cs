
using System.Collections;
using UnityEngine;

public class CameraSettings : MonoBehaviour
{
    private Buildings buildings;
    [SerializeField] private Camera gameCamera;
    Vector2Int Vector2Int;
    private Vector3 touchStart;
    public float groundZ = 0;
    public float cameraZoom = -20;
    public float zoomOutMin = 1;
    public float zoomOutMax = 8;

    [SerializeField] private bool scrolling=false;
    public bool Scrolling
    {
        get { return scrolling; }
        set { scrolling = value; }
    }
    void Start()
    {
        scrolling = false;
        Vector2Int = FindObjectOfType<GameGrid>().GetPosition();
        gameCamera.transform.position = new Vector3(Vector2Int.x / 2 - 0.5f, Vector2Int.y / 2 - 10.5f, cameraZoom);
        buildings = GameObject.Find("Building").GetComponent<Buildings>();
    }


    void Update()
    {
        if (Input.touchCount == 2)
        {
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            float prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

            float difference = currentMagnitude - prevMagnitude;

            Zoom(difference * 0.01f);

        }
        StartCoroutine(WaitForSec());
        if(Input.touchCount == 1)
        {
            if (Input.GetMouseButtonDown(0))
            {
                touchStart = GetWorldPosition(groundZ);
            }
            if (Input.GetMouseButton(0))
            {
                Vector3 direction = touchStart - GetWorldPosition(groundZ);
                Camera.main.transform.position += direction;
            }
        }
        
        Zoom(Input.GetAxis("Mouse ScrollWheel"));
    }
    IEnumerator WaitForSec()
    {
        yield return new WaitForSeconds(0.1f);
    }
    void Zoom(float increment)
    {
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize - increment, zoomOutMin, zoomOutMax);
    }
    private Vector3 GetWorldPosition(float z)
    {
        Ray mousePos = gameCamera.ScreenPointToRay(Input.mousePosition);
        Plane ground = new Plane(Vector3.forward, new Vector3(0, 0, z));
        float distence;
        ground.Raycast(mousePos, out distence);
        return mousePos.GetPoint(distence);
    }
}
