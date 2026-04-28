using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HitFlash : MonoBehaviour
{
    public static HitFlash instance;

    private Image img;

    void Awake()
    {
        instance = this;
        img = GetComponent<Image>();
    }

    public void Flash()
    {
        StopAllCoroutines();
        StartCoroutine(FlashRoutine());
    }

    IEnumerator FlashRoutine()
    {
        Color c = img.color;

        // flash in
        c.a = 0.5f;
        img.color = c;

        yield return new WaitForSeconds(0.1f);

        // fade out
        float t = 0f;
        while (t < 0.3f)
        {
            t += Time.deltaTime;
            c.a = Mathf.Lerp(0.5f, 0f, t / 0.3f);
            img.color = c;
            yield return null;
        }
    }
}