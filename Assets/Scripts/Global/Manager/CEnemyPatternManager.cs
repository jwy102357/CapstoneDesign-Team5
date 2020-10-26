using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;

public class CEnemyPatternManager : MonoBehaviour
{
    //로비씬에서 추가한다.
    private static CEnemyPatternManager instance;
    private int groundLayerMask;
    private int groundWallLayerMask;
    public GameObject playerObj;

    public Vector3 failPos;

    void Awake()
    {
        if (playerObj == null)
        {
            playerObj = GameObject.Find("Player");
            groundLayerMask = (1 << LayerMask.NameToLayer(KDefine.LAYER_PLATFORM)) + (1 << LayerMask.NameToLayer(KDefine.LAYER_GROUND));
            groundWallLayerMask = (1 << LayerMask.NameToLayer(KDefine.LAYER_WALL)) + (1 << LayerMask.NameToLayer(KDefine.LAYER_GROUND));

            failPos = new Vector3(-100f, -100f, -1500f);
        }
    }

    public static CEnemyPatternManager Instance
    {
        get
        {
            if (instance == null)
            {
                var gameObject = new GameObject(typeof(CEnemyPatternManager).ToString());
                gameObject.AddComponent<CEnemyPatternManager>();
                instance = FindObjectOfType<CEnemyPatternManager>();
            }

            return instance;
        }
    }

    public void GroundAttack(Vector3 pos, string bulletName)
    {
        var bullet = CRangedObjectManager.Instance.GetRangedObject(bulletName);
        bullet.GetComponent<CGroundBullet>().bulletName = bulletName;
        bullet.transform.position = pos;
    }

    public Vector3 MakeDangerPosition()
    {
        var groundInfo = Physics2D.Raycast(this.playerObj.transform.position + Vector3.up, Vector2.down, 100f, groundLayerMask);
        if (groundInfo.collider == true)
        {
            return groundInfo.point;
        }

        return failPos;
    }

    public void MakeDanagerObject(Vector3 pos)
    {
        var warningObj = CRangedObjectManager.Instance.GetRangedObject("WarningObject");
        warningObj.transform.position = pos;
        Function.LateCall((oParams) =>
        {
            CRangedObjectManager.Instance.PushRangedObject("WarningObject", warningObj);
        }, 1f);
    }

    public void MakeDanagerObject(Vector3 pos, float fDelay)
    {
        var warningObj = CRangedObjectManager.Instance.GetRangedObject("WarningObject");
        warningObj.transform.position = pos;
        Function.LateCall((oParams) =>
        {
            CRangedObjectManager.Instance.PushRangedObject("WarningObject", warningObj);
        }, fDelay);
    }

    public void EightDirAttack(Vector3 pos, float bulletSpeed)
    {
        Debug.Log(pos);

        for (int i = 0; i < 4; i++)
        {
            var bullet = CRangedObjectManager.Instance.GetRangedObject("EnemyBullet");
            var bulletDiagonal = CRangedObjectManager.Instance.GetRangedObject("EnemyBullet");
            bullet.transform.position = pos;
            bulletDiagonal.transform.position = bullet.transform.position;
            float fAngle = 45 + (45 * i);
            float fDiagonalAngle = fAngle + 180f;
            bullet.transform.rotation = Quaternion.AngleAxis(fAngle, Vector3.forward);
            bulletDiagonal.transform.rotation = Quaternion.AngleAxis(fDiagonalAngle, Vector3.forward);

            bullet.GetComponent<Rigidbody2D>().velocity = bullet.transform.right * bulletSpeed;
            bulletDiagonal.GetComponent<Rigidbody2D>().velocity = bulletDiagonal.transform.right * bulletSpeed;
        }
    }

    public void NormalMissile(Vector3 pos)
    {
        var bullet = CRangedObjectManager.Instance.GetRangedObject("GH01_Missile").GetComponent<CMissile>();
        bullet.SetPoint(pos + new Vector3(0f, 1f, 0f), pos + new Vector3(0f, 5f, 0f),
        this.playerObj.transform.position);
    }

    public void NormalBulletAttack(Vector2 direction, Vector3 pos, float bulletSpeed)
    {
        var bullet = CRangedObjectManager.Instance.GetRangedObject("EnemyBullet");
        bullet.transform.position = pos;
        Vector2 dir = direction;
        bullet.GetComponent<Rigidbody2D>().velocity = direction * bulletSpeed;
    }

    public void PlayerDirBulletAttack(Vector3 pos, float bulletSpeed)
    {
        var bullet = CRangedObjectManager.Instance.GetRangedObject("EnemyBullet");
        bullet.transform.position = pos;

        Vector2 dir = (this.playerObj.transform.position + Vector3.up * 0.5f - bullet.transform.position).normalized;
        float fAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        bullet.transform.rotation = Quaternion.AngleAxis(fAngle, Vector3.forward);
        bullet.GetComponent<Rigidbody2D>().velocity = dir.normalized * bulletSpeed;
    }
    
    public void GuidedMissile(Vector3 pos)
    {
        var bullet = CRangedObjectManager.Instance.GetRangedObject("HBO01_Missile").GetComponent<CGuidedMissile>();
        Vector3 p0 = pos + new Vector3(0f, 1f, 0f);
        Vector3 p1 = pos + new Vector3(0f, 2f, 0f);
        Vector3 p2 = pos + Vector3.right;
        bullet.SetPoint(p0, p1, p2, this.playerObj);
        bullet.Shot();
    }

    public void SpiralBullet(Vector3 pos)
    {
        for (int i = 1; i <= 8; i++)
        {
            var bullet = CRangedObjectManager.Instance.GetRangedObject("SpiralBullet").GetComponent<CSpiralBullet>();
            bullet.fAngle = ((i - 1) * 45);
            bullet.SetPosition(pos);
        }
    }

    public void LaserPattern(Vector3 centerpos, float startAngle, float endAngle, int patternIndex = -1)
    {
        StartCoroutine(this.LaserPatternPlay(centerpos, startAngle, endAngle, patternIndex));
    }

    public void LaserPattern(LineRenderer line, Vector3 centerPos, float startAngle, float endAngle)
    {
        StartCoroutine(this.LaserPatternPlay(line, centerPos, startAngle, endAngle));
    }

    public void NotRotateLaserPattern(LineRenderer line, Vector3 centerPos)
    {
        line.positionCount = 2;
        Vector3 rayDir = this.playerObj.transform.position - centerPos;
        rayDir.Normalize();
        line.SetPosition(0, centerPos);
        var groundWallInfo = Physics2D.Raycast(centerPos, rayDir, 1000f, groundWallLayerMask);

        if (groundWallInfo.collider == true)
        {
            line.SetPosition(1, groundWallInfo.point + (Vector2)rayDir * 0.3f);
        }
        
        line.positionCount = 0;
    }

    public void NotRotateLaserPattern(LineRenderer line, Vector3 startPos, Vector3 endPos)
    {
        StartCoroutine(this.LaserPatternPlay(line, startPos, endPos));
    }


    public void GuidedMissile(Vector3 point0, Vector3 point1, Vector3 point2, bool isBoss = false)
    {
        CGuidedMissile bullet;
        if (!isBoss)
        {
            bullet = CRangedObjectManager.Instance.GetRangedObject("HBO01_Missile").GetComponent<CGuidedMissile>();
        }

        else
        {
            bullet = CRangedObjectManager.Instance.GetRangedObject("FinalBossMissle").GetComponent<CGuidedMissile>();
        }

        bullet.SetPoint(point0, point1, point2, this.playerObj);
        bullet.Shot();
    }

    public void Earthquake(Vector3 point)
    {
        var obj = CRangedObjectManager.Instance.GetRangedObject("EarthquakeObject");
        obj.transform.position = point;
    }

    private IEnumerator LaserPatternPlay(LineRenderer line, Vector3 startPos, Vector3 endPos)
    {
        float fTime = 0f;
        line.positionCount = 2;
        line.SetPosition(0, startPos);
        while (fTime < 1f)
        {
            startPos = Vector3.Lerp(startPos, endPos, fTime);
            fTime += Time.deltaTime;
            line.SetPosition(1, startPos);
            yield return new WaitForEndOfFrame();
        }

        line.positionCount = 0;
        yield break;
    }

    private IEnumerator LaserPatternPlay(Vector3 centerPos, float startAngle, float endAngle, int patternIndex)
    {
        GameObject bullet = CRangedObjectManager.Instance.GetRangedObject("CenterLaser");
        bullet.transform.position = centerPos;
        
        float fDivision180 = 0.0055555555555556f;
        float dirX = Mathf.Sin((startAngle * Mathf.PI) * fDivision180);
        float dirY = Mathf.Cos((startAngle * Mathf.PI) * fDivision180);
        
        Vector3 bulletMoveDir = new Vector3(dirX, dirY, 0f);
        Vector3 rayDir = bulletMoveDir - centerPos;
        rayDir.Normalize();

        while (true)
        {
            if (startAngle >= endAngle)
            {
                break;
            }

            var groundWallInfo = Physics2D.Raycast(centerPos,
               rayDir, 1000f, groundWallLayerMask);

            //Debug.DrawRay(centerPos, rayDir * 100f, Color.red, 5f);
            if (groundWallInfo.collider == true)
            {
                //디스턴스값 로그찍어보기
                var scaleX = Vector2.Distance(groundWallInfo.point + Vector2.down * 0.3f, centerPos) / 10f;
                scaleX = scaleX <= -1 ? scaleX * -1 : scaleX;
                Vector3 scale = new Vector3(scaleX, bullet.transform.localScale.y, bullet.transform.localScale.z);
                bullet.transform.localScale = scale;
            }

            bullet.transform.rotation = Quaternion.AngleAxis(startAngle, Vector3.forward);
            startAngle++;

            dirX = Mathf.Cos((startAngle * Mathf.PI) * fDivision180);
            dirY = Mathf.Sin((startAngle * Mathf.PI) * fDivision180);
            bulletMoveDir.x = dirX;
            bulletMoveDir.y = dirY;

            rayDir = bulletMoveDir;
            rayDir.Normalize();

            yield return new WaitForEndOfFrame();
        }

        CEnemyLaser laser = bullet.GetComponent<CEnemyLaser>();
        laser.patternIndex = patternIndex;
        laser.animator.SetBool("AttackEnd", true);
        yield break;
    }

    private IEnumerator LaserPatternPlay (LineRenderer line, Vector3 centerPos, float startAngle, float endAngle)
    {
        float fDivision180 = 0.0055555555555556f;
        float dirX = Mathf.Sin((startAngle * Mathf.PI) * fDivision180);
        float dirY = Mathf.Cos((startAngle * Mathf.PI) * fDivision180);
        
        line.positionCount = 2;
        Vector3 bulletMoveDir = new Vector3(dirX, dirY, 0f);
        Vector3 rayDir = bulletMoveDir - centerPos;
        rayDir.Normalize();
        line.SetPosition(0, centerPos);
        while (true)
        {
            if (startAngle >= endAngle)
            {
                break;
            }

            var groundWallInfo = Physics2D.Raycast(centerPos, 
                rayDir, 1000f, groundWallLayerMask);
            
            if (groundWallInfo.collider == true)
            {
                line.SetPosition(1, groundWallInfo.point + (Vector2)rayDir * 0.3f);
            }
            startAngle++;
            Debug.Log(startAngle);
            dirX = Mathf.Cos((startAngle * Mathf.PI) * fDivision180);
            dirY = Mathf.Sin((startAngle * Mathf.PI) * fDivision180);
            bulletMoveDir.x = dirX;
            bulletMoveDir.y = dirY;
            
            rayDir = bulletMoveDir - centerPos;
            rayDir.Normalize();
            yield return new WaitForEndOfFrame();
        }

        line.positionCount = 0;
        Debug.Log("End");
        yield break;
    }

    public static CEnemyPatternManager Create()
    {
        return Instance;
    }
}
