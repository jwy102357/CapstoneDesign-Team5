using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CShopItem : CInteractable
{

    public int price;
    public Text priceText;
    public Text itemNameText;
    public string itemName;
    public SpriteRenderer itemImage;
    public string filePath;

    public bool bIsTradable = true;
    CMovement player;
    CPlayerHealth playerHealth;
    private bool bCoroutineIsRunning = false;

    void Awake()
    {
        priceText = transform.GetChild(0).Find("PriceText").GetComponent<Text>();
        itemNameText = transform.GetChild(0).Find("ItemNameText").GetComponent<Text>();
        playerHealth = GameObject.Find("Player").GetComponent<CPlayerHealth>();
        itemImage = transform.GetComponentInChildren<SpriteRenderer>();
    }

    public void SetItem()
    {
        bIsTradable = true;
        if (itemNameText != null)
        {
            itemNameText.text = itemName;
            itemNameText.enabled = false;
        }
        ReWritePrice();
        StartCoroutine("ItemFloating");
    }

    public override void InteractAction()
    {
        if (CMerchandiseManager.Instance.GetCoinAmount() >= price)
        {
            Trade();
        }
        else
        {
            Debug.Log("Not enough coin");
        }
    }

    void ReWritePrice()
    {
        priceText.text = price.ToString();
    }

    public void Trade()
    {
        CMerchandiseManager.Instance.MinusCoin(price);
        bIsTradable = false;
        itemImage.sprite = null;

        if(bCoroutineIsRunning)
        {
            StopCoroutine("ItemFloating");
        }
        if(filePath == "")
        {
            if (itemName == KDefine.ITEM_HEART)
            {
                playerHealth.RestoreHealth(2);
            }
            else if (itemName == KDefine.ITEM_SHIELD)
            {
                playerHealth.AddShield();
            }
            else if (itemName == KDefine.ITEM_BULLETS)
            {
                CWeaponManager.Instance.AllReload(1000);
            }
        }
        else
        {
            if (CWeaponManager.Instance.IsFullInventory())
            {
                CWeaponManager.Instance.DestroyWeapon(CWeaponManager.Instance.currentWeaponIndex);
            }
            Debug.Log(filePath);
            CWeaponManager.Instance.AddWeapon(GameObject.Instantiate(CResourceManager.Instance.GetObjectForKey(filePath), 
                CWeaponManager.Instance.weaponParent).GetComponent<CWeapon>());
        }

        FinishInteract();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(KDefine.TAG_PLAYER) && bIsTradable)
        {
            player = collision.GetComponent<CMovement>();
            player.interactPopup = this;
            player.playerInput.AttackButton.ChangeInteractImage();
            if (itemNameText != null)
            {
                itemNameText.enabled = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(KDefine.TAG_PLAYER) && bIsTradable)
        {
            FinishInteract();
        }
    }

    private void FinishInteract()
    {
        player.interactPopup = null;
        player.playerInput.AttackButton.ChangeAttackImage();
        if (itemNameText != null)
        {
            itemNameText.enabled = false;
        }
    }

    public IEnumerator ItemFloating()
    {
        bCoroutineIsRunning = true;
        float fYpos = itemImage.transform.position.y;
        while(bIsTradable)
        {
            while(itemImage.transform.position.y < fYpos + 0.3f && bIsTradable)
            {
                itemImage.transform.position += Vector3.up * Time.deltaTime * 0.3f;
                yield return new WaitForEndOfFrame();
            }
            while(itemImage.transform.position.y > fYpos && bIsTradable)
            {
                itemImage.transform.position += Vector3.down * Time.deltaTime * 0.3f;
                yield return new WaitForEndOfFrame();
            }
        }
        bCoroutineIsRunning = false;
    }
}
