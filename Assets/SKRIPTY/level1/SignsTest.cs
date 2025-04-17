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
        public Transform spawnPoint;
        public Transform cameraPos;
        public GameObject[] signs;
    }
    public Sign[] allSigns;

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
            btn.onClick.RemoveListener(() => OnAnswareSelected(btn));
        }
    }
    void InitSigns()
    {
        foreach (Sign sign in allSigns)
        {
            int randNumber = Random.Range(0, sign.signs.Length);
            GameObject obj = Instantiate(sign.signs[randNumber], sign.spawnPoint.position, sign.spawnPoint.rotation, parent);
            Debug.Log("Instantiated GO: " + obj.name);
            spawnedSignList.Add(obj.GetComponent<SignPoint>());
        }
    }
    void MoveToTargetPos()
    {
        cam.MoveToTarget(GetCurrentCameraTarget(),GetCurrentSignTransform(), ShowQuestion);
    }
    Transform GetCurrentCameraTarget() 
    {
        Transform cameraPos = allSigns[currentStep].cameraPos;
        return cameraPos;
    }
    Transform GetCurrentSignTransform()
    {
        Transform cameraPos = spawnedSignList[currentStep].transform;
        return cameraPos;
    }
    void OnAnswareSelected(Button pressedBtn)
    {
        if (CheckAnswareValidity(pressedBtn))
        {
            GameManager.Instance.AddPoint();
        }
        UIManager.Instance.ResetButtonCount();
        currentStep++;
        if(currentStep < allSigns.Length)
        {
            MoveToTargetPos();
        }
        else
        {
            UIManager.Instance.ShowFinalScore();
        }
    }
    bool CheckAnswareValidity(Button pressedBtn)
    {
        if(pressedBtn == UIManager.Instance.correctButton)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    void ShowQuestion()
    {
        UIManager.Instance.ShowQuestion(spawnedSignList[currentStep].questionText);
        foreach (SignQuestion sign in spawnedSignList[currentStep].signQuestion)
        {
            UIManager.Instance.ShowOptions(sign.option,IsAnswareCorrect(sign));
        }
    }
    bool IsAnswareCorrect(SignQuestion isSignCorrect)
    {        
        if (isSignCorrect.isCorrect) 
            return true;
        else
            return false;
    }
}
