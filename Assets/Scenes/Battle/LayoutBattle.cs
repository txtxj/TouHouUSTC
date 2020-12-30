using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;


static class StateLayoutBattle
{
    public const int TTLCTRL = 16;
    public static bool[,] isFading = new bool[2, TTLCTRL];
    public static Tilemap grid;
    public static int offset;
    /*  gos[0] = cvPause;
        gos[1] = cvSettings;
        gos[3] = cvObstruct;
        gos[2] = cvTransScene;
        gos[4] = imgUstc;
        gos[8] = cvMenuEncirclChess;
    */

}
/*
 已用的：1 cvSettings,0 cvPause 3, cvObstruct
 注意public，Start()中初始化（多为false）和Fade()函数（声明定义）
 */
public static class ReactLayoutBattle
{
    static GameObject cvMenuEncirclChess;
    public static GameObject cvDialogBottom;
    public static GameObject cvAtkPredict;
    public static GameObject btNextRound;

    public static int diaProcs = -1;
    static bool isFirst = true;
    public static int[] stageDiaNum = { 2, 2 };//from configs
    public static int stage = 0;
    public static string[] dialogs = {
            "公元2076年，世界科技水平突飞猛进，世界各地的科技型大学也备受重视。",
            "其中，位于中国合肥的中国科学技术大学一跃成为了世界第一的科学技术高校。",
            "在之后的数十年里，世界各地英才齐聚科大，形成了一片群英荟萃，百家争鸣的繁荣景象。",
            "然而，在这繁荣的科学盛世之下，一股邪恶的力量蠢蠢欲动…………",
            "“科气”————"

        };//from configs
    public static string[] diaPics;
    static Texture2D texture;
    static FileStream fileStream;
    static Sprite sprite;
    public static void Fade(int identifier, int outOrIn)//out 0 in 1
    {
        StateLayoutBattle.isFading[outOrIn, identifier] = true;
    }
    public static void EncirclChess(Vector3Int selectedTile, GameObject go, bool attack, bool defend, bool wait)
    {
        Color32 buttonUnable = new Color32(180, 180, 180, 220);
        cvMenuEncirclChess = go;
        if (!attack)
        {
            go.transform.GetChild(0).GetComponent<Image>().color = buttonUnable;
        }
        if (!defend)
        {
            go.transform.GetChild(1).GetComponent<Image>().color = buttonUnable;
        }
        if (!wait)
        {
            go.transform.GetChild(2).GetComponent<Image>().color = buttonUnable;
        }
        go.transform.position = StateLayoutBattle.grid.CellToWorld(OldCoord(selectedTile));
        Fade(8, 1);
    }//环形菜单打开，传入参数：选中坐标（变换后）， Gameobject环形菜单 ， 攻击、防守、待机各选项是否可用
     //（虽然不可用并显示灰色但是可点击并弹出警告信息）
    public static void DisCirclChess()
    {
        Fade(8, 0);
    }//环形菜单关闭
    public static void PredictAtk(Vector3 pos, int a, int b, int atk1, int hr1, int cr1, int atk2, int hr2, int cr2, int gpa1, int gpa2, bool isDouble, bool isback)
    {
        cvAtkPredict.transform.position = pos;
        cvAtkPredict.transform.GetChild(4).GetComponent<Image>().sprite = LoadSpriteFromFile("Assets/Resources/img/walk/" + a.ToString() + ".png", 360, 360);
        cvAtkPredict.transform.GetChild(5).GetComponent<Image>().sprite = LoadSpriteFromFile("Assets/Resources/img/walk/" + b.ToString() + ".png", 360, 360);
        cvAtkPredict.transform.GetChild(6).GetComponent<Image>().sprite = LoadSpriteFromFile("Assets/Resources/img/walk/" + b.ToString() + ".png", 360, 360);
        cvAtkPredict.transform.GetChild(7).GetComponent<Image>().sprite = LoadSpriteFromFile("Assets/Resources/img/walk/" + a.ToString() + ".png", 360, 360);
        hr1 = Math.Min(100, hr1);
        cr1 = Math.Min(100, cr1);
        hr2 = Math.Min(100, hr1);
        cr2 = Math.Min(100, cr2);
        cvAtkPredict.transform.GetChild(11).GetComponent<Text>().text = ((float)atk1 / 10f).ToString("F1");
        cvAtkPredict.transform.GetChild(12).GetComponent<Text>().text = hr1.ToString();
        cvAtkPredict.transform.GetChild(13).GetComponent<Text>().text = cr1.ToString();
        if (isback)
        {
            cvAtkPredict.transform.GetChild(14).GetComponent<Text>().text = ((float)atk2 / 10f).ToString("F1");
            cvAtkPredict.transform.GetChild(15).GetComponent<Text>().text = hr2.ToString();
            cvAtkPredict.transform.GetChild(16).GetComponent<Text>().text = cr2.ToString();
        }
        else
        {
            cvAtkPredict.transform.GetChild(14).GetComponent<Text>().text = "--";
            cvAtkPredict.transform.GetChild(15).GetComponent<Text>().text = "--";
            cvAtkPredict.transform.GetChild(16).GetComponent<Text>().text = "--";
        }
        cvAtkPredict.transform.GetChild(17).gameObject.SetActive(isDouble);
        cvAtkPredict.transform.GetChild(18).GetComponent<Text>().text = "我方GPA " + ((float)(gpa1 + 10) / 10f).ToString("F1");
        cvAtkPredict.transform.GetChild(19).GetComponent<Text>().text = "敌方GPA " + ((float)(gpa2 + 10) / 10f).ToString("F1");
        Fade(11, 1);
    }
    //攻击预测窗口
    public static void DisPredictAtk()
    {
        Fade(11, 0);
    }
    //攻击预测窗口 打开
    public static void NextDialog()
    {
        Configs.isFromSavIn = Configs.isFromSavOut = false;
        diaProcs++;
        Debug.Log(diaProcs);
        if (isFirst)
        {
            isFirst = false;
            Fade(9, 1);
        }
        int goalStageNum = 0;
        for (int i = 0; i <= stage; i++)
        {
            goalStageNum += stageDiaNum[i];
        }
        if (diaProcs < goalStageNum)
        {
            //这里有问题
            cvDialogBottom.transform.GetChild(1).GetComponent<Image>().sprite = LoadSpriteFromFile("Assets/Resources/img/" + diaPics[diaProcs] + ".png", 278, 353);
            cvDialogBottom.transform.GetChild(0).GetComponent<Text>().text = dialogs[diaProcs];
            Fade(10, 1);
        }
        else
        {
            stage++;
            diaProcs--;
            isFirst = true;
            Debug.Log("114514");
            Fade(9, 0);
            Fade(10, 0);
            GameObject.Find("Main Camera").GetComponent<GameplayEvent>().status = -1;
        }
    }
    //下一对话
    public static void Warn(string warningContent)
    {

    }

    public static Sprite LoadSpriteFromFile(string path, int width, int height)
    {
        fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
        fileStream.Seek(0, SeekOrigin.Begin);
        //创建文件长度缓冲区
        byte[] bytes = new byte[fileStream.Length];
        //读取文件
        fileStream.Read(bytes, 0, (int)fileStream.Length);
        //释放文件读取流
        fileStream.Close();
        fileStream.Dispose();
        fileStream = null;

        //创建Texture
        texture = new Texture2D(width, height);
        texture.LoadImage(bytes);
        sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        return sprite;
    }
    public static Vector3Int OldCoord(Vector3Int pos)
    {
        int offset = StateLayoutBattle.offset;
        pos.x -= offset;
        pos.z -= offset;
        pos.y = -pos.x - pos.z;
        pos.x = pos.x + (pos.y - (pos.y & 1)) / 2;
        pos.z = 0;
        return pos;
    }
}
public class LayoutBattle : MonoBehaviour
{
    int[] count = new int[StateLayoutBattle.TTLCTRL];//计数 
    bool ustc;
    int fps;
    //0cvpause淡出入帧数 1cvsettings淡出入帧数 2fps
    float[] tmp_charainfo = new float[StateLayoutBattle.TTLCTRL];//临时量 
    //0cvpause.canvasgroup.alpha 1cvsettings.canvasgroup.alpha 
    public int timeWindowFadeInAndOut;
    public int timeTransSceneInAndOut;
    public float scaleMinWindowFadeInAndOut;//窗口淡入淡出帧数
    CanvasGroup canvasGroup;//暂停窗口的CanvasGroup
    public GameObject cvPause;
    public GameObject cvSettings;
    public GameObject cvTransScene;
    public GameObject cvObstruct;
    public GameObject txtDebug;
    public GameObject imgTransSceneLeft;
    public GameObject imgTransSceneRight;
    public GameObject imgUstc;
    public GameObject cvMenuEncirclChess;
    public GameObject cvDialog;//9  10
    public GameObject cvAtkPredict;//11
    public GameObject btNextRound;//none
    int second;
    GameObject[] gos = new GameObject[StateLayoutBattle.TTLCTRL];
    void FadeApplySingle(int num, bool isScale, bool isAlpha, int t)//num is the identifier of cv ,
    {
        if (StateLayoutBattle.isFading[0, num])
        {
            canvasGroup = gos[num].GetComponent<CanvasGroup>();
            count[num]++;
            tmp_charainfo[num] = Convert.ToSingle(FuncEffects.FadeOutEaseIn(count[num], t));
            if (count[num] >= t)
            {
                canvasGroup.alpha = 0;
                StateLayoutBattle.isFading[0, num] = false;
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
        else if (StateLayoutBattle.isFading[1, num])
        {
            canvasGroup = gos[num].GetComponent<CanvasGroup>();
            count[num]++;
            tmp_charainfo[num] = Convert.ToSingle(FuncEffects.FadeInEaseOut(count[num], t));
            if (count[num] >= t)
            {
                StateLayoutBattle.isFading[1, num] = false;
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
    void FadeApply()
    {
        FadeApplySingle(0, true, true, timeWindowFadeInAndOut);
        FadeApplySingle(1, true, true, timeWindowFadeInAndOut);
        FadeApplySingle(3, false, true, timeWindowFadeInAndOut);
        FadeApplySingle(9, false, true, timeWindowFadeInAndOut * 2);
        FadeApplySingle(8, false, true, timeWindowFadeInAndOut);
        FadeApplySingle(10, false, true, timeWindowFadeInAndOut * 5);
        FadeApplySingle(11, false, true, timeWindowFadeInAndOut + 5);
    }
    void TransSceneApply()
    {
        FadeApplySingle(4, false, true, timeTransSceneInAndOut + 6);
        if (imgUstc.GetComponent<CanvasGroup>().alpha == 0 && (StateLayoutBattle.isFading[0, 2] || StateLayoutBattle.isFading[1, 2]))
        {
            count[2]++;
            canvasGroup = cvObstruct.GetComponent<CanvasGroup>();
            if (canvasGroup.alpha != 0)
            {
                canvasGroup.alpha = 0;
                cvObstruct.gameObject.SetActive(true);
                cvTransScene.gameObject.SetActive(true);
            }
            if (StateLayoutBattle.isFading[0, 2])
            {
                imgTransSceneLeft.transform.localPosition = new Vector3(Convert.ToSingle(-1360 + FuncEffects.FadeInEaseOut(count[2], timeTransSceneInAndOut) * 680), -425, 0);
                imgTransSceneRight.transform.localPosition = new Vector3(Convert.ToSingle(1360 - FuncEffects.FadeInEaseOut(count[2], timeTransSceneInAndOut) * 680), -425, 0);
            }
            else
            {
                imgTransSceneLeft.transform.localPosition = new Vector3(Convert.ToSingle(-1360 + FuncEffects.FadeOutEaseIn(count[2], timeTransSceneInAndOut) * 680), -425, 0);
                imgTransSceneRight.transform.localPosition = new Vector3(Convert.ToSingle(1360 - FuncEffects.FadeOutEaseIn(count[2], timeTransSceneInAndOut) * 680), -425, 0);
            }
            if (count[2] >= timeTransSceneInAndOut)
            {
                if (StateLayoutBattle.isFading[0, 2])
                {
                    StateLayoutBattle.isFading[1, 4] = true;
                    ustc = true;
                    //SceneManager.LoadScene(3);//
                    imgTransSceneLeft.transform.localPosition = new Vector3(-680, -425, 0);
                    imgTransSceneRight.transform.localPosition = new Vector3(680, -425, 0);
                    StateLayoutBattle.isFading[0, 2] = false;
                    //ReactLayoutBase.Fade(2,1);//
                    //StateLayoutBattle.isFading[0, 2] = false;//
                    Debug.Log("??");
                    cvObstruct.gameObject.SetActive(false);
                    //cvTransScene.gameObject.SetActive(false);//
                    canvasGroup.alpha = 1;
                    count[2] = 0;
                }
                else
                {
                    imgTransSceneLeft.transform.localPosition = new Vector3(-1360, -425, 0);//
                    imgTransSceneRight.transform.localPosition = new Vector3(1360, -425, 0);//
                    StateLayoutBattle.isFading[1, 2] = false;
                    cvObstruct.gameObject.SetActive(false);
                    cvTransScene.gameObject.SetActive(false);
                    canvasGroup.alpha = 1;
                    count[2] = 0;
                }


            }
        }
        if (ustc && imgUstc.GetComponent<CanvasGroup>().alpha == 1f)
        {

            ReactLayoutBase.Fade(2, 1);//
            StateLayoutBase.isFading[0, 4] = true;
            //cvTransScene.gameObject.SetActive(false);//
            ReactLayoutBase.SetOnce(true);
            StartCoroutine(LoadScene(0));//
        }
    }//转场出/入

    AsyncOperation async;
    IEnumerator LoadScene(int scene)
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(2);
        //async = Application.LoadLevelAsync(1);
        async = SceneManager.LoadSceneAsync(scene);
        yield return async;
    }

    void Awake()
    {
        //修改当前的FPS
        Application.targetFrameRate = 75;
    }
    // Start is called before the first frame update
    public void Start()
    {

        gos[0] = cvPause;
        gos[1] = cvSettings;
        gos[3] = cvObstruct;
        gos[2] = cvTransScene;
        gos[4] = imgUstc;
        gos[8] = cvMenuEncirclChess;
        gos[9] = cvDialog;
        ReactLayoutBattle.cvDialogBottom = gos[10] = cvDialog.transform.GetChild(0).transform.GetChild(0).gameObject;
        ReactLayoutBattle.cvAtkPredict = gos[11] = cvAtkPredict;
        ReactLayoutBattle.dialogs = Configs.dialogs_tmp;
        ReactLayoutBattle.diaPics = Configs.diaPics_tmp;
        ReactLayoutBattle.stageDiaNum = Configs.stageDiaNum_tmp;
        Configs.loader.LoadStory(Configs.uLevel);
        Configs.loader.LoadWeaponAndItemSpriteFromFile();
        if (Configs.isFromSavIn || Configs.isFromSavOut)
        {
        }
        else
        {
            ReactLayoutBattle.NextDialog();
        }
        fps = 0;//FPS
        second = DateTime.Now.Second;
    }

    // Update is called once per frame
    void Update()
    {
        FadeApply();
        TransSceneApply();
        if (DateTime.Now.Second == second)
        {
            fps++;
        }
        else
        {
            second = DateTime.Now.Second;
            txtDebug.GetComponent<Text>().text = (fps + 1).ToString() + "FPS";
            fps = 0;
        }//调试：显示帧率
    }
}
