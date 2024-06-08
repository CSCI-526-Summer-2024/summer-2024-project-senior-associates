using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class SmoothieMachine : MonoBehaviour
{
    public const float TimeIncrementWhenNewItemAdded = 2f;
    public const float MachineTopItemXSpace = 0.2f;
    public readonly Vector3 MachineToItemOffset = new(0f, 1f, -0.1f);
    public IngredientData ingredientData;
    private List<Ingredient> ingredients = new();
    private List<GameObject> topItems = new();
    private float productCountdown = 0f;
    private float productCountdownMax;
    public GameObject progressBar;

    private void Update()
    {
        if (productCountdown > 0f)
        {
            Vector3 scale = progressBar.transform.localScale;
            scale.x = productCountdown / productCountdownMax;
            progressBar.transform.localScale = scale;
            progressBar.SetActive(true);
            productCountdown -= Time.deltaTime;
            if (productCountdown <= 0f)
            {
                ProduceProduct();
            }
        }
        else
        {
            productCountdown = 0f;
            progressBar.SetActive(false);
        }
    }

    public bool AddIngredient(string ingredientName)
    {
        Ingredient ingredient = ingredientData.LookUp(ingredientName);
        if (ingredient == null)
        {
            Debug.Log("Unrecognized ingredient: " + ingredientName);
            return false;
        }
        else
        {
            if (ingredient.type == Ingredient.Type.Liquid
                && ingredients.Any(info => info.type == Ingredient.Type.Liquid))
            {
                Debug.Log("Ignored because there is already a liquid base: " + ingredient.name);
                return false;
            }
            else if (ingredients.Any(info => info.name == ingredient.name))
            {
                Debug.Log("Duplicate ingredient: " + ingredient.name);
                return false;
            }
            else
            {
                Debug.Log("Added ingredient: " + ingredient.name);
                ingredients.Add(ingredient);
                AddTopItem(ingredient);
                UpdateProductCountdown();
                return true;
            }
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
        var newItem = Instantiate(ingredient.prefab, transform.position, Quaternion.identity);
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
        var smoothie = ingredientData.CreateSmoothie(ingredients);
        smoothie.transform.SetParent(transform);
        smoothie.transform.localPosition = MachineToItemOffset;
        topItems.Add(smoothie);
    }

    public GameObject PickUp()
    {
        if (topItems.Count > 0 && productCountdown <= 0f)
        {
            var res = topItems[0];
            topItems.Clear();
            ingredients.Clear();
            return res;
        }
        return null;
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
