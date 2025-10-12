using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Animator))]
public class PlayerMovement : MonoBehaviour
{
    [Header("움직임")]
    public float moveSpeed;
    public float rotationInertia;
    public float acceleration;
    public float deceleration;

    [Header("공격기 쿨다운")]
    public float attackCooldown;
    public float parryCooldown;
    
    [Header("회전")]
    public Transform modelTransform;
    public float maxTiltAngle;
    public float rotationSpeed;


    private float horizontalInput;
    private float verticalInput;
    private float cantDoAnythingTimeButCanMoveForReal;
    private float transformationTimer;
    private float afterLastInput;

    private bool canOnlyMove;
    private bool hasAnyInput;
    private bool isIdle;
    private bool isTransforming;


    private Rigidbody rb;
    private Transform cameraTransform;
    private Vector3 currentVelocityXZ;
    private Animator anim;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        cameraTransform = Camera.main.transform;
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        isIdle = true;
    }

    void Update()
    {
        if (canOnlyMove) CanOnlyMoveTimeHandler();

        InputHandler();
        StatesHandler();
    }

    private void FixedUpdate()
    {
        if (!isTransforming)
        {
            if (!isIdle && !isTransforming)
            {
                MoveHandler();
                RotationHandler();
                TiltHandler();
            }
        }
    }

    void InputHandler()
    {
        float currentAccelDelta = acceleration * Time.deltaTime;

        if (HasAnyInputAtHorizontal())
        {
            float target = Input.GetKey(KeyCode.D) ? 1f : -1f;
            horizontalInput = Mathf.MoveTowards(horizontalInput, target, currentAccelDelta);
        }
        else
        {
            horizontalInput = Mathf.MoveTowards(horizontalInput, 0f, deceleration * Time.deltaTime);
        }

        if (HasAnyInputAtVertical())
        {
            float target = Input.GetKey(KeyCode.W) ? 1f : -1f;
            verticalInput = Mathf.MoveTowards(verticalInput, target, currentAccelDelta);
        }
        else
        {
            verticalInput = Mathf.MoveTowards(verticalInput, 0f, deceleration * Time.deltaTime);
        }

        hasAnyInput = HasAnyInputAtHorizontal() || HasAnyInputAtVertical();
    }

    bool HasAnyInputAtHorizontal()
    {
        return Input.GetKey(KeyCode.A) ^ Input.GetKey(KeyCode.D);
    }

    bool HasAnyInputAtVertical()
    {
        return Input.GetKey(KeyCode.W) != Input.GetKey(KeyCode.S);
    }

    private void MoveHandler()
    {
        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;
        camForward.y = camRight.y = 0;
        camForward.Normalize();
        camRight.Normalize();

        Vector3 inputDirection = (camForward * verticalInput + camRight * horizontalInput).normalized;
        Vector3 targetVelocity = inputDirection * moveSpeed;
        if (!hasAnyInput)
        {
            currentVelocityXZ = Vector3.Lerp(currentVelocityXZ, Vector3.zero, deceleration * Time.deltaTime);
        }

        if (hasAnyInput)
        {
            Vector3 accelerationVector = inputDirection * acceleration * Time.deltaTime;
            currentVelocityXZ += accelerationVector;

            if (currentVelocityXZ.magnitude > moveSpeed)
            {
                currentVelocityXZ = currentVelocityXZ.normalized * moveSpeed;
            }
        }

        Vector3 finalVelocity = new Vector3(currentVelocityXZ.x, rb.velocity.y, currentVelocityXZ.z);
        rb.velocity = finalVelocity;
    }

    private void RotationHandler()
    {
        if (isMoving())
        {
            Vector3 movementDirection = currentVelocityXZ.normalized;

            Quaternion targetRotation = Quaternion.LookRotation(movementDirection);

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
        }
    }

    private void TiltHandler()
    {
        float velocityRatio = currentVelocityXZ.magnitude / moveSpeed;

        float targetTiltX = velocityRatio * maxTiltAngle;

        Quaternion targetRotation = Quaternion.Euler(
            targetTiltX,
            modelTransform.localEulerAngles.y,
            modelTransform.localEulerAngles.z
        );

        modelTransform.localRotation = targetRotation;
    }

    bool isMoving()
    {
        return currentVelocityXZ.sqrMagnitude > 0.01f;
    }

    private void SetOnlyMoveTime(float duration)
    {
        if (duration > 0)
        {
            cantDoAnythingTimeButCanMoveForReal = 0;
            canOnlyMove = false;
            return;
        }

        cantDoAnythingTimeButCanMoveForReal = duration;
        canOnlyMove = true;
    }

    void CanOnlyMoveTimeHandler()
    {
        cantDoAnythingTimeButCanMoveForReal -= Time.deltaTime;
        if (cantDoAnythingTimeButCanMoveForReal <= 0)
        {
            cantDoAnythingTimeButCanMoveForReal = 0;
            canOnlyMove = false;
        }
    }

    void StatesHandler()
    {
        if (!isTransforming)
        {
            if (isIdle)
            {
                if (hasAnyInput)
                {
                    isIdle = false;
                    anim.SetTrigger("trigger_startMove");
                    StartTransform(0.6f);
                }
            }
            else
            {
                if (hasAnyInput) afterLastInput = 0;
                else afterLastInput += Time.deltaTime;

                if (afterLastInput >= 5f)
                {
                    isIdle = true;
                    anim.SetTrigger("trigger_endMove");
                    StartTransform(1.1f);
                }
            }
        }
        else
        {
            TransformTimeHandler();
        }
    }

    void StartTransform(float duration)
    {
        isTransforming = true;
        transformationTimer = duration;
    }

    void TransformTimeHandler()
    {
        transformationTimer -= Time.deltaTime;
        if (transformationTimer <= 0)
        {
            isTransforming = false;
            transformationTimer = 0;
        }
    }
}
