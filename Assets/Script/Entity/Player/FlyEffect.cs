using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class FlyEffect : MonoBehaviour
{
    [Header("레이캐스팅 설정")]
    public LayerMask hitLayerMask;
    public float offsetHeight = 0.01f;

    [Header("크기 및 지속 시간 설정")]
    public float maxScale = 0.3f;
    public float duration = 0.25f;

    private float timer;
    private Vector3 initialScale;
    private SpriteRenderer spriteRenderer;
    private Color currentColor;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentColor = spriteRenderer.color;

        initialScale = new Vector3(maxScale, maxScale, maxScale);
        transform.localScale = Vector3.zero;

        timer = duration;

        InitializePosition();
    }

    private void InitializePosition()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, Mathf.Infinity, hitLayerMask))
        {
            Vector3 targetPosition = hit.point + Vector3.up * offsetHeight;
            transform.position = targetPosition;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            float t = 1.0f - (timer / duration);

            transform.localScale = Vector3.Lerp(Vector3.zero, initialScale, t);

            float alpha = 1.0f - t;
            spriteRenderer.color = new Color(currentColor.r, currentColor.g, currentColor.b, alpha);

            if (timer <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
