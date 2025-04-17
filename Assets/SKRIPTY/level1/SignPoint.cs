using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignPoint : MonoBehaviour
{
    [TextArea] public string questionText = "Èo znamená táto znaèka?";
    [System.Serializable]
    public struct SignQuestion
    {
        public string option;
        public bool isCorrect;
    }
    public SignQuestion[] signQuestion;
}
