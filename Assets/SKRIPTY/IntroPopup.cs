using UnityEngine;
using UnityEngine.UI;

public class IntroPopup : MonoBehaviour
{
    public GameObject introPanel;
    public Button okButton;
    public Button[] menuButtons;

    // Funguje len poèas bežiacej hry
    private static bool hasSeenIntroThisSession = false;

    void Start()
    {
        if (!hasSeenIntroThisSession)
        {
            introPanel.SetActive(true);
            foreach (Button btn in menuButtons)
                btn.interactable = false;

            okButton.onClick.AddListener(CloseIntro);
        }
        else
        {
            introPanel.SetActive(false);
            foreach (Button btn in menuButtons)
                btn.interactable = true;
        }
    }

    void CloseIntro()
    {
        hasSeenIntroThisSession = true;

        introPanel.SetActive(false);
        foreach (Button btn in menuButtons)
            btn.interactable = true;
    }
}
