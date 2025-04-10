using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float rotateSpeed = 5f;

    private List<Transform> signList;
    private Transform currentTarget;
    private Vector3 targetPos;

    Coroutine rotateCoroutine;

    IEnumerator Start()
    {
        yield return null; // Po�kaj na SignShuffler

        // Z�skaj n�hodne zoraden� zoznam zna�iek
        signList = new List<Transform>(SignShuffler.Instance.randomizedSigns);
        Debug.Log("Na��tan�ch zna�iek: " + signList.Count);

        MoveToNextSign();
    }
    IEnumerator RotateTowards(Vector3 targetDirection)
    {
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);

        while (Quaternion.Angle(transform.rotation, targetRotation) > 0.05f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
            yield return null;
        }

        transform.rotation = targetRotation;
    }
    IEnumerator MoveCameraToTarget()
    {
        Vector3 directionToTarget = (targetPos - transform.position).normalized;
        if (rotateCoroutine == null)
        {
            rotateCoroutine = StartCoroutine(RotateTowards(directionToTarget));
        }
        else
        { 
            StopCoroutine(rotateCoroutine);
            rotateCoroutine = StartCoroutine(RotateTowards(directionToTarget));
        }
        while (Vector3.Distance(transform.position, targetPos) > 0.05f)
        {
            transform.position = Vector3.Lerp(transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = targetPos;
        Vector3 directionToSign = ((currentTarget.position + Vector3.up) - transform.position).normalized;
        if (rotateCoroutine == null)
        {
            rotateCoroutine = StartCoroutine(RotateTowards(directionToSign));
        }
        else
        {
            StopCoroutine(rotateCoroutine);
            rotateCoroutine = StartCoroutine(RotateTowards(directionToSign));
        }
            // Show the question
        SignPoint sign = currentTarget.GetComponent<SignPoint>();
        if (sign != null)
        {
            UIManager.Instance.ShowQuestion(sign.questionText, sign.allOptions, sign.correctAnswer);
        }
    }
    public void OnAnswerGiven(bool isCorrect)
    {
        if (isCorrect)
        {
            GameManager.Instance.AddPoint();
        }

        MoveToNextSign();
    }

    void MoveToNextSign()
    {
        if (signList.Count == 0)
        {
            Debug.Log("Hotovo! Test dokon�en�.");
            UIManager.Instance.ShowFinalScore();
            return;
        }
        currentTarget = null;

        // Vyber n�hodn� zna�ku zo zoznamu
        int randomIndex = Random.Range(0, signList.Count);
        currentTarget = signList[randomIndex];
        signList.RemoveAt(randomIndex);

        // V�po�et cie�ovej poz�cie kamery
        Vector3 pos = currentTarget.position;

        float verticalOffset = 1f;  //  Jemne nad zemou (takmer vo v��ke zna�ky)
        float zOffset = 1.0f;         // Pred zna�kou (poz�cia kamery)

        targetPos = new Vector3(pos.x, pos.y + verticalOffset, pos.z + zOffset);

        StartCoroutine(MoveCameraToTarget());
    }
}
