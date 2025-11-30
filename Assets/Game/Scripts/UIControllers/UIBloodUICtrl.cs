using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
/// <summary>
/// 3d血条UI控制
/// </summary>
public class UIBloodUICtrl : UICtrl
{

    private static Transform uiBloodRoot;//实例化到这个节点

    public override void Awake() 
	{
		base.Awake();
	}
	private void Start()
	{
        uiBloodRoot = this.transform.Find("UIBloodRoot");
        
    }


    public static UIBlood  CreateUIBlood()
    {
        // 代码加载资源，实例化一个; 
        GameObject blood = GameObject.Instantiate(FightMgr.Instance.uiBloodPrefab);
        blood.transform.SetParent(uiBloodRoot, false);

        UIBlood uiBlood = blood.AddComponent<UIBlood>();
        uiBlood.Init();
        // end

        return uiBlood;
    }

}
