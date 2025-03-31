using UnityEngine;
using UnityEngine.UI;

public class SettingsButton : MonoBehaviour
{
    private Button button;
    
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }

    public void OnClick()
    {
        UIManager.Instance.ToggleVolumePanel();
    }
}