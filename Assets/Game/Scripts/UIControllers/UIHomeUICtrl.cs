using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class UIHomeUICtrl : UICtrl
{
	public override void Awake() 
	{
		base.Awake();
	}
	private void Start()
	{
		this.AddButtonListener("IT_Button", this.ButtonCallback);
	}
    
    private void ButtonCallback()
    {
        Text text = this.UIView["IT_Text"].GetComponent<Text>();
        if (text == null)
        {
            LogMgr.Instance.Log("没有获取到对应的Text对象!");
            return;
        }
        
        text.text = "君麟科技";

        LogMgr.Instance.Log("UI按钮执行回调！！！");
    }
}
