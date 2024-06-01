using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    private Vector3 LeftItemOffsetWhenHoldingOne = new(0, 0.6f, 0);
    private Vector3 LeftItemOffsetWhenHoldingTwo = new(-0.5f, 0.6f, 0);
    private Vector3 RightItemOffset = new(0.5f, 0.6f, 0);
    private GameObject leftItem;
    private GameObject rightItem;
    private Chest chest;

    private SmoothieMachine smoothieMachine;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            DiscardOneItem();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (chest != null)
            {
                PickUp(chest.itemPrefab); // if near chest, pick up item from chest
            }
            else if (smoothieMachine != null)
            {
                if (GetCurrentItem() == "")
                {
                    // if holding nothing, try to take out smoothie from machine
                    GameObject item = smoothieMachine.PickUp();
                    if (item != null)
                    {
                        PickUp(item);
                    }
                }
                else
                {
                    // if holding something, try to put that into smoothie machine
                    if (smoothieMachine.AddIngredient(GetCurrentItem()))
                    {
                        DiscardOneItem();
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

    private void DiscardOneItem()
    {
        if (rightItem != null)
        {
            Destroy(rightItem);
            rightItem = null;
            leftItem.transform.position = transform.position + LeftItemOffsetWhenHoldingOne;
        }
        else if (leftItem != null)
        {
            Destroy(leftItem);
            leftItem = null;
        }
    }


    private void PickUp(GameObject itemPrefab)
    {
        if (leftItem == null)
        {
            leftItem = Instantiate(itemPrefab, transform.position + LeftItemOffsetWhenHoldingOne, Quaternion.identity);
            leftItem.transform.SetParent(transform);
        }
        else if (rightItem == null)
        {
            leftItem.transform.position = transform.position + LeftItemOffsetWhenHoldingTwo;
            rightItem = Instantiate(itemPrefab, transform.position + RightItemOffset, Quaternion.identity);
            rightItem.transform.SetParent(transform);
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
    }
}
