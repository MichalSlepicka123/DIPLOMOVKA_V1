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

    public Transform cameraTarget;       // cie¾ pre interiér
    public Transform engineViewTarget;   // cie¾ pre motor
    public float cameraMoveSpeed = 2f;

    void Start() { }

    public void StartLevel()
    {
        Debug.Log("StartLevel() spustené");
        currentIndex = 0;

        //  Hneï teleportuj kameru na interiér
        if (cameraTarget != null)
        {
            Camera.main.transform.position = cameraTarget.position;
            Camera.main.transform.rotation = cameraTarget.rotation;
        }
        else
        {
            Debug.LogWarning("cameraTarget nie je nastavený!");
        }

        // Skry A/B/C tlaèidlá z Level1
        foreach (Transform child in questionPanel.transform)
        {
            if (child.name.StartsWith("AnswerButton"))
            {
                child.gameObject.SetActive(false);
            }
        }

        // Zmenši UI panel dolu
        RectTransform rt = questionPanel.GetComponent<RectTransform>();
        if (rt != null)
        {
            rt.anchorMin = new Vector2(0.3f, 0.05f);
            rt.anchorMax = new Vector2(0.7f, 0.25f);
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
            Debug.Log("Interiér dokonèený – presúvame sa k motoru!");
            questionPanel.SetActive(false);

            if (engineViewTarget != null)
            {
                StartCoroutine(MoveCameraTo(engineViewTarget));
            }

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

    IEnumerator MoveCameraTo(Transform target)
    {
        Transform cam = Camera.main.transform;

        while (Vector3.Distance(cam.position, target.position) > 0.05f || Quaternion.Angle(cam.rotation, target.rotation) > 0.5f)
        {
            cam.position = Vector3.Lerp(cam.position, target.position, cameraMoveSpeed * Time.deltaTime);
            cam.rotation = Quaternion.Slerp(cam.rotation, target.rotation, cameraMoveSpeed * Time.deltaTime);
            yield return null;
        }

        cam.position = target.position;
        cam.rotation = target.rotation;

        Debug.Log("Kamera presunutá a otoèená k motoru.");
    }
}
