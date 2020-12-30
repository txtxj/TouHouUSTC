using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public static class StateLayoutBase
{
    public const int TTLCTRL = 12;
    public static bool once;
    public static bool[,] isFading = new bool[2, TTLCTRL];
}
public static class ReactLayoutBase
{
    public static void SetOnce(bool b)
    {
        StateLayoutBase.once = b;
    }
    public static void Fade(int identifier,int outOrIn)
    {
        StateLayoutBase.isFading[outOrIn, identifier] = true;
        Debug.Log("!!");
    }
} 

public class LayoutBase : MonoBehaviour
{
    float[] tmp = new float[StateLayoutBase.TTLCTRL];
    int[] count = new int[StateLayoutBase.TTLCTRL];
    GameObject[] gos = new GameObject[StateLayoutBase.TTLCTRL];
    bool ustc;
    CanvasGroup canvasGroup;

    public int timeWindowFadeInAndOut;//from settings
    public int timeTransSceneInAndOut;
    public float scaleMinWindowFadeInAndOut;
    public GameObject cvObstruct;
    public GameObject imgUstc;
    public GameObject cvTransScene;
    public GameObject imgTransSceneLeft;
    public GameObject imgTransSceneRight;
    bool isHaveLoadScene = false;

    void FadeApplySingle(int num, bool isScale, bool isAlpha, int t)//num is the identifier of cv ,
    {
        if (StateLayoutBase.isFading[0, num])
        {
            canvasGroup = gos[num].GetComponent<CanvasGroup>();
            count[num]++;
            tmp[num] = Convert.ToSingle(FuncEffects.FadeOutEaseIn(count[num], t));
            if (!gos[num].gameObject.activeInHierarchy)
            {
                gos[num].gameObject.SetActive(true);
            }
            if (count[num] >= t)
            {
                canvasGroup.alpha = 0;
                StateLayoutBase.isFading[0, num] = false;
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
        else if (StateLayoutBase.isFading[1, num])
        {
            canvasGroup = gos[num].GetComponent<CanvasGroup>();
            count[num]++;
            tmp[num] = Convert.ToSingle(FuncEffects.FadeInEaseOut(count[num], t));
            if (count[num] >= t)
            {
                StateLayoutBase.isFading[1, num] = false;
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

    void TransSceneApply()
    {
        
        FadeApplySingle(4, false, true ,timeTransSceneInAndOut+6);
        if ((imgUstc.GetComponent<CanvasGroup>().alpha == 0) && (StateLayoutBase.isFading[0, 2] || StateLayoutBase.isFading[1, 2]))
        {
            count[2]++;
            canvasGroup = cvObstruct.GetComponent<CanvasGroup>();
            if (canvasGroup.alpha != 0)
            {
                canvasGroup.alpha = 0;
                cvObstruct.gameObject.SetActive(true);
                cvTransScene.gameObject.SetActive(true);
            }
            if (StateLayoutBase.isFading[0, 2])
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
                if (StateLayoutBase.isFading[0, 2])
                {
                    StateLayoutBase.isFading[1, 4] = true;
                    ustc = true;
                    //SceneManager.LoadScene(3);//
                    imgTransSceneLeft.transform.localPosition = new Vector3(-680, -425, 0);
                    imgTransSceneRight.transform.localPosition = new Vector3(680, -425, 0);
                    StateLayoutBase.isFading[0, 2] = false;
                    //ReactLayoutBase.Fade(2,1);//
                    //StateLayoutBase.isFading[0, 2] = false;//
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
                    StateLayoutBase.isFading[1, 2] = false;
                    cvObstruct.gameObject.SetActive(false);
                    cvTransScene.gameObject.SetActive(false);
                    canvasGroup.alpha = 1;
                    count[2] = 0;
                }


            }
        }
        if (ustc && imgUstc.GetComponent<CanvasGroup>().alpha == 1f)
        {
            //SceneManager.LoadScene(1);//
            ReactLayoutBattle.Fade(2, 1);//
            StateLayoutBattle.isFading[0, 4] = true;
            if (!isHaveLoadScene)
            {
                isHaveLoadScene = true;
                StartCoroutine(loadScene(1));
            }
            
            
            //cvTransScene.gameObject.SetActive(false);//
        }
    }//转场出/入
    AsyncOperation async;
    IEnumerator loadScene(int scene)
    {
        yield return new WaitForSeconds(1);
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
    void Start()
    {
        gos[2] = cvTransScene;
        gos[4] = imgUstc;
        if (StateLayoutBase.once)
        {
            imgTransSceneLeft.transform.localPosition = new Vector3(-680, -425, 0);
            imgTransSceneRight.transform.localPosition = new Vector3(680, -425, 0);
            cvTransScene.gameObject.SetActive(true);
        }
        
    }
    
    // Update is called once per frame
    void Update()
    {
        TransSceneApply();
    }

    
}
