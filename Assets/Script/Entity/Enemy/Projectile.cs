using UnityEngine;
using static F21445a;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour, I_Attackable
{
    [Header("추적")]
    public bool canTrackPlayer = false;
    public float speed = 5.0f;
    public float lifeTime = 2f;

    [Header("대미지")]
    public float damage;


    private Transform playerTarget;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        transform.LookAt(playerTarget);
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        if (canTrackPlayer) transform.LookAt(playerTarget);
    }

    void FixedUpdate()
    {
        Vector3 direction = transform.forward;
        Vector3 targetVelocity = direction * speed;

        rb.velocity = targetVelocity;
    }

    public void SetTarget(Transform target)
    {
        playerTarget = target;
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerMovement playerScript = other.GetComponent<PlayerMovement>();
        if (playerScript != null)
        {
            playerScript.OnAttackWithDamage(damage * 2);
        }

        Destroy(gameObject);
    }

    public void OnAttackWithDamage(float value) { Destroy(gameObject); }
    public void OnAttack() { Destroy(gameObject); }
}
