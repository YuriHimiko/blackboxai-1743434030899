using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class LevelSelectionController : MonoBehaviour
{
    [Header("UI Elements")]
    public Transform levelButtonContainer;
    public GameObject levelButtonPrefab;
    public Button backButton;
    public Text difficultyText;

    [Header("Level Settings")]
    public int levelsPerDifficulty = 5;
    public Sprite[] difficultyIcons;
    public Color[] difficultyColors;

    private string[] difficultyNames = { "Easy", "Medium", "Hard" };

    void Start()
    {
        backButton.onClick.AddListener(ReturnToMainMenu);
        UpdateDifficultyDisplay();
        PopulateLevelGrid();
    }

    void UpdateDifficultyDisplay()
    {
        int diff = GameManager.Instance.selectedDifficulty;
        difficultyText.text = difficultyNames[diff];
        difficultyText.color = difficultyColors[diff];
    }

    void PopulateLevelGrid()
    {
        // Clear existing buttons
        foreach (Transform child in levelButtonContainer)
        {
            Destroy(child.gameObject);
        }

        // Create level buttons
        for (int i = 0; i < levelsPerDifficulty; i++)
        {
            GameObject buttonObj = Instantiate(levelButtonPrefab, levelButtonContainer);
            LevelButton button = buttonObj.GetComponent<LevelButton>();
            
            // Set button properties
            button.Setup(
                levelIndex: i + 1,
                isUnlocked: IsLevelUnlocked(i),
                difficultyIcon: difficultyIcons[GameManager.Instance.selectedDifficulty]
            );
            
            // Add click listener
            int level = i; // Capture value for closure
            button.GetComponent<Button>().onClick.AddListener(() => LoadLevel(level));
        }
    }

    bool IsLevelUnlocked(int levelIndex)
    {
        // First level is always unlocked
        if (levelIndex == 0) return true;
        
        // Check player prefs for unlocked levels
        string key = $"Unlocked_{GameManager.Instance.selectedDifficulty}_{levelIndex}";
        return PlayerPrefs.GetInt(key, 0) == 1;
    }

    void LoadLevel(int levelIndex)
    {
        GameManager.Instance.LoadLevel(levelIndex);
    }

    void ReturnToMainMenu()
    {
        GameManager.Instance.ReturnToMenu();
    }
}