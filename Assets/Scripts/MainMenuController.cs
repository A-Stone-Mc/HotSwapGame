using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenuController : MonoBehaviour
{
    private Animator animator;
    private bool hasPlayedOnce = false;
    private bool exitTriggered = false;
    [SerializeField] private AudioSource buttonPressSound;

    [SerializeField]
    private GameObject nextScreenPanel; 

    [SerializeField]
    private GameObject mainMenuPanel; 
    void Start()
    {
        animator = mainMenuPanel.GetComponent<Animator>();
    }

    void Update()
    {
       
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("PressSpace") &&
            animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f && !hasPlayedOnce)
        {
            hasPlayedOnce = true;
            animator.SetTrigger("HasPlayedOnce");
        }

       
        if (Input.GetKeyDown(KeyCode.Space) && !exitTriggered)
        {
            if (buttonPressSound != null)
            {
                buttonPressSound.Play(); 
            }

            animator.SetTrigger("Exit");
            exitTriggered = true;
            Invoke("LoadNextScreen", 2.2f); 
        }
    }

    void LoadNextScreen()
    {
        
        nextScreenPanel.SetActive(true);
        mainMenuPanel.SetActive(false);
    }
}
