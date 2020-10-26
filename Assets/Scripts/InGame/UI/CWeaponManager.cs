using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CWeaponManager : MonoBehaviour
{
    private static CWeaponManager instance;
    public static CWeaponManager Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject obj = new GameObject(typeof(CWeaponManager).ToString());
                obj.AddComponent<CWeaponManager>();
                instance = obj.GetComponent<CWeaponManager>();
            }

            return instance;
        }
    }
    public CShooter playerShooter { get; set; }
    Text remainBulletText;
    private static int nWeaponMaxCount = 2;
    public int currentWeaponIndex = 0;
    public static List<CWeapon> weaponList;
    private static int[] bulletArray = new int[nWeaponMaxCount];
    CWeapon currentWeapon;
    public Transform weaponParent { get; set; }
    Image gunUIImage;

    public void SetBulletText(string bulletStr)
    {
        if (remainBulletText == null)
        {
            remainBulletText = GameObject.Find("Canvas_UI").transform.Find("RemainBullet").GetComponent<Text>();
        }

        if (bulletStr.Length != 0)
        {
            remainBulletText.text = bulletStr;
        }
    }

    public void SetCurrentWeapon(CWeapon weapon)
    {
        if (this.currentWeapon != null)
        {
            this.currentWeapon.gameObject.SetActive(false);
        }
        currentWeapon = weapon;
        this.currentWeapon.gameObject.SetActive(true);

        currentWeapon.MadeBulletString();
        if (playerShooter == null)
        {
            weaponParent.GetComponentInParent<CShooter>().SetCurrentWeapon(currentWeapon);
        }

        else
        {
            playerShooter.SetCurrentWeapon(currentWeapon);
        }

        if (gunUIImage == null)
        {
            gunUIImage = GameObject.Find("Canvas_UI").transform.Find("Gun_UI").GetComponent<Image>();
        }

        gunUIImage.sprite = currentWeapon.weaponUIImage;
        weapon.SetCameraShake();
    }

    public CWeapon GetCurrentWeapon()
    {
        return currentWeapon;
    }

    public void AddWeapon(CWeapon weapon)
    {
        if (!IsFullInventory())
        {
            var AddWeapon = weapon;
            AddWeapon.transform.parent = weaponParent;
            AddWeapon.transform.localPosition = currentWeapon.transform.localPosition;
            weaponList.Add(AddWeapon);
            if (bulletArray[weaponList.Count - 1] == -1)
            {
                bulletArray[weaponList.Count - 1] = weaponList[weaponList.Count - 1].GetMaxBullet();
            }

            weaponList[weaponList.Count - 1].SetCurrentBullet(bulletArray[weaponList.Count - 1]); 
            this.SetCurrentWeapon(AddWeapon);
            //내 무기는 웨펀리스트 카운트 -1
            this.SaveWeapon();
        }
    }

    public void SetMaxBullet()
    {
        for (int i = 0; i < weaponList.Count; i++)
        {
            bulletArray[i] = weaponList[i].GetMaxBullet();
            weaponList[i].SetCurrentBullet(bulletArray[i]);
        }
        
        ES3.Save<int[]>("BulletCount", bulletArray, "Weapon.es3");
    }

    public void LoadWeaponList()
    {
        if (this.weaponParent == null)
        {
            this.weaponParent = GameObject.Find("GunSlot").transform;
        }

        bool isExist = ES3.FileExists("WeaponList.es3");
        if (isExist)
        {
            var keys = ES3.GetKeys("WeaponList.es3");
            bulletArray = ES3.Load<int[]>("BulletCount", "Weapon.es3");

            if (keys.Length != 0)
            {
                for (int i = keys.Length - 1; i >= 0; i--)
                {
                    string gunName = ES3.Load<string>(keys[i], "WeaponList.es3");
                    GameObject weaponObj = GameObject.Instantiate(CResourceManager.Instance.GetObjectForKey("Prefabs/Gun/" +
                                                                 gunName), this.weaponParent);
                    var weapon = weaponObj.GetComponent<CWeapon>();
                    weapon.gunName = gunName;
                    weapon.SetGunImageSetting();
                    this.SetCurrentWeapon(weapon);
                    this.AddWeapon(weapon);
                }

                this.currentWeaponIndex = ES3.Load<int>("CurrentWeaponIndex", "Weapon.es3");
                this.ChangeWeapon(this.currentWeaponIndex);
            }
        }

        else
        {
            //베이스건으로 놓아야함.
            GameObject weaponObj = GameObject.Instantiate(CResourceManager.Instance.GetObjectForKey("Prefabs/Gun/DAKGUN"), this.weaponParent);
            var weapon = weaponObj.GetComponent<CWeapon>();
            weapon.gunName = "DAKGUN";
            weapon.SetGunImageSetting();
            for (int i = 0; i < bulletArray.Length; i++)
            {
                bulletArray[i] = -1;
            }
            this.SetCurrentWeapon(weapon);
            this.AddWeapon(weapon);
            this.currentWeaponIndex = 0;
            ES3.Save<int>("CurrentWeaponIndex", this.currentWeaponIndex, "Weapon.es3");
        }
       
        
    }

    public void ReturnLobby()
    {
        if (ES3.FileExists("WeaponList.es3"))
        {
            var keys = ES3.GetKeys("WeaponList.es3");
            for (int i = 0; i < keys.Length; i++)
            {
                if (ES3.KeyExists(i.ToString(), "WeaponList.es3"))
                {
                    ES3.DeleteKey(i.ToString(), "WeaponList.es3");
                }
            }
        }

        if (this.weaponParent == null)
        {
            this.weaponParent = GameObject.Find("GunSlot").transform;
        }

        weaponList.Clear();
        GameObject weaponObj;
        CWeapon weapon;
        if (ES3.KeyExists("BaseGun", "Weapon.es3"))
        {
            string weaponName = ES3.Load<string>("BaseGun", "Weapon.es3");
            weaponObj = GameObject.Instantiate(CResourceManager.Instance.GetObjectForKey("Prefabs/Gun/" +
                                                             weaponName), this.weaponParent);

            weapon = weaponObj.GetComponent<CWeapon>();
            weapon.gunName = weaponName;
        }

        else
        {
            weaponObj = GameObject.Instantiate(CResourceManager.Instance.GetObjectForKey("Prefabs/Gun/DAKGUN"), this.weaponParent);
            weapon = weaponObj.GetComponent<CWeapon>();
            weapon.gunName = "DAKGUN";

        }

        
        weapon.SetGunImageSetting();
        bulletArray[0] = weapon.GetMaxBullet();
        for (int i = 1; i < bulletArray.Length; i++)
        {
            bulletArray[i] = -1;
        }

        if (this.weaponParent == null)
        {
            this.weaponParent = GameObject.Find("GunSlot").transform;
        }

        this.SetCurrentWeapon(weapon);
        this.AddWeapon(weapon);
        ES3.Save<int>("CurrentWeaponIndex", this.currentWeaponIndex, "Weapon.es3");
        ES3.Save<string>("BaseGun", weapon.gunName, "Weapon.es3");
    }

    public void listClear()
    {
        if (weaponList != null)
        {
            weaponList.Clear();
        }
    }

    public void SaveWeapon()
    {
        for (int i = 0; i < weaponList.Count; i++)
        {
            ES3.Save<string>(i.ToString(), weaponList[i].gunName, "WeaponList.es3");
        }

        for (int i = 0; i < weaponList.Count; i++)
        {
            bulletArray[i] = weaponList[i].GetCurrentBullet();
            Debug.Log(bulletArray[i]);
        }

        this.currentWeaponIndex = weaponList.Count - 1;
        ES3.Save<int>("CurrentWeaponIndex", this.currentWeaponIndex, "Weapon.es3");
        ES3.Save<int[]>("BulletCount", bulletArray, "Weapon.es3");
    }

    public void AllReload(int bulletCount)
    {
        for (int i = 0; i < weaponList.Count; i++)
        {
            weaponList[i].ReloadBullet(bulletCount);
        }
        weaponList[currentWeaponIndex].MadeBulletString();
    }

    public bool IsFullInventory()
    {
        if (weaponList == null)
        {
            weaponList = new List<CWeapon>();
        }

        if (weaponList.Count >= nWeaponMaxCount)
        {
            return true;
        }

        return false;
    }

    public void DestroyWeapon(int index)
    {
        if (index < weaponList.Count)
        {
            Destroy(weaponList[index].gameObject);
            weaponList.RemoveAt(index);

            for (int i = 0; i < bulletArray.Length; i++)
            {
                bulletArray[i] = -1;
            }
        }

        //인덱스 참조해서 총알 없애기. 
        for (int i = 0; i < weaponList.Count; i++)
        {
            bulletArray[i] = weaponList[i].GetCurrentBullet();
        }
    }

    public void ChangeWeapon()
    {
        for (int i = 0; i < weaponList.Count; i++)
        {
            if (weaponList[i] == currentWeapon)
            {
                if (i == weaponList.Count - 1)
                {
                    this.SetCurrentWeapon(weaponList[0]);
                    this.currentWeaponIndex = 0;
                }

                else
                {
                    this.SetCurrentWeapon(weaponList[i + 1]);
                    this.currentWeaponIndex = i + 1;
                }

                ES3.Save<int>("CurrentWeaponIndex", this.currentWeaponIndex, "Weapon.es3");
                Debug.Log(this.currentWeaponIndex);
                break;
            }
        }
    }

    public void ChangeWeapon(int index)
    {
        if (index >= weaponList.Count || index < 0)
        {
            return;
        }

        else
        {
            this.SetCurrentWeapon(weaponList[index]);
            ES3.Save<int>("CurrentWeaponIndex", index, "Weapon.es3");
        }
    }

    public static CWeaponManager Create()
    {
        if (weaponList == null)
        {
            weaponList = new List<CWeapon>();
        }
        return instance;
    }

    public void SetGunUI()
    {
        gunUIImage.sprite = this.currentWeapon.weaponUIImage;
    }
}
