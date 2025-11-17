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
        per = (per < 0) ? 0 : per;
        per = (per > 1) ? 1 : per;

        this.value.fillAmount = per;
    }

    public void ShowAt(Vector3 screenPos) {
        // UGUI ---> ovverlad模式，sceenPos == UI元素的世界坐标;
        this.transform.position = screenPos;
        // UGUI--->摄像机---》摄像机转sceenTo世界;

    }
}
