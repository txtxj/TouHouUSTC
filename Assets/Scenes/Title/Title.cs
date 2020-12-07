using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public static class StateTitle{
    public static bool[,] isFading = new bool[2,8];//0 out   1 in   
}
public static class ReactTitle
{
    public static void Fade(int identifier,int outOrIn)
    {
        StateTitle.isFading[outOrIn, identifier] = true;
    }
    public static void NewStory()
    {
        GotoBase();
    }

    static void GotoBase()
    {
        StateTitle.isFading[0, 7] = true;
    }
}
public class Title : MonoBehaviour
{
    Loader loader = new Loader();//实例化加载器
    float[] tmp = new float[8];
    int[] count = new int[8];
    GameObject[] gos = new GameObject[8];
    CanvasGroup canvasGroup;

    int timeWindowFadeInAndOut = Configs.settings[0];//from settings↓
    int timeTransSceneInAndOut = Configs.settings[1];
    float scaleMinWindowFadeInAndOut = Configs.settings[2] / 100f;

    public GameObject cvSavings;

    void FadeApplySingle(int num, bool isScale, bool isAlpha, int t)//num is the identifier of cv ,
    {
        if (StateTitle.isFading[0, num])
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
                StateTitle.isFading[0, num] = false;
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
        else if (StateTitle.isFading[1, num])
        {
            canvasGroup = gos[num].GetComponent<CanvasGroup>();
            count[num]++;
            tmp[num] = Convert.ToSingle(FuncEffects.FadeInEaseOut(count[num], t));
            if (count[num] >= t)
            {
                StateTitle.isFading[1, num] = false;
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
        FadeApplySingle(0,true,true,timeWindowFadeInAndOut);
        if (StateTitle.isFading[0,7])
        {
            StartCoroutine(loadScene(3));
            StateTitle.isFading[0,7] = false;
        }
    }

    void applySettings()
    {
        timeWindowFadeInAndOut = Configs.settings[0];//from settings↓
        timeTransSceneInAndOut = Configs.settings[1];
        scaleMinWindowFadeInAndOut = Configs.settings[2] / 100f;
    }

    AsyncOperation async;
    IEnumerator loadScene(int scene)
    {
        yield return new WaitForEndOfFrame();
        //yield return new WaitForSeconds(1);
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
        loader.LoadSettings();
        applySettings();
        gos[0] = cvSavings;
    }

    // Update is called once per frame
    void Update()
    {
        FadeApply();

    }

}
