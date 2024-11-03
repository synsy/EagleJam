using Unity.VisualScripting;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float initialSpeed = 4f;
    public float speed = 4f;
    private int damage = 1;
    private Rigidbody2D rb;
    private Collider2D collider;
    private Animator animator;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
    }

    void OnEnable()
    {
        collider.enabled = true;
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
            animator.SetTrigger("Explode");
            //turn off collider
            collider.enabled = false;
        }

        //KillProjectile();
        if(GameManager.instance.currentWorldState == GameManager.WorldState.Undead)
        {
            SwitchAnimation("Fireball");
        }
    }

    /*private IEnumerator ResetCollider()
    {
            yield return new WaitForSeconds(0.5f);
            collider.enabled = true;
    }*/

    public void KillProjectile()
    {
        gameObject.SetActive(false);
    }

    public void ResetSpeed()
    {
        speed = initialSpeed;
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

    private void UpdateProjectileSpeed()
{
    speed = Mathf.Min(20f, 4f + Player.instance.GetScore() * 1.001f);
}

    void Update()
    {
        UpdateProjectileSpeed();
        // Deactivate the projectile if it goes offscreen
        if (transform.position.x < -10f)
        {
            KillProjectile();
        }
    }
}
