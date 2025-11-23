using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBlood : MonoBehaviour
{
    Image value = null;

    public void Init() {
        this.value = this.transform.Find("value").GetComponent<Image>();
    }

    public void SetPercent(float per) {
        //限制安全取值范围
        per = (per < 0) ? 0 : per;//如果值小于0 就等于0 否则就是值是安全取值范围 直接取传进来的值
        per = (per > 1) ? 1 : per;//如果值大于一那直接等于1 否则就是安全取值范围 直接取传进来的值
        
        this.value.fillAmount = per;
    }

    public void ShowAt(Vector3 screenPos) {
        // UGUI ---> ovverlad模式，sceenPos == UI元素的世界坐标;
        this.transform.position = screenPos;
        // UGUI--->摄像机---》摄像机转sceenTo世界;

    }
}
