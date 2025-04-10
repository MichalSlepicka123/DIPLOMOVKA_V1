using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Level2Manager : MonoBehaviour
{
    public List<Level2Question> interiorQuestions = new List<Level2Question>();

    private int currentIndex = 0;

    public GameObject questionPanel;
    public TMP_Text questionText;

    public Transform cameraTarget; // cie�ov� bod, kde sa kamera "zjav�"

    void Start() { }

    public void StartLevel()
    {
        Debug.Log("StartLevel() spusten�");
        currentIndex = 0;

        // Presko� kameru nad auto
        if (cameraTarget != null)
        {
            Camera.main.transform.position = cameraTarget.position;
            Camera.main.transform.rotation = cameraTarget.rotation;
        }
        else
        {
            Debug.LogWarning("cameraTarget nie je nastaven� v Level2Manager!");
        }

        questionPanel.SetActive(true);
        ShowNextQuestion();
    }

    void ShowNextQuestion()
    {
        if (currentIndex >= interiorQuestions.Count)
        {
            Debug.Log("Interi�r dokon�en� � pokra�ujeme na motor!");
            questionPanel.SetActive(false);
            return;
        }

        var current = interiorQuestions[currentIndex];
        questionText.text = current.questionText;
        EnableClickable(current.correctObjects);
    }

    void EnableClickable(GameObject[] objs)
    {
        foreach (var obj in objs)
        {
            var click = obj.AddComponent<ClickablePart>();
            click.manager = this;
        }
    }

    public void OnCorrectClick()
    {
        Debug.Log("Spr�vne kliknut�.");
        GameManager.Instance.AddPoint();

        currentIndex++;
        ShowNextQuestion();
    }
}
