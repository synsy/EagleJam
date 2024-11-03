using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(PlayerAnimations))]
public class Player : MonoBehaviour
{
    public static Player instance;
    public int maxHealth { get; private set; }
    public int currentHealth { get; private set; }
    public bool canMove = true;
    private int score;
    private float scoreUpdateInterval = 2f;
    private float timeSinceLastScoreUpdate = 0f;
    
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
        UIManager.instance.UpdatePlayerHealth(currentHealth);
        PlayerAnimations playerAnims = GetComponent<PlayerAnimations>();
        AudioManager.instance.PlaySFX(AudioManager.instance.hitClip);
        playerAnims.PlayerHit();
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(int healAmount)
    {
        currentHealth += healAmount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        UIManager.instance.UpdatePlayerHealth(currentHealth);
    }

    public void Die()
    {
        AudioManager.instance.PlaySFX(AudioManager.instance.dieClip);
        if(GameManager.instance.currentWorldState == GameManager.WorldState.Alive)
        {
            GameManager.instance.SetGameState(GameManager.GameState.Dying);
            return;
        }

        if(GameManager.instance.currentWorldState == GameManager.WorldState.Undead)
        {
            GameManager.instance.SetGameState(GameManager.GameState.GameOver);
        }
    }

    public int GetScore()
    {
        return score;
    }

    public void ResetScore()
    {
        score = 0;
    }

    void Update()
    {
        if(GameManager.instance.currentGameState == GameManager.GameState.Playing)
        {
            // Increase timer by the time that has passed since the last frame
            timeSinceLastScoreUpdate += Time.deltaTime;

            // Check if the interval has been reached
            if (timeSinceLastScoreUpdate >= scoreUpdateInterval)
            {
                score += 1; // Increase score by 1 every interval
                timeSinceLastScoreUpdate = 0f; // Reset the timer
            }
        }
    }
}
