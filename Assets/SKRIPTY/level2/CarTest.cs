using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarTest : MonoBehaviour
{
    [System.Serializable]
    public struct Parts
    {
        public Transform cameraPos;
        public Transform cameraLookTarget;
        public Part[] questions;
    }
    public Parts[] allQuestion;

    GameObject currentQuestion;
    int currentPartIndex = 0;
    int currentQuestionIndexInPart = 0;

    CameraController cam;
    bool _isActive = false;
    public bool isActive {  get { return _isActive; } set { _isActive = value; } }
    private void Awake()
    {
        Camera mainCam = Camera.main;
        cam = mainCam.GetComponent<CameraController>();
    }
    private void Update()
    {
        if (!_isActive) return;
        if(Input.GetMouseButtonDown(0)) OnPartPressed();
    }
    public void StartTest()
    {
        UIManager.Instance.ToggleButtonVisibility(false);
        MoveToTargePos();
    }
    void MoveToTargePos()
    {
        cam.MoveToTarget(GetCurrentCameraTarget(), GetCurrentTargetTransform(), ShowQuestion, true);
    }
    Transform GetCurrentCameraTarget()
    {
        Transform cameraPos = allQuestion[currentPartIndex].cameraPos;
        return cameraPos;
    }
    Transform GetCurrentTargetTransform()
    {
        Transform cameraPos = allQuestion[currentPartIndex].cameraLookTarget;
        return cameraPos;
    }
    void OnPartPressed()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            if (hit.collider.TryGetComponent(out Part part))
            {
                if (GetCurrentGameObject() == hit.collider.gameObject)
                {
                    if (part.anim != null) part.PlayAnim(OnAnswareSelected);
                    GameManager.Instance.AddPoint();
                }
                else
                {
                    if (part.anim != null) GetCurrentGameObject().GetComponent<Part>().PlayAnim(OnAnswareSelected);
                }
                if (part.anim == null) OnAnswareSelected();
            }
        }
    }
    void OnAnswareSelected()
    {
        currentQuestionIndexInPart++;
        if(currentQuestionIndexInPart >= allQuestion[currentPartIndex].questions.Length)
        {
            currentPartIndex++;
            currentQuestionIndexInPart = 0;
            MoveToTargePos();
        }
        else
        {
            ShowQuestion();
        }
        if (currentPartIndex > allQuestion.Length)
        {
            GameDirector.Instance.onStepEnd?.Invoke();
        }
    }
    void ShowQuestion()
    {
        UIManager.Instance.ShowQuestion(GetCurrentQuestion());
    }
    string GetCurrentQuestion()
    {
        return allQuestion[currentPartIndex].questions[currentQuestionIndexInPart].question;
    }
    GameObject GetCurrentGameObject()
    {
        return allQuestion[currentPartIndex].questions[currentQuestionIndexInPart].gameObject;
    }
}
