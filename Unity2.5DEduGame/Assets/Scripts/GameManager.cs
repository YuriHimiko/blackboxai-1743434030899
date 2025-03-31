using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    public int currentScore;
    public int selectedDifficulty; // 0=Easy, 1=Medium, 2=Hard
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            
            // Initialize AudioManager
            if (AudioManager.Instance == null)
            {
                Instantiate(Resources.Load<GameObject>("Managers/AudioManager"));
            }
            AudioManager.Instance.PlayMenuMusic();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StartGame(int difficulty)
    {
        selectedDifficulty = difficulty;
        currentScore = 0;
        SceneManager.LoadScene("LevelSelect");
        AudioManager.Instance.PlayGameplayMusic();
        
        // Initialize other managers if needed
        if (QuestionManager.Instance == null)
        {
            Instantiate(Resources.Load<GameObject>("Managers/QuestionManager"));
        }
    }

    public void LoadLevel(int levelIndex)
    {
        // Load specific level with current difficulty
        QuestionManager.Instance.SetupLevel(selectedDifficulty, levelIndex);
        SceneManager.LoadScene("GameScene");
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene("MainMenu");
        AudioManager.Instance.PlayMenuMusic();
    }

    public void CompleteLevel(int scoreEarned)
    {
        currentScore += scoreEarned;
        // Save progress
        PlayerPrefs.SetInt("LastScore", currentScore);
        PlayerPrefs.Save();
        
        // Show results or load next level
        UIManager.Instance.ShowResults(currentScore);
    }

    public void AddScore(int points)
    {
        currentScore += points;
        // Update UI through UIManager
        UIManager.Instance.UpdateScore(currentScore);
    }
}