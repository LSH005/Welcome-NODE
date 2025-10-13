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
        yield return new WaitUntil(() => ps.IsAlive() == false);
        Destroy(gameObject);
    }
}