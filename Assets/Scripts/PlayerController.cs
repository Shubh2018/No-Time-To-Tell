using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    float speed;
    [SerializeField]
    float speedWhenGrounded = 8.0f;
    [SerializeField]
    float speedWhenInAir = 2.0f;
    float hMovement;
    Rigidbody2D rb;
    [SerializeField]
    LayerMask groundMask;
    [SerializeField]
    private float jumpForce = 8.0f;
    [SerializeField]
    private GameObject bulletPrefab;
    Vector2 directionToMove;
    [SerializeField]
    LayerMask wallMask;
    bool leftWallCollider;
    bool rightWallCollider;
    SpriteRenderer spriteRenderer;
    [SerializeField]
    GameObject gunModelSprite;
    [SerializeField]
    Animator playerAnim;
    public AnimationClip idle_anim;
    public AnimationClip walk_anim;
    public AnimationClip jump_anim;
    public AnimationClip death_anim;
    bool canMove = true;
    float fireRate = 0.2f;
    float nextTimeToFire;
    [SerializeField]
    Transform bulletSpawn;
    int noOfJumps = 0;
    bool isDead;
    [SerializeField]
    AudioSource rifleAudioSource;
    AudioSource audioSource;
    [SerializeField]
    AudioClip shootAudio;
    [SerializeField]
    AudioClip jumpAudio;

    private void Start()
    {
        spriteRenderer = this.GetComponent<SpriteRenderer>();
        audioSource = this.GetComponent<AudioSource>();
        gunModelSprite.SetActive(false);
        spriteRenderer.enabled = true;
        rb = this.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        PlayerMovement();

        if (Input.GetKey(KeyCode.Z))
        {
            rifleAudioSource.clip = shootAudio;
            ShowGunModel(gunModelSprite);
            canMove = false;
            Fire(bulletPrefab, bulletSpawn);
        }

        if(Input.GetKeyUp(KeyCode.Z))
        {
            HideGunModel(gunModelSprite);
            canMove = true;
            rifleAudioSource.clip = null;
        }
            
        if (Input.GetKeyDown(KeyCode.Escape))
            Cursor.visible = true;

        if (GameManager.instance.TimeUp == true)
        {
            GameManager.instance.IsDead = true;
            isDead = true;
        }
            

        if (isDead)
        {
            GameManager.instance.IsDead = true;
            HideGunModel(gunModelSprite);
            playerAnim.Play(death_anim.name);
            Destroy(this);
            Destroy(this.GetComponent<BoxCollider2D>());
            Destroy(this.rb);
            Destroy(this.gameObject, 1.5f);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Enemy"))
        {
            isDead = true;
        }
            
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Goal"))
            GameManager.instance.HasReachedDoor = true;
    }

    void PlayerMovement()
    {
        speed = GroundCheck() ? speedWhenGrounded : speedWhenInAir;

        leftWallCollider = Physics2D.OverlapBox(transform.position - new Vector3(0.5f, -0.5f), new Vector2(0.1f, 1f), 0f, wallMask);
        rightWallCollider = Physics2D.OverlapBox(transform.position - new Vector3(-0.5f, -0.5f), new Vector2(0.1f, 1f), 0f, wallMask);

        hMovement = Input.GetAxis("Horizontal") * speed;

        if ((hMovement != 0 && GroundCheck()) || (WallCheck() && hMovement != 0))
            playerAnim.Play(walk_anim.name);            
        else if((GroundCheck() && hMovement == 0) || (WallCheck() && hMovement == 0))
            playerAnim.Play(idle_anim.name);
        
        if (hMovement < 0)
        {
            this.transform.eulerAngles = new Vector3(0f, -180f, 0f);
            directionToMove = Vector2.left;
        }
            
        else if (hMovement > 0)
        {
            this.transform.eulerAngles = new Vector3(0f, 0f, 0f);
            directionToMove = Vector2.right;
        }

        if (GroundCheck() || leftWallCollider || rightWallCollider)
        {
            noOfJumps = 0;
            audioSource.clip = null;

            if(Input.GetKeyDown(KeyCode.LeftShift))
            {
                noOfJumps += 1;
                Jump(jumpForce, Vector2.up);
                audioSource.clip = jumpAudio;
                audioSource.Play();
            }
        }

        else
        {
            if (Input.GetKeyDown(KeyCode.LeftShift) && noOfJumps < 2)
            {
                noOfJumps += 1;
                Jump(jumpForce, Vector2.up);
                audioSource.clip = jumpAudio;
                audioSource.Play();
            }

            if((leftWallCollider || rightWallCollider) && Input.GetKeyDown(KeyCode.LeftShift))
            {
                noOfJumps += 1;
                Jump(jumpForce, Vector2.up);
                audioSource.clip = jumpAudio;
                audioSource.Play();
            }
        }

        if (canMove)
            this.transform.Translate(hMovement * Time.deltaTime * directionToMove);
    }

    bool GroundCheck()
    {
        Collider2D collider = Physics2D.OverlapBox(transform.position, new Vector2(1.0f, 0.1f), 0f, groundMask);
        return collider != null;
    }

    bool WallCheck()
    {
        Collider2D collider = Physics2D.OverlapBox(transform.position, new Vector2(1.0f, 0.1f), 0f, wallMask);
        return collider != null;
    }

    void Fire(GameObject objectToInstantiate, Transform posToInstantiate)
    {
        if(nextTimeToFire < Time.time)
        {
            nextTimeToFire = Time.time + fireRate;
            Instantiate(objectToInstantiate, posToInstantiate.position, this.transform.rotation);
            rifleAudioSource.Play();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawCube(transform.position,  new Vector2(1.0f, 0.1f));
        Gizmos.DrawCube(transform.position - new Vector3(0.5f, -0.5f), new Vector2(0.1f, 1f));
        Gizmos.DrawCube(transform.position - new Vector3(-0.5f, -0.5f), new Vector2(0.1f, 1f));
    }

    void ShowGunModel(GameObject gunModel)
    {
        gunModel.SetActive(true);
        spriteRenderer.enabled = false;
    }

    void HideGunModel(GameObject gunModel)
    {
        gunModel.SetActive(false);
        spriteRenderer.enabled = true;
    }

    void Jump(float jumpForce, Vector2 direction)
    {
        rb.velocity = direction * jumpForce;
        playerAnim.Play(jump_anim.name);
    }
}