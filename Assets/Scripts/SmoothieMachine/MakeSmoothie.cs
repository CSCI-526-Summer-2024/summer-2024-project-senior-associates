using System.Collections.Generic;
using UnityEngine;

public class MakeSmoothie : MonoBehaviour
{
    public const float ProductCountdownDelta = 5.0f;
    private Vector3 TopItemOffset = new(0, -0.35f, -0.1f);
    public List<GameObject> productPrefabs; // 0 - c; 1 - s; 2 - cs
    private int product = -1;
    private GameObject topItem;
    private float productCountdown = 0f;
    private float productCountdownMax = 1f;
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
        }
        else
        {
            progressBar.SetActive(false);
        }
    }

    public bool AddIngredient(string ingredientName)
    {
        int ingredientType = GetIngredientType(ingredientName);
        if (ingredientType < 0)
        {
            return false;
        }

        if (product == -1)
        {
            productCountdownMax = ProductCountdownDelta;
            productCountdown = ProductCountdownDelta;
            SetTopItem(ingredientType);
            return true;
        }
        if ((product == 0 && ingredientType == 1) || (product == 1 && ingredientType == 0))
        {
            productCountdownMax = productCountdown <= 0f ? ProductCountdownDelta : ProductCountdownDelta * 2;
            productCountdown += ProductCountdownDelta;
            SetTopItem(2);
            return true;
        }
        return false;
    }

    public GameObject PickUp()
    {
        if (product >= 0 && productCountdown <= 0f)
        {
            GameObject prefab = productPrefabs[product];
            RemoveTopItem();
            return prefab;
        }
        return null;
    }

    private int GetIngredientType(string ingredientName)
    {
        if (ingredientName.ToLower().Contains("chocolate"))
        {
            return 0;
        }
        if (ingredientName.ToLower().Contains("strawberry"))
        {
            return 1;
        }
        return -1;
    }

    private void SetTopItem(int type)
    {
        RemoveTopItem();
        product = type;
        topItem = Instantiate(productPrefabs[type], transform.position + TopItemOffset, Quaternion.identity);
        topItem.transform.SetParent(transform);
    }

    private void RemoveTopItem()
    {
        if (topItem != null)
        {
            Destroy(topItem);
        }
        product = -1;
    }
}
