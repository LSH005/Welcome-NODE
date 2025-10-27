using System.Collections;
using UnityEngine;

public class Straw : MonoBehaviour, I_Attackable, I_Enemy
{
    [Header("설정")]
    public float attackRange = 10f;

    [Header("공격")]
    public float reloadTime = 2;
    public Transform firePoint;
    public GameObject projectile;

    [Header("데드파츠")]
    public GameObject deadPartsPrefabs;

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

        StartCoroutine(Logic());
    }

    IEnumerator Logic()
    {
        while (true)
        {
            yield return null;
            float distanceToTarget = Vector3.Distance(transform.position, playerTarget.position);

            if (distanceToTarget < attackRange)
            {
                transform.LookAt(playerTarget);

                Vector3 currentRotation = transform.rotation.eulerAngles;
                currentRotation.x = 0f;
                currentRotation.z = 0f;
                transform.rotation = Quaternion.Euler(currentRotation);

                Shoot();
                yield return new WaitForSeconds(reloadTime);
            }
        }
    }

    void Shoot()
    {
        Projectile newP = Instantiate(projectile, firePoint.position, Quaternion.identity).GetComponent<Projectile>();
        newP.SetTarget(playerTarget);
    }

    public void OnAttackWithDamage(float value) { Dead(); }
    public void OnAttack() { Dead(); }

    void Dead()
    {
        StopAllCoroutines();
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

    public float GetHealth()
    {
        return 1;
    }

    public string GetName()
    {
        return "Straw";
    }
}
