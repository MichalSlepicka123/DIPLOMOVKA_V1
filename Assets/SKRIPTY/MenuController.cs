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
        // LEVEL 1 – sprístupni, ak už bol aspoò raz spustený
        if (PlayerPrefs.GetInt("Level1Started", 0) == 1)
        {
            level1Button.interactable = true;
            SetAlpha(level1Button, 1f);
        }
        else
        {
            level1Button.interactable = false;
            SetAlpha(level1Button, 0.4f);
        }

        // LEVEL 2 – len po úspešnom dokonèení Level 1
        if (PlayerPrefs.GetInt("Level1Completed", 0) == 1)
        {
            level2Button.interactable = true;
            SetAlpha(level2Button, 1f);
        }
        else
        {
            level2Button.interactable = false;
            SetAlpha(level2Button, 0.4f);
        }

        // LEVEL 3 – len po úspešnom dokonèení Level 2
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
        Color baseColor = cb.normalColor;
        baseColor.a = alpha;

        cb.normalColor = baseColor;
        cb.highlightedColor = baseColor;
        cb.pressedColor = baseColor;
        cb.selectedColor = baseColor;
        cb.disabledColor = new Color(baseColor.r, baseColor.g, baseColor.b, 0.2f);

        btn.colors = cb;
    }

    // Tlaèidlo ŠTART – spustí hneï LEVEL1 a zároveò uloží stav
    public void OnStartButtonPressed()
    {
        PlayerPrefs.SetInt("Level1Started", 1);
        PlayerPrefs.Save();
        SceneManager.LoadScene("LEVEL1");
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
