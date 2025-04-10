using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public GameObject questionPanel;
    public TMP_Text questionText;
    public Button[] answerButtons;

    public GameObject resultPanel;
    public TMP_Text resultText;
    public Button tryAgainButton;
    public Button continueButton;

    private string correctAnswer;

    void Awake()
    {
        Instance = this;
        questionPanel.SetActive(false);
        resultPanel.SetActive(false);
    }

    public void ShowQuestion(string question, string[] options, string correct)
    {
        questionText.text = question;
        correctAnswer = correct;

        for (int i = 0; i < answerButtons.Length; i++)
        {
            TMP_Text btnText = answerButtons[i].GetComponentInChildren<TMP_Text>();
            btnText.text = options[i];

            string selected = options[i];
            answerButtons[i].onClick.RemoveAllListeners();
            answerButtons[i].onClick.AddListener(() => OnAnswerSelected(selected));
        }

        questionPanel.SetActive(true);
    }

    public void OnAnswerSelected(string selected)
    {
        questionPanel.SetActive(false);
        bool isCorrect = selected == correctAnswer;
        Camera.main.GetComponent<CameraController>().OnAnswerGiven(isCorrect);
    }

    public void ShowFinalScore()
    {
        int score = GameManager.Instance.score;
        resultPanel.SetActive(true);
        resultText.text = $"ZÌskal si {score} z 20 bodov.";

        if (score >= 15)
        {
            continueButton.gameObject.SetActive(true);
        }
        else
        {
            continueButton.gameObject.SetActive(false);
        }
    }

    public void StartLevel2()
    {
        Debug.Log("Sp˙öùam Level 2");

        //  Uloûenie progresu - dokonËen˝ Level1
        PlayerPrefs.SetInt("Level1Completed", 1);
        PlayerPrefs.Save();

        resultPanel.SetActive(false);
        questionPanel.SetActive(false);

        // Zmenöi panel pre Level 2
        RectTransform rt = questionPanel.GetComponent<RectTransform>();
        if (rt != null)
        {
            rt.anchorMin = new Vector2(0.3f, 0.05f);
            rt.anchorMax = new Vector2(0.7f, 0.25f);
            rt.offsetMin = Vector2.zero;
            rt.offsetMax = Vector2.zero;
        }

        // Spusti interiÈrov˝ level (Level 2)
        FindObjectOfType<Level2Manager>().StartLevel();
    }

    public void ReloadLevel()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Level1");
    }
}
