using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CMaterialText : MonoBehaviour
{
    public Text[] textList;

    private void OnGUI()
    {
        if (textList.Length > 0)
        {
            textList[0].text = CMixManager.Instance.GetPlayerMaterial().material1.ToString();
            textList[1].text = CMixManager.Instance.GetPlayerMaterial().material2.ToString();
            textList[2].text = CMixManager.Instance.GetPlayerMaterial().material3.ToString();
            textList[3].text = CMixManager.Instance.GetPlayerMaterial().material4.ToString();
        }
    }
}
