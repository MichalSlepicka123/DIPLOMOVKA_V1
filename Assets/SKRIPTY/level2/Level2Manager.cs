using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Level2Manager : MonoBehaviour
{
    public List<Level2Question> interiorQuestions = new List<Level2Question>();

    private int currentIndex = 0;

    public GameObject questionPanel;
    public TMP_Text questionText;

    public Transform cameraTarget;   // poz�cia pre interi�r
    public Transform cameraTarget2;  // poz�cia pre motor

    public float cameraMoveSpeed = 2f; // r�chlos� kamery pri prechode

    void Start() { }

    public void StartLevel()
    {
        Debug.Log("StartLevel() spusten�");
        currentIndex = 0;

        // R�chle nastavenie kamery do interi�ru
        if (cameraTarget != null)
        {
            Camera.main.transform.position = cameraTarget.position;
            Camera.main.transform.rotation = cameraTarget.rotation;
        }

        // Skry A/B/C tla�idl� z Level 1
        foreach (Transform child in questionPanel.transform)
        {
            if (child.name.StartsWith("AnswerButton"))
            {
                child.gameObject.SetActive(false);
            }
        }

        // Panel � dlh�� a �t�hlej��
        RectTransform rt = questionPanel.GetComponent<RectTransform>();
        if (rt != null)
        {
            rt.anchorMin = new Vector2(0.2f, 0.05f);
            rt.anchorMax = new Vector2(0.8f, 0.15f);
            rt.offsetMin = Vector2.zero;
            rt.offsetMax = Vector2.zero;
        }

        questionPanel.SetActive(true);
        ShowNextQuestion();
    }

    void ShowNextQuestion()
    {
        if (currentIndex >= interiorQuestions.Count)
        {
            Debug.Log("Interi�r dokon�en� � prechod na motor.");
            questionPanel.SetActive(false);

            if (cameraTarget2 != null)
            {
                StartCoroutine(MoveCameraToTarget(cameraTarget2));
            }

            return;
        }

        var current = interiorQuestions[currentIndex];
        questionText.text = current.questionText;
        EnableClickable(current.correctObjects);
    }

    IEnumerator MoveCameraToTarget(Transform target)
    {
        Transform cam = Camera.main.transform;

        while (Vector3.Distance(cam.position, target.position) > 0.05f ||
               Quaternion.Angle(cam.rotation, target.rotation) > 0.5f)
        {
            cam.position = Vector3.Lerp(cam.position, target.position, cameraMoveSpeed * Time.deltaTime);
            cam.rotation = Quaternion.Slerp(cam.rotation, target.rotation, cameraMoveSpeed * Time.deltaTime);
            yield return null;
        }

        cam.position = target.position;
        cam.rotation = target.rotation;

        Debug.Log("Kamera �spe�ne preletela na motor.");
        // Tu m��e� prida�: Spusti ot�zky na motor...
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
