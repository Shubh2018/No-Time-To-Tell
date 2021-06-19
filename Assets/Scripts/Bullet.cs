using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private float speed = 10.0f;

    private void Update()
    {
        transform.Translate(speed * Time.deltaTime * Vector2.right);
        //Destroy(this.gameObject, 1.5f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision != null)
            Destroy(this.gameObject);
    }
}
