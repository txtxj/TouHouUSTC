using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HandlerBattle : MonoBehaviour
{
    bool[] tmp_charainfo = new bool[64];
    public void Initmp_charainfo()
    {
        tmp_charainfo = new bool[64];
    }
    public void HdlBtContinueOnclk()
    {
        ReactLayoutBattle.Fade(0, 0);
        ReactLayoutBattle.Fade(3, 0);
    }//继续游戏
    public void HdlBtPauseOnclk()
    {
        int st = GameObject.Find("Main Camera").GetComponent<GameplayEvent>().status;
        if (st != 0 && st != 1 && st != 3)
        {
            return;
        }
        GameObject.Find("Main Camera").GetComponent<GameplayEvent>().HideRange();
        ReactLayoutBattle.Fade(0, 1);
        ReactLayoutBattle.Fade(3, 1);
    }//暂停游戏
    public void HdlBtSettingsOnclk()
    {
        ReactLayoutBattle.Fade(1, 1);
        ReactLayoutBattle.Fade(0, 0);
    }//打开设置
    public void HdlQuitSettingsOnclk()
    {
        ReactLayoutBattle.Fade(1, 0);
        ReactLayoutBattle.Fade(0, 1);
    }//关闭设置
    public void HdlBtQuitOnclk()
    {
        ReactLayoutBattle.Fade(2, 0);
    }

    public void HdlCvBoardOnclk()//BtEqualToGrid
    {
        StateGameplayEvent.common[0] = true;
        Debug.Log("Bo");
    }//点击棋盘
    public void HdlImgOnEEE(int idEop)
    {
        int id = idEop / 10;
        int op = idEop % 10;
        if (tmp_charainfo[id])
        {
            ReactCharaInfo.Fade(id, 0);
            ReactCharaInfo.DisDescribeItem();
            tmp_charainfo[id] = false;
            if (id == 18)
            {
                tmp_charainfo[29] = tmp_charainfo[30] = tmp_charainfo[31] = false;
            }
            else if (id == 19)
            {
                tmp_charainfo[32] = tmp_charainfo[33] = tmp_charainfo[34] = false;

            }
        }
        else
        {
            if (id == 18)
            {
                ReactCharaInfo.Fade(19, 0);
                tmp_charainfo[19] = false;
                tmp_charainfo[32] = tmp_charainfo[33] = tmp_charainfo[34] = false;
            }
            else if (id == 19)
            {
                ReactCharaInfo.Fade(18, 0);
                tmp_charainfo[18] = false;
                tmp_charainfo[29] = tmp_charainfo[30] = tmp_charainfo[31] = false;
            }
            ReactCharaInfo.DetaileOprt(idEop / 10, idEop % 10);
            tmp_charainfo[id] = true;
        }

    }//鼠标移出入
    public void HdlItemOnclk(int typEidEop)
    {
        int typ = typEidEop / 100;
        int id = typEidEop % 100 / 10;
        int op = typEidEop % 10;
        if (op == 4)
        {
            if (!tmp_charainfo[29 + typ * 3 + id])
            {
                ReactCharaInfo.DescribeItem(typ, id);//参数为该道具是否可用 通过character的获取道具是否可用函数得到
                                                     //上述character的函数传入typ和id返回bool[3]数组 0:该道具是否可用，1：该道具是否可停用，2：是否可丢弃
                tmp_charainfo[29 + typ * 3 + id] = true;
            }
            else
            {
                ReactCharaInfo.DisDescribeItem();
                tmp_charainfo[29 + typ * 3 + id] = false;
            }
        }
        else
        {
            int st = GameObject.Find("Main Camera").GetComponent<GameplayEvent>().status;
            if (st != 2)
            {
                return;
            }
            if (typ == 0)//weapon
            {
                StateGameplayEvent.weapon[id, op] = true;
                ReactCharaInfo.charatmp_charainfo.ChangeWeapon(id);
                GameObject.Find("Main Camera").GetComponent<GameplayEvent>().HideRange();
                GameObject.Find("Main Camera").GetComponent<GameplayEvent>().DisplayRange(ReactCharaInfo.charatmp_charainfo.num, 0);
                ReactCharaInfo.Refresh(ReactCharaInfo.charatmp_charainfo.num, GameObject.Find("Main Camera").GetComponent<GameplayEvent>().MapCells[ReactCharaInfo.charatmp_charainfo.x, ReactCharaInfo.charatmp_charainfo.z]);
            }
            else if (typ == 1)//tool
            {
                bool[] Available = ReactCharaInfo.charatmp_charainfo.ItemAvailable(id);
                StateGameplayEvent.tool[id, op] = true;
                if(Available[op])
                {
                    if (op == 0 || op == 1)
                    {
                        ReactCharaInfo.charatmp_charainfo.UseItem(id);
                    }
                    else if (op == 2)
                    {
                        ReactCharaInfo.charatmp_charainfo.DiscardItem(id);
                    }
                    GameObject.Find("Main Camera").GetComponent<GameplayEvent>().HideRange();
                    GameObject.Find("Main Camera").GetComponent<GameplayEvent>().DisplayRange(ReactCharaInfo.charatmp_charainfo.num, 0);
                    ReactCharaInfo.Refresh(ReactCharaInfo.charatmp_charainfo.num, GameObject.Find("Main Camera").GetComponent<GameplayEvent>().MapCells[ReactCharaInfo.charatmp_charainfo.x, ReactCharaInfo.charatmp_charainfo.z]);
                }
                
                
            }
            else//skill?
            {
                StateGameplayEvent.skill[id, op] = true;
            }
        }

    }
    public void HdlEncirclChess(int i)
    {

        if (i == 0)
        {
            ClkAtk();
        }
        else if (i == 1)
        {
            ClkDef();
        }
        else if (i == 2)
        {
            ClkCcl();
        }
        else if (i == 3)
        {
            ClkWat();
        }
        else if (i == 4)
        {
            ClkPsh();
        }
        StateGameplayEvent.roundMenu[i] = true;
        Debug.Log("EN");
    }


    public void HdlNextDialogOnclk()
    {
        ReactLayoutBattle.NextDialog();
    }

    public void HdlBtSaveSavOnclk()
    {
        Configs.loader.SaveGameSaving(Configs.storyName, GameObject.Find("Main Camera").GetComponent<GameplayEvent>().CollectSav(3, 5));//3,5
    }

    public void HdlBtReadSavOnclk()
    {
        Configs.isFromSavIn = true;
        GameObject.Find("Main Camera").GetComponent<GameplayEvent>().Start();
    }



















    public void SwitchCharaInfoToLandInfo()
    {
        ReactCharaInfo.Fade(36, 0);
        ReactCharaInfo.Fade(37, 1);
    }
    public void SwitchLandInfoToCharaInfo()
    {
        ReactCharaInfo.Fade(37, 0);
        ReactCharaInfo.Fade(36, 1);
    }


    public void ClkAtk()
    {
        StateGameplayEvent.common[0] = false;
        //显示攻击范围
        GameObject.Find("Main Camera").GetComponent<GameplayEvent>().DisplayRange(GameObject.Find("Main Camera").GetComponent<GameplayEvent>().Owner[GameObject.Find("Main Camera").GetComponent<GameplayEvent>().globalDestination.x, GameObject.Find("Main Camera").GetComponent<GameplayEvent>().globalDestination.z], 1);
        //改状态
        GameObject.Find("Main Camera").GetComponent<GameplayEvent>().status = 6;
        ReactLayoutBattle.DisCirclChess();
    }
    public void ClkDef()
    {
        StateGameplayEvent.common[0] = false;
        GameObject.Find("Main Camera").GetComponent<GameplayEvent>().status = 7;
        ReactLayoutBattle.DisCirclChess();
    }

    public void ClkSwp()
    {
        StateGameplayEvent.common[0] = false;
        //显示交换范围
        GameObject.Find("Main Camera").GetComponent<GameplayEvent>().DisplayRange(GameObject.Find("Main Camera").GetComponent<GameplayEvent>().Owner[GameObject.Find("Main Camera").GetComponent<GameplayEvent>().globalDestination.x, GameObject.Find("Main Camera").GetComponent<GameplayEvent>().globalDestination.z], 2);
        GameObject.Find("Main Camera").GetComponent<GameplayEvent>().status = 8;
        ReactLayoutBattle.DisCirclChess();
    }

    public void ClkWat()
    {
        StateGameplayEvent.common[0] = false;
        GameObject.Find("Main Camera").GetComponent<GameplayEvent>().status = 9;
        ReactLayoutBattle.DisCirclChess();
    }

    public void ClkNxt()
    {
        int temp = GameObject.Find("Main Camera").GetComponent<GameplayEvent>().status;
        if (temp != 0 && temp != 1 && temp != 2 && temp != 3)
        {
            return;
        }
        StateGameplayEvent.common[0] = false;
        GameObject.Find("Main Camera").GetComponent<GameplayEvent>().status = 10;
        GameObject.Find("Main Camera").GetComponent<GameplayEvent>().ShowPhase(1);
        ReactLayoutBattle.DisCirclChess();
    }
    public void ClkCcl()
    {
        StateGameplayEvent.common[0] = false;
        GameObject.Find("Main Camera").GetComponent<GameplayEvent>().status = 8;
        ReactLayoutBattle.DisCirclChess();
    }
    public void ClkPsh()
    {
        StateGameplayEvent.common[0] = false;
        //显示攻击范围
        GameObject.Find("Main Camera").GetComponent<GameplayEvent>().DisplayRange(GameObject.Find("Main Camera").GetComponent<GameplayEvent>().Owner[GameObject.Find("Main Camera").GetComponent<GameplayEvent>().globalDestination.x, GameObject.Find("Main Camera").GetComponent<GameplayEvent>().globalDestination.z], 2);
        //改状态
        GameObject.Find("Main Camera").GetComponent<GameplayEvent>().status = 17;
        ReactLayoutBattle.DisCirclChess();
    }
    void Start()
    {

    }

    void Update()
    {

    }
}
