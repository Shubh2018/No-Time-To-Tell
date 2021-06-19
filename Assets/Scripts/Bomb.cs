using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    Animator animator;
    [SerializeField]AnimationClip blastAnim;
    AudioSource audioSource;
    [SerializeField]
    AudioClip explosionClip;

    private void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = this.GetComponent<AudioSource>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision != null)
        {
            audioSource.clip = explosionClip;
            animator.Play(blastAnim.name);
            audioSource.Play();
            Destroy(this.GetComponent<Rigidbody2D>());
            Destroy(this.gameObject.GetComponent<CircleCollider2D>(), blastAnim.length - 0.5f);
            Destroy(this.gameObject, blastAnim.length);
        }
    }
}
