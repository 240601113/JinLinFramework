using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightMgr : MonoBehaviour
{
    public Camera gameCamera = null; //战斗中的游戏摄像机;
    public Player player = null; //战斗中的玩家;

    public GameUICtrl uiGame = null; // 战斗中游戏UI对象;

    public static FightMgr Instance = null;

    // test
    public GameObject uiBloodPrefab = null;

    public void Awake() 
    {
        FightMgr.Instance = this;

        this.gameCamera = this.transform.Find("Main Camera").GetComponent<Camera>();

        this.uiGame = GameObject.Find("Canvas/GameUI").AddComponent<GameUICtrl>();
        this.uiGame.Init();

        this.player = this.transform.Find("Player").gameObject.AddComponent<Player>();
        this.player.Init();
    }
    
}
