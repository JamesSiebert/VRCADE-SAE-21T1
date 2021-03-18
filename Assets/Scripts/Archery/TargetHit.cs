using UnityEngine;

public class TargetHit : MonoBehaviour, IArrowHittable
{
    private TargetSpawner targetSpawner;
    private GameManager gameManager;
    private AudioSource audioSource;
    private Renderer rend;
    
    public GameObject explosionPrefab;
    
    
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
        // Instantiate Explosion
        Instantiate(explosionPrefab, this.transform.position, Quaternion.identity);
        
        targetSpawner.RemoveAndRespawn(this.gameObject); // remove this target from tracker and inform spawner to spawn new target

        gameManager.RecordHit(1);
        
        rend.enabled = false; // target invisible
        
        // delay 5s or when sound finishes playing
        Destroy(this.gameObject);
    }

}
