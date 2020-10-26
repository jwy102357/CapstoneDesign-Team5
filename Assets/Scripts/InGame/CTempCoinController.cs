using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CTempCoinController : MonoBehaviour
{
    public void GetCoin(int num)
    {
        CMerchandiseManager.Instance.PlusCoin(num);
    }

    private void OnEnable()
    {
        CMerchandiseManager.Instance.ManyCoinCreate(198, 199, transform.position);
    }
}
