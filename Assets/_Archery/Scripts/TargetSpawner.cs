using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSpawner : MonoBehaviour
{
    public int targetMaxCount;
    public List<GameObject> targetList;
    public GameObject targetPrefab;

    // Start is called before the first frame update
    void Start()
    {
        if (targetList.Count < targetMaxCount)
        {
            Debug.Log("Targets existing: " + targetList.Count);
            
            for (int i = 0; i < targetMaxCount; i++) {
                targetList.Add(SpawnNewTarget());
                Debug.Log("target added");
            }
            
            Debug.Log("spawn done - targets: " + targetList.Count);
        }
    }

    public void RemoveAndRespawn(GameObject GO)
    {
        targetList.Remove(GO);
        targetList.Add(SpawnNewTarget());
    }

    public GameObject SpawnNewTarget()
    {
        return Instantiate(targetPrefab, new Vector3(Random.Range(-15.0f, 15.0f), Random.Range(0f, 15.0f), Random.Range(3.0f, 15.0f)), Quaternion.Euler((Random.Range(0, 3)*90), (Random.Range(0, 3)*90), (Random.Range(0, 3)*90)));
    }
}
