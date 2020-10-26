using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CLivingEntity : MonoBehaviour, IDamageable
{
    public float fCurrentHealth;
    public float fStartHealth;
    public float fMaxHealth;
    public float fDef;
    public event Action onDeath;
    public bool IsDead { get; set; }
    public bool IsDamaged { get; set; }
    public int shield = 0;
    public float flashTime = 0.2f;

    protected bool bIsImmortal = false;
    protected SpriteRenderer spriteRenderer;
    private Shader flashShader;
    private Shader originShader;

    public virtual void Awake()
    {
        this.flashShader = Shader.Find("GUI/Text Shader");
        this.originShader = Shader.Find("Sprites/Default");
        this.spriteRenderer = this.GetComponent<SpriteRenderer>();
    }

    public virtual void OnDamage(float damage, Vector3 hitPos, bool isGuard = false)
    {
        if (shield > 0) shield--;
        else
        {
            fCurrentHealth -= damage;
        }
        //fCurrentHealth -= damage;

        IsDamaged = true;
        if (!isGuard && this.gameObject.activeSelf)
        {
            StartCoroutine(FlashSprite());
        }

        if (fCurrentHealth <= 0 && !IsDead)
        {
            Death();
        }
    }

    public virtual void RestoreHealth(float fNewHealth)
    {
        if (IsDead)
        {
            return;
        }
        fCurrentHealth = (fCurrentHealth + fNewHealth) >= fMaxHealth ? fMaxHealth : (fCurrentHealth + fNewHealth);
    }

    public virtual void Death()
    {
        StopCoroutine(FlashSprite());
        this.SetSpriteOrigin();
        IsDead = true;

        if (onDeath != null)
        {
            onDeath();
        }
    }

    IEnumerator FlashSprite()
    {
        bIsImmortal = true;

        if (this.gameObject.CompareTag(KDefine.TAG_ENEMY))
        {
            this.spriteRenderer.material.shader = this.flashShader;
            this.spriteRenderer.material.color = Color.white;
            yield return new WaitForSeconds(flashTime * 0.5f);
            this.spriteRenderer.material.shader = this.originShader;
            this.spriteRenderer.material.color = Color.white;
            yield return new WaitForSeconds(flashTime * 0.5f);
        }

        if (this.gameObject.CompareTag(KDefine.TAG_PLAYER))
        {
            for (int i = 0; i < 4; i++)
            {
                this.spriteRenderer.material.shader = this.flashShader;
                this.spriteRenderer.material.color = Color.white;
                yield return new WaitForSeconds(flashTime * 0.5f);
                this.spriteRenderer.material.shader = this.originShader;
                this.spriteRenderer.material.color = Color.white;
                yield return new WaitForSeconds(flashTime * 0.5f);
            }
        }

        bIsImmortal = false;
        IsDamaged = false;
        
        yield break;
    }

    public void SetSpriteOrigin()
    {
        this.spriteRenderer.material.shader = this.originShader;
        this.spriteRenderer.material.color = Color.white;
    }
}
