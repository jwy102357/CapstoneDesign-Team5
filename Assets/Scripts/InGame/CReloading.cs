using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CReloading : MonoBehaviour
{
    public Image bar;
    public float reloadingTime;
    public CMovement movement;

    private void Awake()
    {
        bar = transform.GetComponentInChildren<Image>();
        movement = GameObject.Find("Player").GetComponent<CMovement>();
        bar.fillAmount = 0;
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        bar.fillAmount = 0;
    }

    void LateUpdate()
    {
        transform.localScale = new Vector3(movement.bIsFacingRight ? 1 : -1, 1, 1);
        if (bar.fillAmount >= 1)
        {
            gameObject.SetActive(false);
        }
        if (reloadingTime != 0)
        {
            bar.fillAmount += Time.deltaTime / reloadingTime;
        }
    }
}