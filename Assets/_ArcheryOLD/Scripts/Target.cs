using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    private float speed = 2f;
    public bool move;
    public TargetSpawner targetSpawner;
    public GameManager gameManager;
    private Renderer rend;
    
    public AudioClip explodeSound;
    public AudioSource audioSource;

    public GameObject explosionEffect;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        rend = GetComponent<Renderer>();
        targetSpawner = GameObject.Find("Target Spawner").GetComponent<TargetSpawner>();
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }
    
    
    void Update()
    {
        if (move)
        {
            // Cube movement
            transform.Translate(Vector3.forward * Time.deltaTime * speed);

            if (transform.position.x >= 15 || transform.position.x <= -15 || 
                transform.position.y >= 15 || transform.position.y <= 0 || 
                transform.position.z >= 15 || transform.position.z <= 5)
            {
                speed = speed * -1;
            }
        }
    }

    public void ArrowHit()
    {
        // Instantiate Explosion
        Instantiate(explosionEffect, this.transform.position, Quaternion.identity);
        
        targetSpawner.RemoveAndRespawn(this.gameObject); // remove this target from tracker and inform spawner to spawn new target
        
        audioSource.PlayOneShot(explodeSound, 1.0F); // play explosion sound
        
        gameManager.RecordHit(1);
        
        StartCoroutine(DestroyObjects());    // delayed destroy so sound fully plays
    }
    
    
    IEnumerator DestroyObjects()
    {
        rend.enabled = false; // target invisible
        yield return new WaitForSeconds(1);
        Destroy(this.gameObject);
    }
}

