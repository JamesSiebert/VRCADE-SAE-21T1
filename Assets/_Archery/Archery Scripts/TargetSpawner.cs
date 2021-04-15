using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class TargetSpawner : MonoBehaviour
{
    public int targetMaxCount;
    public List<GameObject> targetList;
    public GameObject targetPrefab;

    public Vector3 spawnAreaMax = new Vector3(15,15,15);
    

    // Start is called before the first frame update
    void Start()
    {
        if (targetList.Count < targetMaxCount)
        {
            Debug.Log("Targets existing: " + targetList.Count);
            
            for (int i = 0; i < targetMaxCount; i++) {
                //targetList.Add(SpawnNewTarget());
                SpawnNewTarget();
                Debug.Log("target added");
            }
            
            Debug.Log("spawn done - targets: " + targetList.Count);
        }
    }

    public void RemoveAndRespawn(GameObject GO)
    {
        // Remove target from tracker list
        targetList.Remove(GO);
        
        // Add newly spawned target to tracker list
        targetList.Add(SpawnNewTarget());
        
        
        //MoveTarget(GO);
    }

    public GameObject SpawnNewTarget()
    {
        
        // Network spawn new target (random location within play space, at random 90 degree rotation)
        return PhotonNetwork.Instantiate(targetPrefab.name, new Vector3(Random.Range(-spawnAreaMax.x, spawnAreaMax.x), Random.Range(1f, spawnAreaMax.y), Random.Range(3.0f, spawnAreaMax.z)), Quaternion.Euler((Random.Range(0, 3)*90), (Random.Range(0, 3)*90), (Random.Range(0, 3)*90)), 0);
        //return Instantiate(targetPrefab, new Vector3(Random.Range(-15.0f, 15.0f), Random.Range(0f, 15.0f), Random.Range(3.0f, 15.0f)), Quaternion.Euler((Random.Range(0, 3)*90), (Random.Range(0, 3)*90), (Random.Range(0, 3)*90)));
    }
    
    // public void MoveTarget(GameObject GO)
    // {
    //     GO.transform.position = new Vector3(Random.Range(-15.0f, 15.0f), Random.Range(0f, 15.0f), Random.Range(3.0f, 15.0f));
    //     GO.transform.rotation = Quaternion.Euler((Random.Range(0, 3) * 90), (Random.Range(0, 3) * 90), (Random.Range(0, 3) * 90));
    // }
}
