using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCameraShake : MonoBehaviour
{
    public float ShakeAmount;
    float ShakeTime;
    Vector3 initialPosition;
    GameObject targetCamera;

    public void VibrateForTime(float time)
    {
        ShakeTime = time;
    }

    public void SetShakeAmount(float amount)
    {
        ShakeAmount = amount;
    }

    void Start()
    {
        targetCamera = GameObject.Find("Main Camera");
        initialPosition = targetCamera.transform.position;
    }

    void Update()
    {
        if(ShakeTime > 0)
        {
            targetCamera.transform.position = Random.insideUnitSphere * ShakeAmount + initialPosition;
            ShakeTime -= Time.deltaTime;
        }
        else
        {
            ShakeTime = 0f;
            targetCamera.transform.position = initialPosition;
        }
    }
}
