using UnityEngine;
using TMPro;

public class GainedScoreText : MonoBehaviour
{
    private readonly float fadeDuration = 1f;
    private readonly Vector3 InitialOffset = new(0, 0, 0);
    private readonly Vector3 FinalOffset = new(0, 0.5f, 0);
    private TextMeshProUGUI textMesh;
    private Color originalColor;
    private float timer;

    void Awake()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
        originalColor = textMesh.color;
    }

    void OnEnable()
    {
        timer = 0f;
        transform.position += InitialOffset;
    }

    void Update()
    {
        timer += Time.deltaTime;
        transform.position += Time.deltaTime / fadeDuration * (FinalOffset - InitialOffset);

        float alpha = 1f;
        if (timer >= fadeDuration / 2)
        {
            float fadeTimer = (timer - fadeDuration / 2) / (fadeDuration / 2);
            alpha = Mathf.Lerp(1f, 0f, fadeTimer);

        }
        textMesh.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);

        if (timer >= fadeDuration)
        {
            Destroy(gameObject);
        }
    }

    public void SetText(string message)
    {
        textMesh.text = message;
    }
}
