using UnityEngine;

public class Background : MonoBehaviour
{
    [SerializeField] private float scrollSpeed = 2f; // Speed at which the background moves
    private const float resetPositionX = -19.9f; // X position at which reset occures
    private const float startPositionX = 19.9f;   // X position to reset to
    public Sprite[] sprite;
    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if(GameManager.instance.currentGameState == GameManager.GameState.Playing)
        {
            ScrollBackground();
        }
    }

    private void ScrollBackground()
    {
        // Move the GameObject to the left based on the scroll speed
        transform.position += Vector3.left * scrollSpeed * Time.deltaTime;

        // Check if the GameObject has moved past the reset position
        if (transform.position.x <= resetPositionX)
        {
            // Reset the position to the start position
            transform.position = new Vector3(startPositionX, transform.position.y, transform.position.z);
        }
    }

    public void ChangeBackground()
    {
        if(sprite[1] != null)
        {
            spriteRenderer.sprite = sprite[1];
        }
        else
        {
            Debug.LogError("Sprite is null: " + gameObject.name);
        }
    }
}
