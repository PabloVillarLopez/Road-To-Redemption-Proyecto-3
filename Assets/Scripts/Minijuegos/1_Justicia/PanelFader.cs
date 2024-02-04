using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelFader : MonoBehaviour
{
    #region Panel Fader Variables

    private bool panelFaded = true;
    [Header("Fade Variables")]
    public float duration = 0.4f;
    public CanvasGroup canvGroup;

    #endregion Panel Fader Variables

    #region Fade Effect
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

    #endregion Fade Effect
}
