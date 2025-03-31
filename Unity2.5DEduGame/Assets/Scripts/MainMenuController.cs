using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [Header("UI References")]
    public Button startButton;
    public Button exitButton;
    public GameObject settingsButton;
    public ToggleGroup difficultyToggleGroup;

    [Header("Audio")]
    public AudioClip buttonClickSound;
    private AudioSource audioSource;

    void Start()
    {
        // Initialize audio
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;

        // Setup button listeners
        startButton.onClick.AddListener(OnStartClicked);
        exitButton.onClick.AddListener(OnExitClicked);

        // Default to medium difficulty
        difficultyToggleGroup.SetAllTogglesOff();
        difficultyToggleGroup.transform.GetChild(1).GetComponent<Toggle>().isOn = true;
    }

    public void OnStartClicked()
    {
        PlayButtonSound();
        
        // Get selected difficulty
        int difficulty = 1; // Default to medium
        foreach (Toggle toggle in difficultyToggleGroup.ActiveToggles())
        {
            difficulty = toggle.transform.GetSiblingIndex();
            break;
        }

        // Start game with selected difficulty
        GameManager.Instance.StartGame(difficulty);
    }

    public void OnExitClicked()
    {
        PlayButtonSound();
        
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    private void PlayButtonSound()
    {
        if (buttonClickSound != null)
        {
            audioSource.PlayOneShot(buttonClickSound);
        }
    }
}