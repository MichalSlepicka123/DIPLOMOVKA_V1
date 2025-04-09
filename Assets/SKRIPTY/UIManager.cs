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

    private string correctAnswer;

    void Awake()
    {
        Instance = this;
        questionPanel.SetActive(false);
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
        Debug.Log("Uk� sk�re alebo prechod na �al�� level");
        // M��e� tu zobrazi� panel s v�sledkom alebo prepn�� sc�nu
    }
}

