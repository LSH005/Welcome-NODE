using UnityEngine;
using System.Collections;

public class F21445a : MonoBehaviour, I_Attackable
{
    [Header("속도")]
    public float rotationSpeed = 2.0f;
    public float moveSpeed = 2.0f;
    public float dashSpeed = 2.0f;

    [Header("공격")]
    public float attackRange = 15f;
    public float damage = 0.3f;

    private bool isRage = false;
    public enum F21445aState { rotate, flying, backing, attacking, dashing }
    public F21445aState currentState = F21445aState.rotate;
    private Quaternion targetRotation;
    private Coroutine currentBackingCoroutine;
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
        switch (currentState)
        {
            case F21445aState.rotate:
                RotationToTarget();
                break;

            case F21445aState.flying:
                transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime, Space.Self);
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

    private IEnumerator BackingCoroutine()
    {
        float currentBackingTime = 0f;
        float backingSpeed = moveSpeed * 2f;

        while (currentBackingTime < 0.25f)
        {
            transform.Translate(Vector3.back * backingSpeed * Time.deltaTime, Space.Self);
            currentBackingTime += Time.deltaTime;

            yield return null;
        }

        yield return new WaitForSeconds(0.6f);

        StartRotate();
        currentBackingCoroutine = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (currentState == F21445aState.flying)
        {
            currentState = F21445aState.backing;
            currentBackingCoroutine = StartCoroutine(BackingCoroutine());
        }
    }

    float GetRandom(float min, float max)
    {
        return Random.Range(min, max);
    }

    public void OnAttack()
    {
        if (isRage) Destruction();
        else isRage = true;
    }

    public void OnAttackWithDamage(float damage)
    {
        if (damage >= 0.6 || isRage) Destruction();
        else isRage = true;
    }

    void Destruction()
    {
        Destroy(gameObject);
    }
}
