using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    private Vector3 LeftItemFirstOffset = new(0, 0.6f, 0);
    private Vector3 LeftItemSecondOffset = new(-0.6f, 0.6f, 0);
    private Vector3 RightItemOffset = new(0.6f, 0.6f, 0);
    private ChestContent chestContent;
    private GameObject leftItem;
    private GameObject rightItem;

    private MakeSmoothie makeSmoothie;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            RemoveOneItem();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (chestContent != null)
            {
                PickUpItem(chestContent.itemPrefab);
            }
            else if (makeSmoothie != null)
            {
                if (GetCurrentItem() == "")
                {
                    GameObject itemPrefab = makeSmoothie.PickUp();
                    if (itemPrefab != null)
                    {
                        PickUpItem(itemPrefab);
                    }
                }
                else
                {
                    if (makeSmoothie.AddIngredient(GetCurrentItem()))
                    {
                        RemoveOneItem();
                    }
                }
            }
        }
    }

    private string GetCurrentItem()
    {
        if (rightItem != null)
        {
            return rightItem.name;
        }
        if (leftItem != null)
        {
            return leftItem.name;
        }
        return "";
    }

    private void RemoveOneItem()
    {
        if (rightItem != null)
        {
            Destroy(rightItem);
            rightItem = null;
            leftItem.transform.position = transform.position + LeftItemFirstOffset;
        }
        else if (leftItem != null)
        {
            Destroy(leftItem);
            leftItem = null;
        }
    }


    private void PickUpItem(GameObject itemPrefab)
    {
        if (leftItem == null)
        {
            leftItem = Instantiate(itemPrefab, transform.position + LeftItemFirstOffset, Quaternion.identity);
            leftItem.transform.SetParent(transform);
        }
        else if (rightItem == null)
        {
            leftItem.transform.position = transform.position + LeftItemSecondOffset;
            rightItem = Instantiate(itemPrefab, transform.position + RightItemOffset, Quaternion.identity);
            rightItem.transform.SetParent(transform);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Chest"))
        {
            chestContent = collision.GetComponent<ChestContent>();
        }
        else if (collision.gameObject.CompareTag("SmoothieMachine"))
        {
            makeSmoothie = collision.GetComponent<MakeSmoothie>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Chest"))
        {
            chestContent = null;
        }
        else if (collision.gameObject.CompareTag("SmoothieMachine"))
        {
            makeSmoothie = null;
        }
    }
}
