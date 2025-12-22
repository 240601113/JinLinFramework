using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Unity.VisualScripting;
public class UIMgr : UnitySingleton<UIMgr>
{
    public  Transform canvas = null;

    private Dictionary<string,UICtrl> PanelPool = new Dictionary<string,UICtrl>();//已生成的Ui面板会储存在这里

    private static Transform uiBloodRoot;

    private Canvas UIboll3d;

    /// <summary>
    /// 初始化UIMgr，层级面板中需要有UI/Canvas物体
    /// </summary>
    public void Init()
    {
        //this.canvas = GameObject.Find("UI/Canvas").transform;  //初始化canvas

        TimerMgr.Instance.ScheduleOnce(Getfield,3);


    }

    private void Getfield(object cc)
    {
        Cavas3d.transform.SetParent(this.canvas, false);

    }


    /// <summary>
    /// 显示面板 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T ShowPanel <T>()where T : UICtrl
    {

        string PanelName = typeof(T).Name;//通过反射找到传进来类的名字 也就是 类名

        if (!PanelPool.ContainsKey(PanelName))
        {
            Debug.Log("未找到视图 请确认是否初始化生成视图");
            return default(T);//返回一个默认的T
        }

        PanelPool[PanelName].ShowUIViewMe();

        return PanelPool[PanelName] as T; //将存在里面的隐式子类 的父类 强转成真正的子类 返回出去 如果外部还想有什么特殊定制化操作的话 我不管 返回实例出去给别的类操作

    }
    /// <summary>
    /// 隐藏面板
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T ClosePanel<T>() where T : UICtrl
    {

        string PanelName = typeof(T).Name;//通过反射找到传进来类的名字 也就是 类名

        if (!PanelPool.ContainsKey(PanelName))
        {
            Debug.LogWarning("未找到视图 请确认是否初始化生成视图");
            return default(T);//返回一个默认的T
        }

        PanelPool[PanelName].CloseUIViewMe();

        return PanelPool[PanelName] as T; //将存在里面的隐式子类 的父类 强转成真正的子类 返回出去 如果外部还想有什么特殊定制化操作的话 我不管 返回实例出去给别的类操作

    }






    /// <summary>
    /// 生成没有带Canvas的UI视图，直接通过选中UI预制体-右键-Copy Path复制路径即可
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public UICtrl ShowUIView(string namePath)
    {

        this.canvas = GameObject.Find("UI/MainCanvas").transform;  //初始化canvas  //每次加载都要找一遍？？是否可以优化
        GameObject UIPrefab = (GameObject)ResMgr.Instance.LoadAssetSync<GameObject>(namePath);
        GameObject UIObject = GameObject.Instantiate(UIPrefab);
        UIObject.name = UIPrefab.name;
        UIObject.transform.SetParent(this.canvas, false);

        //int lastIndex = name.LastIndexOf("/");  //获取路径最后一个/后面的名字，也就是预制体名（可以预制体放在多个层级文件夹下）
        //if (lastIndex > 0)
        //{
        //    name = name.Substring(lastIndex + 1);
        //}
        string UIName = UIPrefab.name + "UICtrl";//把类名拼接好存起来因为要多次使用

        Type type = Type.GetType(UIName);//根据反射查找类元数据动态获取该类的类型和字段 也可以说是根据反射动态获取该类的脚本
        UICtrl ctrl = (UICtrl)UIObject.AddComponent(type);  //给UI添加脚本组件，也就是显示之后自动添加控制脚本

        if (PanelPool.ContainsKey(UIName))
        {
            Debug.LogWarning("重复生成视图！注意场景里多余的节点!!" + UIPrefab.name);
            return ctrl;
        }
        PanelPool.Add(UIName, ctrl);//给存到字典里 供下次使用
        return ctrl;
    }

    /// <summary>
    /// 生成带Canvas的UI创口，直接通过选中UI预制体-右键-Copy Path复制路径即可
    /// </summary>
    /// <param name="namePath"></param>
    /// <returns></returns>  
    public UICtrl ShowUIViewWithCanvas(string namePath)
    {
        this.canvas = GameObject.Find("UI/WindowsCanvas").transform;  //初始化canvas //每次加载都要找一遍？？是否可以优化
        GameObject UIPrefab = (GameObject)ResMgr.Instance.LoadAssetSync<GameObject>(namePath);
        GameObject UIObject = GameObject.Instantiate(UIPrefab);
        UIObject.name = UIPrefab.name;
        UIObject.transform.SetParent(this.canvas, false);

        string UIName = UIPrefab.name + "UICtrl";

        Type type = Type.GetType(UIName);
        UICtrl ctrl = (UICtrl)UIObject.AddComponent(type);  //给UI添加脚本组件，也就是显示之后自动添加控制脚本


        if (PanelPool.ContainsKey(UIName))
        {
            Debug.LogWarning("重复生成窗口！注意场景里多余的节点!!"+ UIPrefab.name);
            return ctrl;
        }

        PanelPool.Add(UIName, ctrl);//给存到字典里 供下次使用
        return ctrl;
    }


    GameObject Cavas3d;

    /// <summary>
    /// 生成3d血条
    /// </summary>
    /// <returns></returns>
    public UIBloodUICtrl ShowUIBlood()
    {

        if(uiBloodRoot == null)
        {
           GameObject cavas3d =  ResMgr.Instance.LoadAssetSync<GameObject>("Assets/Game/Resources/UI/3DBlood/Prefabs/UIboold3d.prefab");
           
             cavas3d = GameObject.Instantiate(cavas3d);
            uiBloodRoot = cavas3d.transform;
            
            Cavas3d = cavas3d;


        }

        // 代码加载资源，实例化一个; 
        GameObject blood = GameObject.Instantiate(FightMgr.Instance.uiBloodPrefab);
       

        //UIBlood uiBlood = blood.AddComponent<UIBlood>();
       UIBloodUICtrl uIBloodUICtrl = blood.gameObject.AddComponent<UIBloodUICtrl>();
        uIBloodUICtrl.Init();
        uIBloodUICtrl.gameObject.transform.SetParent(uiBloodRoot, false);

        return uIBloodUICtrl;


    }



}
