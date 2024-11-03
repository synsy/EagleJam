using UnityEngine;

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
}
