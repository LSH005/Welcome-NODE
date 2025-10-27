using UnityEngine;

public class Suppressor : MonoBehaviour, I_Attackable
{
    [Header("µ¥µåÆÄÃ÷")]
    public GameObject deadPartsPrefabs;

    public void OnAttack()
    {
        Destruction();
    }

    public void OnAttackWithDamage(float damage)
    {
        Destruction();
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
