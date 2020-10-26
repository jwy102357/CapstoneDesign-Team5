using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CWeaponSelectLock : MonoBehaviour
{
    private CWeaponSelectPopup selectPopup;
    private bool bNowUnlocking;
    public List<Sprite> lockAnimation = new List<Sprite>();
    private Sprite[] unlockSprite;
    public Image lockImage;

    public void Setting()
    {
        selectPopup = transform.parent.GetComponent<CWeaponSelectPopup>();
        lockImage = this.GetComponent<Image>();
        unlockSprite = Resources.LoadAll<Sprite>("Sprites/Lobby/Interface/LockAnim");
        lockAnimation.Add(Resources.Load<Sprite>("Sprites/Lobby/Interface/Lock"));
        for(int i = 0; i < unlockSprite.Length; i++)
        {
            lockAnimation.Add(unlockSprite[i]);
        }
        bNowUnlocking = false;
    }

    public void Unlock()
    {
        lockImage.enabled = true;
        lockImage.sprite = lockAnimation[0];
        StartCoroutine("UnlockAnima");
    }

    public void NoLock()
    {
        if (bNowUnlocking)
        {
            StopCoroutine("UnlockAnima");
        }
        lockImage.enabled = false;
    }

    public void Lock()
    {
        if (bNowUnlocking)
        {
            StopCoroutine("UnlockAnima");
        }
        lockImage.sprite = lockAnimation[0];
        lockImage.enabled = true;
    }

    public IEnumerator UnlockAnima()
    {
        bNowUnlocking = true;
        yield return new WaitForSecondsRealtime(0.1f);
        lockImage.sprite = lockAnimation[1];
        yield return new WaitForSecondsRealtime(0.7f);
        lockImage.sprite = lockAnimation[2];
        yield return new WaitForSecondsRealtime(0.1f);
        lockImage.sprite = lockAnimation[3];
        yield return new WaitForSecondsRealtime(0.1f);
        lockImage.sprite = lockAnimation[4];
        yield return new WaitForSecondsRealtime(0.1f);
        lockImage.sprite = lockAnimation[5];
        yield return new WaitForSecondsRealtime(0.5f);
        lockImage.enabled = false;
        yield return new WaitForSecondsRealtime(0.3f);
        lockImage.enabled = true;
        yield return new WaitForSecondsRealtime(0.3f);
        lockImage.enabled = false;
        yield return new WaitForSecondsRealtime(0.3f);
        lockImage.enabled = true;
        yield return new WaitForSecondsRealtime(0.3f);
        lockImage.enabled = false;
        selectPopup.EndUnlock();
        bNowUnlocking = false;
    }
}
