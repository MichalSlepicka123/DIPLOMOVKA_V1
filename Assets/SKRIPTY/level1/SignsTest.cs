using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static SignPoint;

public class SignsTest : MonoBehaviour
{
    [SerializeField] Transform parent;

    [System.Serializable]
    public struct Sign
    {
        public Transform point; // Jeden objekt pre spawn aj kameru
        public GameObject[] signs;
        public Vector3 cameraOffset; // Individu·lny offset pre kameru
    }

    public Sign[] allSigns;
    public float lookAtYOffset = 1.5f; // V˝öka, na ktor˙ sa m· kamera pozeraù nad znaËku

    int currentStep = 0;

    List<SignPoint> spawnedSignList = new();

    CameraController cam;

    private void Awake()
    {
        Camera mainCam = Camera.main;
        cam = mainCam.GetComponent<CameraController>();
    }

    public void StartTest()
    {
        InitSigns();
        MoveToTargetPos();
    }

    private void OnEnable()
    {
        foreach (Button btn in UIManager.Instance.answerButtons)
        {
            btn.onClick.AddListener(() => OnAnswareSelected(btn));
        }
    }

    private void OnDisable()
    {
        foreach (Button btn in UIManager.Instance.answerButtons)
        {
            // btn.onClick.RemoveListener(() => OnAnswareSelected(btn)); // lambda ned· odobraù spr·vne
        }
    }

    void InitSigns()
    {
        GameObject previousPrefab = null;

        foreach (Sign sign in allSigns)
        {
            GameObject selectedPrefab = null;
            int tries = 0;

            do
            {
                int randNumber = Random.Range(0, sign.signs.Length);
                selectedPrefab = sign.signs[randNumber];
                tries++;
            }
            while (selectedPrefab == previousPrefab && tries < 10);

            GameObject obj = Instantiate(selectedPrefab, sign.point.position, sign.point.rotation, parent);
            Debug.Log("Instantiated GO: " + obj.name);

            previousPrefab = selectedPrefab;
            spawnedSignList.Add(obj.GetComponent<SignPoint>());
        }
    }

    void MoveToTargetPos()
    {
        Transform target = GetCurrentSignTransform();

        Vector3 camTargetPos = allSigns[currentStep].point.position + allSigns[currentStep].cameraOffset;
        Vector3 lookAtPoint = target.position + new Vector3(0f, lookAtYOffset, 0f);
        Quaternion camTargetRot = Quaternion.LookRotation(lookAtPoint - camTargetPos);

        cam.MoveToTarget(camTargetPos, camTargetRot, ShowQuestion);
    }

    Transform GetCurrentSignTransform()
    {
        return spawnedSignList[currentStep].transform;
    }

    void OnAnswareSelected(Button pressedBtn)
    {
        if (CheckAnswareValidity(pressedBtn))
        {
            GameManager.Instance.AddPoint();
        }
        UIManager.Instance.HideQuestionPanel();
        UIManager.Instance.ResetButtonCount();
        currentStep++;
        if (currentStep < allSigns.Length)
        {
            MoveToTargetPos();
        }
        else
        {
            GameDirector.Instance.onStepEnd?.Invoke();
            // UIManager.Instance.ShowFinalScore();
        }
    }

    bool CheckAnswareValidity(Button pressedBtn)
    {
        return pressedBtn == UIManager.Instance.correctButton;
    }

    void ShowQuestion()
    {
        UIManager.Instance.ShowQuestion(spawnedSignList[currentStep].questionText);
        foreach (SignQuestion sign in spawnedSignList[currentStep].signQuestion)
        {
            UIManager.Instance.ShowOptions(sign.option, IsAnswareCorrect(sign));
        }
    }

    bool IsAnswareCorrect(SignQuestion isSignCorrect)
    {
        return isSignCorrect.isCorrect;
    }
}
