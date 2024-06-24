using System.Collections.Generic;
using UnityEngine;
using System.Linq;


// IngredientData contains all ingredients that will appear in the current level.
// We use this as the single source of truth.
// Chests need to refer to IngredientData and specify the index of their ingredient.
[CreateAssetMenu]
public class IngredientData : ScriptableObject
{
    public Ingredient[] allIngredients;
    public GameObject cupPrefab;
    public GameObject backgroundPrefab;

    private static readonly float CupTopItemXSpace = 0.3f;
    private static readonly Vector3 CupToBackgroundOffset = new(0f, 0.5f, -0.1f);
    private static readonly Vector3 CupToIngredientOffset = new(0f, 0.5f, -0.2f);

    public GameObject CreateSmoothieObj(List<Ingredient> ingredients, bool normalSizedCup = false)
    {
        var obj = new GameObject("Smoothie");
        var cup = Instantiate(cupPrefab);
        cup.transform.SetParent(obj.transform);
        cup.transform.localPosition = new(0f, 0f, 0f);
        if (normalSizedCup)
        {
            cup.transform.localScale = new(0.24f, 0.24f, 0f);
        }

        // add ingredients as children
        float offset = -(ingredients.Count - 1) * CupTopItemXSpace;
        foreach (var ingredient in ingredients)
        {
            var background = Instantiate(backgroundPrefab, CupToBackgroundOffset, Quaternion.identity);
            background.transform.SetParent(obj.transform);
            background.transform.localPosition = Util.ChangeX(CupToBackgroundOffset, offset);

            var newItem = Instantiate(ingredient.prefab, CupToIngredientOffset, Quaternion.identity);
            newItem.transform.SetParent(obj.transform);
            newItem.transform.localPosition = Util.ChangeX(CupToIngredientOffset, offset);

            offset += CupTopItemXSpace * 2;
        }
        return obj;
    }

    public GameObject CreateIngredientObj(Ingredient ingredient)
    {
        return Instantiate(ingredient.prefab);
    }

    public List<Ingredient> Solids
    {
        get
        {
            return allIngredients.Where(ingredient => ingredient.type == Ingredient.Type.Solid).ToList();
        }
    }

    public List<Ingredient> Liquids
    {
        get
        {
            return allIngredients.Where(ingredient => ingredient.type == Ingredient.Type.Liquid).ToList();
        }
    }

    public List<Ingredient> GetSmoothieIngredients(int numSolidIngredients)
    {
        Debug.Log($"liquids count: {allIngredients.Length}, {Util.ChooseRandom(Liquids)}");
        List<Ingredient> res = new() { Util.ChooseRandom(Liquids) };
        for (int i = 0; i < numSolidIngredients; i++)
        {
            res.Add(Util.ChooseRandom(Solids));
        }
        return res;
    }
}

// An ingredient is something that could be added to the smoothie machine.
[System.Serializable]
public class Ingredient
{
    public GameObject prefab;
    public enum Type { Liquid, Solid }
    public Type type;

    public static bool operator ==(Ingredient a, Ingredient b)
    {
        if (ReferenceEquals(a, b)) return true;
        if (a is null || b is null) return false;
        return a.prefab.name == b.prefab.name;
    }

    public static bool operator !=(Ingredient a, Ingredient b)
    {
        return !(a == b);
    }

    public override bool Equals(object obj)
    {
        return obj is Ingredient ingredient &&
               prefab.name == ingredient.prefab.name;
    }

    public override int GetHashCode()
    {
        return prefab.name.GetHashCode();
    }
}

// An item is something that the player can hold (ingredient or smoothie).
[System.Serializable]
public class Item
{
    public GameObject obj;
    public enum Type { Ingredient, Smoothie }
    public Type type;
    public List<Ingredient> ingredients;

    public static bool operator ==(Item a, Item b)
    {
        if (ReferenceEquals(a, b)) return true;
        if (a is null || b is null) return false;
        return a.type == b.type && a.ingredients.OrderBy(i => i.prefab.name).SequenceEqual(b.ingredients.OrderBy(i => i.prefab.name));
    }

    public static bool operator !=(Item a, Item b)
    {
        return !(a == b);
    }

    public override bool Equals(object obj)
    {
        return obj is Item item &&
               type == item.type &&
               ingredients.OrderBy(i => i.prefab.name).SequenceEqual(item.ingredients.OrderBy(i => i.prefab.name));
    }

    public override int GetHashCode()
    {
        int hashCode = -1969571246;
        hashCode = hashCode * -1521134295 + type.GetHashCode();
        foreach (var ingredient in ingredients.OrderBy(i => i.prefab.name))
        {
            hashCode = hashCode * -1521134295 + ingredient.GetHashCode();
        }
        return hashCode;
    }
}