using System.Collections;
using Photon.Pun;
using UnityEngine;

public class TargetHit : MonoBehaviour, IArrowHittable
{
    private TargetSpawner targetSpawner;
    private GameManager gameManager;
    private AudioSource audioSource;
    private Renderer rend;
    
    public GameObject targetExplosionPrefab;
    
    
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        rend = GetComponent<Renderer>();
        targetSpawner = GameObject.Find("Target Spawner").GetComponent<TargetSpawner>();
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }
    
    
    // executes on hit
    public void Hit(Arrow arrow)
    {
        Debug.Log("TargetHit()");
        
        // Instantiate Explosion locally
        Instantiate(targetExplosionPrefab, this.transform.position, Quaternion.identity);

        targetSpawner.RemoveAndRespawn(this.gameObject); // remove this target from tracker and inform spawner to spawn new target

        gameManager.RecordHit(1);
        
        // Make GO Invisible
         rend.enabled = false; // target invisible
        
        // Destroy this target in X seconds, allows for all scoring to finish properly
        StartCoroutine(DestroyThisTarget());
    }


    IEnumerator DestroyThisTarget()
    {
        //Print the time of when the function is first called.
        Debug.Log("Target destroys in 2 secs");

        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(2);

        // Destroy self
        PhotonNetwork.Destroy(this.gameObject);
        //Destroy(this.gameObject);
    }
}
