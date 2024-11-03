using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    private VisualElement root;
    private Label playerHealth;
    private Label playerScore;
    private VisualElement gameOverScreen;
    private Button playAgainButton;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return; // Exit to avoid further setup on this duplicate instance
        }
    }

    void Start()
    {
        // Initialize UI elements here to ensure that everything is loaded
        InitializeUI();
    }

    private void InitializeUI()
    {
        var uiDocument = GetComponent<UIDocument>();
        if (uiDocument != null)
        {
            root = uiDocument.rootVisualElement;
            playerHealth = root.Q<Label>("health");
            playerScore = root.Q<Label>("score");
            gameOverScreen = root.Q<VisualElement>("gameOverContainer");
            playAgainButton = root.Q<Button>("playAgain");

            // Hide game over screen initially
            gameOverScreen.style.display = DisplayStyle.None;

            // Add click listener for play again button to reload the scene
        playAgainButton?.RegisterCallback<ClickEvent>(evt => 
            SceneManager.LoadScene(SceneManager.GetActiveScene().name)
        );
        }
    }


    public void UpdatePlayerHealth(int health)
    {
        if (playerHealth != null)
        {
            playerHealth.text = health.ToString();
        }
    }

    public void UpdatePlayerScore(int score)
    {
        if (playerScore != null)
        {
            playerScore.text = score.ToString();
        }
    }

    public void ToggleGameOverScreen(bool active)
    {
        // Show or hide the game over screen
        if (root != null)
        {
            var gameOverScreen = root.Q<VisualElement>("gameOverContainer");
            if (gameOverScreen != null)
            {
                gameOverScreen.style.display = active ? DisplayStyle.Flex : DisplayStyle.None;
            }
        }
    }   
}
