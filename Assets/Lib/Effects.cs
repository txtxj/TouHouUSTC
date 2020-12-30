using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public static class FuncEffects
{
    static double conic(double x)
    {
        return (x - 1) * (x - 1);
    }
    public static double FadeOutEaseOut(int f,int windowFadeInAndOutTime)
    {
        return conic( Convert.ToDouble(f) / windowFadeInAndOutTime );
    }
    public static double FadeOutEaseIn(int f, int windowFadeInAndOutTime)
    {
        return (1 - conic(-(Convert.ToDouble(f) / windowFadeInAndOutTime) + 1));
    }
    public static double FadeOutEaseInOut(int f, int windowFadeInAndOutTime)
    {
        if (f < windowFadeInAndOutTime / 3.0)
        {
            return (-2.0 / windowFadeInAndOutTime * f + 1);
        }
        else
        {
            return (-0.5 / windowFadeInAndOutTime * f + 0.5);
        }
    }//UNFINISHED
    public static double FadeInEaseOut(int f, int windowFadeInAndOutTime)
    {
        return 1 - conic(Convert.ToDouble(f) / windowFadeInAndOutTime);
    }
    public static double FadeInEaseIn(int f, int windowFadeInAndOutTime)
    {
        return conic(-(Convert.ToDouble(f) / windowFadeInAndOutTime) + 1);
    }
}


public static class FuncVA
{
    
}
public class Effects : MonoBehaviour
{
    
}
