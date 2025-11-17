using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class joystick : MonoBehaviour 
{
    private Canvas cs;

    public Transform mobileStick;  //要移动的移动图标
    public float max_R = 80;  //滑动范围

    private Vector2 touch_dir = Vector2.zero;  //要传给人物移动的数据
    public Vector2 dir 
    {
        get 
        {
            return this.touch_dir;
        }
    }

	void Start () 
    {
        this.cs = GameObject.Find("Canvas").GetComponent<Canvas>();
        this.mobileStick = this.transform.Find("MobileJoystick");
        this.mobileStick.localPosition = Vector2.zero;
        this.touch_dir = Vector2.zero;
	}
	
    public void OnStickDrag() 
    {
        //1、将鼠标拖拽位置转换为摇杆本地坐标
        Vector2 pos = Vector2.zero;  //摇杆本地坐标
        RectTransformUtility.ScreenPointToLocalPointInRectangle(this.transform as RectTransform, 
                                                                Input.mousePosition,  //鼠标/手指屏幕位置 
                                                                this.cs.worldCamera, 
                                                                out pos);
        //end

        //2、计算拖拽方向（供人物移动）
        float len = pos.magnitude;  //获取位置模长
        if (len <= 0) 
        {
            this.touch_dir = Vector2.zero;
            return;
        }
        

        //归一化, 把手指移动的方向保存下来
        this.touch_dir.x = pos.x / len; // cos(r)
        this.touch_dir.y = pos.y / len; // (sinr) cos^2 + sin ^ 2 = 1;
        //end


        //3、限制摇杆移动范围（避免超出背景）
        if (len >= this.max_R)
        {
            LogMgr.Instance.Log(len.ToString());
            pos.x = pos.x * this.max_R / len;
            pos.y = pos.y * this.max_R / len;
        }
        //end

        //4、刷新摇杆视觉位置
        this.mobileStick.localPosition = pos;
        //end
    }

    /// <summary>
    /// 如果弹起来
    /// </summary>
    public void OnStickEndDrag() 
    {
        this.mobileStick.localPosition = Vector2.zero;
        this.touch_dir = Vector2.zero;
    }

}
