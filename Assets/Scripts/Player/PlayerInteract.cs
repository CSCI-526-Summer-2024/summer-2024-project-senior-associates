using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerInteract : MonoBehaviour
{
    public bool disableDiscard = false;
    public GameObject cPrefab;
    public GameObject activeItemBorderPrefab;
    private readonly Vector3 LeftItemOffsetWhenHoldingOne = new(0, 1.2f, 0);
    private readonly Vector3 LeftItemOffsetWhenHoldingTwo = new(-0.6f, 1.2f, 0);
    private readonly Vector3 RightItemOffset = new(0.6f, 1.2f, 0);
    private bool rightActive = true;
    private Item leftItem;
    private Item rightItem;
    private Chest chest;
    private SmoothieMachine smoothieMachine;
    private Manager manager;
    private bool isNearBed = false;
    private PlayerEnergy playerEnergy;
    private PlayerControl playerControl;
    private GameObject cKeyHint;

    void Awake()
    {
        playerEnergy = GetComponent<PlayerEnergy>();
        playerControl = GetComponent<PlayerControl>();
        if (Util.GetCurrentLevelNum() == 1)
        {
            cKeyHint = CreateCKeyHint(gameObject, new(1.45f, 1.8f, 0f));
            HideCHint();
        }
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
            ShowCHint();
            if (Input.GetKeyDown(KeyCode.C))
            {
                var item = chest.GetItem();
                if (item != null)
                {
                    PickUp(item);
                    HideCHint();
                }
            }
        }
        else if (smoothieMachine != null)
        {
            if (CanTakeOutFromSmoothie())
            {
                if (smoothieMachine.HasProduct())
                {
                    ShowCHint();
                }
                if (Input.GetKeyDown(KeyCode.C))
                {
                    var product = smoothieMachine.GetProduct();
                    if (product != null)
                    {
                        PickUp(product);
                        HideCHint();
                    }
                }
            }
            else if (GetCurrentItem() != null)
            {
                ShowCHint();
                if (Input.GetKeyDown(KeyCode.C))
                {
                    if (smoothieMachine.AddIngredient(GetCurrentItem()))
                    {
                        DiscardOneItem();
                        if (GetCurrentItem() == null)
                        {
                            HideCHint();
                        }
                    }
                }
            }
        }
        else if (manager != null)
        {
            if (manager.Submit(GetCurrentItem(), true))
            {
                ShowCHint();
            }
            if (Input.GetKeyDown(KeyCode.C))
            {
                if (GetCurrentItem() != null && manager.Submit(GetCurrentItem(), false))
                {
                    DiscardOneItem();
                    HideCHint();
                }
            }
            else if (Input.GetKeyDown(KeyCode.T))
            {
                if (playerEnergy.CanSchmooze())
                {
                    manager.Schmooze();
                    playerEnergy.LoseEnergy();
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
    }

    public Item GetCurrentItem()
    {
        if (rightActive)
        {
            return rightItem ?? leftItem;
        }
        else
        {
            return leftItem ?? rightItem;
        }
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
            if (rightActive)
            {
                Destroy(rightItem.obj);
            }
            else
            {
                Destroy(leftItem.obj);
                leftItem = rightItem;
            }
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
            HideCHint();
        }
        else if (collision.gameObject.CompareTag("SmoothieMachine"))
        {
            smoothieMachine = null;
            HideCHint();
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

    private GameObject CreateCKeyHint(GameObject obj, Vector3 offset)
    {
        var cKeyHint = Instantiate(cPrefab);
        cKeyHint.GetComponent<FloatingAnim>().Init(obj, offset);
        return cKeyHint;
    }

    private void ShowCHint()
    {
        if (cKeyHint != null)
        {
            cKeyHint.SetActive(true);
        }
    }

    private void HideCHint()
    {
        if (cKeyHint != null)
        {
            cKeyHint.SetActive(false);
        }
    }
}
