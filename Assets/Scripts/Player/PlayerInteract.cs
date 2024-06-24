using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerInteract : MonoBehaviour
{
    public bool disableDiscard = false;
    public GameObject cPrefab;
    public GameObject player;
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
    private GameObject indicator;

    void Awake()
    {
        playerEnergy = GetComponent<PlayerEnergy>();
        playerControl = GetComponent<PlayerControl>();
        indicator = CreateIndicator(player, new(-1f, 0f, 0f));
        indicator.SetActive(false);
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

        if (chest != null && CanTakeOutFromChest())
        {
            indicator.SetActive(true);
            if (Input.GetKeyDown(KeyCode.C))
            {
                indicator.SetActive(false);
                var item = chest.GetItem();
                if (item != null)
                {
                    PickUp(item);
                }
            }
        }
        else if (smoothieMachine != null)
        {

            if (CanTakeOutFromSmoothie())
            {
                if (smoothieMachine.GetProduct(true) != null)
                {
                    indicator.SetActive(true);
                }
                if (Input.GetKeyDown(KeyCode.C))
                {
                    var product = smoothieMachine.GetProduct(false);
                    if (product != null)
                    {
                        PickUp(product);
                        indicator.SetActive(false);
                    }
                }
            }
            else if (GetCurrentItem() != null)
            {
                indicator.SetActive(true);
                if (Input.GetKeyDown(KeyCode.C))
                {

                    if (smoothieMachine.AddIngredient(GetCurrentItem()))
                    {
                        DiscardOneItem();
                    }
                    if (GetCurrentItem() == null)
                    {
                        indicator.SetActive(false);
                    }
                }
            }

        }
        else if (manager != null)
        {
            if (manager.Submit(GetCurrentItem(), true))
            {
                indicator.SetActive(true);
            }
            if (Input.GetKeyDown(KeyCode.C))
            {
                if (GetCurrentItem() != null && manager.Submit(GetCurrentItem(), false))
                {
                    DiscardOneItem();
                    indicator.SetActive(false);
                }
            }
        }
        else if (isNearBed)
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                playerEnergy.ToggleSleeping();
            }
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            if (manager != null && playerEnergy.CanSchmooze())
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
            indicator.gameObject.SetActive(false);
        }
        else if (collision.gameObject.CompareTag("SmoothieMachine"))
        {
            smoothieMachine = null;
            indicator.gameObject.SetActive(false);
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

    private GameObject CreateIndicator(GameObject obj, Vector3 offset)
    {
        var indicator = Instantiate(cPrefab);
        indicator.GetComponent<FloatingAnim>().Init(obj, offset);
        return indicator;
    }
}
