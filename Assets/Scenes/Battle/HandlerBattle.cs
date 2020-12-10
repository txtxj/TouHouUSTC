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
    public void HdlCvBoardOnclk()//BtEqualToGrid
    {
        StateGameplayEvent.tmp[0] = true;
    }//点击棋盘
    public void HdlImgOnEEE(int idEop)
    {
        ReactCharaInfo.DetaileOprt(idEop/10, idEop%10);
    }//鼠标移出入
    
    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
