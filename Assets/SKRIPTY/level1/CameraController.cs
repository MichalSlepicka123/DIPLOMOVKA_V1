using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float rotateSpeed = 5f;

    Coroutine rotateCoroutine;

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
    public void MoveToTarget(Transform target,Transform lookTarget, Action onTargetReached = null , bool lookDirectlyToTarget = false)
    {
        StartCoroutine(MoveCameraToTarget(target, lookTarget, onTargetReached, lookDirectlyToTarget));
    }
    IEnumerator MoveCameraToTarget(Transform target, Transform lookTarget, Action onTargetReached = null, bool lookDirectlyToTarget = false)
    {
        Vector3 directionToTarget = (target.position - transform.position).normalized;

        if (rotateCoroutine != null)
            StopCoroutine(rotateCoroutine);

        rotateCoroutine = StartCoroutine(RotateTowards(directionToTarget));

        while (Vector3.Distance(transform.position, target.position) > 0.05f)
        {
            transform.position = Vector3.Lerp(transform.position, target.position, moveSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = target.position;
        Vector3 directionToSign = new();
        if (!lookDirectlyToTarget) directionToSign = ((lookTarget.position + Vector3.up) - transform.position).normalized;
        else directionToSign = (lookTarget.position - transform.position).normalized;


        if (rotateCoroutine != null)
            StopCoroutine(rotateCoroutine);

        rotateCoroutine = StartCoroutine(RotateTowards(directionToSign));

        onTargetReached?.Invoke();
    }
}
