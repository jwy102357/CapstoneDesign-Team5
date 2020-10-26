using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CHeartUI : MonoBehaviour
{
    public Sprite fullHeart;
    public Sprite halfHeart;
    public Sprite emptyHeart;
    public Sprite shield;
    public Sprite halfShield;
    //public int defaultHeart = 6;
    public int currentHeart;
    public int currentShield;
    public int maxHeart;

    List<GameObject> heartList = new List<GameObject>();

    void CreateObj(Sprite sprite)
    {
        heartList.Add(new GameObject());
        heartList[heartList.Count-1].transform.parent = this.transform;
        Destroy(heartList[heartList.Count - 1].GetComponent<Transform>());
        RectTransform rect = heartList[heartList.Count - 1].AddComponent<RectTransform>();
        rect.anchoredPosition3D = new Vector3(-420 + 50 * (heartList.Count - 1), 264f, 0f);
        rect.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        Image image = heartList[heartList.Count - 1].AddComponent<Image>();
        image.sprite = sprite;
    }

    public void AddHeart(int num = 1)
    {
        for(int i = 0; i < num; i++)
        {
            if (currentHeart == maxHeart) maxHeart += 2;
            if(currentHeart%2 == 0)
            {
                if (currentShield > 0)
                {
                    int tempNum = currentShield > 1 ? 2 : 1;
                    currentShield -= tempNum;
                    AddShield(tempNum);
                }
                if (heartList.Count > currentHeart / 2)
                {
                    heartList[currentHeart / 2].SetActive(true);
                    heartList[currentHeart / 2].GetComponent<Image>().sprite = halfHeart;
                }
                else
                {
                    CreateObj(halfHeart);
                }
                currentHeart++;
            }
            else
            {
                heartList[(currentHeart - 1) / 2].GetComponent<Image>().sprite = fullHeart;
                currentHeart++;
            }
        }
    }

    public void SubtractHeart(int num = 1)
    {
        if (currentHeart+currentShield < num) num = currentHeart+currentShield;
        for(int i = 0; i < num; i++)
        {
            if(currentShield > 0)
            {
                if(currentShield % 2 == 0)
                {

                    heartList[((maxHeart + 1) / 2) + (currentShield + 1) / 2 - 1].GetComponent<Image>().sprite = halfShield;
                }
                else
                {
                    heartList[((maxHeart + 1) / 2) + (currentShield + 1) / 2 - 1].SetActive(false);
                }
                currentShield--;
            }
            else
            {
                if (currentHeart % 2 == 0)
                {
                    heartList[currentHeart / 2 - 1].GetComponent<Image>().sprite = halfHeart;
                }
                else
                {
                    heartList[(currentHeart - 1) / 2].GetComponent<Image>().sprite = emptyHeart;
                }
                currentHeart--;
            }
        }
    }

    public void AddShield(int num = 2)
    {
        for (int i = 0; i < num; i++)
        {
            int index = ((maxHeart + 1) / 2) + (currentShield+1)/2;
            if(currentShield % 2 == 0)
            {
                if (heartList.Count > index)
                {
                    heartList[index].SetActive(true);
                    heartList[index].GetComponent<Image>().sprite = halfShield;
                }
                else
                {
                    CreateObj(halfShield);
                }
            }
            else
            {
                if(heartList.Count > index-1) 
                {
                    heartList[index - 1].GetComponent<Image>().sprite = shield;
                }   
                else
                {
                    CreateObj(shield);
                }
            }
            currentShield++;
        }
    }

    public void SetHeart(int cHeart = 6, int mHeart = 6, int shield = 0)
    {
        maxHeart = mHeart;
        for(int i = 0; i < heartList.Count-1; i++)
        {
            heartList[i].SetActive(false);
        }
        currentHeart = 0;
        currentShield = 0;
        for(int i = 0; i < cHeart; i++)
        {
            AddHeart();
        }
        for(int i = 0; i < (mHeart-cHeart)/2; i++)
        {
            if(heartList.Count > (cHeart+1)/2 + i)
            {
                heartList[(cHeart + 1) / 2 + i].SetActive(true);
                heartList[(cHeart + 1) / 2 + i].GetComponent<Image>().sprite = emptyHeart;
            }
            else
            {
                CreateObj(emptyHeart);
            }
        }
        for(int i = 0; i < shield; i++)
        {
            AddShield(1);
        }
    }
}
