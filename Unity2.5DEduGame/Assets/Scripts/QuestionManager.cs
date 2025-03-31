using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class QuestionManager : MonoBehaviour
{
    public static QuestionManager Instance;
    
    [System.Serializable]
    public class Question
    {
        public string questionText;
        public List<string> options;
        public int correctAnswerIndex;
        public string category;
    }

    [System.Serializable]
    public class QuestionList
    {
        public List<Question> questions;
    }

    public QuestionList allQuestions = new QuestionList();

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
        LoadQuestions();
    }

    void LoadQuestions()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("Data/Questions");
        allQuestions = JsonUtility.FromJson<QuestionList>(jsonFile.text);
    }

    private List<Question> currentLevelQuestions;
    private int currentQuestionIndex = 0;
    private int currentLevelIndex = 0;

    public void SetupLevel(int difficulty, int levelIndex)
    {
        currentLevelIndex = levelIndex;
        currentQuestionIndex = 0;
        currentLevelQuestions = GetQuestionsForDifficulty(difficulty, 5); // 5 questions per level
    }

    public Question GetCurrentQuestion()
    {
        if (currentQuestionIndex < currentLevelQuestions.Count)
        {
            return currentLevelQuestions[currentQuestionIndex];
        }
        return null;
    }

    public bool AnswerCurrentQuestion(int answerIndex)
    {
        bool isCorrect = currentLevelQuestions[currentQuestionIndex].correctAnswerIndex == answerIndex;
        
        if (isCorrect)
        {
            currentQuestionIndex++;
            
            // If completed all questions, mark level as complete
            if (currentQuestionIndex >= currentLevelQuestions.Count)
            {
                MarkLevelComplete();
            }
        }
        
        return isCorrect;
    }

    private void MarkLevelComplete()
    {
        string key = $"Completed_{GameManager.Instance.selectedDifficulty}_{currentLevelIndex}";
        PlayerPrefs.SetInt(key, 1);
        
        // Unlock next level
        string unlockKey = $"Unlocked_{GameManager.Instance.selectedDifficulty}_{currentLevelIndex + 1}";
        PlayerPrefs.SetInt(unlockKey, 1);
        PlayerPrefs.Save();
    }

    public List<Question> GetQuestionsForDifficulty(int difficulty, int count)
    {
        // Filter questions by difficulty and return random subset
        List<Question> filtered = allQuestions.questions.FindAll(q => 
            (difficulty == 0 && q.category == "Easy") ||
            (difficulty == 1 && q.category == "Medium") ||
            (difficulty == 2 && q.category == "Hard"));
        
        // Shuffle and return requested count
        return ShuffleList(filtered).GetRange(0, Mathf.Min(count, filtered.Count));
    }

    public bool HasMoreQuestions()
    {
        return currentQuestionIndex < currentLevelQuestions.Count;
    }

    public int GetQuestionsRemaining()
    {
        return currentLevelQuestions.Count - currentQuestionIndex;
    }

    List<Question> ShuffleList(List<Question> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            Question temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
        return list;
    }
}