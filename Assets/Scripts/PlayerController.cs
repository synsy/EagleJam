using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 8f;
    private Vector2 inputDirection;
    private Rigidbody2D rb;
    private float halfWidth;
    private float halfHeight;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        // Calculate half the width and height of the player based on its sprite bounds
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        halfWidth = spriteRenderer.bounds.extents.x;
        halfHeight = spriteRenderer.bounds.extents.y;
    }

    private void FixedUpdate()
    {
        MovePlayer();
        ConstrainToCameraBounds();
    }

    public void SetInputDirection(Vector2 direction)
    {
        inputDirection = direction;
    }

    private void MovePlayer()
    {
        if(Player.instance.canMove)
            rb.linearVelocity = inputDirection * moveSpeed;
    }

    private void ConstrainToCameraBounds()
    {
        Camera cam = Camera.main;

        // Calculate camera bounds in world coordinates
        Vector3 bottomLeft = cam.ViewportToWorldPoint(new Vector3(0, 0, cam.transform.position.z));
        Vector3 topRight = cam.ViewportToWorldPoint(new Vector3(1, 1, cam.transform.position.z));

        // Clamp the player's position within the adjusted camera bounds
        Vector3 clampedPosition = transform.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, bottomLeft.x + halfWidth, topRight.x - halfWidth);
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, bottomLeft.y + halfHeight, topRight.y - halfHeight);

        // Apply the clamped position to the player
        transform.position = clampedPosition;
    }

    public void StopMovement()
    {
        rb.linearVelocity = Vector2.zero;
    }
}
