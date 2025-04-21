using UnityEngine;
using UnityEngine.UI;

public class IntroPopup : MonoBehaviour
{
    public GameObject introPanel;
    public Button okButton;
    public Button[] menuButtons;

    void Start()
    {
        introPanel.SetActive(true);

        // Zablokuj interakcie
        foreach (Button btn in menuButtons)
            btn.interactable = false;

        okButton.onClick.AddListener(CloseIntro);
    }

    void CloseIntro()
    {
        introPanel.SetActive(false);

        // Aktivuj menu tlaèidlá
        foreach (Button btn in menuButtons)
            btn.interactable = true;
    }
}
