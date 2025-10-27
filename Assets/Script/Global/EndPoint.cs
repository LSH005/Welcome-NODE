using System;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class EndPoint : MonoBehaviour
{

    public GameObject suppressor;
    public String nexeLv;

    private void OnTriggerEnter(Collider other)
    {
        if (suppressor != null) return;

        if (other.CompareTag("Player"))
        {
            ScreenTransition.ScreenTransitionGoto(nexeLv, "Null", Color.black, 0f, 1f, 0f, 0.5f, 0f);
        }
    }
}
