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
            continueButton.gameObject.SetActive(true);  // Aktivuj tlaËidlo "PokraËovaù"
        }
        else
        {
            continueButton.gameObject.SetActive(false); // Skry ho ak skÛre nestaËÌ
        }
    }

    public void StartLevel2()
    {
        Debug.Log("Sp˙öùam Level 2");

        // Skry v˝sledkov˝ panel
        resultPanel.SetActive(false);

        // Spusti Level 2 aû teraz
        FindObjectOfType<Level2Manager>().StartLevel();
    }

    public void ReloadLevel()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Level1");
    }
}
