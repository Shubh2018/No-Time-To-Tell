using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 6.0f;
    bool isDead;
    Animator enemyAnimator;
    public AnimationClip monsterDeathAnim;
    int health = 20;

    private void Start()
    {
        enemyAnimator = this.GetComponent<Animator>();
    }

    private void Update()
    {
        transform.Translate(speed * Time.deltaTime * Vector2.left);
        if (health <= 0)
            isDead = true;
        else
            isDead = false;

        if (isDead)
        {
            speed = 0;
            enemyAnimator.Play(monsterDeathAnim.name);
            Destroy(this.gameObject.GetComponent<Rigidbody2D>());
            Destroy(this.gameObject.GetComponent<BoxCollider2D>());
            Destroy(this.gameObject, 1.0f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Bounds"))
        {
            this.transform.eulerAngles += new Vector3(0.0f, -180f, 0.0f);
            //direction = -direction;
            Debug.Log("Bound Check");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Bullet"))
            health -= 5;
    }
}
