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
        //  Debug výpis stavu progresu pri štarte
        Debug.Log("====== MENU DEBUG ======");
        Debug.Log("Level1Unlocked: " + PlayerPrefs.GetInt("Level1Unlocked", 0));
        Debug.Log("Level1Completed: " + PlayerPrefs.GetInt("Level1Completed", 0));
        Debug.Log("GoToLevel2: " + PlayerPrefs.GetInt("GoToLevel2", 0));
        Debug.Log("========================");

        //  Volite¾ne: reset na èistý stav (ODSTRÁÒ po teste!)
        // PlayerPrefs.DeleteAll();

        SetupLevelButtons();
    }

    public void SetupLevelButtons()
    {
        bool level1Unlocked = PlayerPrefs.GetInt("Level1Unlocked", 0) == 1;
        level1Button.interactable = level1Unlocked;
        SetAlpha(level1Button, level1Unlocked ? 1f : 0.4f);

        bool level2Unlocked = PlayerPrefs.GetInt("Level1Completed", 0) == 1;
        level2Button.interactable = level2Unlocked;
        SetAlpha(level2Button, level2Unlocked ? 1f : 0.4f);

        level3Button.interactable = false;
        SetAlpha(level3Button, 0.4f);
    }

    void SetAlpha(Button btn, float alpha)
    {
        ColorBlock cb = btn.colors;
        Color baseColor = cb.normalColor;
        cb.normalColor = new Color(baseColor.r, baseColor.g, baseColor.b, alpha);
        cb.highlightedColor = new Color(baseColor.r, baseColor.g, baseColor.b, alpha);
        cb.pressedColor = new Color(baseColor.r, baseColor.g, baseColor.b, alpha);
        cb.disabledColor = new Color(baseColor.r, baseColor.g, baseColor.b, alpha);
        btn.colors = cb;
    }

    public void ShowInfo()
    {
        infoPanel.SetActive(true);
    }

    public void HideInfo()
    {
        infoPanel.SetActive(false);
    }

    public void StartGame()
    {
        Debug.Log(" StartGame() zavolané – odomykám Level1");
        PlayerPrefs.SetInt("Level1Unlocked", 1);
        PlayerPrefs.SetInt("GoToLevel2", 0);
        SceneManager.LoadScene("LEVEL1");
    }

    public void StartLevel1()
    {
        Debug.Log(" Štartujem Level 1 manuálne");
        PlayerPrefs.SetInt("GoToLevel2", 0);
        SceneManager.LoadScene("LEVEL1");
    }

    public void StartLevel2()
    {
        Debug.Log(" Pokus o vstup do Level2 z MENU");
        PlayerPrefs.SetInt("GoToLevel2", 1);
        SceneManager.LoadScene("LEVEL1");
    }
}
