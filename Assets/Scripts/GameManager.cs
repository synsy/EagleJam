using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public enum WorldState
    {
        Alive,
        Undead
    }

    public WorldState currentWorldState { get; private set; }

    public enum GameState
    {
        Playing,
        Dying,
        GameOver
    }

    public GameState currentGameState { get; private set; }

    public Light2D globalLight;
    private float fadeDuration = 1.3f;
    public GameObject[] backgrounds = new GameObject[2];

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        currentWorldState = WorldState.Alive;
        currentGameState = GameState.Playing;
    }

    void Start()
    {
        
    }

    public void SetWorldState(WorldState state)
    {
        currentWorldState = state;
        
    }

    public void SetGameState(GameState state)
    {
        currentGameState = state;
        switch (currentGameState)
        {
            case GameState.Playing:
                HandlePlayingState();
                break;

            case GameState.Dying:
                HandleDyingState();
                break;

            case GameState.GameOver:
                HandleGameOverState();
                break;
        }
    }

    private void HandlePlayingState()
    {
        Player.instance.canMove = true;
    }
    private void HandleDyingState()
    {
        Player.instance.canMove = false;
        Player.instance.GetComponent<PlayerController>().StopMovement();
        StartCoroutine(ChangeWorld());

    }

    private IEnumerator ChangeWorld()
    {
        SetWorldState(WorldState.Undead);
        Color startColor = globalLight.color;
        Color targetColor = Color.black;
        float elapsed = 0f; // Time elapsed since the start of the fade

        while (elapsed < fadeDuration)
        {
            // Interpolate the color based on time elapsed
            globalLight.color = Color.Lerp(startColor, targetColor, elapsed / fadeDuration);
            elapsed += Time.deltaTime; // Increment elapsed time
            yield return null; // Wait for the next frame
        }

        // Ensure the final color is set to black
        globalLight.color = targetColor;
        elapsed = 0f; // Reset elapsed time
        ChangeBackgrounds();
        yield return new WaitForSeconds(0.5f); // Wait for 1 second
        while (elapsed < fadeDuration)
        {
            // Interpolate the color based on time elapsed
            globalLight.color = Color.Lerp(targetColor, startColor, elapsed / fadeDuration);
            elapsed += Time.deltaTime; // Increment elapsed time
            yield return null; // Wait for the next frame
        }

        // Ensure the final color is set to black
        globalLight.color = startColor;
        SetGameState(GameState.Playing);
        yield return null;
    }

    private void HandleGameOverState()
    {
        EndGame();
    }

    public void EndGame()
    {
        // End the game
    }

    public void ChangeBackgrounds()
    {
        foreach (GameObject backgroundObject in backgrounds)
        {
            // Get all children of the current GameObject
            foreach (Transform child in backgroundObject.transform)
            {
                // Try to get the Background component
                Background backgroundScript = child.GetComponent<Background>();

                // If the Background script is found, call the ChangeBackground() method
                if (backgroundScript != null)
                {
                    backgroundScript.ChangeBackground();
                }
            }
        }
    }
}