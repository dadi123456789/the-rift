using UnityEngine;

public class FadeInOnEnable : MonoBehaviour
{
    CanvasGroup group;
    void Awake() => group = GetComponent<CanvasGroup>();

    void OnEnable()
    {
        group.alpha = 0f;
        StartCoroutine(Fade());
    }

    System.Collections.IEnumerator Fade()
    {
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / 0.4f;
            group.alpha = Mathf.Clamp01(t);
            yield return null;
        }
    }
}