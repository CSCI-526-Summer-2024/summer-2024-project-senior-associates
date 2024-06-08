using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu]
public class IngredientData : ScriptableObject
{
    public Ingredient[] allIngredients;
    public GameObject cupPrefab;
    public GameObject backgroundPrefab;

    public static readonly float CupTopItemXSpace = 0.3f;
    public static readonly Vector3 CupToBackgroundOffset = new(0f, 0.5f, -0.1f);
    public static readonly Vector3 CupToIngredientOffset = new(0f, 0.5f, -0.2f);

    public Ingredient LookUp(string name)
    {
        foreach (Ingredient ingredient in allIngredients)
        {
            if (name.Contains(ingredient.name))
            {
                return ingredient;
            }
        }
        return null;
    }

    public GameObject CreateSmoothie(List<Ingredient> ingredients)
    {
        var smoothie = new GameObject("Smoothie");
        var cup = Instantiate(cupPrefab, new(0f, 0f, 0f), Quaternion.identity);
        cup.transform.SetParent(smoothie.transform);
        cup.transform.localPosition = new(0f, 0f, 0f);

        // add ingredients as children
        float offset = -(ingredients.Count - 1) * CupTopItemXSpace;
        foreach (var ingredient in ingredients)
        {
            var background = Instantiate(backgroundPrefab, CupToBackgroundOffset, Quaternion.identity);
            background.transform.SetParent(smoothie.transform);
            background.transform.localPosition = Util.ChangeX(CupToBackgroundOffset, offset);

            var newItem = Instantiate(ingredient.prefab, CupToIngredientOffset, Quaternion.identity);
            newItem.transform.SetParent(smoothie.transform);
            newItem.transform.localPosition = Util.ChangeX(CupToIngredientOffset, offset);

            offset += CupTopItemXSpace * 2;
        }

        return smoothie;
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
        List<Ingredient> res = new() { Util.ChooseRandom(Liquids) };
        for (int i = 0; i < numSolidIngredients; i++)
        {
            res.Add(Util.ChooseRandom(Solids));
        }
        return res;
    }
}

[System.Serializable]
public class Ingredient
{
    public string name;
    public GameObject prefab;
    public enum Type { Liquid, Solid }
    public Type type;
}

