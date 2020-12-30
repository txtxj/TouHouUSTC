using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

unsafe public class HandlerTitle : MonoBehaviour
{
    public GameObject cvSavings;
    public int[] LoadGameSaving(string storyName)
    {
        int[] sav = new int[2];
        int nMe, nEne, len;
        byte[] buffer = new byte[8];
        byte* bI;
        if(!File.Exists("sav/game/" + storyName))
        {
            return new int[1];
        }
        FileStream fs = new FileStream("sav/game/" + storyName, FileMode.Open, FileAccess.Read, FileShare.None);
        fs.Read(buffer, 0, 8);
        fixed (int* tmp = &sav[0])
        {
            bI = (byte*)tmp;
        }
        for (int i = 0; i < 8; i++)
        {
            *(bI + i) = buffer[i];
        }
        nMe = sav[0];
        nEne = sav[1];
        len = (nMe + nEne + 2) * (22 + 3 * 8 + 3 * 6) + 2 + 4;//0
        buffer = new byte[len * 4];
        fs.Seek(0, SeekOrigin.Begin);
        fs.Read(buffer, 0, len * 4);
        fs.Close();
        sav = new int[len];
        fixed (int* tmp = &sav[0])
        {
            bI = (byte*)tmp;
        }
        for (int i = 0; i < len * 4; i++)
        {
            *(bI + i) = buffer[i];
        }
        return sav;

    }//加载战局
    public void HdlStartOnclk()
    {
        ReactTitle.NewStory();
        Configs.isFromSavOut = false;
        Configs.isFromSavIn = false;
    }
    public void HdlContinueOnclk()
    {
        Debug.Log("Continue onclk");
        int[] sav;
        sav = LoadGameSaving(Configs.storyName);
        if (sav[0] == 0)
        {
            ReactTitle.Fade(1, 1);
            //无存档
        }
        else
        {
            string round = sav[(sav[0] + sav[1] + 2) * (22 + 3 * 8 + 3 * 6) + 2 + 4 - 3].ToString();
            string score = sav[(sav[0] + sav[1] + 2) * (22 + 3 * 8 + 3 * 6) + 2 + 4 - 4].ToString();
            cvSavings.transform.GetChild(1).GetComponent<Text>().text = "局数:  " + round + "\n" + "学分:  " + score;
            ReactTitle.Fade(0, 1);
        }
    }
    public void CloseCvSavings()
    {
        ReactTitle.Fade(0, 0);
    }
    public void CloseCvNoSav()
    {
        ReactTitle.Fade(1, 0);
    }
    public void EnterSavv()
    {
        Configs.isFromSavOut = true;
        Configs.isFromSavIn = false;
        GameObject.Find("New_Game").GetComponent<HandlerBase>().HdlBtStartOnclk();
    }
    public void HdlQuitOnclk()
    {
        Application.Quit();
    }
    
}
