using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float rotateSpeed = 5f;

    // Použijeme pôvodné znaèky
    private Queue<Transform> orderedSigns = new Queue<Transform>();
    private Transform currentTarget;
    private Vector3 targetPos;

    Coroutine rotateCoroutine;
    private bool skipLevel1 = false;

    IEnumerator Start()
    {
        if (PlayerPrefs.GetInt("GoToLevel2", 0) == 1)
        {
            skipLevel1 = true;
            yield break;
        }

        yield return null;

        // Namiesto groupedSigns použijeme randomizedSigns
        orderedSigns.Clear();
        foreach (var sign in SignShuffler.Instance.randomizedSigns)
        {
            orderedSigns.Enqueue(sign);
        }

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

        if (rotateCoroutine != null)
            StopCoroutine(rotateCoroutine);

        rotateCoroutine = StartCoroutine(RotateTowards(directionToTarget));

        while (Vector3.Distance(transform.position, targetPos) > 0.05f)
        {
            transform.position = Vector3.Lerp(transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = targetPos;

        Vector3 directionToSign = ((currentTarget.position + Vector3.up) - transform.position).normalized;

        if (rotateCoroutine != null)
            StopCoroutine(rotateCoroutine);

        rotateCoroutine = StartCoroutine(RotateTowards(directionToSign));

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
        if (skipLevel1) return;

        if (orderedSigns.Count == 0)
        {
            Debug.Log("Hotovo! Test dokonèený.");
            UIManager.Instance.ShowFinalScore();
            return;
        }

        currentTarget = orderedSigns.Dequeue();

        float verticalOffset = 1f;
        float zOffset = 1.0f;

        Vector3 pos = currentTarget.position;
        targetPos = new Vector3(pos.x, pos.y + verticalOffset, pos.z + zOffset);

        StartCoroutine(MoveCameraToTarget());
    }
}
