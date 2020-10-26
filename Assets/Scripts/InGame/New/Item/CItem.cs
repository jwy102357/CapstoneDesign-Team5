using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CItem : MonoBehaviour
{
    public float gravityScale = 0.5f;
    protected Rigidbody2D rigidbody;
    protected bool bIsGrounded = false;
    protected bool bIsContact = false;
    protected Transform playerTransform;
    public string itemName;

    public IEnumerator ItemFloating()
    {
        float fYpos = transform.position.y;
        while (transform.position.y < fYpos + 0.3f)
        {
            transform.Translate(Vector3.up * Time.deltaTime * 0.9f);
            yield return new WaitForSeconds(0.034f);
        }
        while (transform.position.y > fYpos + 0.1f)
        {
            transform.Translate(Vector3.down * Time.deltaTime * 0.9f);
            yield return new WaitForSeconds(0.034f);
        }
        rigidbody.gravityScale = gravityScale;
        bIsGrounded = false;
    }

    public IEnumerator Magnetic()
    {
        gameObject.layer = 18;

        while (true)
        {
            rigidbody.velocity = (playerTransform.position + new Vector3(0f, 0.6f, 0f) - transform.position).normalized * 10f;
            yield return new WaitForSeconds(0.1f);
        }
    }
}
