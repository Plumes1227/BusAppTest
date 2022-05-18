using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 子彈發射器,升級版,不須再添加鋼體，改由添加程式控制子彈
/// </summary>
public class BulletLauncherNoRig : MonoBehaviour
{
    [Header("---持續發射---")]
    [SerializeField] bool isOnLoop;
    [Header("---子彈預制體---")]
    [SerializeField] GameObject emBulletPrefab;
    GameObject bullet;  //存取臨時子彈用
    [Header("---子彈存活時間---")]
    [SerializeField][Min(0)] float lifeTime = 1;
    [Header("---子彈發射次數---")]
    [SerializeField][Min(0)] float fireLoop = 1;
    [Header("---子彈發射頻率---")]
    [SerializeField][Min(0)] float fireLoopTime;
    WaitForSeconds waitFireLoopTime;
    [Header("---子彈飛行速度---")]
    [SerializeField] float flySpeed = 20;
    
    //模式切換用
    enum FireModel
    {
        Normal,     //一般,就單純朝右邊放射
        Scattering,     //散射,依照
        Annular
    }
    [Header("---發射模式---")]
    [SerializeField] FireModel fireModel;
    [Header("---是否點放---僅散射and環狀")]
    [SerializeField] bool IsALittle;
    [Header("---點放頻率---僅散射and環狀")]
    [SerializeField][Min(0)] float aLittleTime;
    WaitForSeconds waitALittleTime;
    [Header("---一次發射數量---僅散射and環狀有用")]
    [SerializeField][Min(0)] int fireAmount = 3;
    [Header("---散射角度---僅散射有用")]
    [SerializeField][Min(0.1f)] float fireAngle = 30;       //散射角度
    Vector2 direction;      //以角度換算來臨時存取的方向
    Quaternion qtAngle;     //計算用四元數

    void Awake()
    {
        waitFireLoopTime = new WaitForSeconds(fireLoopTime);
        waitALittleTime = new WaitForSeconds(aLittleTime);        
    }

    void Start()
    {
        Shooting(); //開始時即發射一次,
    }

    //顯示在編輯器上'方便觀察用，只管用，不需嘗試理解！
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Quaternion left = transform.rotation * Quaternion.AngleAxis(fireAngle/2, Vector3.forward);
        Quaternion right = transform.rotation * Quaternion.AngleAxis(fireAngle/2, Vector3.back);
        Vector2 leftPoint = transform.position + left * Vector3.right;
        Vector2 rightPoint = transform.position + right * Vector3.right;        
        Gizmos.DrawLine(transform.position , leftPoint);
        Gizmos.DrawLine(transform.position , rightPoint);
    }

    /// <summary>
    /// 攻擊時調用這個方法，就可以依照設定好的模式觸發特定的發射方法
    /// </summary>
    public void Shooting()
    {
        switch(fireModel)
        {
            case FireModel.Normal:
            StartCoroutine(NormalFireCoroution());
            break;
            case FireModel.Scattering:
            StartCoroutine(ScatteringFireCoroution());
            break;
            case FireModel.Annular:
            StartCoroutine(AnnularFireCoroution());
            break;
        }
    }

    //一般發射
    IEnumerator NormalFireCoroution()
    {
        //依照fireLoop設定的次數循環,
        for (int i = 0; i < fireLoop; i++)
        {
            //生成物件,發射器位置,依角度旋轉 並存入bullet方便立即調用
            bullet = Instantiate(emBulletPrefab, transform.position, Quaternion.Euler(0 ,0 
                , Mathf.Atan2(transform.right.y ,transform.right.x)* Mathf.Rad2Deg)); 
                
            //為對象添加一個自動飛行C#(這邊我C#在最後方創建,未來同學也可以改成直接在資料夾新建一個C#)
            bullet.AddComponent<AutoAddFlyBullet>().FlyStart(transform.right, flySpeed); //並調用腳本內寫好的飛行方法
            Destroy(bullet, lifeTime);  //讓他在設定好的秒數後消失
            yield return waitFireLoopTime;  //fireTime=循環頻率(發射頻率)
        }
        if(isOnLoop) Shooting();    //如果循環發射開啟就會一直重來(像是陷阱之累的在用，不然都讓怪物攻擊時調用Shooting就好)
    }

    //散行發射
    IEnumerator ScatteringFireCoroution()
    {
        for (int i = 0; i < fireLoop; i++)
        {
            //看一次發射幾顆
            for (int j = 0; j < fireAmount; j++)
            {
                                    //彈數-1才能平均分散在你設定的角度
                //發射器角度*((角度/(彈數-1)) * J = "從0開始一直發射到最大角度", 依照世界座標(0,0,1)去計算)
                qtAngle = transform.rotation * Quaternion.AngleAxis((fireAngle / (fireAmount-1) * j) - fireAngle/2, Vector3.forward);
                //將計算完的四元數 * 一小段距離(這邊用右(1,0)) 就會得到這個角度的二維方向(x,y);
                direction = qtAngle * Vector2.right;

                bullet = Instantiate(emBulletPrefab, transform.position, Quaternion.Euler(0 ,0 
                , Mathf.Atan2(direction.y ,direction.x)* Mathf.Rad2Deg));
                bullet.AddComponent<AutoAddFlyBullet>().FlyStart(direction, flySpeed);                
                Destroy(bullet, lifeTime);
                if(IsALittle) yield return waitALittleTime; //每顆子彈小時間差,IsALittle 要開才會生效
            }            
            yield return waitFireLoopTime;  //頻率一樣是在fireLoop這邊,
        }
        if(isOnLoop) Shooting();
    }
    IEnumerator AnnularFireCoroution()
    {
        for (int i = 0; i < fireLoop; i++)
        {
            for (int j = 0; j < fireAmount; j++)
            {            
                
                //改成 發射器角度*((360/彈數),其餘同上
                qtAngle = transform.rotation * Quaternion.AngleAxis((360 / (fireAmount) * j), Vector3.forward);
                direction = qtAngle * Vector2.right;

                bullet = Instantiate(emBulletPrefab, transform.position, Quaternion.Euler(0 ,0 
                , Mathf.Atan2(direction.y ,direction.x)* Mathf.Rad2Deg));
                bullet.AddComponent<AutoAddFlyBullet>().FlyStart(direction, flySpeed);                
                Destroy(bullet, lifeTime);
                if(IsALittle) yield return waitALittleTime;
            }            
            yield return waitFireLoopTime;
        }
        if(isOnLoop) Shooting();
    }

}

/// <summary>
/// 自動添加的子彈飛行程式
/// </summary>
class AutoAddFlyBullet : MonoBehaviour
{    
    public void FlyStart(Vector2 direction, float flySpeed)
    {
        StartCoroutine(MoveCoroution(direction, flySpeed));
    }
    IEnumerator MoveCoroution(Vector2 direction, float flySpeed)
    {
        while(gameObject.activeInHierarchy) //未禁用
        {
            transform.position = Vector2.MoveTowards(transform.position, (Vector2)transform.position + direction, flySpeed * Time.deltaTime);
            yield return null;
        }
    }
}
