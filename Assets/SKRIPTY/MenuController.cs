using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public GameObject infoPanel;

    public Button level1Button;
    public Button level2Button;
    public Button level3Button;

    void Start()
    {
        SetupLevelButtons();
    }

    public void SetupLevelButtons()
    {
        // Level 1 je vždy aktívny
        level1Button.interactable = true;

        // Level 2 sa odomkne až po splnení Level1
        if (PlayerPrefs.GetInt("Level1Completed", 0) == 1)
        {
            level2Button.interactable = true;
            SetAlpha(level2Button, 1f); // plne vidite¾ný
        }
        else
        {
            level2Button.interactable = false;
            SetAlpha(level2Button, 0.4f); // priesvitný
        }

        // Rovnaká logika pre Level 3
        if (PlayerPrefs.GetInt("Level2Completed", 0) == 1)
        {
            level3Button.interactable = true;
            SetAlpha(level3Button, 1f);
        }
        else
        {
            level3Button.interactable = false;
            SetAlpha(level3Button, 0.4f);
        }
    }

    void SetAlpha(Button btn, float alpha)
    {
        ColorBlock cb = btn.colors;
        cb.normalColor = new Color(cb.normalColor.r, cb.normalColor.g, cb.normalColor.b, alpha);
        btn.colors = cb;
    }

    public void StartLevel1()
    {
        SceneManager.LoadScene("LEVEL1");
    }

    public void StartLevel2()
    {
        SceneManager.LoadScene("LEVEL2");
    }

    public void StartLevel3()
    {
        SceneManager.LoadScene("LEVEL3");
    }

    public void ShowInfo()
    {
        infoPanel.SetActive(true);
    }

    public void HideInfo()
    {
        infoPanel.SetActive(false);
    }
}
