using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
public class UIMgr : UnitySingleton<UIMgr>
{
    private Transform canvas = null;

    /// <summary>
    /// 初始化UIMgr，层级面板中需要有UI/Canvas物体
    /// </summary>
    public void Init()
    {
        //this.canvas = GameObject.Find("UI/Canvas").transform;  //初始化canvas
    }

    /// <summary>
    /// 显示没有带Canvas的UI视图，直接通过选中UI预制体-右键-Copy Path复制路径即可
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public UICtrl ShowUIView(string namePath)
    {
        this.canvas = GameObject.Find("UI/MainCanvas").transform;  //初始化canvas
        GameObject UIPrefab = (GameObject)ResMgr.Instance.LoadAssetSync<GameObject>(namePath);
        GameObject UIObject = GameObject.Instantiate(UIPrefab);
        UIObject.name = UIPrefab.name;
        UIObject.transform.SetParent(this.canvas, false);

        //int lastIndex = name.LastIndexOf("/");  //获取路径最后一个/后面的名字，也就是预制体名（可以预制体放在多个层级文件夹下）
        //if (lastIndex > 0)
        //{
        //    name = name.Substring(lastIndex + 1);
        //}

        Type type = Type.GetType(UIPrefab.name + "UICtrl");
        UICtrl ctrl = (UICtrl)UIObject.AddComponent(type);  //给UI添加脚本组件，也就是显示之后自动添加控制脚本

        return ctrl;
    }

    /// <summary>
    /// 显示带Canvas的UI创口，直接通过选中UI预制体-右键-Copy Path复制路径即可
    /// </summary>
    /// <param name="namePath"></param>
    /// <returns></returns>
    public UICtrl ShowUIViewWithCanvas(string namePath)
    {
        this.canvas = GameObject.Find("UI/WindowsCanvas").transform;  //初始化canvas
        GameObject UIPrefab = (GameObject)ResMgr.Instance.LoadAssetSync<GameObject>(namePath);
        GameObject UIObject = GameObject.Instantiate(UIPrefab);
        UIObject.name = UIPrefab.name;
        UIObject.transform.SetParent(this.canvas, false);

        Type type = Type.GetType(UIPrefab.name + "UICtrl");
        UICtrl ctrl = (UICtrl)UIObject.AddComponent(type);  //给UI添加脚本组件，也就是显示之后自动添加控制脚本

        return ctrl;
    }
}
