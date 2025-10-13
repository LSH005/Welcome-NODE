using UnityEngine;
using System.Collections;

public class F21445a : MonoBehaviour, I_Attackable
{
    [Header("속도")]
    public float rotationSpeed = 2.0f;
    public float moveSpeed = 2.0f;
    public float dashSpeed = 5.0f;

    [Header("공격")]
    public float attackRange = 15f;
    public float damage = 0.3f;

    [Header("반짝임")]
    public Material switchMaterial;
    public float switchPeriod;
    public GameObject[] partGameObjects;

    [Header("데드파츠")]
    public GameObject deadPartsPrefabs;

    private bool isRage = false;
    private bool isAttacking = false;
    public enum F21445aState { rotate, flying, backing, attacking, dashing }
    public F21445aState currentState = F21445aState.rotate;
    private Quaternion targetRotation;
    private Coroutine currentBackingCoroutine;
    private Coroutine currentRotationCoroutine;
    private Transform playerTarget;

    void Start()
    {
        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
        {
            playerTarget = playerObj.transform;
        }
        else
        {
            Debug.LogError("씬에 플레이어 없음");
        }

        StartRotate();
    }

    void Update()
    {
        if (!isAttacking)
        {
            float distanceToTarget = Vector3.Distance(transform.position, playerTarget.position);
            if (distanceToTarget < attackRange)
            {
                isAttacking = true;
                currentState = F21445aState.attacking;

                if (currentBackingCoroutine != null)
                {
                    StopCoroutine(currentBackingCoroutine);
                    currentBackingCoroutine = null;
                }

            }
        }

        switch (currentState)
        {
            case F21445aState.rotate:
                RotationToTarget();
                break;

            case F21445aState.flying:
                transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime, Space.Self);
                break;
            case F21445aState.attacking:
                if (currentRotationCoroutine == null)
                {
                    currentRotationCoroutine = StartCoroutine(LookAtTarget());
                }
                break;
            case F21445aState.dashing:
                transform.Translate(Vector3.forward * dashSpeed * Time.deltaTime, Space.Self);
                break;

        }
    }

    void StartRotate()
    {
        SetNewRandomRotaiton();
        currentState = F21445aState.rotate;
    }

    void SetNewRandomRotaiton()
    {
        float randomYAngle = GetRandom(0f, 360f);
        Vector3 randomEuler = new Vector3(0, randomYAngle, 0);

        targetRotation = Quaternion.Euler(randomEuler);
    }

    void RotationToTarget()
    {
        transform.localRotation = Quaternion.RotateTowards(
            transform.localRotation,
            targetRotation,
            Time.deltaTime * rotationSpeed
        );

        if (transform.localRotation == targetRotation)
        {
            currentState = F21445aState.flying;
        }
    }

    private IEnumerator BackingCoroutine(float backingSpeed, float BackingTime)
    {
        float currentBackingTime = 0f;

        while (currentBackingTime < BackingTime)
        {
            transform.Translate(Vector3.back * backingSpeed * Time.deltaTime, Space.Self);
            currentBackingTime += Time.deltaTime;

            yield return null;
        }

        yield return new WaitForSeconds(0.6f);

        if (!isAttacking) StartRotate();
        else currentRotationCoroutine = StartCoroutine(LookAtTarget());
        currentBackingCoroutine = null;
    }

    public IEnumerator LookAtTarget()
    {
        float DURATION = 0.2f;
        float elapsedTime = 0f;
        Vector3 directionToTarget = playerTarget.position - transform.position;

        Quaternion startRotation = transform.rotation;
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);

        while (elapsedTime < DURATION)
        {
            float t = elapsedTime / DURATION;
            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, t);
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        transform.rotation = targetRotation;

        currentState = F21445aState.dashing;
        currentRotationCoroutine = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (currentState == F21445aState.flying)
        {
            currentState = F21445aState.backing;
            currentBackingCoroutine = StartCoroutine(BackingCoroutine(moveSpeed * 2, 0.5f));
        }
        else if (currentState == F21445aState.dashing)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                PlayerMovement playerScript = other.GetComponent<PlayerMovement>();
                if (playerScript != null)
                {
                    if (isRage) playerScript.OnAttackWithDamage(damage * 2);
                    else playerScript.OnAttackWithDamage(damage);

                    currentState = F21445aState.backing;
                    currentBackingCoroutine = StartCoroutine(BackingCoroutine(dashSpeed, 0.5f));
                }
            }
            else
            {
                Destruction();
            }
        }
    }


    float GetRandom(float min, float max)
    {
        return Random.Range(min, max);
    }

    void StartSwtichMaterial()
    {
        foreach (GameObject part in partGameObjects)
        {
            if (part == null) continue;

            MaterialSwitcher switcher = part.GetComponent<MaterialSwitcher>();
            if (switcher != null)
            {
                switcher.StartSwitch(switchMaterial, switchPeriod);
            }
        }
    }

    public void OnAttack()
    {
        if (isRage) Destruction();
        else
        {
            if (currentState == F21445aState.dashing)
            {
                currentState = F21445aState.backing;
                currentBackingCoroutine = StartCoroutine(BackingCoroutine(dashSpeed * 2, 0.25f));
            }

            isRage = true;
            StartSwtichMaterial();
        }
    }

    public void OnAttackWithDamage(float damage)
    {
        if (damage >= 0.6 || isRage) Destruction();
        else
        {
            isRage = true;
            StartSwtichMaterial();
        }
    }

    void Destruction()
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
}
