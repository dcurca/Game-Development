using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    [System.Serializable]
    public class PlayerStats
    {
        public int maxHealth = 100;
        private int _currentHealth;

        public int currentHealth
        {
            get { return _currentHealth; }
            set { _currentHealth = Mathf.Clamp(value, 0, maxHealth); }
        }

        public void Init()
        {
            currentHealth = maxHealth;
        }
    }

    public PlayerStats stats = new PlayerStats();
    [SerializeField] private StatusIndicator statusIndicator;

    [SerializeField]
    private Rigidbody2D playerRB;
    private float directionX;

    private Animator animation;
    private SpriteRenderer flip;

    [SerializeField] private float speed = 7f;
    [SerializeField] private float jump = 14f;

    private BoxCollider2D collider;
    [SerializeField] private LayerMask ground;

    private enum AnimationState {Idle, Running, Jumping, Falling, Attack}

    [SerializeField] private AudioSource jumpSound;
    [SerializeField] private AudioSource heartSound;
    [SerializeField] private AudioSource deathSound;
    [SerializeField] private AudioSource finishSound;

    private void Start()
    {
        stats.Init();
        if(statusIndicator != null)
        {
            statusIndicator.SetHealth(stats.currentHealth, stats.maxHealth);
        }

        playerRB = GetComponent<Rigidbody2D>();
        animation = GetComponent<Animator>();
        flip = GetComponent<SpriteRenderer>();
        collider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        directionX = Input.GetAxisRaw("Horizontal");
        playerRB.velocity = new Vector2(directionX * speed, playerRB.velocity.y);

        if(Input.GetKeyDown(KeyCode.Space) && isGrounded())
        {
            jumpSound.Play();
            playerRB.velocity = new Vector2(playerRB.velocity.x, jump);
        }

        Animations();
    }

    private void Animations()
    {
        AnimationState state;

        if (directionX > 0)
        {
            state = AnimationState.Running;
            flip.flipX = false;
        }
        else if (directionX < 0)
        {
            state = AnimationState.Running;
            flip.flipX = true;
        }
        else
        {
            state = AnimationState.Idle;
        }

        if(playerRB.velocity.y > .1f)
        {
            state = AnimationState.Jumping;
        }
        else if(playerRB.velocity.y < -.1f)
        {
            state = AnimationState.Falling;
        }

        animation.SetInteger("state", (int)state);
    }

    private bool isGrounded()
    {
        return Physics2D.BoxCast(collider.bounds.center, collider.bounds.size, 0f, Vector2.down, .1f, ground);
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Trap"))
        {
            Death();
        }
    }

    public void Death()
    {
        animation.SetTrigger("death");
        deathSound.Play();
        playerRB.bodyType = RigidbodyType2D.Static;
    }

    private void restartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void DamagePlayer(int damage)
    {
        stats.currentHealth -= damage;
        if(stats.currentHealth <= 0)
        {
            Death();
        }
        if(statusIndicator != null)
        {
            statusIndicator.SetHealth(stats.currentHealth, stats.maxHealth);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Heart"))
        {
            heartSound.Play();
            Destroy(collision.gameObject);
            stats.currentHealth += 50;
        }
        if (collision.gameObject.CompareTag("damage"))
        {
            Destroy(collision.gameObject);
            DamagePlayer(10);
        }
    }
}
