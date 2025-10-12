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
        // 1. ���� ����: 'isAlive' �Ӽ� ��� 'IsAlive()' �޼��带 ����մϴ�.
        // ps.IsAlive()�� ���� ��ƼŬ�� ��� ���̰ų� ����ִ���(Alive)�� ��ȯ�մϴ�.
        // �ڽ� ��ƼŬ �ý��۱��� �����Ͽ� Ȯ���Ϸ��� ps.IsAlive(true)�� ����մϴ�.
        yield return new WaitUntil(() => ps.IsAlive() == false);

        // �Ǵ� �����ϰ�
        // yield return new WaitUntil(() => !ps.IsAlive());

        // 2. ��ƼŬ �ý��� ������Ʈ�� �ı��մϴ�.
        Destroy(gameObject);
    }
}