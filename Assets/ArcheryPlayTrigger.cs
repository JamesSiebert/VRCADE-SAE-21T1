using UnityEngine;

public class ArcheryPlayTrigger : MonoBehaviour, IArrowHittable
{
    public GameManager gameManager;
    private AudioSource audioSource;
    private Renderer rend;

    public Material gameOnMaterial;
    public Material gameOffMaterial;
    
    
    
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        rend = GetComponent<Renderer>();
        rend.material = gameOffMaterial;
    }
    
    
    // executes on hit
    public void Hit(Arrow arrow)
    {
        audioSource.Play();

        if (gameManager.timerActive)
        {
            // Game is in play mode, end session
            gameManager.EndGameSession();
            rend.material = gameOffMaterial;
            Destroy(arrow);
        }
        else
        {
            // Start play mode
            gameManager.StartGameSession();
            rend.material = gameOnMaterial;
            Destroy(arrow);
            
        }
        
    }

}