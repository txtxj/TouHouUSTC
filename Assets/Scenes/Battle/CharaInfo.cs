using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;

using static ReactLayoutBattle;

//此文件代码的结构和Layoutbattle不同
public static class ReactCharaInfo
{
    public static bool[,] isFading = new bool[2, 48];
    public static string[] landDescription = new string[]{
        "平地",
        "小树林 闪避+20%",
        "废墟 闪避+10%",
        "补给点 闪避+25% 每回合恢复20%Mgpa",
        "水 闪避-20%",
        "墙",
        "墙",
        "火 闪避-10% 每回合损失20%Mgpa",
        "丘陵"
    };
    public static GameObject[] gos = new GameObject[48];
    private static Sprite[] roleImages = new Sprite[16];//精灵
    public static Character charatmp_charainfo;
    private static Color32 DefaultColor = new Color32(255, 129, 129, 255);
    private static Color32 SelectedColor = new Color32(240, 60, 60, 255);
    public static void Init(GameObject[] gos_tmp_charainfo, Sprite[] roleImages_tmp_charainfo)
    {
        roleImages[0] = roleImages_tmp_charainfo[0];
        gos = gos_tmp_charainfo;
    }
    public static void Fade(int identifier,int outOrIn)
    {
        isFading[outOrIn, identifier] = true;
    }
    public static void Open(int owner, int land)
    {

        Refresh(owner, land);
        Debug.Log("CvInfoOpen");
        Fade(0, 1);

    }//打开
    public static void Refresh(int owner, int land)
    {
    	gos[45].GetComponent<Text>().text = "学分：" + gos[38].GetComponent<GameplayEvent>().score + "\n局数：" + gos[38].GetComponent<GameplayEvent>().round;
        if (owner != 0)//landform
        {
            bool b = (owner > 0) ? true : false;
            Color32 me_color = new Color32(50, 200, 255, 255);
            Color32 ene_color = new Color32(255, 115, 114, 255);
            Character chara = charatmp_charainfo = b ? gos[38].GetComponent<GameplayEvent>().CharacterList[owner] : gos[38].GetComponent<GameplayEvent>().EnemyList[-owner];
            gos[1].GetComponent<Image>().sprite = LoadSpriteFromFile("Assets/Resources/img/walk/" + chara.role.ToString() + ".png", 360, 360);
            gos[20].GetComponent<Slider>().value = gos[2].GetComponent<Slider>().value = (chara.gpa + 0f) / chara.mgpa;
            gos[21].GetComponent<Text>().text = gos[3].GetComponent<Text>().text = ((chara.gpa + 10) / 10f).ToString("F1") + "/" + ((chara.mgpa + 10) / 10f).ToString("F1");
            gos[22].GetComponent<Slider>().value = gos[4].GetComponent<Slider>().value = (chara.mag + 0f) / chara.mmag;
            gos[23].GetComponent<Text>().text = gos[5].GetComponent<Text>().text = chara.mag + "/" + chara.mmag;
            gos[24].GetComponent<Slider>().value = gos[6].GetComponent<Slider>().value = (chara.exp + 0f) / chara.mexp;
            gos[25].GetComponent<Text>().text = gos[7].GetComponent<Text>().text = chara.exp.ToString() + "/" + chara.mexp.ToString();
            gos[8].GetComponent<Text>().text = chara.def.ToString();
            gos[9].GetComponent<Text>().text = chara.atk.ToString();
            gos[10].GetComponent<Text>().text = chara.tec.ToString();
            gos[11].GetComponent<Text>().text = chara.agi.ToString();
            gos[12].GetComponent<Text>().text = chara.mov.ToString();
            gos[13].GetComponent<Text>().text = chara.luk.ToString();
            gos[14].GetComponent<Text>().text = chara.wgt.ToString();
            gos[15].GetComponent<Text>().text = chara.rid == 0 ? "步行" : (chara.rid == 1 ? "飞行" : "骑行");
            gos[16].gameObject.SetActive(false);
            gos[18].gameObject.SetActive(false);
            gos[19].gameObject.SetActive(false);
            gos[27].gameObject.SetActive(false);
            gos[35].GetComponent<HandlerBattle>().Initmp_charainfo();
            gos[36].gameObject.SetActive(true);
            gos[0].GetComponent<Image>().color = gos[37].GetComponent<Image>().color = gos[36].GetComponent<Image>().color = (b ? me_color : ene_color);
            gos[37].gameObject.SetActive(false);
            gos[39].gameObject.SetActive(b);
            gos[40].gameObject.SetActive(b);
            gos[29].transform.GetChild(0).GetComponent<Image>().sprite = Configs.WeaponSprites[Convert.ToInt32(charatmp_charainfo.arsenal[0].img[0])-48];
            gos[30].transform.GetChild(0).GetComponent<Image>().sprite = Configs.WeaponSprites[Convert.ToInt32(charatmp_charainfo.arsenal[1].img[0])-48];
            gos[32].transform.GetChild(0).GetComponent<Image>().sprite = Configs.ItemSprites[Convert.ToInt32(charatmp_charainfo.bag[0].img[0])-48];
            gos[33].transform.GetChild(0).GetComponent<Image>().sprite = Configs.ItemSprites[Convert.ToInt32(charatmp_charainfo.bag[1].img[0])-48];
            gos[34].transform.GetChild(0).GetComponent<Image>().sprite = Configs.ItemSprites[Convert.ToInt32(charatmp_charainfo.bag[2].img[0])-48];
            gos[32].gameObject.SetActive(Convert.ToInt32(charatmp_charainfo.bag[0].img[0]) == 48 ? false : true);
            gos[33].gameObject.SetActive(Convert.ToInt32(charatmp_charainfo.bag[1].img[0]) == 48 ? false : true);
            gos[34].gameObject.SetActive(Convert.ToInt32(charatmp_charainfo.bag[2].img[0]) == 48 ? false : true);
            gos[43].GetComponent<Text>().text = "Lv." + chara.lv.ToString();
            gos[44].GetComponent<Text>().text = "Lv." + chara.lv.ToString();
        }
        else
        {
            gos[36].gameObject.SetActive(false);
            gos[37].gameObject.SetActive(true);
        }
        gos[41].GetComponent<Text>().text = landDescription[land];
        //gos[42].GetComponent<Image>().sprite = landImages[0];landImages[0]**还有预览**！
        /*
        
        gos[18] = cvWeapon;//18
        gos[19] = cvRack;//19
        gos[22] = sldMag;//22
        gos[23] = txtValueMag_;//23
        gos[24] = sldExp_;//24
        gos[25] = txtValueExp_;//25
        gos[27] = cvItemDes;//27
        gos[28] = txtItemDes;//28
        gos[29] = CvWeaponItem1;//29
        gos[30] = CvWeaponItem2;//30
        gos[31] = CvWeaponItem3;//31
        gos[32] = CvWeaponItem4;//32
        gos[33] = CvWeaponItem5;//33
        gos[34] = CvWeaponItem6;//34
        gos[24] = sldExp_;//24
        gos[25] = txtValueExp_;//25
        gos[27] = cvItemDes;//27
        gos[28] = txtItemDes;//28
        gos[29] = CvWeaponItem1;//29
        gos[30] = CvWeaponItem2;//30
        gos[31] = CvWeaponItem3;//31
        gos[32] = CvWeaponItem4;//32
        gos[33] = CvWeaponItem5;//33
        gos[34] = CvWeaponItem6;//34
        gos[35] = Canvas;
        gos[36] = cvInfoChara;
        gos[37] = cvInfoLand;
        gos[38] = camera;
        gos[39] = imgWeapon;
        gos[40] = imgRack;
        gos[41] = txtLandDes;
        gos[42] = imgLandDes;
        */

    }//刷新
    public static void RefreshRound()
    {
        gos[45].GetComponent<Text>().text = "学分：" + gos[38].GetComponent<GameplayEvent>().score + "\n局数：" + gos[38].GetComponent<GameplayEvent>().round;
    }
    public static void Close()
    {
        isFading[0, 0] = true;
    }
    public static void DescribeItem(int typ, int id)
    {//描述文本在角色charatmp_charainfo
        if (typ == 0)
        {
            gos[28].GetComponent<Text>().text = charatmp_charainfo.arsenal[id].des;

        }
        else if (typ == 1)
        {
            bool[] available = charatmp_charainfo.ItemAvailable(id);
            gos[28].GetComponent<Text>().text = charatmp_charainfo.bag[id].des + "\n" + (available[0] ? "可使用\n" : "不可使用\n") + (available[1] ? "可卸下\n" : "不可卸下\n") + (available[2] ? "可丢弃" : "不可丢弃");

        }

        Fade(27, 1);
        if (typ == 0)
        {
            gos[29].GetComponent<Image>().color = DefaultColor;
            gos[30].GetComponent<Image>().color = DefaultColor;
            gos[31].GetComponent<Image>().color = DefaultColor;
        }
        else if (typ == 1)
        {
            gos[32].GetComponent<Image>().color = DefaultColor;
            gos[33].GetComponent<Image>().color = DefaultColor;
            gos[34].GetComponent<Image>().color = DefaultColor;
        }
        gos[typ * 3 + id + 29].GetComponent<Image>().color = SelectedColor;
    }
    public static void DisDescribeItem()
    {
        for(int i = 0;i < 6; i++)
        {
            gos[29 + i].GetComponent<Image>().color = DefaultColor;
        }
        Debug.Log("选项详情关闭 CharaInfo.DisDescribeItem()");
        Fade(27, 0);
    }

    
    public static void DetaileOprt(int id,int openOrClose)
    {
        /* imgChara;//16
            cvLandform;//17
            cvWeapon;//18
            cvRack;//19
        */
        DisDescribeItem();
        Fade(id,openOrClose);
    }
    
}

public class CharaInfo : MonoBehaviour
{
    GameObject[] gos = new GameObject[48];
    Sprite[] roleImages = new Sprite[16];//精灵
    int timeWindowFadeInAndOut;
    int timeTransSceneInAndOut;
    int scaleMinWindowFadeInAndOut;
    int[] count = new int[48];//计数 
    float[] tmp_charainfo = new float[48];//临时量 
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
    public GameObject cvWeapon;//18
    public GameObject cvRack;//19
    public GameObject sldGpa_;//20
    public GameObject txtValueGpa_;//21
    public GameObject sldMag_;//22
    public GameObject txtValueMag_;//23
    public GameObject sldExp_;//24
    public GameObject txtValueExp_;//25
    public Sprite rim;//26
    public GameObject cvItemDes;//27
    public GameObject txtItemDes;//28
    public GameObject CvWeaponItem1;//29
    public GameObject CvWeaponItem2;//30
    public GameObject CvWeaponItem3;//31
    public GameObject CvWeaponItem4;//32
    public GameObject CvWeaponItem5;//33
    public GameObject CvWeaponItem6;//34
    public GameObject Canvas;//35
    public GameObject cvInfoChara;//36
    public GameObject cvInfoLand;//37
    public GameObject camera;//38
    public GameObject imgWeapon;//39
    public GameObject imgRack;//40
    public GameObject txtLandDes;//41
    public GameObject imgLandDes;//42
    public GameObject txtlv;
    public GameObject txtlv_;
    public GameObject others;
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
        gos[18] = cvWeapon;//18
        gos[19] = cvRack;//19
        gos[20] = sldGpa_;//20
        gos[21] = txtValueGpa_;//21
        gos[22] = sldMag_;//22
        gos[23] = txtValueMag_;//23
        gos[24] = sldExp_;//24
        gos[25] = txtValueExp_;//25
        gos[27] = cvItemDes;//27
        gos[28] = txtItemDes;//28
        gos[29] = CvWeaponItem1;//29
        gos[30] = CvWeaponItem2;//30
        gos[31] = CvWeaponItem3;//31
        gos[32] = CvWeaponItem4;//32
        gos[33] = CvWeaponItem5;//33
        gos[34] = CvWeaponItem6;//34
        gos[35] = Canvas;
        gos[36] = cvInfoChara;
        gos[37] = cvInfoLand;
        gos[38] = camera;
        gos[39] = imgWeapon;
        gos[40] = imgRack;
        gos[41] = txtLandDes;
        gos[42] = imgLandDes;
        gos[43] = txtlv;
        gos[44] = txtlv_;
        gos[45] = others;
        roleImages[0] = rim;
        timeWindowFadeInAndOut = Configs.settings[0];
        scaleMinWindowFadeInAndOut = Configs.settings[2] / 100;
    }
    void FadeApplySingle(int num, bool isScale, bool isAlpha, int t)//num is the identifier of cv ,
    {
        if (ReactCharaInfo.isFading[0, num])
        {
            canvasGroup = gos[num].GetComponent<CanvasGroup>();
            count[num]++;
            tmp_charainfo[num] = Convert.ToSingle(FuncEffects.FadeOutEaseIn(count[num], t));
            if (count[num] >= t)
            {
                canvasGroup.alpha = 0;
                ReactCharaInfo.isFading[0, num] = false;
                count[num] = 0;
                tmp_charainfo[num] = 0;
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
                    canvasGroup.alpha = tmp_charainfo[num];
                }
                if (isScale)
                {
                    gos[num].transform.localScale = new Vector3(tmp_charainfo[num] * (1 - scaleMinWindowFadeInAndOut) + scaleMinWindowFadeInAndOut, tmp_charainfo[num] * (1 - scaleMinWindowFadeInAndOut) + scaleMinWindowFadeInAndOut, 1f);
                }
            }
        }//gos[num]淡出
        else if (ReactCharaInfo.isFading[1, num])
        {
            canvasGroup = gos[num].GetComponent<CanvasGroup>();
            count[num]++;
            tmp_charainfo[num] = Convert.ToSingle(FuncEffects.FadeInEaseOut(count[num], t));
            if (count[num] >= t)
            {
                ReactCharaInfo.isFading[1, num] = false;
                count[num] = 0;
                tmp_charainfo[num] = 0;
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
                    canvasGroup.alpha = tmp_charainfo[num];
                }
                if (isScale)
                {
                    gos[num].transform.localScale = new Vector3(tmp_charainfo[num] * (1 - scaleMinWindowFadeInAndOut) + scaleMinWindowFadeInAndOut, tmp_charainfo[num] * (1 - scaleMinWindowFadeInAndOut) + scaleMinWindowFadeInAndOut, 1f);
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
        FadeApplySingle(37, true, true, timeWindowFadeInAndOut);
        FadeApplySingle(36, true, true, timeWindowFadeInAndOut);
        FadeApplySingle(18, false, true, timeWindowFadeInAndOut);
        FadeApplySingle(19, false, true, timeWindowFadeInAndOut);
        FadeApplySingle(16, true, true, timeWindowFadeInAndOut);
        FadeApplySingle(27, true, true, timeWindowFadeInAndOut);
    }
    // Start is called before the first frame update
    void Start()
    {
        GosInit();
        ReactCharaInfo.Init(gos, roleImages);
        ReactCharaInfo.Close();
    }

    // Update is called once per frame
    void Update()
    {
        
        FadeApply();
        
    }
}
