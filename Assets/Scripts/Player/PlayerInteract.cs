using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerInteract : MonoBehaviour
{
    public bool disableDiscard = false;
    public GameObject cPrefab;
    public GameObject activeItemBorderPrefab;
    public GameObject clearText;
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
    private GameObject activeItemBorder;

    void Awake()
    {
        playerEnergy = GetComponent<PlayerEnergy>();
        playerControl = GetComponent<PlayerControl>();
        //if (Util.GetCurrentLevelNum() == 1)
        //{
        cKeyHint = CreateCKeyHint(gameObject, new(-1.45f, 1.8f, 0f));
        HideCHint();
        //}
        if (activeItemBorderPrefab != null)
        {
            activeItemBorder = Instantiate(activeItemBorderPrefab);
            activeItemBorder.transform.SetParent(transform);
            activeItemBorder.SetActive(false);
        }
        clearText.SetActive(false);
    }

    void Update()
    {
        if (playerControl.disableAllAction)
        {
            return;
        }

        if (!disableDiscard && Input.GetKeyDown(KeyCode.Q))
        {
            DiscardOneItem(rightActive);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            rightActive = !rightActive;
            UpdateActiveItemBorder();
        }

        if (chest != null && !chest.Disabled && !InventoryIsFull())
        {
            ShowCHint();
            if (Input.GetKeyDown(KeyCode.C))
            {
                PickUp(chest.GetItem());
                HideCHint();
            }
        }
        else if (smoothieMachine != null && !smoothieMachine.Disabled)
        {
            clearText.SetActive(false);
            if (smoothieMachine.HasItem() && leftItem == null)
            {
                clearText.SetActive(true);
                if (Input.GetKeyDown(KeyCode.Q))
                {
                    clearText.SetActive(false);
                    smoothieMachine.ClearSmoothie();
                    Debug.Log("smoothie machine cleared");
                }
            }
            var hasAddedIngredient = false;
            if (smoothieMachine.TryAddIngredient(GetActiveItem()))
            {
                ShowCHint();
                if (Input.GetKeyDown(KeyCode.C))
                {
                    smoothieMachine.AddIngredient(GetActiveItem());
                    Debug.Log("Active");
                    DiscardOneItem(rightActive);
                    hasAddedIngredient = true;
                    if (!smoothieMachine.TryAddIngredient(GetActiveItem()))
                    {
                        HideCHint();
                    }
                }
            }
            else if (smoothieMachine.TryAddIngredient(GetNonActiveItem()))
            {
                ShowCHint();
                if (Input.GetKeyDown(KeyCode.C))
                {
                    smoothieMachine.AddIngredient(GetNonActiveItem());
                    Debug.Log("NonActive");
                    DiscardOneItem(!rightActive);
                    hasAddedIngredient = true;
                    if (!smoothieMachine.TryAddIngredient(GetNonActiveItem()))
                    {
                        HideCHint();
                    }
                }
            }

            if (!hasAddedIngredient && !InventoryIsFull() && smoothieMachine.HasProduct())
            {
                ShowCHint();
                if (Input.GetKeyDown(KeyCode.C))
                {
                    PickUp(smoothieMachine.GetProduct());
                    HideCHint();
                }
            }
        }
        else if (manager != null)
        {
            var res = manager.TrySubmit(GetAllItems());
            if (res == SubmitResult.SubmittedLeft || res == SubmitResult.SubmittedRight)
            {
                ShowCHint();
            }
            if (Input.GetKeyDown(KeyCode.C))
            {
                res = manager.Submit(GetAllItems());
                if (res == SubmitResult.SubmittedLeft || res == SubmitResult.SubmittedRight)
                {
                    DiscardOneItem(res == SubmitResult.SubmittedRight);
                    HideCHint();
                }
            }
            else if (Input.GetKeyDown(KeyCode.T))
            {
                if (playerEnergy.CanSchmooze())
                {
                    manager.Schmooze();
                    playerEnergy.LoseEnergyFromSchmooze();
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

    public Item GetActiveItem()
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

    public Item GetNonActiveItem()
    {
        if (rightActive)
        {
            return leftItem ?? rightItem;
        }
        else
        {
            return rightItem ?? leftItem;
        }
    }


    public List<Item> GetAllItems()
    {
        return new List<Item> { leftItem, rightItem }.Where(item => item != null).ToList();
    }

    private bool InventoryIsFull()
    {
        return rightItem != null;
    }

    private void DiscardOneItem(bool discardRightFirst)
    {
        if (rightItem != null)
        {
            if (discardRightFirst)
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
        UpdateActiveItemBorder();
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
        UpdateActiveItemBorder();
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
            clearText.SetActive(false);
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

    private void UpdateActiveItemBorder()
    {
        if (GetActiveItem() == null)
        {
            activeItemBorder.SetActive(false);
        }
        else
        {
            activeItemBorder.SetActive(true);
            Vector3 itemToBorderOffset = new(0f, 0.5f, 0f);
            if (rightItem == null)
            {
                activeItemBorder.transform.localPosition = LeftItemOffsetWhenHoldingOne + itemToBorderOffset;
            }
            else
            {
                if (rightActive)
                {
                    activeItemBorder.transform.localPosition = RightItemOffset + itemToBorderOffset;
                }
                else
                {
                    activeItemBorder.transform.localPosition = LeftItemOffsetWhenHoldingTwo + itemToBorderOffset;
                }
            }
        }
    }
}
