using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TranslateObject : MonoBehaviour
{
    [SerializeField] Vector2 translatePosition;
    Vector2 originalPosition;
    AnimationCurve easeInOut;
    bool translated = false;

    [SerializeField] float lerpDuration = .5f;

    private void Start()
    {
        originalPosition = transform.localPosition;
        easeInOut = AnimationCurve.EaseInOut(0, 0, 1, 1);
    }

    public void Translate()
    {
        StopAllCoroutines();
        StartCoroutine(LerpTranslate());
    }
    IEnumerator LerpTranslate()
    {
        float time = lerpDuration;
        float multiplier = 1 / lerpDuration;

        Vector2 start = translated ? originalPosition : transform.localPosition;
        Vector2 end = translated ? transform.localPosition : translatePosition;
        translated = !translated;

        while(time > 0)
        {
            time -= Time.deltaTime;
            float t = Mathf.Abs((translated ? 1 : 0) - time * multiplier);
            transform.localPosition = Vector2.Lerp(start, end, easeInOut.Evaluate(t));
            yield return null;
        }
    }
}
