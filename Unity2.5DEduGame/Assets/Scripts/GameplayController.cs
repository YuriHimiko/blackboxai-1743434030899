using UnityEngine;
using System.Collections;

public class GameplayController : MonoBehaviour
{
    [Header("References")]
    public PlayerController playerController;
    public ParallaxBackground parallaxBackground;
    public float timePerQuestion = 30f;
    public int pointsPerCorrectAnswer = 100;
    public int timeBonusMultiplier = 10;

    private float currentTime;
    private bool isGameActive;
    private int currentScore;

    void Start()
    {
        StartLevel();
    }

    void StartLevel()
    {
        isGameActive = true;
        currentTime = timePerQuestion;
        currentScore = 0;
        UIManager.Instance.ShowGameUI();
        DisplayNextQuestion();
    }

    void Update()
    {
        if (!isGameActive) return;

        currentTime -= Time.deltaTime;
        UIManager.Instance.UpdateTimer(currentTime);

        if (currentTime <= 0)
        {
            TimeOut();
        }
    }

    void DisplayNextQuestion()
    {
        Question question = QuestionManager.Instance.GetCurrentQuestion();
        if (question != null)
        {
            currentTime = timePerQuestion; // Reset timer for new question
            UIManager.Instance.UpdateQuestionUI(question.questionText, question.options);
        }
        else
        {
            EndLevel(true);
        }
    }

    public void OnAnswerSelected(int answerIndex)
    {
        if (!isGameActive) return;

        bool isCorrect = QuestionManager.Instance.AnswerCurrentQuestion(answerIndex);
        
        if (isCorrect)
        {
            AudioManager.Instance.PlayCorrectAnswer();
            
            // Calculate score with time bonus
            int timeBonus = Mathf.FloorToInt(currentTime) * timeBonusMultiplier;
            int pointsEarned = pointsPerCorrectAnswer + timeBonus;
            
            currentScore += pointsEarned;
            GameManager.Instance.AddScore(pointsEarned);
            
            if (QuestionManager.Instance.HasMoreQuestions())
            {
                DisplayNextQuestion();
            }
            else
            {
                EndLevel(true);
            }
        }
        else
        {
            AudioManager.Instance.PlayWrongAnswer();
            EndLevel(false);
        }
    }

    void TimeOut()
    {
        EndLevel(false);
    }

    void EndLevel(bool success)
    {
        isGameActive = false;
        playerController.FreezePlayer();
        
        if (success)
        {
            GameManager.Instance.CompleteLevel(currentScore);
            UIManager.Instance.ShowResults(currentScore);
        }
        else
        {
            UIManager.Instance.ShowGameOver();
        }
    }

    public void ReturnToMenu()
    {
        GameManager.Instance.ReturnToMenu();
    }
}