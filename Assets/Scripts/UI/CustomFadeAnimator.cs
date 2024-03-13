using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CustomFadeAnimator : MonoBehaviour
{
    private static float m_timerCurrent;
    private static AnimationCurve m_smoothCurve = new AnimationCurve(new Keyframe[] { new Keyframe(0f, 0f), new Keyframe(1f, 1f) });
    private static readonly WaitForSeconds m_skipFrame = new WaitForSeconds(0.01f);

    /// <summary>
    /// This function creates fade effect.
    /// </summary>
    /// <param name="image">The image used to create fade effect.</param>
    /// <param name="start">When the effect starts.</param>
    /// <param name="end">When the effect ends.</param>
    /// <param name="duration">The duration of the effect.</param>
    public static IEnumerator Fade(Image image, float start, float end, float duration)
    {
        m_timerCurrent = 0f;

        while (m_timerCurrent <= duration)
        {
            m_timerCurrent += Time.deltaTime;
            Color c = image.color;
            image.color = new Color(c.r, c.g, c.b, Mathf.Lerp(start, end, m_smoothCurve.Evaluate(m_timerCurrent / duration)));
            yield return m_skipFrame;
        }
    }
    public static IEnumerator Fade(TextMeshProUGUI text, float start, float end, float duration)
    {
        m_timerCurrent = 0f;

        while (m_timerCurrent <= duration)
        {
            m_timerCurrent += Time.deltaTime;
            Color c = text.color;
            text.color = new Color(c.r, c.g, c.b, Mathf.Lerp(start, end, m_smoothCurve.Evaluate(m_timerCurrent / duration)));
            yield return m_skipFrame;
        }
    }
}
