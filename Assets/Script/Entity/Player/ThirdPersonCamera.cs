using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    [Header("타겟 설정")]
    public Transform target;
    public Vector3 offset = new Vector3(0f, 0.5f, -5f);

    [Header("제한 설정")]
    public float minYAngle = -60f;
    public float maxYAngle = 60f;

    [Header("줌 설정")]
    public float zoomSpeed = 40f;
    public float maxZoomDistance = -2f;
    public float minZoomDistance = -5f;

    private float rotationX;
    private float rotationY;
    private float currentDistance;
    private float rotationSpeed = 5f;

    private void Start()
    {
        currentDistance = offset.z;
    }

    void Update()
    {
        if (target == null) return;

        CameraPositionHandler();
        CameraDistanceHandler();
    }

    void CameraPositionHandler()
    {
        rotationX += Input.GetAxis("Mouse X") * rotationSpeed;
        rotationY -= Input.GetAxis("Mouse Y") * rotationSpeed;

        rotationY = Mathf.Clamp(rotationY, minYAngle, maxYAngle);

        Quaternion targetRotation = Quaternion.Euler(rotationY, rotationX, 0);

        Vector3 finalPosition = target.position + targetRotation * offset;

        transform.position = finalPosition;
        transform.rotation = targetRotation;
    }

    void CameraDistanceHandler()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll > 0)
        {
            currentDistance += zoomSpeed * Time.deltaTime;
        }
        else if (scroll < 0)
        {
            currentDistance -= zoomSpeed * Time.deltaTime;
        }

        currentDistance = Mathf.Clamp(currentDistance, minZoomDistance, maxZoomDistance);
        offset.z = currentDistance;
    }
}