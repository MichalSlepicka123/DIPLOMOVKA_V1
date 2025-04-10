using System.Collections;
using UnityEngine;

public class ClickableDoor : MonoBehaviour
{
    public float doorOpenAngle = 60f;
    public float rotateSpeed = 3f;

    public float zoomDistance = 1.2f;
    public float zoomSpeed = 2f;

    private bool isOpen = false;

    private Vector3 originalCamPos;
    private bool isZoomed = false;

    private Transform cameraTransform;

    void Start()
    {
        cameraTransform = Camera.main.transform;
    }

    void OnMouseDown()
    {
        StopAllCoroutines();
        StartCoroutine(RotateDoor());

        if (!isZoomed)
        {
            originalCamPos = cameraTransform.position;
            Vector3 zoomTarget = cameraTransform.position + cameraTransform.forward * zoomDistance;
            StartCoroutine(MoveCameraTo(zoomTarget));
        }
        else
        {
            StartCoroutine(MoveCameraTo(originalCamPos));
        }

        isZoomed = !isZoomed;
        isOpen = !isOpen;
    }

    IEnumerator RotateDoor()
    {
        Quaternion startRot = transform.localRotation;
        Quaternion endRot = isOpen ? Quaternion.Euler(0, 0, 0) : Quaternion.Euler(0, doorOpenAngle, 0);

        float t = 0;
        while (t < 1f)
        {
            t += Time.deltaTime * rotateSpeed;
            transform.localRotation = Quaternion.Slerp(startRot, endRot, t);
            yield return null;
        }

        transform.localRotation = endRot;
    }

    IEnumerator MoveCameraTo(Vector3 targetPos)
    {
        Vector3 start = cameraTransform.position;
        float t = 0;

        while (t < 1f)
        {
            t += Time.deltaTime * zoomSpeed;
            cameraTransform.position = Vector3.Lerp(start, targetPos, t);
            yield return null;
        }

        cameraTransform.position = targetPos;
    }
}
