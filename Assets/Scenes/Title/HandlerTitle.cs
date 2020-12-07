using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandlerTitle : MonoBehaviour
{
    public void HdlStartOnclk()
    {
        ReactTitle.NewStory();
    }
    public void HdlContinueOnclk()
    {
        ReactTitle.Fade(0,1);
        Debug.Log("Continue onclk");
    }
    public void HdlQuitOnclk()
    {
        Application.Quit();
    }

}
