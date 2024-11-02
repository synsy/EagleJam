using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(PlayerController))]
public class Player : MonoBehaviour
{
    public static Player instance;
    public int maxHealth { get; private set; }
    public int currentHealth { get; private set; }
    public bool canMove = true;
    
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        
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
        if(GameManager.instance.currentWorldState == GameManager.WorldState.Alive)
        {
            GameManager.instance.SetGameState(GameManager.GameState.Dying);
        }

        if(GameManager.instance.currentWorldState == GameManager.WorldState.Undead)
        {
            GameManager.instance.SetGameState(GameManager.GameState.GameOver);
        }
    }
}
