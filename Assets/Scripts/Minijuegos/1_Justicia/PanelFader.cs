using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelFader : MonoBehaviour
{
    private bool panelFaded = true;
    public float duration = 0.4f;
    public CanvasGroup canvGroup;

    public void Fade()
    {
        StartCoroutine(DoFade(canvGroup, canvGroup.alpha, panelFaded ? 1 : 0));
    }

    public IEnumerator DoFade(CanvasGroup canvGroup, float start, float end)
    {
        float counter = 0f;

        while (counter < duration)
        {
            counter += Time.deltaTime;
            canvGroup.alpha = Mathf.Lerp(start, end, counter / duration);

            yield return null;
        }
    }
}
