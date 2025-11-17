using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EditorTemplate : EditorWindow
{
    //枚举
    public enum MyEnum
    { 
        A,
        B,
        C
    }
    public static EditorTemplate ins;
    private GUIStyle labelStyle;
    private GUIStyle buttonStyle;

    private string names = "请输入想要修改的名字";
    public Color color = Color.blue;
    public MyEnum enums = MyEnum.A;
    public GameObject gameObject;
    private bool toggle = false;
    public static Rect parentWindowRect = new Rect(810, 60, 300, 400);  //父窗口的位置（以Unity编辑器窗口左上角为原点，向下为正，向右为正）和大小（250*250）。
    public Rect windowRect = new Rect(25, 260, 250, 250);  //子窗口的位置（以父窗口左上角为原点，向下为正，向右为正）和大小（250*250）。

    [MenuItem("君麟工具箱/编辑器模板/EditorTemplate")]  //在Unity编辑器的Tools菜单栏添加子菜单,一定要写在类字段下面，否则会报错
    static void run()  //run一定要写，否则不会出现在Unity编辑器中
    {
        ins = EditorWindow.GetWindow<EditorTemplate>(false, "我的第一个窗口");
        ins.position = EditorTemplate.parentWindowRect;
        ins.Show();

    }

    void OnGUI()
    {
        //垂直布局
        GUILayout.BeginVertical(GUILayout.Width(position.width), GUILayout.Height(position.height));
        //自定义文字
        if (this.labelStyle == null)
        {
            this.labelStyle = new GUIStyle(GUI.skin.label);
            this.labelStyle.fontSize = 16;  //设置字体大小
            this.labelStyle.normal.textColor = Color.red;
        }
        GUILayout.Label("这是一个编辑器扩展模板", this.labelStyle);  //按钮等也可以传入labelStyle参数
        GUILayout.Space(5);

        #region  水平布局-自定义输入框  
        GUILayout.BeginHorizontal("box", GUILayout.Width(position.width));  //水平布局
        this.names = GUILayout.TextField(this.names);  //自定义输入框
        int i = 0;
        this.buttonStyle = new GUIStyle(GUI.skin.button);
        this.buttonStyle.fontSize = 12;

        if (GUILayout.Button("确认修改", this.buttonStyle, GUILayout.MinHeight(20), GUILayout.MinWidth(60)))  //自定义按钮
        {
            foreach (GameObject gameObject in Selection.gameObjects)
            {
                i++;
                gameObject.name = this.names + i;
            }
        }
        GUILayout.EndHorizontal();  //结束水平布局
        GUILayout.Space(5);
        #endregion

        #region  自定义颜色框
        GUILayout.BeginHorizontal("box", GUILayout.Width(position.width));
        GUILayout.Label("请选择颜色");
        
        color = EditorGUILayout.ColorField(color);  //自定义颜色框
        GUILayout.EndHorizontal();  //结束水平布局
        GUILayout.Space(5);
        #endregion

        #region  水平布局-自定义枚举框
        GUILayout.BeginHorizontal("box", GUILayout.Width(position.width));
        this.enums = (MyEnum)EditorGUILayout.EnumPopup(new GUIContent("请选择枚举的类型"), this.enums);
        GUILayout.EndHorizontal();  //结束水平布局
        #endregion

        #region  水平布局-自定义物体
        GUILayout.BeginHorizontal("box", GUILayout.Width(position.width));
        this.gameObject = EditorGUILayout.ObjectField(new GUIContent("物体"), this.gameObject, typeof(GameObject), true) as GameObject;
        GUILayout.EndHorizontal();  //结束水平布局
        #endregion

        #region  水平布局-自定义Toggle按钮
        GUILayout.BeginHorizontal("box", GUILayout.Width(position.width));
        GUILayout.Label("请点击Toggle按钮");
        this.toggle = GUILayout.Toggle(toggle, new GUIContent("是否全屏"));
        GUILayout.EndHorizontal();  //结束水平布局
        #endregion

        #region  水平布局-自定义提示信息
        //自定义提示信息
        EditorGUILayout.HelpBox("提示信息", MessageType.Warning, true);
        #endregion

        #region  创建子窗口
        BeginWindows();
        //创建内联窗口，参数分别为id， 大小位置， 创建子窗口的组件的函数，标题
        this.windowRect = GUILayout.Window(1, this.windowRect, DoWindow, "子窗口");
        EndWindows();

        #endregion

        GUILayout.EndVertical();  //结束垂直布局
    }
    void DoWindow(int unuseWindowID)
    {
        GUILayout.Button("按钮");
        GUI.DragWindow();  //画出子窗口
    }
}
