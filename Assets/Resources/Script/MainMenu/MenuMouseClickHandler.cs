using UnityEngine;

public class MenuMouseClickHandler : MonoBehaviour
{
    public float rayMaxDistance = 40;
    public LayerMask buttonLayer;

    private void Update()
    {
        ClickHandler();
    }

    void ClickHandler()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ProcessClick(0);
        }

        if (Input.GetMouseButtonDown(1))
        {
            ProcessClick(1);
        }
    }

    private void ProcessClick(int mouseButtonIndex)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, rayMaxDistance, buttonLayer))
        {
            I_MouseClickable clickable = hit.collider.gameObject.GetComponent<I_MouseClickable>();

            if (clickable != null)
            {
                if (mouseButtonIndex == 0)
                {
                    clickable.OnLeftClicked();
                }
                else if (mouseButtonIndex == 1)
                {
                    clickable.OnRightClicked();
                }
            }
        }
    }

}
