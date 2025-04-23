using UnityEngine;
using UnityEngine.Events;

public enum Steps
{
    TalkWithOfficer,
    SignTest,
    CarTest,
    Level3Test
}
public class GameDirector : MonoBehaviour
{
    public static GameDirector Instance;

    [SerializeField] SignsTest signsTest;
    [SerializeField] CarTest carTest;

    Steps _step;

    [HideInInspector] public UnityEvent onStepEnd;
    private void Awake()
    {
        Instance = this;
    }
    private void OnEnable()
    {
        onStepEnd.AddListener(EndStep);
    }
    private void Start()
    {
        StartStep(Steps.SignTest);
    }
    public void StartStep(Steps step)
    {
        switch(step)
        {
            case Steps.TalkWithOfficer:
                _step = step;

                break;
            case Steps.SignTest:
                _step = step;
                signsTest.StartTest();
                break;
            case Steps.CarTest:
                _step = step;
                carTest.isActive = true;
                carTest.StartTest();
                break;
            case Steps.Level3Test:
                _step = step;
                break;
        }
    }
    void EndStep()
    {
        switch (_step)
        {
            case Steps.TalkWithOfficer:
                StartStep(Steps.SignTest);
                break;
            case Steps.SignTest:
                StartStep(Steps.CarTest);
                break;
            case Steps.CarTest:
                StartStep(Steps.Level3Test);
                break;
            case Steps.Level3Test:
                UIManager.Instance.ShowFinalScore();
                break;
        }
    }
}
