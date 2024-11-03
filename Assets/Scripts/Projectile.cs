using Unity.VisualScripting;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 4f;
    private int damage = 1;
    private Rigidbody2D rb;
    private Animator animator;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void OnEnable()
    {
        // Reset the projectile's velocity each time it is reused from the pool
        rb.linearVelocity = Vector2.zero;
        rb.linearVelocity = transform.right * -speed;
        if(GameManager.instance != null)
        {
            if(GameManager.instance.currentWorldState == GameManager.WorldState.Undead)
            {
                SwitchAnimation("Fireball");
            }
            else
            {
                SwitchAnimation("Rock");
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player.instance.TakeDamage(damage);
        }

        KillProjectile();
    }

    public void KillProjectile()
    {
        gameObject.SetActive(false);
    }

    private void SwitchAnimation(string anim)
    {
        if(anim == "Fireball")
        {
            animator.SetBool("IsRock", false);
            return;
        }
        else
        {
            animator.SetBool("IsRock", true);
        }
    }

    public void StopMovement()
    {
        rb.linearVelocity = Vector2.zero;
    }

    void Update()
    {
        // Deactivate the projectile if it goes offscreen
        if (transform.position.x < -10f)
        {
            KillProjectile();
        }
    }
}