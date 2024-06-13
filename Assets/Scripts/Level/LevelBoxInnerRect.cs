using UnityEngine;

public class LevelBoxInnerRect : MonoBehaviour
{
    public int levelNum;
    private bool disabled;
    private readonly Color HoverColor = new(1f, 1f, 1f);
    private readonly Color DisabledColor = new(0f, 0f, 0f);
    private readonly float HoverOpacity = 0.3f;
    private readonly float DisabledOpacity = 0.7f;
    private Color originalColor;
    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
    }

    public void Disable()
    {
        disabled = true;
        spriteRenderer.color = Color.Lerp(originalColor, DisabledColor, DisabledOpacity);
    }

    private void OnMouseDown()
    {
        if (!disabled)
        {
            Util.EnterLevel(levelNum);
        }
    }

    private void OnMouseEnter()
    {
        if (!disabled)
        {
            spriteRenderer.color = Color.Lerp(originalColor, HoverColor, HoverOpacity);
        }
    }

    private void OnMouseExit()
    {
        if (!disabled)
        {
            spriteRenderer.color = originalColor;
        }
    }
}