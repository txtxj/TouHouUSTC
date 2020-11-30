using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HandlerBattle : MonoBehaviour
{
    public void HdlBtContinueOnclk()
    {
        ReactLayoutBattle.Fade(0, 0);
        ReactLayoutBattle.Fade(3, 0);
    }//继续游戏
    public void HdlBtPauseOnclk()
    {
        ReactLayoutBattle.Fade(0, 1);
        ReactLayoutBattle.Fade(3, 1);
    }//暂停游戏
    public void HdlBtSettingsOnclk()
    {
        ReactLayoutBattle.Fade(1, 1);
        ReactLayoutBattle.Fade(0, 0);
    }//打开设置
    public void HdlBtQuitOnclk()
    {
        ReactLayoutBattle.Fade(2, 0);
    }
    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
