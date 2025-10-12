using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class cl2044 : MonoBehaviour, I_Attackable
{
    public enum cl2044State { Idle, Chasing, Charging, selfDestruction }
    private cl2044State currentState = cl2044State.Idle;

    [Header("설정")]
    public float attackRange = 10f;
    public float rotationSpeed = 5f;
    public float Speed = 15f;
    public float explosionRange = 3;

    [Header("데드파츠")]
    public GameObject deadPartsPrefabs;
    public GameObject explosionParticle;


    private Transform playerTarget;
    private Rigidbody rb;


    void Start()
    {
        rb = GetComponent<Rigidbody>();

        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
        {
            playerTarget = playerObj.transform;
        }
        else
        {
            Debug.LogError("씬에 플레이어 없음");
        }
    }

    void Update()
    {
        float distanceToTarget = Vector3.Distance(transform.position, playerTarget.position);


        switch (currentState)
        {
            case cl2044State.Idle:

                if (distanceToTarget < attackRange)
                {
                    currentState = cl2044State.Charging;
                }

                break;

            case cl2044State.Charging:

                Vector3 directionToTarget = playerTarget.position - transform.position;
                directionToTarget.y = 0;

                float angleDifference = Vector3.Angle(transform.forward, directionToTarget);

                if (angleDifference <= 5f)
                {
                    currentState = cl2044State.Chasing;
                }
                else
                {
                    Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);

                    transform.rotation = Quaternion.Slerp(
                        transform.rotation,
                        targetRotation,
                        rotationSpeed * Time.deltaTime
                    );
                }

                break;

            case cl2044State.Chasing:
                transform.position += transform.forward * Speed * Time.deltaTime;

                break;

        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (currentState != cl2044State.Chasing) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(WaitForAttack());
            currentState = cl2044State.selfDestruction;
        }
        else
        {
            SelfDestruction();
        }
    }
    IEnumerator WaitForAttack()
    {
        yield return new WaitForSeconds(1);
        SelfDestruction();
    }

    public void SelfDestruction()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, explosionRange);

        foreach (Collider hitCollider in hitColliders)
        {
            if (hitCollider.gameObject == gameObject)
            {
                continue;
            }

            I_Attackable target = hitCollider.GetComponent<I_Attackable>();

            if (target != null)
            {
                target.OnAttackWithDamage(0.75f);
            }
        }


        SummonDeadParts();
        SummonParticle();
        Destroy(gameObject);
    }


    public void OnAttackWithDamage(float value)
    {
        SummonDeadParts();
        Destroy(gameObject);
    }

    public void OnAttack()
    {
        SummonDeadParts();
        Destroy(gameObject);
    }

    void SummonDeadParts()
    {
        GameObject newDP = Instantiate(deadPartsPrefabs, transform.position, Quaternion.identity);
        DeadPartsLoot newDPScript = newDP.GetComponent<DeadPartsLoot>();

        if (newDPScript != null)
        {
            newDPScript.SetRotation(transform.eulerAngles);
        }
        else
        {
            Destroy(newDP);
        }
    }

    void SummonParticle()
    {
        Instantiate(explosionParticle, transform.position, Quaternion.identity);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRange);
    }
}
