using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public enum WorldState{ Alive, Undead }
    [SerializeField]
    public WorldState currentWorldState { get; private set; }
    public enum GameState { Playing, Dying, GameOver }
    [SerializeField]
    public GameState currentGameState { get; private set; }
    public Light2D globalLight;
    public Light2D glowLight;
    private float fadeDuration = 1.3f;
    public GameObject[] backgrounds = new GameObject[2];
    private Vector3 playersLastPosition;
    public AudioManager audioManager;
    public AudioClip undeadMusicClip;

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
        Player.instance.GetComponent<PlayerAnimations>().SwitchAnimation();
        playersLastPosition = Player.instance.transform.position;
        Player.instance.GetComponent<Rigidbody2D>().gravityScale = 1;
        Player.instance.canMove = false;
        Player.instance.GetComponent<PlayerController>().StopMovement();
        ProjectileSpawner.instance.StopProjectiles();
        StartCoroutine(ChangeWorld());
    }

    private void HandleGameOverState()
    {
        Player.instance.canMove = false;
        Player.instance.GetComponent<PlayerController>().StopMovement();
        ProjectileSpawner.instance.StopProjectiles();
        StartCoroutine(EndGame());
        Player.instance.GetComponent<PlayerAnimations>().DeathAnimation();
        AudioManager.instance.GameOver();
    }

    private IEnumerator ChangeWorld()
    {
        audioManager.PlayUndeadMusic(undeadMusicClip);
        SetWorldState(WorldState.Undead);
        Color startColor = globalLight.color;
        Color targetColor = Color.black;
        float elapsed = 0f; // Time elapsed since the start of the fade

        // Fade to black
        while (elapsed < fadeDuration)
        {
            // Interpolate the color based on time elapsed
            globalLight.color = Color.Lerp(startColor, targetColor, elapsed / fadeDuration);
            elapsed += Time.deltaTime; // Increment elapsed time
            yield return null; // Wait for the next frame
        }
        globalLight.color = targetColor;
        elapsed = 0f; // Reset elapsed time

        // Change backgrounds and lights, reset projectiles, and move player to death location
        ChangeBackgrounds();
        glowLight.gameObject.SetActive(false);
        glowLight.color = Color.red;
        ProjectileSpawner.instance.ResetProjectiles();
        Player.instance.GetComponent<PlayerAnimations>().SwitchAnimation();
        Player.instance.GetComponent<Rigidbody2D>().gravityScale = 0;
        Player.instance.transform.position = playersLastPosition;
        yield return new WaitForSeconds(0.5f);

        // Fade back to white
        while (elapsed < fadeDuration)
        {
            // Interpolate the color based on time elapsed
            globalLight.color = Color.Lerp(targetColor, startColor, elapsed / fadeDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        //Reset lights, player health, and projectile speeds
        glowLight.gameObject.SetActive(true);
        Player.instance.GetComponent<Player>().Heal(3);
        globalLight.color = startColor;
        ProjectileSpawner.instance.ResetSpeeds();

        SetGameState(GameState.Playing);
        yield return null;
    }

    private IEnumerator EndGame()
    {
        glowLight.gameObject.SetActive(false);
        Color startColor = globalLight.color;
        Color targetColor = Color.black;
        float elapsed = 0f; // Time elapsed since the start of the fade

        while (elapsed < fadeDuration)
        {
            // Interpolate the color based on time elapsed
            globalLight.color = Color.Lerp(startColor, targetColor, elapsed / fadeDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Ensure the final color is set to black
        globalLight.color = targetColor;
        yield return new WaitForSeconds(0.5f);
        UIManager.instance.ToggleGameOverScreen(true);
    }

    public void ChangeBackgrounds()
    {
        foreach (GameObject backgroundObject in backgrounds)
        {
            foreach (Transform child in backgroundObject.transform)
            {
                // Try to get the Background component
                Background backgroundScript = child.GetComponent<Background>();

                if (backgroundScript != null)
                {
                    backgroundScript.ChangeBackground();
                }
            }
        }
    }

    // public void RestartGame()
    // {
    //     StartCoroutine(RestartGameRoutine());
    // }
    // private IEnumerator RestartGameRoutine()
    // {
    //     Player.instance.transform.position = new Vector3(-8, 0, 0);
    //     Player.instance.ResetScore();
    //     Player.instance.GetComponent<Player>().Heal(3);
    //     ProjectileSpawner.instance.ResetProjectiles();
    //     UIManager.instance.ToggleGameOverScreen(false);
    //     Color startColor = globalLight.color;
    //     Color targetColor = Color.white;
    //     float elapsed = 0f;
    //     while (elapsed < fadeDuration)
    //     {
    //         // Interpolate the color based on time elapsed
    //         globalLight.color = Color.Lerp(targetColor, startColor, elapsed / fadeDuration);
    //         elapsed += Time.deltaTime; // Increment elapsed time
    //         yield return null; // Wait for the next frame
    //     }
    //     glowLight.gameObject.SetActive(true);
    //     Player.instance.GetComponent<Player>().Heal(3);
    //     // Ensure the final color is set to white
    //     globalLight.color = startColor;
    //     SetWorldState(WorldState.Alive);
    //     SetGameState(GameState.Playing);
    // }

    void Update()
    {
        if(currentGameState == GameState.Playing)
        {
            UIManager.instance.UpdatePlayerScore(Player.instance.GetScore());
        }
    }
}
