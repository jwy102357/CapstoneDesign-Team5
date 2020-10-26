using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CShop : MonoBehaviour
{
    [System.Serializable]
    public struct stItemInfo
    {
        public GameObject item;
        public float fProbability;
        public string filePath;
    }
    public CShopItem[] itemStand;
    public stItemInfo[] itemList;
    public int iMinPrice;
    
    void Awake()
    {
        itemStand = transform.parent.GetComponentsInChildren<CShopItem>();
    }

    private void OnEnable()
    {
        for(int i = 0; i < itemStand.Length; i++)
        {
            if (!itemStand[i].bIsTradable)
            {
                int idx = ChoiceItem();
                itemStand[i].itemName = itemList[idx].item.name;
                itemStand[i].itemImage.sprite = itemList[idx].item.GetComponent<SpriteRenderer>().sprite;
                if (itemList[idx].filePath != null)
                {
                    itemStand[i].filePath = itemList[idx].filePath;
                }
                itemStand[i].price = iMinPrice * 100 / (int)(itemList[idx].fProbability);
                itemStand[i].SetItem(); 
            }
        }
    }

    private void Start()
    {
        for (int i = 0; i < itemStand.Length; i++)
        {
            int idx = ChoiceItem();
            itemStand[i].itemName = itemList[idx].item.name;
            //Debug.Log(itemList[idx].item.GetComponent<SpriteRenderer>().sprite);
            itemStand[i].itemImage.sprite = itemList[idx].item.GetComponent<SpriteRenderer>().sprite;
            if (itemList[idx].filePath != null)
            {
                itemStand[i].filePath = itemList[idx].filePath;
            }
            //itemStand[i].price = iMinPrice + (int)(50 - itemList[idx].fProbability);
            itemStand[i].price = iMinPrice * 100 / (int)(itemList[idx].fProbability);
            itemStand[i].SetItem();
        }
    }

    int ChoiceItem()
    {
        float fMax = 0;
        for(int i=0; i<itemList.Length; i++)
        {
            fMax += itemList[i].fProbability;
        }
        float choiceNum = Random.Range(0f, fMax);
        Debug.Log("Choice = " + choiceNum);
        float checkNum = 0;
        for(int i=0; i<itemList.Length; i++)
        {
            checkNum += itemList[i].fProbability;
            if(choiceNum <= checkNum)
            {
                return i; 
            }
        }
        return 0;
    }

}
