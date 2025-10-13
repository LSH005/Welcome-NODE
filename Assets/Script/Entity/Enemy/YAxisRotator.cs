using UnityEngine;

public class YAxisRotator : MonoBehaviour
{
    [Header("°¢¼Óµµ")]
    public float rotationSpeed = 50f;
    public bool canRotate = true;

    void Update()
    {
        if (canRotate)
        {
            transform.Rotate(0, rotationSpeed * Time.deltaTime, 0, Space.Self);
        }
    }
}
