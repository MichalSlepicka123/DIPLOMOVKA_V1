using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Part : MonoBehaviour
{
    public string question;
    public GameObject correctPart;

    public Animation anim;

    private void Start()
    {
        if(TryGetComponent(out Animation animation)) anim = animation;
    }
    public void PlayAnim(Action onAnimEnd = null)
    {
        if (anim == null) return;
        StartCoroutine(PlayAnimCoroutine(onAnimEnd));
    }
    IEnumerator PlayAnimCoroutine(Action onAnimEnd)
    {
        anim.Play();
        yield return new WaitUntil(() => !anim.isPlaying);
        onAnimEnd?.Invoke();
    }
}
