using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class CInput : MonoBehaviour
{
    public CAttackButton AttackButton;
    public CJumpButton JumpButton;
    public float fMove;
    public float fMobileMove;
    public float fSpeed = 30f;
    private Dictionary<int, KeyValuePair<string, Image>> oDict;
    private List<KeyValuePair<int,string>> pTouchList;
    private const string oRightTag = "RightDirection";
    private const string oLeftTag = "LeftDirection";
    private int UILayer;

    public CAvoidButton AvoidButton;
    
    public void Awake()
    {
        AttackButton = GameObject.Find("AttackButton").GetComponent<CAttackButton>();
        JumpButton = GameObject.Find("JumpButton").GetComponent<CJumpButton>();
        AvoidButton = GameObject.Find("AvoidButton").GetComponent<CAvoidButton>();
        UILayer = (1 << LayerMask.NameToLayer(KDefine.LAYER_UI));
        pTouchList = new List<KeyValuePair<int, string>>();
        oDict = new Dictionary<int, KeyValuePair<string, Image>>();
        Mathf.Clamp(fMobileMove, -1, 1);
    }

    public void Update()
    {
        fMove = Input.GetAxis("Horizontal");

        for (int i = 0; i < Input.touchCount; i++)
        {
            var touch = Input.GetTouch(i);
            
            if (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
            {
                if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                {
                    var camera = Function.FindComponent<Camera>("UI Camera");
                    var pos = camera.ScreenToWorldPoint(touch.position);
                    var hit = Physics2D.Raycast(pos, transform.forward, 15.0f, UILayer);
                    if (hit)
                    {
                        if (hit.transform.CompareTag(KDefine.TAG_RIGHTDIRECTION))
                        {
                            var image = hit.transform.GetComponent<Image>();
                            image.color = Color.gray;
                            var keyPair = new KeyValuePair<string, Image>(oRightTag, image);

                            if (!oDict.ContainsKey(i))
                            {
                                oDict.Add(i, keyPair);
                            }

                            else if (oDict.ContainsKey(i))
                            {
                                if (oDict[i].Key != KDefine.TAG_RIGHTDIRECTION)
                                {
                                    oDict[i].Value.color = Color.black;
                                    oDict[i] = keyPair;
                                }
                            }
                            fMobileMove = Mathf.MoveTowards(fMobileMove, this.CalMoveDirection(oRightTag), fSpeed * Time.deltaTime);
                        }

                        else if (hit.transform.CompareTag(KDefine.TAG_LEFTDIRECTION))
                        {
                            var image = hit.transform.GetComponent<Image>();
                            image.color = Color.gray;
                            var keyPair = new KeyValuePair<string, Image>(oLeftTag, image);
                            
                            if (!oDict.ContainsKey(i))
                            {
                                oDict.Add(i, keyPair);
                            }

                            else if (oDict.ContainsKey(i))
                            {
                                if (oDict[i].Key != KDefine.TAG_LEFTDIRECTION)
                                {
                                    oDict[i].Value.color = Color.black;
                                    oDict[i] = keyPair;
                                }
                            }

                            fMobileMove = Mathf.MoveTowards(fMobileMove, this.CalMoveDirection(oLeftTag), fSpeed * Time.deltaTime);
                        }

                    }

                    else
                    {
                        if (oDict.ContainsKey(i))
                        {
                            oDict[i].Value.color = Color.black;
                            oDict.Remove(i);
                        }
                    }
                }

                else
                {
                    if (oDict.ContainsKey(i))
                    {
                        oDict[i].Value.color = Color.black;
                        oDict.Remove(i);
                    }
                }

                
            }

            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                if (oDict.ContainsKey(i))
                {
                    oDict[i].Value.color = Color.black;
                    oDict.Remove(i);
                }
                Debug.Log("CANCLE");
            }
        }

        if (Input.touchCount <= 0)
        {
            oDict.Clear();
        }

        //새로 추가된부분 문제시 삭제
        if (Input.touchCount > 0)
        {
            if (oDict.Count > 0)
            {
                int cnt = 0;
                for (int i = 0; i < oDict.Count; i++)
                {
                    if (oDict[i].Key == KDefine.TAG_RIGHTDIRECTION || oDict[i].Key == KDefine.TAG_LEFTDIRECTION)
                    {
                        cnt++;
                    }
                }

                if (cnt <= 0)
                {
                    oDict.Clear();
                }
            }
        }

        if (oDict.Count <= 0)
        {
            fMobileMove = Mathf.MoveTowards(fMobileMove, 0, fSpeed * Time.deltaTime);
        }

        

        if (Input.GetKeyDown(KeyCode.Space))
        {
            this.Jump();
        }
    }

    public void Jump()
    {
        JumpButton.IsJump = true;
        StartCoroutine(JumpEnd());
    }

    IEnumerator JumpEnd()
    {
        yield return new WaitForEndOfFrame();
        JumpButton.IsJump = false;
    }

    private float CalMoveDirection(string dirTag)
    {
        var result = dirTag == oRightTag ? 1 : -1;
        return result;
    }
}
