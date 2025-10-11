using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    [Header("타겟 설정")]
    public Transform targetTransform;
    public Vector3 offset = new Vector3(0f, 0.5f, 0f);

    [Header("제한 설정")]
    public float minYAngle = -60f;
    public float maxYAngle = 60f;

    [Header("줌 설정")]
    public float zoomSpeed = 40f;
    public float cameraDistance;
    public float maxZoomDistance = 5f;
    public float minZoomDistance = 2f;

    [Header("충돌 감지 레이어")]
    public LayerMask obstacleMask;

    private float rotationX;
    private float rotationY;
    private float rotationSpeed = 5f;

    private Vector3 targetPos;
    private Quaternion currentRotation;

    private void Start()
    {
        Vector3 currentEuler = transform.eulerAngles;
        rotationX = currentEuler.y;
        rotationY = currentEuler.x;
    }

    void Update()
    {
        if (targetTransform == null) return;

        CameraRotationHandler();
        CameraDistanceHandler();
        CameraPositionHandler();
    }

    void CameraRotationHandler()
    {
        rotationX += Input.GetAxis("Mouse X") * rotationSpeed;
        rotationY -= Input.GetAxis("Mouse Y") * rotationSpeed;
        rotationY = Mathf.Clamp(rotationY, minYAngle, maxYAngle);
        currentRotation = Quaternion.Euler(rotationY, rotationX, 0);
        transform.rotation = currentRotation;
    }

    void CameraPositionHandler()
    {
        targetPos = targetTransform.position + offset;
        Vector3 desiredPos = targetPos - (currentRotation * Vector3.forward * cameraDistance);
        Vector3 direction = (desiredPos - targetPos).normalized;
        float cameraCollisionRadius = 0.15f;

        RaycastHit hit;
        bool isHit = Physics.SphereCast(
            targetPos,                  // 구의 시작점 (레이의 시작점)
            cameraCollisionRadius,      // 구의 반지름 (카메라가 뚫지 않도록)
            direction,                  // 레이의 방향
            out hit,                    // 충돌 정보 저장
            cameraDistance,             // 레이의 최대 길이
            obstacleMask                // 충돌을 감지할 레이어 마스크
        );

        if (isHit)
        {
            float hitDistance = hit.distance;
            Vector3 newCameraPos = targetPos - (currentRotation * Vector3.forward * hitDistance);
            transform.position = newCameraPos;
        }
        else
        {
            transform.position = desiredPos;
        }

    }

    void CameraDistanceHandler()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll > 0)
        {
            cameraDistance -= zoomSpeed * Time.deltaTime;
        }
        else if (scroll < 0)
        {
            cameraDistance += zoomSpeed * Time.deltaTime;
        }

        cameraDistance = Mathf.Clamp(cameraDistance, minZoomDistance, maxZoomDistance);
    }
}