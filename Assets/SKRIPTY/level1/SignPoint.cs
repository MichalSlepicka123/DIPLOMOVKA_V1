using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignPoint : MonoBehaviour
{
    [TextArea] public string questionText = "Èo znamená táto znaèka?";
    public string correctAnswer;
    public string[] allOptions = new string[3];
}
