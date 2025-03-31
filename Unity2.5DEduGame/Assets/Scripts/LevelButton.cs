using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelButton : MonoBehaviour
{
    [Header("UI Elements")]
    public Image iconImage;
    public TextMeshProUGUI levelText;
    public Image lockIcon;
    public GameObject completedCheck;
    public Image background;

    [Header("Colors")]
    public Color normalColor;
    public Color highlightedColor;
    public Color pressedColor;

    private Button button;
    private int levelIndex;

    void Awake()
    {
        button = GetComponent<Button>();
        
        // Setup button colors
        ColorBlock colors = button.colors;
        colors.normalColor = normalColor;
        colors.highlightedColor = highlightedColor;
        colors.pressedColor = pressedColor;
        button.colors = colors;
    }

    public void Setup(int levelIndex, bool isUnlocked, Sprite difficultyIcon)
    {
        this.levelIndex = levelIndex;
        levelText.text = levelIndex.ToString();
        iconImage.sprite = difficultyIcon;
        
        // Set locked/unlocked state
        lockIcon.gameObject.SetActive(!isUnlocked);
        levelText.gameObject.SetActive(isUnlocked);
        button.interactable = isUnlocked;

        // Show completion status
        bool isCompleted = PlayerPrefs.GetInt($"Completed_{GameManager.Instance.selectedDifficulty}_{levelIndex-1}", 0) == 1;
        completedCheck.SetActive(isCompleted);

        // Visual feedback for current level
        if (isUnlocked && !isCompleted)
        {
            background.color = new Color(0.8f, 0.8f, 0.2f, 0.3f);
        }
    }

    public void OnClick()
    {
        if (GetComponent<Button>().interactable)
        {
            // Play sound effect
            AudioManager.Instance.PlayButtonClick();
            
            // Load level
            GameManager.Instance.LoadLevel(levelIndex - 1);
        }
    }
}