using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLevelTrigger : MonoBehaviour
{
    public GameObject player;              
    public GameObject endLevelSprite;       
    public GameObject endLevelAnimationPrefab;  
    public LevelCounter levelTimer;         
    public CDTimer countdownTimer;          
    public float animationDuration = 3.50f; 

    public CameraController cameraController; 

    private SpriteRenderer spriteRenderer;      
    private Collider2D triggerCollider;  

    private AudioSource audioSource;

    public AudioClip victorySound;      

    private void Start()
    {
        
        spriteRenderer = GetComponent<SpriteRenderer>();
        triggerCollider = GetComponent<Collider2D>();
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {

            audioSource.PlayOneShot(victorySound);
            DisablePlayer();

            DisableEndLevelTrigger();
            StartCoroutine(HandleEndLevelSequence());
        }
    }

    
    private IEnumerator HandleEndLevelSequence()
    {
        
        GameObject endLevelAnimation = Instantiate(endLevelAnimationPrefab, transform.position, Quaternion.identity);

        
        if (cameraController != null)
        {
            cameraController.SetFollowTarget(endLevelAnimation.transform);
        }

       
        Destroy(endLevelSprite);
        countdownTimer.DeactivateCountdown();
        
        yield return new WaitForSeconds(animationDuration);

        // End the level 
        levelTimer.OnLevelCompleted();
    }

    
    private void DisablePlayer()
    {
        
        if (player != null)
        {
            player.SetActive(false); 
        }
    }


    private void DisableEndLevelTrigger()
    {
        
        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = false; 
        }

        
        if (triggerCollider != null)
        {
            triggerCollider.enabled = false;
        }
    }
}




