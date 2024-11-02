using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    // Create a reference to PlayerController
    private PlayerController playerController;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
            {
                Player.instance.Die();
            }
    }

    private void FixedUpdate()
    {
        if(Player.instance.canMove)
        {
            float horizontal = Input.GetAxis("Horizontal"); // Left and right arrow keys or A/D keys
            float vertical = Input.GetAxis("Vertical"); // Up and down arrow keys or W/S keys

            playerController.SetInputDirection(new Vector2(horizontal, vertical));
        }
    }
}
