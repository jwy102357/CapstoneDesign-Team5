using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CWeaponMakePopup : MonoBehaviour
{
    public struct STMakingGun
    {
        public string gunName;
        public string imagePath;
        public int indexNumber;
        public float power, rapidFire, range;
        public CMixManager.STMaterial necessaryMaterial;

        public STMakingGun(int gunNum, string namePath, string imgPath, float pwr, float rpFire, float rg, int m1, int m2, int m3, int m4, int m5, int m6)
        {
            indexNumber = gunNum;
            gunName = namePath;
            imagePath = imgPath;
            power = pwr;
            rapidFire = rpFire;
            range = rg;
            necessaryMaterial = new CMixManager.STMaterial(m1, m2, m3, m4, m5, m6);
        }
    }
    List<STMakingGun> gunList = new List<STMakingGun>();
    //private Image gunName;
    private Image gunImage;
    private Button leftArrow;
    private Button rightArrow;
    private Button makeButton;

    private GameObject materialPopup;

    private int currentGunIndex = 0;
    public bool[] bIsMade = new bool[3];
    private Image[] images;
    private List<Image> materialList = new List<Image>();
    private List<Image> materialNum = new List<Image>();
    private List<Image> materialImage = new List<Image>();
    private Sprite[] numberFont = new Sprite[10];

    private Image[] statBar;
    public List<CMakingGunInfo.STMakingGun> gunInfo;

    private Text gunNameText;
    private Image gunImage2;

    private void Awake()
    {
        //gunName = transform.Find("GunName").GetComponent<Image>();
        gunImage = transform.Find("GunImage").GetComponent<Image>();
        leftArrow = transform.Find("LeftArrow").GetComponent<Button>();
        rightArrow = transform.Find("RightArrow").GetComponent<Button>();
        makeButton = transform.Find("MakeButton").GetComponent<Button>();
        materialPopup = transform.Find("MaterialPopup").gameObject;
        images = materialPopup.transform.Find("MaterialList").GetComponentsInChildren<Image>();
        statBar = transform.Find("StatBar").GetComponentsInChildren<Image>();
        numberFont = Resources.LoadAll<Sprite>("Sprites/NumberFont");
        gunInfo = Resources.Load<CMakingGunInfo>("MakingGunInfo").infoList;

        gunNameText = transform.GetComponentInChildren<Text>();
        gunImage2 = transform.Find("MaterialPopup").transform.Find("GunImage2").GetComponent<Image>();

        for (int i = 0; i < images.Length; i++)
        {
            if (i % 6 == 0)
            {
                materialList.Add(images[i]);
            }
            else if(i % 6 == 1)
            {
                materialImage.Add(images[i]);
            }
            else
            {
                materialNum.Add(images[i]);
            }
        }
        this.MakeFileLoad();
    }

    private void OnEnable()
    {
        Function.StopGame();
        for (int i = 0; i < gunInfo.Count; i++)
        {
            if (!bIsMade[i])
            {
                gunList.Add(new STMakingGun(gunInfo[i].indexNumber, gunInfo[i].gunName, gunInfo[i].gunImagePath, gunInfo[i].power, gunInfo[i].rapidFire, gunInfo[i].range,
                    gunInfo[i].needMaterial1, gunInfo[i].needMaterial2, gunInfo[i].needMaterial3, gunInfo[i].needMaterial4, gunInfo[i].needMaterial5, gunInfo[i].needMaterial6));
            }
        }
        currentGunIndex = 0;
        if(materialPopup.activeSelf)
        {
            materialPopup.SetActive(false);
        }
        if (gunList.Count > 0)
        {
            ShowGun();
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    private void OnDisable()
    {
        Function.StartGame();
        gunList.Clear();
    }

    public void NextGun()
    {
        currentGunIndex++;
        ShowGun();
    }

    public void PreviousGun()
    {
        currentGunIndex--;
        ShowGun();
    }

    void ShowGun()
    {
        leftArrow.gameObject.SetActive(currentGunIndex < 1 ? false : true);
        rightArrow.gameObject.SetActive(currentGunIndex + 1 < gunList.Count ? true : false);

        //gunName.sprite = Resources.Load<Sprite>(gunList[currentGunIndex].gunNamePath);
        gunNameText.text = gunList[currentGunIndex].gunName;
        gunImage.sprite = Resources.Load<Sprite>(gunList[currentGunIndex].imagePath);

        // 능력치 막대 표시
        statBar[0].fillAmount = gunList[currentGunIndex].power/10f;
        statBar[1].fillAmount = gunList[currentGunIndex].rapidFire/10f;
        statBar[2].fillAmount = gunList[currentGunIndex].range/10f;
    }

    public void ShowMaterialPopup()
    {
        int index = 0;
        gunImage2.sprite = gunImage.sprite;

        if (!materialPopup.activeSelf)
        {
            materialPopup.SetActive(true);
        }

        if(gunList[currentGunIndex].necessaryMaterial.material1 != 0)
        {
            materialList[index].gameObject.SetActive(true);
            materialImage[index].sprite = CMixManager.Instance.GetMaterialSprite(0);
            materialNum[index * 4 + 0].sprite = numberFont[CMixManager.Instance.GetPlayerMaterial().material1 / 10];
            materialNum[index * 4 + 1].sprite = numberFont[CMixManager.Instance.GetPlayerMaterial().material1 % 10];
            materialNum[index * 4 + 2].sprite = numberFont[gunList[currentGunIndex].necessaryMaterial.material1 / 10];
            materialNum[index * 4 + 3].sprite = numberFont[gunList[currentGunIndex].necessaryMaterial.material1 % 10];
            index++;
        }
        if(gunList[currentGunIndex].necessaryMaterial.material2 != 0)
        {
            materialList[index].gameObject.SetActive(true);
            materialImage[index].sprite = CMixManager.Instance.GetMaterialSprite(1);
            materialNum[index * 4 + 0].sprite = numberFont[CMixManager.Instance.GetPlayerMaterial().material2 / 10];
            materialNum[index * 4 + 1].sprite = numberFont[CMixManager.Instance.GetPlayerMaterial().material2 % 10];
            materialNum[index * 4 + 2].sprite = numberFont[gunList[currentGunIndex].necessaryMaterial.material2 / 10];
            materialNum[index * 4 + 3].sprite = numberFont[gunList[currentGunIndex].necessaryMaterial.material2 % 10];
            index++;
        }
        if (gunList[currentGunIndex].necessaryMaterial.material3 != 0)
        {
            materialList[index].gameObject.SetActive(true);
            materialImage[index].sprite = CMixManager.Instance.GetMaterialSprite(2);
            materialNum[index * 4 + 0].sprite = numberFont[CMixManager.Instance.GetPlayerMaterial().material3 / 10];
            materialNum[index * 4 + 1].sprite = numberFont[CMixManager.Instance.GetPlayerMaterial().material3 % 10];
            materialNum[index * 4 + 2].sprite = numberFont[gunList[currentGunIndex].necessaryMaterial.material3 / 10];
            materialNum[index * 4 + 3].sprite = numberFont[gunList[currentGunIndex].necessaryMaterial.material3 % 10];
            index++;
        }
        if (gunList[currentGunIndex].necessaryMaterial.material4 != 0)
        {
            materialList[index].gameObject.SetActive(true);
            materialImage[index].sprite = CMixManager.Instance.GetMaterialSprite(3);
            materialNum[index * 4 + 0].sprite = numberFont[CMixManager.Instance.GetPlayerMaterial().material4 / 10];
            materialNum[index * 4 + 1].sprite = numberFont[CMixManager.Instance.GetPlayerMaterial().material4 % 10];
            materialNum[index * 4 + 2].sprite = numberFont[gunList[currentGunIndex].necessaryMaterial.material4 / 10];
            materialNum[index * 4 + 3].sprite = numberFont[gunList[currentGunIndex].necessaryMaterial.material4 % 10];
            index++;
        }
        if (gunList[currentGunIndex].necessaryMaterial.material5 != 0)
        {
            materialList[index].gameObject.SetActive(true);
            materialImage[index].sprite = CMixManager.Instance.GetMaterialSprite(4);
            materialNum[index * 4 + 0].sprite = numberFont[CMixManager.Instance.GetPlayerMaterial().material5 / 10];
            materialNum[index * 4 + 1].sprite = numberFont[CMixManager.Instance.GetPlayerMaterial().material5 % 10];
            materialNum[index * 4 + 2].sprite = numberFont[gunList[currentGunIndex].necessaryMaterial.material5 / 10];
            materialNum[index * 4 + 3].sprite = numberFont[gunList[currentGunIndex].necessaryMaterial.material5 % 10];
            index++;
        }
        if (gunList[currentGunIndex].necessaryMaterial.material6 != 0)
        {
            materialList[index].gameObject.SetActive(true);
            materialImage[index].sprite = CMixManager.Instance.GetMaterialSprite(5);
            materialNum[index * 4 + 0].sprite = numberFont[CMixManager.Instance.GetPlayerMaterial().material6 / 10];
            materialNum[index * 4 + 1].sprite = numberFont[CMixManager.Instance.GetPlayerMaterial().material6 % 10];
            materialNum[index * 4 + 2].sprite = numberFont[gunList[currentGunIndex].necessaryMaterial.material6 / 10];
            materialNum[index * 4 + 3].sprite = numberFont[gunList[currentGunIndex].necessaryMaterial.material6 % 10];
            index++;
        }

        while (index < materialList.Count)
        {
            materialList[index++].gameObject.SetActive(false);
        }
    }

    public void Making()
    {
        if(CMixManager.Instance.IsMakeCondition(gunList[currentGunIndex].necessaryMaterial))
        {
            CMixManager.Instance.PlayerMaterialMinus(gunList[currentGunIndex].necessaryMaterial);
            bIsMade[gunList[currentGunIndex].indexNumber] = true;
            this.MakeFileSave();
            gunList.RemoveAt(currentGunIndex);
            ExitMaterialPopup();

            if(currentGunIndex < gunList.Count)
            {
                ShowGun();
            }
            else if(currentGunIndex > 0)
            {
                currentGunIndex--;
                ShowGun();
            }
            else
            {
                Cancel();
                // 제작할 수 있는 무기 없음
            }
        }
        else
        {
            // 삐빅 재료없음!
        }
    }

    public void ExitMaterialPopup()
    {
        if(materialPopup.activeSelf)
        {
            materialPopup.SetActive(false);
        }
    }

    public void Cancel()
    {
        gameObject.SetActive(false);
    }

    public void MakeFileSave()
    {
        ES3.Save<bool[]>("bIsMade", bIsMade, "MakeWeapon.es3");
    }

    public void MakeFileLoad()
    {
        if (!ES3.KeyExists("bIsMade", "MakeWeapon.es3"))
        {
            SettingDefault();
        }

        this.bIsMade = ES3.Load<bool[]>("bIsMade", "MakeWeapon.es3");
    }

    public void SettingDefault()
    {
        for (int i = 0; i < 3; i++)
        {
            bIsMade[i] = false;
        }
        bIsMade[0] = true;

        ES3.Save<bool[]>("bIsMade", bIsMade, "MakeWeapon.es3");
    }
}
