using UnityEngine;

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

    private float horizontalInput;
    private float verticalInput;
    private float cantDoAnythingTimeButCanMoveForReal;
    private float currentAcceleration;

    private bool canOnlyMove;
    private bool hasAnyInput;


    private Rigidbody rb;
    private Transform cameraTransform;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        cameraTransform = Camera.main.transform;
    }

    void Update()
    {
        if (canOnlyMove) CanOnlyMoveTimeHandler();

        InputHandler();
        MoveHandler();
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

        Vector3 moveDirection = (camForward * verticalInput + camRight * horizontalInput);
        Vector3 targetVelocity = moveDirection * moveSpeed;
        targetVelocity.y = rb.velocity.y;
        rb.velocity = targetVelocity;
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
}
