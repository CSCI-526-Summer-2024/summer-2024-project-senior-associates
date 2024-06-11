using UnityEngine;

public class ClickableRect : MonoBehaviour
{
    public LevelBox levelBox;
    public int levelNum;
    private readonly Color HoverColor = new(1f, 1f, 1f);
    private readonly float Opacity = 0.3f;
    private Color originalColor;
    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
    }

    private void OnMouseDown()
    {
        levelBox.EnterLevel(levelNum);
    }

    private void OnMouseEnter()
    {
        spriteRenderer.color = Color.Lerp(originalColor, HoverColor, Opacity);
    }

    private void OnMouseExit()
    {
        spriteRenderer.color = originalColor;
    }
}