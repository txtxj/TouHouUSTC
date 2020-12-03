using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


static class StateLayoutBattle
{
    public const int TTLCTRL = 8;
    public static bool[,] isFading = new bool[2,TTLCTRL];
}
/*
 已用的：1 cvSettings,0 cvPause 3, cvObstruct
 注意public，Start()中初始化（多为false）和Fade()函数（声明定义）
 */
public static class ReactLayoutBattle
{
    public static void Fade(int identifier,int outOrIn)//out 0 in 1
    {
        StateLayoutBattle.isFading[outOrIn,identifier] = true;
    }
    
}
public class LayoutBattle : MonoBehaviour
{
    int[] count = new int[StateLayoutBattle.TTLCTRL];//计数 
    bool ustc;
    int fps;
    //0cvpause淡出入帧数 1cvsettings淡出入帧数 2fps
    float[] tmp = new float[StateLayoutBattle.TTLCTRL];//临时量 
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
    int second;
    GameObject[] gos = new GameObject[StateLayoutBattle.TTLCTRL];
    void FadeApplySingle(int num,bool isScale,bool isAlpha,int t)//num is the identifier of cv ,
    {
        if (StateLayoutBattle.isFading[0,num])
        {
            canvasGroup = gos[num].GetComponent<CanvasGroup>();
            count[num]++;
            tmp[num] = Convert.ToSingle(FuncEffects.FadeOutEaseIn(count[num], t));
            if (count[num] >= t)
            {
                canvasGroup.alpha = 0;
                StateLayoutBattle.isFading[0,num] = false;
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
        else if (StateLayoutBattle.isFading[1,num])
        {
            canvasGroup = gos[num].GetComponent<CanvasGroup>();
            count[num]++;
            tmp[num] = Convert.ToSingle(FuncEffects.FadeInEaseOut(count[num], t));
            if (count[num] >= t)
            {
                StateLayoutBattle.isFading[1,num] = false;
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
    void FadeApply()
    {
        FadeApplySingle(0, true, true , timeWindowFadeInAndOut);
        FadeApplySingle(1, true, true, timeWindowFadeInAndOut);
        FadeApplySingle(3, false, true, timeWindowFadeInAndOut);
    }
    void TransSceneApply()
    {
        FadeApplySingle(4,false,true,timeTransSceneInAndOut+6);
        if (imgUstc.GetComponent<CanvasGroup>().alpha == 0 && (StateLayoutBattle.isFading[0,2] || StateLayoutBattle.isFading[1, 2]))
        {
            count[2]++;
            canvasGroup = cvObstruct.GetComponent<CanvasGroup>();
            if(canvasGroup.alpha != 0)
            {
                canvasGroup.alpha = 0;
                cvObstruct.gameObject.SetActive(true);
                cvTransScene.gameObject.SetActive(true);
            }
            if (StateLayoutBattle.isFading[0, 2])
            {
                imgTransSceneLeft.transform.localPosition = new Vector3(Convert.ToSingle(-1360 +FuncEffects.FadeInEaseOut(count[2], timeTransSceneInAndOut) * 680), -425, 0);
                imgTransSceneRight.transform.localPosition = new Vector3(Convert.ToSingle(1360 -FuncEffects.FadeInEaseOut(count[2], timeTransSceneInAndOut) * 680), -425, 0);
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
            StartCoroutine(LoadScene(3));//
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
            txtDebug.GetComponent<Text>().text = (fps+1).ToString()+"FPS";
            fps = 0;
        }//调试：显示帧率
    }
}
