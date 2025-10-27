using System.Collections;
using UnityEngine;

public class EntitySpawn : MonoBehaviour
{
    public float WaitTime = 10;
    public GameObject[] entityObjs; 

    bool canSpawn = false;

    void Start()
    {
        canSpawn = false;
        StartCoroutine(WaitForWaitTime());
    }

    IEnumerator WaitForWaitTime()
    {
        yield return new WaitForSeconds(WaitTime);
        canSpawn = true;
    }

    private void Update()
    {
        if (!canSpawn) return;

        int randomIndex = Random.Range(0, entityObjs.Length);
        GameObject selectedPrefab = entityObjs[randomIndex];

        Instantiate(selectedPrefab, transform.position, Quaternion.identity);
    }
}
