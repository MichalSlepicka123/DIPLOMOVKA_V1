using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickAndFall : MonoBehaviour
{
    private Rigidbody rb;
    private Vector3 startPosition;
    private Quaternion startRotation;
    private bool falling = false;

    public float resetY = -5f; // výška pod ktorú keï padne, resetne sa

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        startPosition = transform.position;
        startRotation = transform.rotation;

        rb.useGravity = false;
        rb.isKinematic = true;
    }

    void OnMouseDown()
    {
        rb.useGravity = true;
        rb.isKinematic = false;
        falling = true;
    }

    void Update()
    {
        if (falling && transform.position.y < resetY)
        {
            ResetCar();
        }
    }

    void ResetCar()
    {
        rb.useGravity = false;
        rb.isKinematic = true;
        transform.position = startPosition;
        transform.rotation = startRotation;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        falling = false;
    }
}