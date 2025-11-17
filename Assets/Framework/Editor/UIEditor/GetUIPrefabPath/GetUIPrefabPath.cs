using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GetUIPrefabPath : EditorWindow 
{
    public static Rect parentWindowRect = new Rect(810, 60, 300, 400);  //父窗口的位置（以Unity编辑器窗口左上角为原点，向下为正，向右为正）和大小（300*400）
    public static GetUIPrefabPath getUIPrefabPath;
    private GUIStyle labelStyle;  //第一个标签样式(选择一个UI 视图根节点)
    private GUIStyle labelGameObjectNameStyle;  //显示选中的物体名称标签
    private static GUIStyle labelStyleMessage;  //提示标签样式
    private GUIStyle buttonStyle;    //按钮标签样式
    private GUIStyle textAreaStyle;  //输入框样式
    private string prefabPath = "";
    private GameObject originGameObject = null;

    [MenuItem("君麟工具箱/UI工具/获取UI视图路径")]  //在Unity编辑器的Tools菜单栏添加子菜单
    private static void run()  //这里只能是静态的run方法，否则Unity不会显示相关菜单
    {
        getUIPrefabPath = EditorWindow.GetWindow<GetUIPrefabPath>(false, "获取UI视图路径");
        getUIPrefabPath.position = GetUIPrefabPath.parentWindowRect;
        getUIPrefabPath.Show();
        
    }

    void OnGUI() 
    {
        this.InitStyle();  //初始化样式-因为涉及GUI样式设置，初始化必须在OnGUI()函数中
        //垂直布局
        GUILayout.BeginVertical(GUILayout.Width(position.width), GUILayout.Height(position.height));
        GUILayout.Label("选择一个UI 视图节点", this.labelStyle);

        #region  水平布局-自定义输入框  
        GUILayout.BeginHorizontal("box", GUILayout.Width(position.width));  //水平布局
        if (GUILayout.Button("生成"+ "\n" + "PATH", this.buttonStyle, GUILayout.MinHeight(80), GUILayout.MinWidth(80), GUILayout.MaxWidth(80))) 
        {
            if (Selection.activeGameObject != null && Selection.activeGameObject != originGameObject)  //防止同一物体点击多次生成路径，路径重叠的问题
            {
                this.originGameObject = Selection.activeGameObject;  
                Transform transform = Selection.activeGameObject.transform;
                string name = Selection.activeGameObject.name;
                List<string> pathList = new List<string>();
                while (true)
                {
                    if (!name.Contains("Canvas"))
                    {
                        pathList.Add(name);
                        transform = transform.parent;
                        if (transform == null)  //经过观察，目前只有点到UI预制体根物体才会为null
                        {
                            break;
                        }
                        name = transform.name;
                    }
                    else
                    {
                        break;
                    }
                }
                if (pathList.Count < 2)  //表明点击到了UI预制体根物体
                {
                    this.prefabPath = "UI根预制体不需要生成路径";
                }
                else 
                {
                    for (int i = pathList.Count - 2; i >= 0; i--)
                    {
                        if (i == pathList.Count - 2)  //如果是第一个，则不加 /
                        {
                            this.prefabPath += pathList[i];
                        }
                        else
                        {
                            this.prefabPath += "/" + pathList[i];
                        }
                    }
                }            
            }        
        }

        GUILayout.TextArea(this.prefabPath, 220, this.textAreaStyle, GUILayout.MinHeight(80));  //自定义输入框
        GUILayout.EndHorizontal();  //结束水平布局
        GUILayout.Space(5);  //间距设置为5
        #endregion

        if (Selection.activeGameObject != null) 
        {    
            GUILayout.Label(Selection.activeGameObject.name, this.labelGameObjectNameStyle);  
        }
        else 
        {
            GUILayout.Label("没有选中UI视图节点，无法生成路径", labelStyleMessage);
        }

        GUILayout.EndVertical();  //结束垂直布局
    }

    private void InitStyle()
    {
        //提示Label样式设置（"选择一个UI 视图节点"）
        this.labelStyle = new GUIStyle(GUI.skin.label);
        this.labelStyle.fontSize = 18;  //设置字体大小
        this.labelStyle.normal.textColor = Color.white;

        //按钮样式设置
        this.buttonStyle = new GUIStyle(GUI.skin.button);
        this.buttonStyle.fontSize = 18;  //设置字体大小
        this.labelStyle.normal.textColor = Color.white;

        //输入框样式设置
        this.textAreaStyle = new GUIStyle(GUI.skin.textArea);
        this.textAreaStyle.fontSize = 16;

        //选择物体显示名称样式设置
        this.labelGameObjectNameStyle = new GUIStyle(GUI.skin.label);
        this.labelGameObjectNameStyle.fontSize = 16;  //设置字体大小
        this.labelGameObjectNameStyle.normal.textColor = Color.yellow;

        //提示Label样式设置（"没有选中UI视图节点，无法生成路径"）
        labelStyleMessage = new GUIStyle(GUI.skin.label);
        labelStyleMessage.fontSize = 16;  //设置字体大小
        labelStyleMessage.normal.textColor = Color.red;
    }
    void OnSelectionChange() {
        this.Repaint();
        this.prefabPath = "";  //如果重新选择了物体对象，则路径就不显示
    }
}
