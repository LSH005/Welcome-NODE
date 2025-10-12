using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ParticleSystem))]
public class SelfDestroyAfterParticle : MonoBehaviour
{
    private ParticleSystem ps;

    void Start()
    {
        ps = GetComponent<ParticleSystem>();
        StartCoroutine(WaitUntilParticleStop());
    }

    private IEnumerator WaitUntilParticleStop()
    {
        // 1. 오류 수정: 'isAlive' 속성 대신 'IsAlive()' 메서드를 사용합니다.
        // ps.IsAlive()는 현재 파티클이 재생 중이거나 살아있는지(Alive)를 반환합니다.
        // 자식 파티클 시스템까지 포함하여 확인하려면 ps.IsAlive(true)를 사용합니다.
        yield return new WaitUntil(() => ps.IsAlive() == false);

        // 또는 간단하게
        // yield return new WaitUntil(() => !ps.IsAlive());

        // 2. 파티클 시스템 오브젝트를 파괴합니다.
        Destroy(gameObject);
    }
}