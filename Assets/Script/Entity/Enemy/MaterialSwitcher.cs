using UnityEngine;

public class MaterialSwitcher : MonoBehaviour
{
    public Material switchMaterial;
    public float switchPeriod;


    private bool isMaterialOriginal = true;
    private bool canSwitch = false;

    private float lastSwitch;

    private Material originalMaterial;

    private MeshRenderer meshRenderer;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();

        if (meshRenderer == null)
        {
            Debug.LogError("오브젝트에 MeshRenderer 컴포넌트가 없음");
            this.enabled = false;
        }
    }

    private void Start()
    {
        originalMaterial = meshRenderer.material;
        lastSwitch = Time.time;
    }

    void Update()
    {
        if (canSwitch)
        {
            SwitchMaterialHandler();
        }
    }

    public void StartSwitch(Material newSwitchMaterial, float newSwitchPeriod)
    {
        canSwitch = true;
        switchMaterial = newSwitchMaterial;
        switchPeriod = newSwitchPeriod;
        switchPeriod = Mathf.Max(switchPeriod, 0);
    }

    public void StopSwitch()
    {
        canSwitch = false;
    }

    void SwitchMaterialHandler()
    {
        if (Time.time >= lastSwitch + switchPeriod)
        {
            SwitchMaterial();
            lastSwitch = Time.time;
        }
    }

    void SwitchMaterial()
    {
        isMaterialOriginal = !isMaterialOriginal;

        if (isMaterialOriginal) meshRenderer.material = originalMaterial;
        else meshRenderer.material = switchMaterial;
    }
}
