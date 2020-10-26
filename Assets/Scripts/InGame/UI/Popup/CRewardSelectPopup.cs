using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CRewardSelectPopup : MonoBehaviour
{
    struct STItem
    {
        public string name;
        public Sprite nameImage;
        public Sprite itemImage;
        public int rarity;
        public string filePath;

        public STItem(string a_name, string namePath, string imagePath, int a_rarity, string file = null)
        {
            name = a_name;
            nameImage = Resources.Load<Sprite>(namePath);
            itemImage = Resources.Load<Sprite>(imagePath);
            rarity = a_rarity;
            filePath = file;
        }
    }
    
    private Image[] images;
    private List<Image> itemName = new List<Image>();
    private List<Image> itemImage = new List<Image>();
    private List<Image> equipGunName = new List<Image>();
    private List<Image> equipGunImage = new List<Image>();
    private STItem health;
    private STItem shield;
    private STItem bullets;
    private List<STItem> gunList = new List<STItem>();
    private List<STItem> tempGunList = new List<STItem>();

    private STItem[] rewards = new STItem[2];
    private int rewardIndex;

    private Text[] nameText; //임시 텍스트

    private CPlayerHealth player;

    private void Awake()
    {
        SetItemList();
        SetAllRewards();
        itemName[0].transform.parent.parent.gameObject.SetActive(true);
        equipGunName[0].transform.parent.parent.gameObject.SetActive(false);

        nameText = transform.GetComponentsInChildren<Text>();

        player = GameObject.Find("Player").GetComponent<CPlayerHealth>();
    }

    void SetItemList()
    {
        images = transform.Find("ItemSlots").GetComponentsInChildren<Image>();
        for (int i = 0; i < images.Length; i++)
        {
            if (i % 4 == 1)
            {
                itemName.Add(images[i]);
            }
            else if (i % 4 == 2)
            {
                itemImage.Add(images[i]);
            }
        }
        images = transform.Find("GunSlots").GetComponentsInChildren<Image>();
        for (int i = 0; i < images.Length; i++)
        {
            if (i % 4 == 1)
            {
                equipGunName.Add(images[i]);
            }
            else if (i % 4 == 2)
            {
                equipGunImage.Add(images[i]);
            }
        }
    }

    private void OnEnable()
    {
        CreateReward();
    }

    private void OnDisable()
    {
        itemName[0].transform.parent.parent.gameObject.SetActive(true);
        equipGunName[0].transform.parent.parent.gameObject.SetActive(false);
    }

    void SetAllRewards()
    {
        health = new STItem(KDefine.ITEM_HEART, "Sprites/New UI/RewardHeart", "Sprites/New UI/RewardHeart", 0);
        shield = new STItem(KDefine.ITEM_SHIELD, "Sprites/New UI/RewardShield", "Sprites/New UI/RewardShield", 0);
        bullets = new STItem(KDefine.ITEM_BULLETS, "Sprites/New UI/RewardBullets",  "Sprites/New UI/RewardBullets", 0);

        gunList.Add(new STItem("DAKDAKGUN", "Sprites/Gun/DAKDAKGUN", "Sprites/Gun/UI/DAKDAKGUN_UI", 1, "Prefabs/Gun/DAKDAKGUN"));
        gunList.Add(new STItem("M.GUN", "Sprites/Gun/M.GUN", "Sprites/Gun/M.GUN", 2, "Prefabs/Gun/M.GUN"));
        gunList.Add(new STItem("S.GUN", "Sprites/Gun/S.GUN", "Sprites/Gun/S.GUN", 2, "Prefabs/Gun/S.GUN"));
        gunList.Add(new STItem("M.GUN II", "Sprites/Gun/M.GUN", "Sprites/Gun/M.GUN", 3, "Prefabs/Gun/M.GUN II"));
        gunList.Add(new STItem("S.GUN II", "Sprites/Gun/S.GUN", "Sprites/Gun/S.GUN", 3, "Prefabs/Gun/S.GUN II"));
        gunList.Add(new STItem("ONEPUNGUN", "Sprites/Gun/DAKDAKGUN", "Sprites/Gun/UI/DAKDAKGUN_UI", 4, "Prefabs/Gun/ONEPUNGUN"));
        // ...
    }

    public void SelectReward(int index)
    {
        if(rewards[index].filePath == null)
        {
            //CItemManager.Instance.GetObjectForKey(rewards[index].name).GetComponent<IItem>().Use();
            if(rewards[index].name == KDefine.ITEM_HEART)
            {
                player.RestoreHealth(2);
            }
            else if(rewards[index].name == KDefine.ITEM_SHIELD)
            {
                player.AddShield();
            }
            else if(rewards[index].name == KDefine.ITEM_BULLETS)
            {
                CWeaponManager.Instance.AllReload(1000);
            }
        }
        else
        {
            rewardIndex = index;
            if(CWeaponManager.Instance.IsFullInventory())
            {
                itemName[0].transform.parent.parent.gameObject.SetActive(false);
                equipGunName[0].transform.parent.parent.gameObject.SetActive(true);

                for(int i = 0; i < CWeaponManager.weaponList.Count; i++)
                {
                    equipGunName[i].sprite = CWeaponManager.weaponList[i].weaponUIImage;
                    equipGunImage[i].sprite = CWeaponManager.weaponList[i].weaponUIImage;
                }
                return;
            }
            else
            {
                CWeaponManager.Instance.AddWeapon(GameObject.Instantiate(CResourceManager.Instance.GetObjectForKey(rewards[rewardIndex].filePath),
                    CWeaponManager.Instance.weaponParent).GetComponent<CWeapon>());
            }
        }
        this.gameObject.SetActive(false);
    }

    public void DeleteGun(int index)
    {
        CWeaponManager.Instance.DestroyWeapon(index);

        CWeaponManager.Instance.AddWeapon(GameObject.Instantiate(CResourceManager.Instance.GetObjectForKey(rewards[rewardIndex].filePath),
            CWeaponManager.Instance.weaponParent).GetComponent<CWeapon>());

        this.gameObject.SetActive(false);
    }

    STItem CreateRandomItem()
    {
        STItem item;
        int randNum;
        int rarity;
        randNum = Random.Range(0, gunList.Count * 3);
        if (randNum % 3 == 0)
        {
            randNum = Random.Range(1, 101);
            if(randNum > 96)
            {
                rarity = 4;
            }
            else if(randNum > 89)
            {
                rarity = 3;
            }
            else if(randNum > 77)
            {
                rarity = 2;
            }
            else
            {
                rarity = 1;
            }
            
            for(int i = 0; i<gunList.Count; i++)
            {
                if(gunList[i].rarity == rarity)
                {
                    tempGunList.Add(gunList[i]);
                }
            }
            randNum = Random.Range(0, tempGunList.Count);
            return tempGunList[randNum];
        }
        else
        {
            randNum = Random.Range(1, 11);
            item = randNum < 6 ? health : randNum < 9 ? bullets : shield;

        }
        return item;
    }

    void CreateReward()
    {
        rewards[0] = CreateRandomItem();
        do
        {
            rewards[1] = CreateRandomItem();
        } while (rewards[0].name == rewards[1].name);

        //itemName[0].sprite = rewards[0].nameImage;
        nameText[0].text = rewards[0].name;
        itemImage[0].sprite = rewards[0].itemImage;

        //itemName[1].sprite = rewards[1].nameImage;
        nameText[1].text = rewards[1].name;
        itemImage[1].sprite = rewards[1].itemImage;
    }

    public void ExitButton()
    {
        if(equipGunName[0].transform.parent.parent.gameObject.activeSelf)
        {
            itemName[0].transform.parent.parent.gameObject.SetActive(true);
            equipGunName[0].transform.parent.parent.gameObject.SetActive(false);
        }
        else
        {
            this.gameObject.SetActive(false);
        }
    }

}
