using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(PlayerController))]
public class Player : MonoBehaviour
{
    public int maxHealth { get; private set; }
    public int currentHealth { get; private set; }
    
    void Awake()
    {
        maxHealth = 3;
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        // Check state of player/game i.e. Alive or Undead
    }
}
