using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneShot : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip deathSound;
    void Start()
    {
       audioSource = gameObject.AddComponent<AudioSource>();
       audioSource.playOnAwake = false;
       audioSource.PlayOneShot(deathSound);
    }


}
