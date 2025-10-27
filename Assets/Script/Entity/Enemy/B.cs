using UnityEngine;

public class B : MonoBehaviour
{
    [Header("회전 설정")]
    public float angularSpeed = 50f;

    [Header("초당 대미지")]
    public float damage = 1.5f;

    void Update()
    {
        transform.Rotate(Vector3.up * angularSpeed * Time.deltaTime);
    }

    private void OnTriggerStay(Collider other)
    {
        PlayerMovement playerScript = other.GetComponent<PlayerMovement>();
        if (playerScript != null)
        {
            playerScript.OnAttackWithDamage(Time.deltaTime / damage);
        }
    }
}
