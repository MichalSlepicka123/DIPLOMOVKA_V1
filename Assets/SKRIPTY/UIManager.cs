using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] GameObject questionPanel;
    [SerializeField] TMP_Text questionText;
    [SerializeField] Button[] _answerButtons;
    public Button[] answerButtons { get {  return _answerButtons; } }

    [SerializeField] GameObject resultPanel;
    [SerializeField] TMP_Text resultText;
    [SerializeField] Button tryAgainButton;
    [SerializeField] Button continueButton;

    int buttonCount = 0;

    Button _correctButton;
    public Button correctButton {  get { return _correctButton; } } 

    void Awake()
    {
        Instance = this;
        questionPanel.SetActive(false);
        resultPanel.SetActive(false);
    }

    //void Start()
    //{
    //    if (PlayerPrefs.GetInt("GoToLevel2", 0) == 1)
    //    {
    //        if (PlayerPrefs.GetInt("Level1Completed", 0) == 1)
    //        {
    //            PlayerPrefs.SetInt("GoToLevel2", 0);
    //            StartLevel2();
    //        }
    //        else
    //        {
    //            PlayerPrefs.SetInt("GoToLevel2", 0);
    //            SceneManager.LoadScene("MENU");
    //        }
    //    }
    //}
    public void ShowQuestion(string question) 
    {
        questionText.text = question;
        questionPanel.SetActive(true);
    }
    public void ShowOptions(string option, bool isCorrect)
    {
        if (buttonCount > answerButtons.Length) 
        {
            Debug.LogError("There are more answared then button instantiated");
            return;
        }
        answerButtons[buttonCount].GetComponentInChildren<TMP_Text>().text = option;
        if (isCorrect)
        {
            _correctButton = answerButtons[buttonCount];
        }

        buttonCount++;
    }
    public void ShowFinalScore()
    {
        int score = GameManager.Instance.score;
        resultPanel.SetActive(true);
        questionPanel.SetActive(false);
        resultText.text = $"ZÌskal si {score} z 20 bodov.";

        if (score >= 15)
        {
            continueButton.gameObject.SetActive(true);
            PlayerPrefs.SetInt("Level1Completed", 1);
            PlayerPrefs.Save();
        }
        else
        {
            continueButton.gameObject.SetActive(false);
        }
    }

    //public void StartLevel2()
    //{
    //    Debug.Log("Sp˙öùam Level 2");

    //    resultPanel.SetActive(false);
    //    questionPanel.SetActive(false);

    //    RectTransform rt = questionPanel.GetComponent<RectTransform>();
    //    if (rt != null)
    //    {
    //        rt.anchorMin = new Vector2(0.3f, 0.05f);
    //        rt.anchorMax = new Vector2(0.7f, 0.25f);
    //        rt.offsetMin = Vector2.zero;
    //        rt.offsetMax = Vector2.zero;
    //    }

    //    FindObjectOfType<Level2Manager>().StartLevel();
    //}

    public void ReloadLevel()
    {
        SceneManager.LoadScene("LEVEL1");
    }
    public void ResetButtonCount() => buttonCount = 0;
    public void HideQuestionPanel() => questionPanel.SetActive(false);
}
