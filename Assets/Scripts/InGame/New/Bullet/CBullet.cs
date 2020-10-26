using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CBullet : MonoBehaviour
{
    protected Rigidbody2D rigidbody;
    public string bulletName;
    public float fDamage;
    public float fDistance;
    protected bool bIsEnemyBullet;
    private Vector3 startPos;

    public virtual void Awake()
    {
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer(KDefine.LAYER_PLAYERBULLET), LayerMask.NameToLayer(KDefine.LAYER_ENEMYBULLET), true);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer(KDefine.LAYER_ENEMYBULLET), LayerMask.NameToLayer(KDefine.LAYER_ENEMYBULLET), true);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer(KDefine.LAYER_PLAYERBULLET), LayerMask.NameToLayer(KDefine.LAYER_PLAYERBULLET), true);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer(KDefine.LAYER_ENEMY), LayerMask.NameToLayer(KDefine.LAYER_ENEMYBULLET));

        rigidbody = this.GetComponent<Rigidbody2D>();
    }

    public void FixedUpdate()
    {
        if (this.gameObject.activeSelf && !bIsEnemyBullet)
        {
            if (Vector2.SqrMagnitude(startPos - this.transform.position) >= fDistance * fDistance)
            {
                //CEffectManager.Instance.GetEffect("Boom", this.transform.position); //수정필요
                CRangedObjectManager.Instance.PushRangedObject(bulletName, this.gameObject);
            }
        }
    }

    public void SetStartPos()
    {
        startPos = this.transform.position;
    }

}
