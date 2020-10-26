using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ArmLaser 처럼 피격판정 수정
public class CC1BOLowerPunch : MonoBehaviour
{
    public Vector3 basePosition;
    public float punchMaxX;
    public float fCurrentHealth;
    public float flashTime = 0.2f;
    private CC1BOLowerArm myArm;
    private Shader flashShader;
    private Shader originShader;
    private SpriteRenderer spriteRenderer;
    private bool bIsRocketPunch = false;

    public void Awake()
    {
        basePosition = this.transform.localPosition;
        myArm = this.GetComponentInParent<CC1BOLowerArm>();
        fCurrentHealth = myArm.fCurrentHealth;
        this.flashShader = Shader.Find("GUI/Text Shader");
        this.originShader = Shader.Find("Sprites/Default");
        this.spriteRenderer = this.GetComponent<SpriteRenderer>();
    }

    public IEnumerator PunchMove(IBasicAI enemyAI, int patternIndex)
    {
        float fTime = 0f;
        bIsRocketPunch = true;
        while (fTime < 1f)
        {
            fTime += Time.deltaTime * 0.3f;
            this.transform.position = Vector3.Lerp(this.transform.position, new Vector3(punchMaxX, myArm.transform.position.y, this.transform.position.z), fTime);
            yield return new WaitForEndOfFrame();
        }

        bIsRocketPunch = false;
        this.RockectReturn(enemyAI, patternIndex);
        yield break;
    }

    public IEnumerator PunchReturn(Vector3 endPos, IBasicAI enemyAI, int patternIndex)
    {
        float fTime = 0f;
        while (fTime < 1f)
        {
            fTime += Time.deltaTime * 1.5f;
            this.transform.localPosition = Vector3.Lerp(this.transform.localPosition, endPos, fTime);
            yield return new WaitForEndOfFrame();
        }
        
        this.transform.localPosition = new Vector3(this.transform.localPosition.x, -0.2f, this.transform.localPosition.z);
        fTime = 0f;

        enemyAI.AttackEnd();
        enemyAI.IdleTimeChk(enemyAI.fIdleTime);
        enemyAI.LivingEntity.gameObject.SendMessage("PatternCoolDownSetting", patternIndex);
        myArm.ArmAttackEnd();

        bIsRocketPunch = false;
        yield break;
    }

    public void RocketPunch(IBasicAI enemyAI, int patternIndex)
    {
        StartCoroutine(PunchMove(enemyAI, patternIndex));
    }

    public void RockectReturn(IBasicAI enemyAI, int patternIndex)
    {
        StartCoroutine(PunchReturn(new Vector3(basePosition.x, basePosition.y, basePosition.z), enemyAI, patternIndex));
    }

    public IEnumerator FlashSprite()
    {
        this.spriteRenderer.material.shader = this.flashShader;
        this.spriteRenderer.material.color = Color.white;
        yield return new WaitForSeconds((flashTime + Time.deltaTime) * 0.5f);
        this.spriteRenderer.material.shader = this.originShader;
        this.spriteRenderer.material.color = Color.white;
        yield return new WaitForSeconds((flashTime + Time.deltaTime) * 0.5f);

        yield break;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!myArm.IsDead)
        {
            if (collision.transform.CompareTag(KDefine.TAG_PLAYER) && bIsRocketPunch)
            {
                IDamageable target = collision.GetComponent<IDamageable>();
                if (target != null)
                {
                    if (myArm.fDamage != 0)
                    {
                        target.OnDamage(myArm.fDamage, Vector3.zero);
                    }
                }
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!myArm.IsDead)
        {
            if (collision.transform.CompareTag(KDefine.TAG_PLAYER) && bIsRocketPunch)
            {
                IDamageable target = collision.GetComponent<IDamageable>();
                if (target != null)
                {
                    if (myArm.fDamage != 0)
                    {
                        target.OnDamage(myArm.fDamage, Vector3.zero);
                    }
                }
            }
        }
    }

}
