using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMovement : MonoBehaviour
{
    public bool bIsFacingRight;
    public bool isWalk;
    public bool dashLock;
    public float fMoveSpeed;
    public Animator animator;
    //public CDirectionPad directionPad;
    public bool bIsDash = false;
    public float jumpDownPower = 20;
    public float fDashCoolDown = 0.0f;
    public CInput playerInput;
    public int nJumpPower;
    public float fDashPower;
    public Vector3 inputVector;
    public GameObject attackPivot;
    public Rigidbody2D rigidbody;
    public Collider2D myCollider;
    public Coroutine dashCoroutine;
    public Collider2D platformcollider;
    public CInteractable interactPopup;

    private int nJumpCount = 0;
    private int nJumpCountLimit = 2;
    private float lastDashLockTime = 0f, timer = 0f;
    private bool jumpUp = true;
    private bool isGrounded = true;
    private int dashCounter = 0;
    private CShooter shooter;
    private List<IInteractItem> interactItems;
    private Dictionary<IInteractItem, bool> interactList;

    private bool isWakeup = false;
    private bool isEnterPortal = false;

    void Awake()
    {
        playerInput = this.GetComponent<CInput>();
        rigidbody = this.GetComponent<Rigidbody2D>();
        myCollider = this.GetComponent<CapsuleCollider2D>();
        shooter = this.GetComponent<CShooter>();
        interactItems = new List<IInteractItem>();
        interactList = new Dictionary<IInteractItem, bool>();
    }

    private void Start()
    {
        bIsFacingRight = true;
        inputVector = Vector3.zero;
        //fMoveSpeed = 250;
        //nJumpPower = 800;
        fDashPower = 0.3f;
    }

    void LateUpdate()
    {
        if(isWakeup || isEnterPortal)
        {
            return;
        }
        if(playerInput.AvoidButton.bDashButton)
        {
            Dash();
        }
        
        if (playerInput.fMove != 0 || playerInput.fMobileMove != 0 || inputVector.x != 0)
        {
            if (!bIsDash)
            {
                this.Move();
            }
        }
        else
        {
            dashLock = false;
            this.Idle();
        }
        if ((playerInput.JumpButton.IsJump) && (nJumpCount < nJumpCountLimit))
        {
            if (!bIsDash)
            {
                this.Jump();
            }
        }
        if (timer < 10000) timer += Time.deltaTime;
        else
        {
            timer = 0;
            lastDashLockTime -= 10000;
        }
        if (timer - lastDashLockTime > 0.4f)
        {
            dashCounter = 0;
        }

        if (playerInput.AttackButton.IsInteract)
        {
            //문제시 백업본에서 복붙
            if (interactPopup != null)
            {
                interactPopup.InteractAction();
            }
        }
    }

    protected void Idle()
    {
        animator.SetBool("isWalk", false);
        rigidbody.velocity = new Vector2(0, rigidbody.velocity.y);
    }

    void Move()
    {
        if (playerInput.fMove < 0 || playerInput.fMobileMove < 0)
        {
            bIsFacingRight = false;
        }
        else if (playerInput.fMove > 0 || playerInput.fMobileMove > 0)
        {
            bIsFacingRight = true;
        }

        transform.localScale = new Vector3(bIsFacingRight ? 1f : -1f, 1f, 1f);
        rigidbody.velocity = new Vector2((playerInput.fMove + playerInput.fMobileMove)* fMoveSpeed * Time.deltaTime, rigidbody.velocity.y);

        if (dashLock == false)
        {
            dashLock = true;
            lastDashLockTime = timer;
            if (bIsFacingRight)
            {
                if (dashCounter != 1) dashCounter = 1;
                else
                {
                    dashCounter = 0;
                    Dash();
                }
            }
            else
            {
                if (dashCounter != -1) dashCounter = -1;
                else
                {
                    dashCounter = 0;
                    Dash();
                }
            }
        }
        isWalk = true;
        //animator.SetBool("DirectionRight", bIsFacingRight);
        animator.SetBool("isWalk", isWalk);
    }

    protected void Jump()
    {
        //jumpUp = directionPad.bIsDownJump ? false : true;
        jumpUp = true;

        if (jumpUp)
        {
            rigidbody.velocity = Vector2.zero;
            rigidbody.AddForce(new Vector2(0, nJumpPower));

        }
        else if (isGrounded && platformcollider.CompareTag(KDefine.TAG_PLATFORM))
        {
            StartCoroutine(JumpDown());
            rigidbody.velocity = Vector2.zero;
            rigidbody.AddForce(new Vector2(0, -jumpDownPower));
        }
        else
        {
            return;
        }

        nJumpCount++;
        isGrounded = false;

        animator.SetBool("JumpUp", jumpUp);
        animator.SetBool("Grounded", isGrounded);
    }

    protected void Dash()
    {
        if (!bIsDash)
        {
            if (transform.parent != null)
            {
                transform.parent = null;
            }
            dashCoroutine = StartCoroutine(DashPlayer());
        }
    }

    private IEnumerator DashPlayer()
    {
        bIsDash = true;
        animator.SetBool("isDash", bIsDash);
        shooter.enabled = false;
        for (int i = 1; i <= 7; i++)
        {
            if (bIsDash)
            {
                rigidbody.MovePosition(new Vector2(transform.position.x + (bIsFacingRight ? 1 * fDashPower : -1 * fDashPower), rigidbody.transform.position.y));
                yield return new WaitForSeconds(0.04f);
            }
        }
    }

    public void EndOfDash()
    {
        bIsDash = false;
        animator.SetBool("isDash", bIsDash);
        shooter.enabled = true;
        this.Idle();
    }
    /*
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("TrEnter");
        if (collision.CompareTag("Tile") || collision.CompareTag("Platform"))
        {
            if (rigidbody.velocity.y <= 0.1)
            {
                isGrounded = true;
                animator.SetBool("Grounded", isGrounded);
                nJumpCount = 0;
            }
        }
    }*/
    /*
    private void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log("TrStay");
        if (collision.CompareTag("Tile") || collision.CompareTag("Platform"))
        {
            if(rigidbody.velocity.y <= 0.1)
            {
                isGrounded = true;
                animator.SetBool("Grounded", isGrounded);
                nJumpCount = 0;
            }
        }
    }*/
    
    private void OnTriggerExit2D(Collider2D collision)
    {
        //문제시 백업본에서 복붙
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Platform"))
        {
            //Debug.Log("Platform OnCollisionEnter");
            //Debug.Log(collision.contacts[0].normal.y);
        }
        
        if (collision.contacts[0].normal.y > 0.7f)
        {
            if (collision.gameObject.CompareTag("Tile") || collision.gameObject.CompareTag("Platform"))
            {
                isGrounded = true;
                animator.SetBool("Grounded", isGrounded);
                nJumpCount = 0;
            }
        }

        if (collision.contacts[0].normal.y == -1f)
        {
            rigidbody.AddForce(new Vector2(0, -1f));
        }

        platformcollider = collision.collider;
        animator.SetBool("Grounded", isGrounded);
        if(collision.gameObject.CompareTag(KDefine.TAG_PUSHABLE)) 
        {
            Debug.Log("Detect push");
            if ((collision.contacts[0].normal.y <= 0.7 || collision.contacts[0].normal.x >= -0.7) && collision.contacts[0].normal.y <= 0.7f)
            {
                collision.gameObject.GetComponent<IPushable>().Push(playerInput.fMove + playerInput.fMobileMove);
                Debug.Log("Success");
                shooter.enabled = false;
            }
        }
    }
    /*
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(KDefine.TAG_PUSHABLE))
        {
            if ((collision.contacts[0].normal.y <= 0.7 || collision.contacts[0].normal.x >= -0.7) && collision.contacts[0].normal.y <= 0.7f)
            {
                collision.gameObject.GetComponent<IPushable>().Push(playerInput.fMove + playerInput.fMobileMove);
            }
        }
    }*/

    private void OnCollisionExit2D(Collision2D collision)
    {
        if(!shooter.enabled) shooter.enabled = true;
        IPushable target = collision.gameObject.GetComponent<IPushable>();
        if(target != null)
        {
            collision.gameObject.GetComponent<IPushable>().IsContact = false;
        }
    }

    private void JumpUpEnd()
    {
        jumpUp = false;
        animator.SetBool("JumpUp", jumpUp);
    }

    private IEnumerator JumpDown()
    {
        Physics2D.IgnoreCollision(myCollider, platformcollider);
        yield return new WaitForSeconds(0.2f);
        Physics2D.IgnoreCollision(myCollider, platformcollider, false);

    }

    public void WakeUp()
    {
        isWakeup = true;
        shooter.weapon.gameObject.SetActive(false);
        animator.SetBool("WakeUp", isWakeup);
    }

    public void WakeUpEnd()
    {
        isWakeup = false;
        shooter.weapon.gameObject.SetActive(true);
        animator.SetBool("WakeUp", isWakeup);
    }

    public void SetDirection(int i)
    {
        transform.localScale = new Vector3(i, 1, 1);
    }

    public void EnterPortal()
    {
        rigidbody.velocity = Vector3.zero;
        isEnterPortal = true;
        shooter.weapon.gameObject.SetActive(false);
        animator.SetBool("EnterPortal", true);
    }

    public void EnterPortalEnd()
    {
        isEnterPortal = false;
        shooter.weapon.gameObject.SetActive(true);
        animator.SetBool("EnterPortal", false);
    }
}
