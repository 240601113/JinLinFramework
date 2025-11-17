using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUICtrl : MonoBehaviour
{
    private joystick stick;
    private Transform uiBloodRoot;
    public void Init() 
    {
        this.uiBloodRoot = this.transform.Find("UIBloodRoot");
        this.stick = this.transform.Find("Joystick").GetComponent<joystick>();
    }

    public void Update()
    {
        //根据遥感的方向来移动我们的角色; 16.16
        //( 1 << 16))表示2的16次方

        FightMgr.Instance.player.Move((int)(this.stick.dir.x * ( 1 << 16)), (int)(this.stick.dir.y * (1 << 16)));
        // end 
    }

    //public UIBlood CreateUIBlood() {
    //    // 代码加载资源，实例化一个; 
    //    GameObject blood = GameObject.Instantiate(FightMgr.Instance.uiBloodPrefab);
    //    blood.transform.SetParent(this.uiBloodRoot, false);

    //    UIBlood uiBlood = blood.AddComponent<UIBlood>();
    //    uiBlood.Init();
    //    // end

    //    return uiBlood;
    //}
}
