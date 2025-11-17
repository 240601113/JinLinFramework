using UnityEngine;
using UnityEditor;
using System.IO;

public class AutoGenUICtrl : EditorWindow
{
    public static Rect parentWindowRect = new Rect(810, 60, 300, 400);  //父窗口的位置（以Unity编辑器窗口左上角为原点，向下为正，向右为正）和大小（300*400）
    public static AutoGenUICtrl autoGenUICtrl;
    private GUIStyle labelStyle;  //第一个标签样式(选择一个UI 视图根节点)
    private GUIStyle buttonStyle;  //按钮标签样式
    private GUIStyle textAreaStyle;  //输入框样式
    private bool finishFlag = false;  //是否生成控制脚本
    private GUIStyle labelGameObjectNameStyle;  //显示选中的物体名称标签
    private GUIStyle labelStyleMessage;  //提示标签样式

    [MenuItem("君麟工具箱/UI工具/自动生成UI视图控制脚本")]
    static void run()
    {
        autoGenUICtrl = EditorWindow.GetWindow<AutoGenUICtrl>(false, "自动生成UI视图控制脚本");  //创建或获取一个指定类型（T）的编辑器窗口实例。 false：表示该窗口不成为浮动窗口（即会被收纳到 Unity 编辑器的窗口体系中，可与其他窗口停靠）；若设为 true，则窗口为独立浮动状态。
        autoGenUICtrl.position = AutoGenUICtrl.parentWindowRect;
        autoGenUICtrl.Show(); //Show()：EditorWindow 的方法，用于在 Unity 编辑器中显示该窗口（如果窗口已隐藏，调用后会显示；若已显示，可能会将其置顶）。
    }

    void OnGUI()
    {
        this.InitStyle();  //初始化样式-因为涉及GUI样式设置，初始化必须在OnGUI()函数中

        //垂直布局
        GUILayout.BeginVertical(GUILayout.Width(position.width), GUILayout.Height(position.height));
        GUILayout.Label("选择一个UI 视图节点", this.labelStyle);

        #region  水平布局-自定义输入框  
        GUILayout.BeginHorizontal("box", GUILayout.Width(position.width));  //水平布局
        if (GUILayout.Button("生成代码", this.buttonStyle, GUILayout.MinHeight(80), GUILayout.MinWidth(80), GUILayout.MaxWidth(80)))
        {
            if (Selection.activeGameObject != null)
            {
                CreatUISourceFile(Selection.activeGameObject);
                this.finishFlag = true;
                AssetDatabase.Refresh();
            }
        }

        if (this.finishFlag == true)
        {
            GUILayout.TextArea("UI预制体的脚本控制文件已生成", this.textAreaStyle, GUILayout.MinHeight(80));  //自定义输入框
        }
        else
        {
            GUILayout.TextArea("", this.textAreaStyle, GUILayout.MinHeight(80));  //自定义输入框
        }
        GUILayout.Space(5);  //间距设置为5
        GUILayout.EndHorizontal();  //结束水平布局
        #endregion

        if (Selection.activeGameObject != null)
        {
            GUILayout.Label(Selection.activeGameObject.name, this.labelGameObjectNameStyle);
        }
        else
        {
            GUILayout.Label("没有选中的UI节点，无法生成", this.labelStyleMessage);
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
        this.labelStyleMessage = new GUIStyle(GUI.skin.label);
        this.labelStyleMessage.fontSize = 16;  //设置字体大小
        this.labelStyleMessage.normal.textColor = Color.red;
    }
    void OnSelectionChange()
    {
        this.Repaint();
        this.finishFlag = false;
    }

    /// <summary>
    /// 创建UISource源文件
    /// </summary>
    /// <param name="selectGameObject">被选中的UI预制体</param>

    public static void CreatUISourceFile(GameObject selectGameObject)
    {
        string gameObjectName = selectGameObject.name;
        string className = gameObjectName + "UICtrl";
        StreamWriter streamWriter = null;

        if (File.Exists(Application.dataPath + "/Game/Scripts/UIControllers/" + className + ".cs"))
        {
            return;
        }

        streamWriter = new StreamWriter(Application.dataPath + "/Game/Scripts/UIControllers/" + className + ".cs");
        streamWriter.WriteLine("using UnityEngine;\nusing System.Collections;\nusing UnityEngine.UI;\nusing System.Collections.Generic;\n");

        streamWriter.WriteLine("public class " + className + " : UICtrl");
        streamWriter.WriteLine("{");
        streamWriter.WriteLine("\t" + "public override void Awake() ");
        streamWriter.WriteLine("\t" + "{");
        streamWriter.WriteLine("\t\t" + "base.Awake();");
        streamWriter.WriteLine("\t" + "}");

        streamWriter.WriteLine("\t" + "private void Start()");
        streamWriter.WriteLine("\t" + "{" + "\n");
        streamWriter.WriteLine("\t" + "}");
        streamWriter.WriteLine("}");
        streamWriter.Flush();
        streamWriter.Close();
    }
}

