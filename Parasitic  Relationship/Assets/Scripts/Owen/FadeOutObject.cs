using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOutObject : MonoBehaviour
{
    const float fadeOutTime = 1.0f;
    public void FadeOut()
    {
        StartCoroutine(FadeOutCoroutine());
    }
    IEnumerator FadeOutCoroutine()
    {
        SpriteRenderer[] sprites = GetComponentsInChildren<SpriteRenderer>();
        Color[] originalColors = GetColors(sprites);
        Color[] alphaColors = GetColors(sprites, true);

        float time = fadeOutTime;
        float multiplier = 1 / fadeOutTime;
        while(time > 0)
        {
            time -= Time.deltaTime;
            for (int i = 0; i < sprites.Length; i++)
            {
                float t = 1 - (time * multiplier);
                sprites[i].color = Color.Lerp(originalColors[i], alphaColors[i], t);
            }

            yield return null;
        }

        DisableGameObject();
    }

    Color[] GetColors(SpriteRenderer[] sprites, bool alpha = false)
    {
        Color[] colors = new Color[sprites.Length];

        for(int i = 0; i < sprites.Length; i++)
        {
            colors[i] = new Color(sprites[i].color.r, sprites[i].color.g, sprites[i].color.b, alpha ? 0 : sprites[i].color.a);
        }

        return colors;
    }

    void DisableGameObject()
    {
        gameObject.SetActive(false);
    }
}
