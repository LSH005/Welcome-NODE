using TMPro;
using UnityEngine;
using System.Collections.Generic;

public class EnemyHealth : MonoBehaviour
{
    public GameObject switchObj;
    public Transform gage;
    public TextMeshProUGUI nameText;

    [Header("감지")]
    private const float DetectionRadius = 15f;
    public LayerMask enemyLayer;

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

        switchObj.SetActive(false);
    }


    void Update()
    {
        float currentEnemyHealth;
        string currentEnemyName;

        if (FindNearestEnemyData(out currentEnemyHealth, out currentEnemyName))
        {
            switchObj.SetActive(true);

            Vector2 newGage = new Vector2(gage.localScale.x, currentEnemyHealth);
            gage.localScale = newGage;

            nameText.text = currentEnemyName;
        }
        else
        {
            switchObj.SetActive(false);
        }
    }

    public bool FindNearestEnemyData(out float health, out string enemyName)
    {
        health = -1f;
        enemyName = "None";

        Collider[] hitColliders = Physics.OverlapSphere(playerTarget.position, DetectionRadius, enemyLayer);

        I_Enemy nearestEnemy = null;
        float minDistanceSqr = float.MaxValue;

        foreach (Collider hit in hitColliders)
        {
            I_Enemy enemy = hit.GetComponent<I_Enemy>();

            if (enemy != null)
            {
                float distanceSqr = (hit.transform.position - playerTarget.position).sqrMagnitude;

                if (distanceSqr < minDistanceSqr)
                {
                    minDistanceSqr = distanceSqr;
                    nearestEnemy = enemy;
                }
            }
        }

        if (nearestEnemy != null)
        {
            health = nearestEnemy.GetHealth();
            enemyName = nearestEnemy.GetName();
            return true;
        }

        return false; // 인터페이스를 가진 적을 찾지 못함
    }
}
