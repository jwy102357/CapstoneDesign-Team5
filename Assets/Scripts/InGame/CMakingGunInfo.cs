using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MakingGunInfo", menuName = "MakingGunInfo", order = 2)]
public class CMakingGunInfo : ScriptableObject
{
    [System.Serializable]
    public struct STMakingGun
    {
        public int indexNumber;
        public string gunName;
        public string gunImagePath;
        public string gunPrefabPath;
        public float power, rapidFire, range;
        public int needMaterial1, needMaterial2, needMaterial3, needMaterial4, needMaterial5, needMaterial6;
        /*
        public STMakingGun(int num, string name, string image, string prefab, float pwr, float rapid, float rg, int m1, int m2, int m3, int m4, int m5, int m6)
        {
            indexNumber = num;
            gunName = name;
            gunImagePath = image;
            gunPrefabPath = prefab;
            power = pwr;
            rapidFire = rapid;
            range = rg;
            needMaterial1 = m1;
            needMaterial2 = m2;
            needMaterial3 = m3;
            needMaterial4 = m4;
            needMaterial5 = m5;
            needMaterial6 = m6;
        }*/
    }
    [SerializeField]
    public List<STMakingGun> infoList = new List<STMakingGun>();
}
