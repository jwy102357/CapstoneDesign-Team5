using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CALAP01EnemyController : CRangedEnemyController
{
    private IRandomMoveAI randomAI;
    private Vector3 currentDir;
    public override void Awake()
    {
        base.Awake();
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer(KDefine.LAYER_ENEMY), LayerMask.NameToLayer(KDefine.LAYER_ENEMYBULLET));
    }

    public void Update()
    {
        if (randomAI.IsMoving)
        {
            this.MoveForwardObject(currentDir);
        }
    }

    public override void TurnObject()
    {
        base.TurnObject();
    }

    public override void MoveForwardObject(Vector3 moveDirection)
    {
        base.MoveForwardObject(moveDirection);
        if (currentDir != moveDirection)
        {
            currentDir = moveDirection;
        }

    }

    public override void AttackObject(int nDiceValue)
    {
        base.AttackObject(nDiceValue);
    }

    void Shot()
    {
        if (this.enemyAI.Target != null)
        {
            CEnemyPatternManager.Instance.PlayerDirBulletAttack(this.transform.position + new Vector3(0f, 0.2f, 0f), this.fBulletSpeed);
        }
        
    }

    public override void OnDisable()
    {
        base.OnDisable();
        currentDir = Vector3.zero;
        randomAI = null;
    }

    public override void OnEnable()
    {
        base.OnEnable();
        randomAI = enemyObject.GetComponent<IRandomMoveAI>();
    }
}
