using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private int dirx;  //当前移动的X方向，用定点数做，更好判断
    private int diry;  //当前移动的Y方向

    private bool isMoving = false;
    private float speed = 5.0f;  //移动速度，可以从配置配件中读取
    private UIBlood uiBlood;
    private Transform mountPoint = null;

    public void Init() 
    {
        this.diry = 0;
        this.dirx = 0;
        this.isMoving = false;
        this.speed = 5;

        // 血条
        //this.uiBlood = FightMgr.Instance.uiGame.CreateUIBlood();
        //this.uiBlood.SetPercent(0.3f);
        // end 

        this.mountPoint = this.transform.Find("mountPoint");
    }

    public void Move(int dirx, int diry) 
    {
        if (this.dirx == dirx && this.diry == diry)  //如果当前的方向和之前的方向一致，那就表示遥感没有动，则保持原来处理方式
        {
            return;  
        }

        this.dirx = dirx;  //更新X遥感方向
        this.diry = diry;  //更新Y遥感方向
        if (this.dirx == 0 && this.diry == 0)  //这个表示遥感弹起来了，停止移动
        { 
            this.isMoving = false;
            return;
        }

        this.isMoving = true;
        float r = Mathf.Atan2(this.diry, this.dirx);
        float degree = r * Mathf.Rad2Deg;
        degree = degree - 90; // 对齐起点;
        degree = 360 - degree; // 时针方向;

        Vector3 eRot = this.transform.eulerAngles;
        eRot.y = degree;
        this.transform.eulerAngles = eRot;
    }

   

    public void Update() 
    {
        if (this.isMoving) 
        {
            this.WalkUpdate();
        }
    }

    /// <summary>
    /// 在3D中，角色是在XOZ平面移动的
    /// </summary>
    private void WalkUpdate()
    {
        float deltaTime = Time.deltaTime;
        float walkSpeed = this.speed * deltaTime;
        float xSpeed = (walkSpeed * this.dirx) / (1 << 16);
        float zSpeed = (walkSpeed * this.diry) / (1 << 16);

        Vector3 pos = this.transform.position;  //脚本肯定挂在在角色物体上，获取的就是角色物体的位置
        pos.x += xSpeed;
        pos.z += zSpeed;
        this.transform.position = pos;
    }

    public void LateUpdate()
    {
        //this.syncUIBlood();
    }

    private void syncUIBlood() {
        // 挂载点的世界坐标---》屏幕坐标;--->屏幕坐标--->UI机制下的坐标;
        Vector3 worldPos = this.mountPoint.position;
        Vector3 sceenPos = FightMgr.Instance.gameCamera.WorldToScreenPoint(worldPos);
        this.uiBlood.ShowAt(sceenPos);
    }


}
