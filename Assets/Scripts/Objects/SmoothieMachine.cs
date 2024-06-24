using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class SmoothieMachine : MonoBehaviour
{
    public IngredientData ingredientData;
    public GameObject progressBar;
    public bool allowDoubleSmoothie = true;
    private readonly float TimeIncrementWhenNewItemAdded = 3f;
    private readonly float MachineTopItemXSpace = 0.2f;
    private readonly Vector3 MachineToItemOffset = new(0f, 1f, -0.1f);
    private readonly List<Ingredient> ingredients = new();
    private readonly List<GameObject> topItems = new();
    private float productCountdown = 0f; public float ProductCountdown => productCountdown;
    private float productCountdownMax;
    private readonly Color NormalColor = new(1f, 1f, 1f);
    private readonly Color DisabledColor = new(0.5f, 0.5f, 0.5f);
    private bool disabled = false;
    private SpriteRenderer[] spriteRenderers;

    void Awake()
    {
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
    }

    void Update()
    {
        if (!disabled && productCountdown > 0f)
        {
            Vector3 scale = progressBar.transform.localScale;
            scale.x = productCountdown / productCountdownMax;
            progressBar.transform.localScale = scale;
            progressBar.SetActive(true);
            productCountdown -= Time.deltaTime;
            if (productCountdown <= 0f)
            {
                ProduceProduct();
                productCountdownMax = 0f;
            }
        }
        else
        {
            productCountdown = 0f;
            progressBar.SetActive(false);
        }
    }

    public bool AddIngredient(Item item)
    {
        if (disabled || item == null || item.type != Item.Type.Ingredient)
        {
            return false;
        }

        var ingredient = item.ingredients[0];
        if (ingredient.type == Ingredient.Type.Liquid
            && ingredients.Any(info => info.type == Ingredient.Type.Liquid))
        {
            Debug.Log("Ignored because there is already a liquid base: " + ingredient.prefab.name);
            return false;
        }
        else if (!allowDoubleSmoothie && ingredients.Count >= 2)
        {
            Debug.Log("Ignored because double smoothie is not allowed: " + ingredient.prefab.name);
            return false;
        }
        else
        {
            ingredients.Add(ingredient);
            AddTopItem(ingredient);
            if (ingredients.Count > 1)
            {
                UpdateProductCountdown();
            }
            return true;
        }
    }

    public bool HasProduct()
    {
        return !disabled && topItems.Count > 0 && productCountdown <= 0f && ingredients.Count > 1;
    }

    public Item GetProduct()
    {
        if (HasProduct())
        {
            var item = new Item
            {
                obj = topItems[0],
                type = Item.Type.Smoothie,
                ingredients = new List<Ingredient>(ingredients)
            };
            topItems.Clear();
            ingredients.Clear();
            return item;
        }
        return null;
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

    private void AddTopItem(Ingredient ingredient)
    {
        float offset = -topItems.Count * MachineTopItemXSpace;
        foreach (var item in topItems)
        {
            item.transform.localPosition = Util.ChangeX(item.transform.localPosition, offset);
            offset += MachineTopItemXSpace * 2;
        }
        var newItem = Instantiate(ingredient.prefab);
        newItem.transform.SetParent(transform);
        newItem.transform.localPosition = new(offset, MachineToItemOffset.y, MachineToItemOffset.z);
        topItems.Add(newItem);
    }

    private void UpdateProductCountdown()
    {
        productCountdown += TimeIncrementWhenNewItemAdded;
        productCountdownMax += TimeIncrementWhenNewItemAdded;
    }

    private void ProduceProduct()
    {
        ClearTopItems();
        var smoothie = ingredientData.CreateSmoothieObj(ingredients, true);
        smoothie.transform.SetParent(transform);
        smoothie.transform.localPosition = MachineToItemOffset;
        topItems.Add(smoothie);
    }

    private void ClearTopItems()
    {
        foreach (var item in topItems)
        {
            Destroy(item);
        }
        topItems.Clear();
    }
}
