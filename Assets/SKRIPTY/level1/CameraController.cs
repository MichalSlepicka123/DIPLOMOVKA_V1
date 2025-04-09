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
        yield return null; // Poèkaj na SignShuffler

        // Získaj náhodne zoradený zoznam znaèiek
        signList = new List<Transform>(SignShuffler.Instance.randomizedSigns);
        Debug.Log("Naèítaných znaèiek: " + signList.Count);

        MoveToNextSign();
    }

    void Update()
    {
        if (isMoving && currentTarget != null)
        {
            // Plynulý presun kamery
            transform.position = Vector3.Lerp(transform.position, targetPos, moveSpeed * Time.deltaTime);

            
            // ZMENENÉ – kamera sa pozerá o nieèo vyššie nad pivot znaèky
            Vector3 lookPoint = currentTarget.position + new Vector3(0, 1f, 0);
            transform.LookAt(lookPoint);
            

            // Ak sme dostatoène blízko, zobrazíme otázku
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
            Debug.Log("Hotovo! Test dokonèený.");
            UIManager.Instance.ShowFinalScore();
            return;
        }

        isMoving = true;

        // Vyber náhodnú znaèku zo zoznamu
        int randomIndex = Random.Range(0, signList.Count);
        currentTarget = signList[randomIndex];
        signList.RemoveAt(randomIndex);

        // Výpoèet cie¾ovej pozície kamery
        Vector3 pos = currentTarget.position;

        float verticalOffset = 1f;  //  Jemne nad zemou (takmer vo výške znaèky)
        float zOffset = 1.5f;         // Pred znaèkou (pozícia kamery)

        targetPos = new Vector3(pos.x, pos.y + verticalOffset, pos.z + zOffset);
    }
}
