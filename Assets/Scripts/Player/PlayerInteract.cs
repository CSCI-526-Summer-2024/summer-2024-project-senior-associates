using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerInteract : MonoBehaviour
{
    public bool disableDiscard = false;
    private readonly Vector3 LeftItemOffsetWhenHoldingOne = new(0, 1.2f, 0);
    private readonly Vector3 LeftItemOffsetWhenHoldingTwo = new(-0.6f, 1.2f, 0);
    private readonly Vector3 RightItemOffset = new(0.6f, 1.2f, 0);
    private Item leftItem;
    private Item rightItem;
    private Chest chest;
    private SmoothieMachine smoothieMachine;
    private Manager manager;
    private bool isNearBed = false;
    private PlayerEnergy playerEnergy;
    private PlayerControl playerControl;
    private int levelNum;

    void Awake()
    {
        playerEnergy = GetComponent<PlayerEnergy>();
        playerControl = GetComponent<PlayerControl>();
        levelNum = Util.GetCurrentLevelNum();
    }

    void Update()
    {
        if (playerControl.disableAllAction)
        {
            return;
        }
        
        if (!disableDiscard && Input.GetKeyDown(KeyCode.Q))
        {
            DiscardOneItem();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (chest != null && CanTakeOutFromChest())
            {
                var item = chest.GetItem();
                if (item != null)
                {
                    PickUp(item);
                }
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
            else if (isNearBed)
            {
                playerEnergy.ToggleSleeping();
            }
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (levelNum >= 3 && manager != null && playerEnergy.CanSchmooze())
            {
                manager.Schmooze();
                playerEnergy.LoseEnergy();
            }
        }
    }

    public Item GetCurrentItem()
    {
        return rightItem ?? leftItem;
    }

    public List<Item> GetAllItems()
    {
        return new List<Item> { leftItem, rightItem }.Where(item => item != null).ToList();
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
            isNearBed = true;
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
            isNearBed = false;
        }
    }
}
