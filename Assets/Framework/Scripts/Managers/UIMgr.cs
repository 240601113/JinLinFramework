using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
public class UICtrl : MonoBehaviour
{
    public Dictionary<string, GameObject> UIView = new Dictionary<string, GameObject>();  //UI物体路径和UI物体映射字典

    /// <summary>
    /// 加载当前UI下的所有物体
    /// </summary>
    /// <param name="root"></param>
    /// <param name="path"></param>
    private void LoadUIAllObject(GameObject root, string path)
    {
        foreach (Transform tf in root.transform)
        {
            if (!tf.gameObject.name.StartsWith("IT_"))  //如果交互控件名字前面没有IT_，则表示该控件不是交互控件，仅只是显示，不纳入UIView字典中
            {
                continue;
            }
            if (this.UIView.ContainsKey(path + tf.gameObject.name))
            {
                continue;
            }
            this.UIView.Add(path + tf.gameObject.name, tf.gameObject);
            LoadUIAllObject(tf.gameObject, path + tf.gameObject.name + "/");
        }
    }

    public virtual void Awake()
    {
        this.LoadUIAllObject(this.gameObject, "");
    }

    /// <summary>
    /// 添加UI界面按钮监听事件
    /// </summary>
    /// <param name="buttonPath">按钮在UI预制体中的路径</param>
    /// <param name="onClick">按钮按下后要执行的方法</param>
    public void AddButtonListener(string buttonPath, UnityAction onClick) 
    {
        
        Button button = this.UIView[buttonPath].GetComponent<Button>();
        if (button == null)
        {
            LogMgr.Instance.Log("UIManager.cs: 没有获取到对应的Button对象!");
            return;
        }
        LogMgr.Instance.Log("AddButtonListener执行！！!");
        button.onClick.AddListener(onClick);
    }

    /// <summary>
    /// 修改Text内容
    /// </summary>
    /// <param name="textPath">Text物体对象在UI预制体中的路径</param>
    /// <param name="newContent">新的内容</param>
    public void ModifyTextContent(string textPath, string newContent)
    {
        Text text = this.UIView[textPath].GetComponent<Text>();
        if (text == null)
        {
            Debug.LogWarning("UIManager.cs: 没有获取到对应的Text对象!");
        }
        else
        {
            text.text = newContent;
        }
    }
}
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
