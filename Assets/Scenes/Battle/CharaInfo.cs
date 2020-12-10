using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;

//此文件代码的结构和Layoutbattle不同
public static class ReactCharaInfo
{
    public static bool[,] isFading = new bool[2,48];
    private static GameObject[] gos = new GameObject[48];
    private static Sprite[] roleImages = new Sprite[16];//精灵
    public static void Init(GameObject[] gos_tmp, Sprite[] roleImages_tmp)
    {
        roleImages[0] = roleImages_tmp[0];
        gos[1] = gos_tmp[1];
        
    }
    public static void Fade(int identifier,int outOrIn)
    {
        isFading[outOrIn, identifier] = true;
    }
    public static void Open(Character chara)
    {
        gos[1].GetComponent<Image>().sprite = roleImages[0];
        Debug.Log("CvInfoOpen");
        Fade(0, 1);
    }
    public static void Close()
    {
        isFading[0, 0] = true;
    }

    
    public static void DetaileOprt(int id,int openOrClose)
    {
        /* imgChara;//16
            cvLandform;//17
            cvWeapon;//18
            cvRack;//19
        */
        Fade(id,openOrClose);
    }
    
}

public class CharaInfo : MonoBehaviour
{
    public GameObject[] gos = new GameObject[48];
    Sprite[] roleImages = new Sprite[16];//精灵
    int timeWindowFadeInAndOut;
    int timeTransSceneInAndOut;
    int scaleMinWindowFadeInAndOut;
    int[] count = new int[48];//计数 
    float[] tmp = new float[48];//临时量 
    CanvasGroup canvasGroup = new CanvasGroup();

    public GameObject cvInfo;//0
    public GameObject imgChara;//1
    public GameObject sldGpa;//2
    public GameObject txtValueGpa;//3
    public GameObject sldMag;//4
    public GameObject txtValueMag;//5
    public GameObject sldExp;//6
    public GameObject txtValueExp;//7
    public GameObject txtDef;//8
    public GameObject txtAtk;//9
    public GameObject txtTec;//10
    public GameObject txtAgi;//11
    public GameObject txtMov;//12
    public GameObject txtLuk;//13
    public GameObject txtWgt;//14
    public GameObject txtVeh;//15
    public GameObject cvCharaInfoDetail;//16
    public GameObject cvLandform;//17
    public GameObject cvWeapon;//18
    public GameObject cvRack;//19
    public GameObject sldGpa_;//20
    public GameObject txtValueGpa_;//21
    public GameObject sldMag_;//22
    public GameObject txtValueMag_;//23
    public GameObject sldExp_;//24
    public GameObject txtValueExp_;//25
    public Sprite rim;
    //public GameObject c; //26
    //public GameObject c;//27

    void GosInit()
    {
        gos[0] = cvInfo;//0
        gos[1] = imgChara;//1
        gos[2] = sldGpa;//2
        gos[3] = txtValueGpa;//3
        gos[4] = sldMag;//4
        gos[5] = txtValueMag;//5
        gos[6] = sldExp;//6
        gos[7] = txtValueExp;//7
        gos[8] = txtDef;//8
        gos[9] = txtAtk;//9
        gos[10] = txtTec;//10
        gos[11] = txtAgi;//11
        gos[12] = txtMov;//12
        gos[13] = txtLuk;//13
        gos[14] = txtWgt;//14
        gos[15] = txtVeh;//15
        gos[16] = cvCharaInfoDetail;//16
        gos[17] = cvLandform;//17
        gos[18] = cvWeapon;//18
        gos[19] = cvRack;//19
        gos[20] = sldGpa_;//20
        gos[21] = txtValueGpa_;//21
        gos[22] = sldMag;//22
        gos[23] = txtValueMag_;//23
        gos[24] = sldExp_;//24
        gos[25] = txtValueExp_;//25
        roleImages[0] = rim;
        timeWindowFadeInAndOut = Configs.settings[0];
        scaleMinWindowFadeInAndOut = Configs.settings[2];
    }
    void FadeApplySingle(int num, bool isScale, bool isAlpha, int t)//num is the identifier of cv ,
    {
        if (ReactCharaInfo.isFading[0, num])
        {
            canvasGroup = gos[num].GetComponent<CanvasGroup>();
            count[num]++;
            tmp[num] = Convert.ToSingle(FuncEffects.FadeOutEaseIn(count[num], t));
            if (count[num] >= t)
            {
                canvasGroup.alpha = 0;
                ReactCharaInfo.isFading[0, num] = false;
                count[num] = 0;
                tmp[num] = 0;
                gos[num].gameObject.SetActive(false);
                //修正
            }
            else
            {//变换
                if (canvasGroup.interactable)
                {
                    canvasGroup.interactable = false;
                }
                if (isAlpha)
                {
                    canvasGroup.alpha = tmp[num];
                }
                if (isScale)
                {
                    gos[num].transform.localScale = new Vector3(tmp[num] * (1 - scaleMinWindowFadeInAndOut) + scaleMinWindowFadeInAndOut, tmp[num] * (1 - scaleMinWindowFadeInAndOut) + scaleMinWindowFadeInAndOut, 1f);
                }
            }
        }//gos[num]淡出
        else if (ReactCharaInfo.isFading[1, num])
        {
            canvasGroup = gos[num].GetComponent<CanvasGroup>();
            count[num]++;
            tmp[num] = Convert.ToSingle(FuncEffects.FadeInEaseOut(count[num], t));
            if (count[num] >= t)
            {
                ReactCharaInfo.isFading[1, num] = false;
                count[num] = 0;
                tmp[num] = 0;
                canvasGroup.alpha = 1;
                gos[num].transform.localScale = new Vector3(1f, 1f, 1f);
                //修正alpha和scale
                if (!canvasGroup.interactable)
                {
                    canvasGroup.interactable = true;
                }
            }
            else
            {//变换
                if (isAlpha)
                {
                    canvasGroup.alpha = tmp[num];
                }
                if (isScale)
                {
                    gos[num].transform.localScale = new Vector3(tmp[num] * (1 - scaleMinWindowFadeInAndOut) + scaleMinWindowFadeInAndOut, tmp[num] * (1 - scaleMinWindowFadeInAndOut) + scaleMinWindowFadeInAndOut, 1f);
                }
            }
            if (!gos[num].gameObject.activeInHierarchy)
            {
                gos[num].gameObject.SetActive(true);
            }
        }//gos[num]淡入
    }
    void FadeApply()//listen fade
    {
        FadeApplySingle(0,false,true,timeWindowFadeInAndOut + 2);
        FadeApplySingle(17, false, true, timeWindowFadeInAndOut);
        FadeApplySingle(18, false, true, timeWindowFadeInAndOut);
        FadeApplySingle(19, false, true, timeWindowFadeInAndOut);
        FadeApplySingle(16, false, true, timeWindowFadeInAndOut);
    }
    // Start is called before the first frame update
    void Start()
    {
        GosInit();
        ReactCharaInfo.Init(gos, roleImages);
    }

    // Update is called once per frame
    void Update()
    {
        
        FadeApply();
        
    }
}
