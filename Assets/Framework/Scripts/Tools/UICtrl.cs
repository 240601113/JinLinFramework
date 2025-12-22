using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

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
    /// 显示面板
    /// </summary>
    public virtual void ShowUIViewMe()
    {
        if (this.gameObject.activeSelf)
        {
            return;
        }
        this.gameObject.SetActive(true);
    }
    /// <summary>
    /// 关闭面板
    /// </summary>
    public virtual void CloseUIViewMe()
    {
        if (!this.gameObject.activeSelf)
        {
            return;
        }
        this.gameObject.SetActive(false);
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
