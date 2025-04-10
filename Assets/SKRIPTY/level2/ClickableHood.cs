using System.Collections;
using UnityEngine;

public class ClickableHood : MonoBehaviour
{
    public float openAngleX = 40f;
    public float rotateSpeed = 3f;

    public float zoomDistance = 1.5f;
    public float zoomSpeed = 2f;

    private bool isOpen = false;
    private bool isZoomed = false;

    private Vector3 originalCamPos;
    private Transform cam;

    void Start()
    {
        cam = Camera.main.transform;
    }

    void OnMouseDown()
    {
        StopAllCoroutines();
        StartCoroutine(RotateHood());

        if (!isZoomed)
        {
            originalCamPos = cam.position;
            Vector3 zoomTarget = cam.position + cam.forward * zoomDistance;
            StartCoroutine(MoveCameraTo(zoomTarget));
        }
        else
        {
            StartCoroutine(MoveCameraTo(originalCamPos));
        }

        isZoomed = !isZoomed;
        isOpen = !isOpen;
    }

    IEnumerator RotateHood()
    {
        Quaternion start = transform.localRotation;
        Quaternion end = isOpen ? Quaternion.Euler(0, 0, 0) : Quaternion.Euler(openAngleX, 0, 0);

        float t = 0;
        while (t < 1f)
        {
            t += Time.deltaTime * rotateSpeed;
            transform.localRotation = Quaternion.Slerp(start, end, t);
            yield return null;
        }

        transform.localRotation = end;
    }

    IEnumerator MoveCameraTo(Vector3 targetPos)
    {
        Vector3 start = cam.position;
        float t = 0;

        while (t < 1f)
        {
            t += Time.deltaTime * zoomSpeed;
            cam.position = Vector3.Lerp(start, targetPos, t);
            yield return null;
        }

        cam.position = targetPos;
    }
}
