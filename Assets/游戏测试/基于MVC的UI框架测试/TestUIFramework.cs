using UnityEngine;

public class TestUIFramework : MonoBehaviour
{
    //private UIHomeUICtrl uiHomeUICtrl;
    void Start()
    {
        this.gameObject.AddComponent<UIMgr>().Init();
        //UIMgr.Instance.ShowUIView("UIHome");
    }

}
