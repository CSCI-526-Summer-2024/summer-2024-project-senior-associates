using UnityEngine;

public class Chest : MonoBehaviour
{
    private bool disabled = false; public bool Disabled => disabled;
    public IngredientData ingredientData;
    public int ingredientIndex;
    private Ingredient ingredient;
    private readonly Color NormalColor = new(1f, 1f, 1f);
    private readonly Color DisabledColor = new(0.5f, 0.5f, 0.5f);
    private SpriteRenderer[] spriteRenderers;

    void Awake()
    {
        ingredient = ingredientData.allIngredients[ingredientIndex];
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
    }

    public Item GetItem()
    {
        return new Item
        {
            obj = Instantiate(ingredient.prefab),
            type = Item.Type.Ingredient,
            ingredients = new() { ingredient },
        };
    }

    public void Enable()
    {
        disabled = false;
        foreach (SpriteRenderer sr in spriteRenderers)
        {
            sr.color = NormalColor;
        }
    }

    public void Disable()
    {
        disabled = true;
        foreach (SpriteRenderer sr in spriteRenderers)
        {
            sr.color = DisabledColor;
        }
    }
}
