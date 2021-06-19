using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{
    public Animator animator;
    bool isEnterTrigger = false;
    [SerializeField]
    Light doorLight;
    AudioSource audioSource;
    [SerializeField]
    AudioClip switchAudio;

    private void Start()
    {
        audioSource = this.GetComponent<AudioSource>();    
    }

    private void Update()
    {
        if(isEnterTrigger && Input.GetKeyDown(KeyCode.X))
        {
            animator.SetTrigger("TriggerSwitch");
            SwitchOnPosition();
        }
            
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            isEnterTrigger = true;
            audioSource.clip = switchAudio;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isEnterTrigger = false;
            audioSource.clip = null;
        }
    }

    void SwitchOnPosition()
    {
        GameManager.instance.IsDoorOpen = true;
        doorLight.color = Color.green;
        GameManager.instance.TotalTime -= 30f;
        audioSource.Play();
    }
}
