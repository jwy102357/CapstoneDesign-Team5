using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CTrap : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        IDamageable target = collision.gameObject.GetComponent<IDamageable>();

        if (target != null)
        {
            Vector3 hitnormal = collision.transform.position - this.transform.position;
            target.OnDamage(1, hitnormal);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IDamageable target = collision.gameObject.GetComponent<IDamageable>();

        if (target != null)
        {
            Vector3 hitnormal = collision.transform.position - this.transform.position;
            target.OnDamage(1, hitnormal);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        IDamageable target = collision.gameObject.GetComponent<IDamageable>();

        if (target != null)
        {
            Vector3 hitnormal = collision.transform.position - this.transform.position;
            target.OnDamage(1, hitnormal);
        }
    }
}
