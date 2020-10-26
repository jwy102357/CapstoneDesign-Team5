using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMissile : CEnemyBullet
{
    private Vector3 point0;
    private Vector3 point1;
    private Vector3 targetPoint;
    private Vector2 result;
    public float fBulletSpeed;
    public bool bIsTargetAim = false;
    private float fTime;
    private bool bIsShot = false;
    private bool bIsAirShot = false;
    private bool bIsShotStart = false;

    void OnEnable()
    {
        Function.LateCall((oParams) =>
        {
            if (!bIsShotStart)
            {
                CRangedObjectManager.Instance.PushRangedObject(bulletName, this.gameObject);
            }
        }, 0.1f);
    }

    void OnDisable()
    {
        if (bIsShot)
        {
            StopCoroutine(Curve());
        }

        if (bIsAirShot)
        {
            StopCoroutine(ForwardDir());
        }


        this.transform.position = Vector3.zero;
        this.fTime = 0f;
        this.bIsShot = false;
        this.bIsAirShot = false;
        this.bIsTargetAim = false;
        bIsShotStart = false;
    }

    IEnumerator Curve()
    {
        bIsShot = true;
        while (fTime < 1)
        {
            fTime += Time.deltaTime;
            var prevPos = this.transform.position;
            this.transform.position = GetCurve(point0, point1, targetPoint, fTime);

            var dir = (this.transform.position - prevPos).normalized;
            float fAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg  - 90f;
            this.transform.rotation = Quaternion.AngleAxis(fAngle, Vector3.forward);
            yield return new WaitForEndOfFrame();
        }

        if (this.bIsTargetAim)
        {
            CEffectManager.Instance.GetEffect("Boom", this.transform.position);
            CRangedObjectManager.Instance.PushRangedObject(bulletName, this.gameObject);
        }
        
        else if (this.gameObject.activeSelf && !this.bIsTargetAim)
        {
            StartCoroutine(ForwardDir());
        }

        fTime = 0f;
        bIsShot = false;
    }

    IEnumerator ForwardDir()
    {
        bIsAirShot = true;
        while(this.gameObject.activeSelf)
        {
            this.rigidbody.velocity = this.transform.up * 10f;
            yield return new WaitForEndOfFrame();
        }
        bIsAirShot = false;
    }

    public void SetPoint(Vector2 p0, Vector2 p1, Vector2 target)
    {
        bIsShotStart = true;
        point0 = p0;
        point1 = p1;
        targetPoint = target;
        this.Shot();
    }

    public void Shot()
    {
        StartCoroutine(Curve());
    }

    Vector3 GetCurve(Vector3 p0, Vector3 p1, Vector3 p2, float t)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        Vector3 p = uu * p0;
        p += 2 * u * t * p1;
        p += tt * p2;
        return p;
    }
}
