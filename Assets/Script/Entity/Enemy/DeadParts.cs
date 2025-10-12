using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class DeadParts : MonoBehaviour
{
    public float thrust = 10f;
    public float rotationThrust = 5f;
    public float shrinkDuration = 3.0f;

    private Rigidbody rb;
    private Vector3 initialScale;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        LaunchRandomly();
        ApplyRandomTorque();

        initialScale = transform.localScale;
        StartCoroutine(ShrinkAndDestroySequence());
    }

    void LaunchRandomly()
    {
        float randomX = GetRandom(-1f, 1f);
        float randomY = GetRandom(0.5f, 1f);
        float randomZ = GetRandom(-1f, 1f);

        Vector3 randomDirection = new Vector3(randomX, randomY, randomZ);
        rb.AddForce(randomDirection.normalized * thrust, ForceMode.Impulse);
    }

    void ApplyRandomTorque()
    {
        Vector3 randomTorqueAxis = Random.onUnitSphere;
        rb.AddTorque(randomTorqueAxis * rotationThrust, ForceMode.Impulse);
    }

    float GetRandom(float min, float max)
    {
        return Random.Range(min, max);
    }

    private IEnumerator ShrinkAndDestroySequence()
    {
        float elapsedTime = 0f;

        while (elapsedTime < shrinkDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / shrinkDuration);
            transform.localScale = Vector3.Lerp(initialScale, Vector3.zero, t);

            yield return null;
        }

        Destroy(gameObject);
    }
}
