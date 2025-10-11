using UnityEngine;

public class IntTest : MonoBehaviour
{

    float TheThing;
    void Start()
    {
        TheThing = Mathf.Infinity - Mathf.Infinity;
        Debug.Log($"Bruh : {TheThing}");
    }
}
