using UnityEngine;

public class Chest : MonoBehaviour
{
    public IngredientData ingredientData;
    public int ingredientIndex;
    private Ingredient ingredient;

    void Start()
    {
        ingredient = ingredientData.allIngredients[ingredientIndex];
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
}
