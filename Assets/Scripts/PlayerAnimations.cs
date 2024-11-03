using UnityEngine;
using System.Collections;

public class PlayerAnimations : MonoBehaviour
{
    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void SwitchAnimation()
    {
        if (animator.GetBool("IsAlive"))
        {
            animator.SetBool("IsAlive", false);
        }
        else
        {
            animator.SetBool("IsAlive", true);
        }
    }

    
    public void DeathAnimation()
    {
        animator.SetTrigger("GameOverDeath");
    }


    public void PlayerHit()
    {
        StartCoroutine(FlashRed());
    }

    // Coroutine to flash red and revert back
    private IEnumerator FlashRed()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        Color originalColor = spriteRenderer.color;  // Store the original color
        
        // Change color to red
        spriteRenderer.color = Color.red;
        
        // Wait for a short moment
        yield return new WaitForSeconds(0.1f);

        // Revert to the original color
        spriteRenderer.color = originalColor;
    }
}
