using UnityEngine;

public class HitTest : MonoBehaviour, I_Attackable
{
    private int hitCount = 0;
    public void OnAttack()
    {
        hitCount++;
        Debug.Log($" HIT : {hitCount}");
    }

    public void OnAttackWithDamage(float nothing)
    {
        // 동작하지 않음
    }
}
