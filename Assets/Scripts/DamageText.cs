using System.Collections;
using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    public TextMeshProUGUI text;
    public float floatUpSpeed = 50f;
    public float fadeDuration = 0.5f;
    private RectTransform rect;
    private CanvasGroup canvasGroup;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
        canvasGroup = gameObject.AddComponent<CanvasGroup>();
    }

    public void Show(int damage)
    {
        text.text = damage.ToString();
        StartCoroutine(FloatUp());
    }

    private IEnumerator FloatUp()
    {
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            rect.anchoredPosition += Vector2.up * floatUpSpeed * Time.deltaTime;
            canvasGroup.alpha = 1 - (elapsed / fadeDuration);
            yield return null;
        }
        Destroy(gameObject);
    }
}
