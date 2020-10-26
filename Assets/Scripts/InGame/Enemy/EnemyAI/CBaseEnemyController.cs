using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CBaseEnemyController : CLivingEntity
{
    protected IController enemyController;
    protected IBasicAI enemyAI;
    public GameObject enemyObject;
    public Animator animator;
    public float fAtk;
    public float fSpeed;
    public CEnemySight Sight { get; set; }
    public bool bIsBuff = false;

    private float fOriginSpeed;
    private float fOriginAtk;

    public override void Awake()
    {   
        base.Awake();
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer(KDefine.LAYER_ENEMY), LayerMask.NameToLayer(KDefine.LAYER_PLAYER), true);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer(KDefine.LAYER_ENEMY), LayerMask.NameToLayer(KDefine.LAYER_ENEMY), true);

        fOriginSpeed = fSpeed;
        fOriginAtk = fAtk;
        onDeath += Die;
    }

    public virtual void Die()
    {
        CEffectManager.Instance.GetEffect("DeathBoom_01", this.transform.position + new Vector3(0f, 0.4f, 0f));
        CMerchandiseManager.Instance.ManyCoinCreate(1+(int)fMaxHealth/100, 1+(int)fMaxHealth/30, this.transform.position);
        if(20 + (fMaxHealth / 50) * (fMaxHealth / 50) > Random.Range(1, 100)){
            CItemManager.Instance.MaterialItemCreate(this.transform.position);
        }
        if(Random.Range(1, 101) < 10)
        {
            CItemManager.Instance.HeartItemCreate(this.transform.position);
        }
        this.gameObject.SetActive(false);
        CEnemyManager.Instance.PushEnemyObject(gameObject.name, this.gameObject);
    }

    public virtual void TurnObject()
    {
        if (enemyAI.Target != null)
        {
            var dir = enemyAI.Target.transform.position.x - this.transform.position.x;
            if ((dir > 0 && !enemyAI.FacingRight) || (dir < 0 && enemyAI.FacingRight))
            {
                enemyAI.FacingRight = !enemyAI.FacingRight;
                animator.SetBool("FacingRight", enemyAI.FacingRight);
                enemyObject.transform.localScale = new Vector3(enemyObject.transform.localScale.x * -1f, enemyObject.transform.localScale.y * 1f, 1f);
            }
        }

        else if (enemyAI.Target == null)
        {
            enemyAI.FacingRight = !enemyAI.FacingRight;
            animator.SetBool("FacingRight", enemyAI.FacingRight);
            enemyObject.transform.localScale = new Vector3(enemyObject.transform.localScale.x * -1f, enemyObject.transform.localScale.y * 1f, 1f);
        }

    }
    public virtual void MoveForwardObject(Vector3 moveDirection)
    {
        animator.SetBool("Move", enemyAI.IsMoving);
        animator.SetBool("Attack", enemyAI.IsAttack);
        if (animator.GetBool("Move") && !animator.GetBool("Attack"))
        {
            this.transform.Translate(moveDirection * (this.enemyAI.Speed * Time.deltaTime));
        }
    }

    public virtual void MoveForwardObject()
    {
        animator.SetBool("Move", enemyAI.IsMoving);
        animator.SetBool("Attack", enemyAI.IsAttack);
        if (animator.GetBool("Move") && !animator.GetBool("Attack"))
        {
            this.transform.Translate(GetDirection() * (this.enemyAI.Speed * Time.deltaTime));
        }
    }

    public virtual void AttackObject(int nDiceValue)
    {
        animator.SetBool("Move", enemyAI.IsMoving);
        animator.SetInteger("AttackState", nDiceValue);
    }

    public void SpeedUp()
    {
        if (!bIsBuff)
        {
            enemyAI.Speed *= 1.5f;
            bIsBuff = true;
        }
    }

    public void ReturnToOriginSpeed()
    {
        this.bIsBuff = false;
        enemyAI.Speed = fOriginSpeed;
    }

    public void ReturnToOriginAtk()
    {
        this.bIsBuff = false;
        fAtk = fOriginAtk;
    }

    public Vector3 GetDirection()
    {
        return this.enemyAI.FacingRight ? Vector3.right : Vector3.left;

    }

    public int GetDirectionToInt()
    {
        return this.enemyAI.FacingRight ? 1 : -1;
    }

    public virtual void AttackTime() { }

    public virtual void AttackTimeEnd() { }

    public virtual void OnEnable()
    {
        if (!enemyObject)
        {
            enemyObject = this.gameObject;
        }
        if (this.IsDead) this.IsDead = false;
        
        if (enemyObject)
        {
            this.Sight = this.GetComponentInChildren<CEnemySight>();
            enemyController = enemyObject.GetComponent<IController>();
            enemyAI = enemyObject.GetComponent<IBasicAI>();
            if (this.GetComponent<SpriteRenderer>())
            {
                this.SetSpriteOrigin();
            }
            enemyAI.LivingEntity = this;
            enemyAI.Speed = this.fSpeed;
            enemyController.AtkEvent += this.AttackObject;
            enemyController.MoveEvent += this.MoveForwardObject;
            enemyController.TurnEvent += this.TurnObject;
            enemyController.RandomMoveEvent += this.MoveForwardObject;
            enemyAI.FacingRight = false;
            enemyAI.EnemySight = this.Sight;
            animator = this.GetComponent<Animator>();
        }

        this.fCurrentHealth = fMaxHealth;
        this.IsDamaged = false;
    }

    public virtual void OnDisable()
    {
        this.ReturnToOriginAtk();
        this.ReturnToOriginSpeed();
        enemyController = null;
        enemyAI = null;
        this.Sight.IsAttackRange = false;
        this.Sight = null;
        this.bIsBuff = false;
    }
}

