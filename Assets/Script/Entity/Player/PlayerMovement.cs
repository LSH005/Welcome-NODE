using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Animator))]
public class PlayerMovement : MonoBehaviour, I_Attackable
{
    [Header("움직임")]
    public float moveSpeed;
    public float rotationInertia;
    public float acceleration;
    public float deceleration;

    [Header("공격기 쿨다운, 범위")]
    public float attackCooldown;
    public float parryDuration;
    public float attackRange = 2.0f;
    public bool isParring;

    [Header("회전")]
    public Transform modelTransform;
    public float maxTiltAngle;
    public float rotationSpeed;

    [Header("HP")]
    public float maxHP = 1;

    [Header("이펙트")]
    public GameObject flyEffect;
    public float flyEffectCycle;


    float horizontalInput;
    float verticalInput;
    float cantDoAnythingTimeButCanMoveForReal;
    float transformationTimer;
    float afterLastInput;
    float gotoIdleTime = 5f;
    float HP;
    float lastFlyEffect;
    float currentParryDuration;

    bool canOnlyMove;
    bool hasAnyInput;
    bool isIdle;
    bool isTransforming;


    Rigidbody rb;
    Transform cameraTransform;
    Vector3 currentVelocityXZ;
    Animator anim;

    public static PlayerMovement Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        rb = GetComponent<Rigidbody>();
        cameraTransform = Camera.main.transform;
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        isIdle = true;
        anim.SetBool("bool_isIdle", true);
        HP = maxHP;
    }

    void Update()
    {
        if (canOnlyMove) CanOnlyMoveTimeHandler();

        InputHandler();
        StatesHandler();

        if (!canOnlyMove && !isTransforming)
        {
            AttackHandler();
        }

        if (isParring)
        {
            ParryDurationHandler();
        }
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
                FlyEffectHandler();
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
        if (duration <= 0)
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
        LastInputTimer();

        if (!isTransforming)
        {
            if (isIdle)
            {
                if (hasAnyInput)
                {
                    isIdle = false;
                    anim.SetBool("bool_isIdle", false);
                    anim.SetTrigger("trigger_startMove");
                    StartTransform(0.6f);
                }
            }
            else if (afterLastInput >= gotoIdleTime)
            {
                isIdle = true;
                anim.SetBool("bool_isIdle", true);
                anim.SetTrigger("trigger_endMove");
                StartTransform(1.1f);
            }
        }
        else
        {
            TransformTimeHandler();
        }
    }

    void LastInputTimer()
    {
        if (hasAnyInput) afterLastInput = 0;
        else afterLastInput += Time.deltaTime;
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

    void AttackHandler()
    {
        if (HasAttackKeyClick())
        {
            anim.SetTrigger("trigger_attack");
            SetOnlyMoveTime(attackCooldown);
            if (cantDoAnythingTimeButCanMoveForReal * 2 > gotoIdleTime - afterLastInput)
            {
                afterLastInput -= cantDoAnythingTimeButCanMoveForReal * 2;
                afterLastInput = Mathf.Max(0, afterLastInput);
            }
            Attack();
            StartParry();
        }
    }

    void Attack()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackRange);

        foreach (Collider hitCollider in hitColliders)
        {
            if (hitCollider.gameObject == gameObject)
            {
                continue;
            }

            I_Attackable target = hitCollider.GetComponent<I_Attackable>();

            if (target != null)
            {
                target.OnAttack();
            }
        }
    }

    bool HasAttackKeyClick()
    {
        bool mouse = Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2);
        bool key = Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.X);

        return mouse || key;
    }

    void StartParry()
    {
        isParring = true;
        currentParryDuration = 0;
    }

    void ParryDurationHandler()
    {
        currentParryDuration += Time.deltaTime;
        if (currentParryDuration >= parryDuration)
        {
            isParring = false;
        }
    }

    void FlyEffectHandler()
    {
        lastFlyEffect += Time.deltaTime;
        if ( lastFlyEffect > flyEffectCycle)
        {
            Quaternion flyEffectRotation = Quaternion.Euler(-90f, 0f, 0f);
            Instantiate(flyEffect, transform.position, flyEffectRotation);
            lastFlyEffect = 0;
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }


    public void OnAttack()
    {
        HP -= 0.1f;
    }

    public void OnAttackWithDamage(float damage)
    {
        HP -= damage;
        Debug.Log($"ouch {HP}");
    }
}
