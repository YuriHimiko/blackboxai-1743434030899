using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Main Menu")]
    public GameObject mainMenuPanel;
    public Button[] difficultyButtons;

    [Header("Game UI")] 
    public GameObject gameUIPanel;
    public TMP_Text questionText;
    public Button[] answerButtons;
    public TMP_Text scoreText;
    public TMP_Text timerText;
    public Image progressBar;

    [Header("Results")]
    public GameObject resultsPanel;
    public TMP_Text finalScoreText;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void UpdateQuestionUI(string question, List<string> options)
    {
        questionText.text = question;
        for (int i = 0; i < answerButtons.Length; i++)
        {
            if (i < options.Count)
            {
                answerButtons[i].gameObject.SetActive(true);
                answerButtons[i].GetComponentInChildren<TMP_Text>().text = options[i];
            }
            else
            {
                answerButtons[i].gameObject.SetActive(false);
            }
        }
    }

    public void UpdateScore(int score)
    {
        scoreText.text = $"Score: {score}";
    }

    public void UpdateTimer(float time)
    {
        timerText.text = $"Time: {time:F1}s";
    }

    [Header("Game UI")]
    public GameObject gameUIPanel;
    public TMP_Text questionText;
    public Button[] answerButtons;
    public TMP_Text timerText;
    public Image progressBar;
    public TMP_Text scoreText;
    public GameObject correctAnswerEffect;
    public GameObject wrongAnswerEffect;

    public void ShowResults(int score)
    {
        gameUIPanel.SetActive(false);
        resultsPanel.SetActive(true);
        finalScoreText.text = $"Final Score: {score}";
    }

    public void ShowGameOver()
    {
        gameUIPanel.SetActive(false);
        resultsPanel.SetActive(true);
        finalScoreText.text = "Game Over!";
    }

    public void ShowGameUI()
    {
        mainMenuPanel.SetActive(false);
        resultsPanel.SetActive(false);
        gameUIPanel.SetActive(true);
    }

    public void UpdateQuestionUI(string question, List<string> options)
    {
        questionText.text = question;
        for (int i = 0; i < answerButtons.Length; i++)
        {
            if (i < options.Count)
            {
                answerButtons[i].gameObject.SetActive(true);
                answerButtons[i].GetComponentInChildren<TMP_Text>().text = options[i];
                
                // Reset button state
                answerButtons[i].interactable = true;
                answerButtons[i].image.color = Color.white;
            }
            else
            {
                answerButtons[i].gameObject.SetActive(false);
            }
        }
    }

    public void UpdateTimer(float time)
    {
        timerText.text = $"Time: {time:F1}s";
        progressBar.fillAmount = time / GameplayController.Instance.timePerQuestion;
    }

    public void UpdateScore(int score)
    {
        scoreText.text = $"Score: {score}";
    }

    public void ShowAnswerFeedback(int selectedIndex, int correctIndex)
    {
        // Highlight correct answer in green
        answerButtons[correctIndex].image.color = Color.green;
        
        // If wrong answer selected, show it in red
        if (selectedIndex != correctIndex)
        {
            answerButtons[selectedIndex].image.color = Color.red;
            Instantiate(wrongAnswerEffect, answerButtons[selectedIndex].transform);
        }
        else
        {
            Instantiate(correctAnswerEffect, answerButtons[selectedIndex].transform);
        }
        
        // Disable all buttons after selection
        foreach (Button button in answerButtons)
        {
            button.interactable = false;
        }
    }

    public GameObject volumePanel;
    
    public void PlayButtonClick()
    {
        AudioManager.Instance.PlayButtonClick();
    }

    public void ToggleVolumePanel()
    {
        if (volumePanel != null)
        {
            bool isShowing = !volumePanel.activeSelf;
            volumePanel.SetActive(isShowing);
            
            // Pause game when settings are open (except in main menu)
            if (SceneManager.GetActiveScene().name != "MainMenu")
            {
                Time.timeScale = isShowing ? 0 : 1;
            }
            
            PlayButtonClick();
        }
    }
}