using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickablePart : MonoBehaviour
{
    public Level2Manager manager;

    void OnMouseDown()
    {
        manager.OnCorrectClick();
        Destroy(this); // odstr�nime skript po kliknut�
    }
}

