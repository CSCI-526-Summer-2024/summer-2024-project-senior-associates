using System;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    private readonly Vector3 LeftItemOffsetWhenHoldingOne = new(0, 1.2f, 0);
    private readonly Vector3 LeftItemOffsetWhenHoldingTwo = new(-0.6f, 1.2f, 0);
    private readonly Vector3 RightItemOffset = new(0.6f, 1.2f, 0);
    private Item leftItem;
    private Item rightItem;
    private Chest chest;
    private SmoothieMachine smoothieMachine;
    private Manager manager;

    private GameObject bed;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            DiscardOneItem();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (chest != null && CanTakeOutFromChest())
            {
                PickUp(chest.GetItem()); // if near chest, pick up item from chest
            }
            else if (smoothieMachine != null)
            {
                if (CanTakeOutFromSmoothie())
                {
                    var product = smoothieMachine.GetProduct();
                    if (product != null)
                    {
                        PickUp(product);
                    }
                }
                else if (GetCurrentItem() != null)
                {
                    if (smoothieMachine.AddIngredient(GetCurrentItem()))
                    {
                        DiscardOneItem();
                    }
                }
            }
            else if (manager != null)
            {
                if (GetCurrentItem() != null && manager.Submit(GetCurrentItem()))
                {
                    DiscardOneItem();
                }
            }
            else if (bed != null)
            {
                GetComponent<PlayerEnergy>().Sleep();
            }
        }
    }

    private Item GetCurrentItem()
    {
        return rightItem ?? leftItem;
    }

    private bool CanTakeOutFromChest()
    {
        return rightItem == null;
    }

    private bool CanTakeOutFromSmoothie()
    {
        return rightItem == null && (GetCurrentItem() == null || GetCurrentItem().type != Item.Type.Ingredient);
    }

    private void DiscardOneItem()
    {
        if (rightItem != null)
        {
            Destroy(rightItem.obj);
            rightItem = null;
            leftItem.obj.transform.localPosition = LeftItemOffsetWhenHoldingOne;
        }
        else if (leftItem != null)
        {
            Destroy(leftItem.obj);
            leftItem = null;
        }
    }


    private void PickUp(Item item)
    {
        if (leftItem == null)
        {
            leftItem = item;
            leftItem.obj.transform.SetParent(transform);
            leftItem.obj.transform.localPosition = LeftItemOffsetWhenHoldingOne;
        }
        else if (rightItem == null)
        {
            rightItem = item;
            rightItem.obj.transform.SetParent(transform);
            rightItem.obj.transform.localPosition = RightItemOffset;
            leftItem.obj.transform.localPosition = LeftItemOffsetWhenHoldingTwo;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Chest"))
        {
            chest = collision.GetComponent<Chest>();
        }
        else if (collision.gameObject.CompareTag("SmoothieMachine"))
        {
            smoothieMachine = collision.GetComponent<SmoothieMachine>();
        }
        else if (collision.gameObject.CompareTag("Manager"))
        {
            manager = collision.GetComponent<Manager>();
        }
        else if (collision.gameObject.CompareTag("Bed"))
        {
            bed = GameObject.FindWithTag("Bed");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Chest"))
        {
            chest = null;
        }
        else if (collision.gameObject.CompareTag("SmoothieMachine"))
        {
            smoothieMachine = null;
        }
        else if (collision.gameObject.CompareTag("Manager"))
        {
            manager = null;
        }
        else if (collision.gameObject.CompareTag("Bed"))
        {
            bed = null;
        }
    }
}
