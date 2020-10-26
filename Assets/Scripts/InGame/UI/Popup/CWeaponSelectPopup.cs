using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CWeaponSelectPopup : MonoBehaviour
{
    private Button leftArrow;
    private Button rightArrow;
    private int currentGunIndex;
    private Image gunName;
    private Image gunImage;
    private Image[] statBar;
    private bool[] bIsMade;
    private bool[] bIsOpened = new bool[3];

    private Image bgLock;
    public CMakingGunInfo infoObj;
    public List<CMakingGunInfo.STMakingGun> gunList;

    public CWeaponMakePopup weaponMakePopup;
    public CWeaponSelectLock selectLock;

    private GameObject SelectButton;
    Text gunNameText;

    private void Awake()
    {
        gunName = transform.Find("GunName").GetComponent<Image>();
        gunImage = transform.Find("GunImage").GetComponentInChildren<Image>();
        selectLock = transform.Find("Lock").GetComponent<CWeaponSelectLock>();
        leftArrow = transform.Find("LeftArrow").GetComponent<Button>();
        rightArrow = transform.Find("RightArrow").GetComponent<Button>();
        statBar = transform.Find("StatBar").GetComponentsInChildren<Image>();
        weaponMakePopup = transform.parent.Find("WeaponMakePopup").GetComponent<CWeaponMakePopup>();
        bgLock = transform.Find("BGLock").GetComponent<Image>();
        gunList = Resources.Load<CMakingGunInfo>("MakingGunInfo").infoList;
        SelectButton = transform.Find("SelectButton").gameObject;

        selectLock.Setting();
        gunNameText = transform.GetComponentInChildren<Text>();
    }
    
    private void OnEnable()
    {
        Function.StopGame();
        this.MakeFileLoad();
        OpenedWeaponListLoad();
        currentGunIndex = 0;
        ShowGun();
    }

    private void OnDisable()
    {
        Function.StartGame();
    }
    
    void ShowGun()
    {
        leftArrow.gameObject.SetActive(currentGunIndex < 1 ? false : true);
        rightArrow.gameObject.SetActive(currentGunIndex + 1 < gunList.Count ? true : false);
        //gunName.sprite = Resources.Load<Sprite>(gunList[currentGunIndex].gunNamePath);
        gunNameText.text = gunList[currentGunIndex].gunName;
        gunImage.sprite = Resources.Load<Sprite>(gunList[currentGunIndex].gunImagePath);
        if(CWeaponManager.Instance.GetCurrentWeapon().gunName == gunList[currentGunIndex].gunName)
        {
            SelectButton.SetActive(false);
        }
        else
        {
            SelectButton.SetActive(true);
        }

        statBar[0].fillAmount = gunList[currentGunIndex].power / 10f;
        statBar[1].fillAmount = gunList[currentGunIndex].rapidFire / 10f;
        statBar[2].fillAmount = gunList[currentGunIndex].range / 10f;

        if(bIsMade[currentGunIndex])
        {
            if(bIsOpened[currentGunIndex])
            {
                bgLock.gameObject.SetActive(false);
                selectLock.NoLock();
            }
            else
            {
                bgLock.gameObject.SetActive(true);
                selectLock.Unlock();
            }
        }
        else
        {
            bgLock.gameObject.SetActive(true);
            selectLock.Lock();
        }
    }

    public void NextList()
    {
        currentGunIndex++;
        ShowGun();
    }

    public void PreviousList()
    {
        currentGunIndex--;
        ShowGun();
    }

    public void Cancel()
    {
        gameObject.SetActive(false);
    }

    public void SelectWeapon()
    {
        if (bIsOpened[currentGunIndex])
        {
            GameObject weaponObj = GameObject.Instantiate(CResourceManager.Instance.GetObjectForKey(gunList[currentGunIndex].gunPrefabPath),
                                                      CWeaponManager.Instance.weaponParent);
            CWeapon selectWeapon = weaponObj.GetComponent<CWeapon>();
            CWeaponManager.Instance.DestroyWeapon(0);
            CWeaponManager.Instance.AddWeapon(selectWeapon);
            ES3.Save<string>("BaseGun", selectWeapon.gunName ,"Weapon.es3");
            this.gameObject.SetActive(false);
        }
    }

    public void OpenedWeaponListSave()
    {
        ES3.Save<bool[]>("bIsOpened", bIsOpened, "MakeWeapon.es3");
    }

    public void OpenedWeaponListLoad()
    {
        if (!ES3.KeyExists("bIsOpened", "MakeWeapon.es3"))
        {
            bIsOpened[0] = true;
            OpenedWeaponListSave();
        }
        this.bIsOpened = ES3.Load<bool[]>("bIsOpened", "MakeWeapon.es3");
    }

    public void MakeFileLoad()
    {
        if (!ES3.KeyExists("bIsMade", "MakeWeapon.es3"))
        {
            weaponMakePopup.MakeFileSave();
        }
        this.bIsMade = ES3.Load<bool[]>("bIsMade", "MakeWeapon.es3");
    }

    public void EndUnlock()
    {
        bgLock.gameObject.SetActive(false);
        bIsOpened[currentGunIndex] = true;
        OpenedWeaponListSave();
    }
}
