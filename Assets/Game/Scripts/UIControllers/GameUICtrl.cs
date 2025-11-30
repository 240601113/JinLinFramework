using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUICtrl : MonoBehaviour
{
    public joystick stick;
    
     public static GameUICtrl  gameUICtrl = null;
    public void Init() 
    {
        gameUICtrl = this;
       
        this.stick = this.transform.Find("Joystick").GetComponent<joystick>();
    }

    public void Update()
    {
        //根据遥感的方向来移动我们的角色; 16.16
        //( 1 << 16))表示2的16次方

        FightMgr.Instance.player.Move((int)(this.stick.dir.x * ( 1 << 16)), (int)(this.stick.dir.y * (1 << 16)));
        // end 
    }
}
