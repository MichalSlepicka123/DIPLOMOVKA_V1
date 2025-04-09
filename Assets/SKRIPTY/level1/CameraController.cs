using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float moveSpeed = 2f;

    private List<Transform> signList;
    private Transform currentTarget;
    private Vector3 targetPos;
    private bool isMoving = false;

    IEnumerator Start()
    {
        yield return null; // Po�kaj na SignShuffler

        // Z�skaj n�hodne zoraden� zoznam zna�iek
        signList = new List<Transform>(SignShuffler.Instance.randomizedSigns);
        Debug.Log("Na��tan�ch zna�iek: " + signList.Count);

        MoveToNextSign();
    }

    void Update()
    {
        if (isMoving && currentTarget != null)
        {
            // Plynul� presun kamery
            transform.position = Vector3.Lerp(transform.position, targetPos, moveSpeed * Time.deltaTime);

            
            // ZMENEN� � kamera sa pozer� o nie�o vy��ie nad pivot zna�ky
            Vector3 lookPoint = currentTarget.position + new Vector3(0, 1f, 0);
            transform.LookAt(lookPoint);
            

            // Ak sme dostato�ne bl�zko, zobraz�me ot�zku
            if (Vector3.Distance(transform.position, targetPos) < 0.1f)
            {
                isMoving = false;

                SignPoint sign = currentTarget.GetComponent<SignPoint>();
                if (sign != null)
                {
                    UIManager.Instance.ShowQuestion(sign.questionText, sign.allOptions, sign.correctAnswer);
                }
            }
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

        isMoving = true;

        // Vyber n�hodn� zna�ku zo zoznamu
        int randomIndex = Random.Range(0, signList.Count);
        currentTarget = signList[randomIndex];
        signList.RemoveAt(randomIndex);

        // V�po�et cie�ovej poz�cie kamery
        Vector3 pos = currentTarget.position;

        float verticalOffset = 1f;  //  Jemne nad zemou (takmer vo v��ke zna�ky)
        float zOffset = 1.5f;         // Pred zna�kou (poz�cia kamery)

        targetPos = new Vector3(pos.x, pos.y + verticalOffset, pos.z + zOffset);
    }
}
