using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotTrap : MonoBehaviour
{
    public float fBulletSpeed;
    public float fShotDelay;

    private float timeCount;
    private Animator animator;
    private Transform shotPivot;

    void Start()
    {
        animator = transform.GetComponent<Animator>();
        animator.speed = 1/fShotDelay;
        shotPivot = transform.Find("ShotPivot").GetComponent<Transform>();
    }
    
    public void TrapShot()
    {
        var firstbullet = CRangedObjectManager.Instance.GetRangedObject("EnemyBullet");
        firstbullet.transform.position = shotPivot.position;

        firstbullet.GetComponent<Rigidbody2D>().velocity = -transform.right.normalized * fBulletSpeed;
    }
}
