using UnityEngine;

public class SpinButton : MonoBehaviour
{
    public float rotationSpeed = 45f;

    private bool canSpin;
    private void Start()
    {
        canSpin = false;
    }

    void Update()
    {
        TheSpin();
    }

    void TheSpin()
    {
        if (!canSpin) return;

        float angleToRotate = rotationSpeed * Time.deltaTime;
        transform.Rotate(0, 0, angleToRotate);
    }

    public void EndAnimation()
    {
        canSpin = true;
    }
}
