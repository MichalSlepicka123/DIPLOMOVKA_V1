using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Level2Manager : MonoBehaviour
{
    public List<Level2Question> interiorQuestions = new List<Level2Question>();

    private int currentIndex = 0;

    public GameObject questionPanel;
    public TMP_Text questionText;

    public Transform cameraTarget; // cie¾ový bod, kde sa kamera "zjaví"

    void Start() { }

    public void StartLevel()
    {
        Debug.Log("StartLevel() spustené");
        currentIndex = 0;

        // Preskoè kameru nad auto
        if (cameraTarget != null)
        {
            Camera.main.transform.position = cameraTarget.position;
            Camera.main.transform.rotation = cameraTarget.rotation;
        }
        else
        {
            Debug.LogWarning("cameraTarget nie je nastavený v Level2Manager!");
        }

        questionPanel.SetActive(true);
        ShowNextQuestion();
    }

    void ShowNextQuestion()
    {
        if (currentIndex >= interiorQuestions.Count)
        {
            Debug.Log("Interiér dokonèený – pokraèujeme na motor!");
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
        Debug.Log("Správne kliknuté.");
        GameManager.Instance.AddPoint();

        currentIndex++;
        ShowNextQuestion();
    }
}
