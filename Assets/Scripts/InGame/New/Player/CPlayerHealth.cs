using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class CPlayerHealth : CLivingEntity
{
    private CMovement movement;
    private CShooter shooter;
    public CHeartUI heartUI;
    public Animator animator;

    public override void Awake()
    {
        base.Awake();
        animator = this.GetComponent<Animator>();
        this.fCurrentHealth = this.fStartHealth;
        movement = this.GetComponent<CMovement>();
        shooter = this.GetComponent<CShooter>();
        this.flashTime = 0.25f;

        if (ES3.KeyExists("CurrentHealth", "Player.es3"))
        {
            fCurrentHealth = ES3.Load<float>("CurrentHealth", "Player.es3");
            fMaxHealth = ES3.Load<float>("MaxHealth", "Player.es3");
            shield = ES3.Load<int>("Shield", "Player.es3");
        }

        else
        {
            ES3.Save<int>("Shield", shield, "Player.es3");
            ES3.Save<float>("CurrentHealth", fCurrentHealth, "Player.es3");
            ES3.Save<float>("MaxHealth", fMaxHealth, "Player.es3");
        }

        heartUI = GameObject.Find("Heart_UI").GetComponent<CHeartUI>();
        heartUI.SetHeart((int)fCurrentHealth, (int)fMaxHealth, shield);
        onDeath += EndGame;
    }

    public override void RestoreHealth(float NewHealth = 1f)
    {
        heartUI.AddHeart(((int)fCurrentHealth + (int)NewHealth) <= (int)fMaxHealth ? (int)NewHealth : (int)(fMaxHealth - fCurrentHealth));
        base.RestoreHealth(NewHealth);
        ES3.Save<float>("CurrentHealth", fCurrentHealth, "Player.es3");
    }

    public void AddShield(int num = 2)
    {
        shield+=num;
        heartUI.AddShield(num);
        ES3.Save<int>("Shield", shield, "Player.es3");
    }

    public override void OnDamage(float damage, Vector3 hitPos, bool isGuard = false)
    {
        if (!movement.bIsDash)
        {
            if (!this.bIsImmortal)
            {
                base.OnDamage(damage, hitPos);
                heartUI.SubtractHeart((int)damage);
                ES3.Save<int>("Shield", shield, "Player.es3");
                ES3.Save<float>("CurrentHealth", fCurrentHealth, "Player.es3");
            }
        }
    }

    public override void Death()
    {
        base.Death();
        animator.SetBool("isDeath", true);
        movement.rigidbody.velocity = new Vector2(0f, 0f);
        movement.rigidbody.isKinematic = true;
        if (this.IsDead)
        {
            movement.enabled = false;
            shooter.enabled = false;
        }
    }

    public void EndGame()
    {
        //CFadeInOut.FadeOut(1.1f);
        //Invoke("GoLobby", 1f);
        Initiate.Fade("LobbyScene", Color.black, 1f);
        CEnemyManager.Instance.PushGameEndObject();
        CStageManager.Instance.ReUseStageSetting();
        CStageManager.Instance.PushStageObject(CStageManager.Instance.CurrentStage.name, CStageManager.Instance.CurrentStage.gameObject);
        ES3.Save<int>("Shield", 0, "Player.es3");
        Destroy(this.gameObject);
    }

    public void GoLobby()
    {
        //SceneManager.LoadScene("LobbyScene");
    }

    public void AddMaxHealth(int num = 2)
    {
        fMaxHealth += num;
        heartUI.SetHeart((int)fCurrentHealth, (int)fMaxHealth, shield);
        ES3.Save<float>("MaxHealth", fMaxHealth, "Player.es3");
    }

    public void SynchronizeUI()
    {
        if (ES3.KeyExists("CurrentHealth", "Player.es3"))
        {
            fCurrentHealth = ES3.Load<float>("CurrentHealth", "Player.es3");
            fMaxHealth = ES3.Load<float>("MaxHealth", "Player.es3");
            shield = ES3.Load<int>("Shield", "Player.es3");
        }

        else
        {
            ES3.Save<int>("Shield", shield, "Player.es3");
            ES3.Save<float>("CurrentHealth", fCurrentHealth, "Player.es3");
            ES3.Save<float>("MaxHealth", fMaxHealth, "Player.es3");
        }
        heartUI.SetHeart((int)fCurrentHealth, (int)fMaxHealth, shield);
    }

}
