﻿using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Configs
{
    /*public static int timeWindowFadeInAndOut = 8;
    public static int timeTransSceneInAndOut = 81;
    public static int scaleMinWindowFadeInAndOut = 0.88;//乘一百取int
    public static int frameRate = 75;*/
    public static int[] Settings = new int[8] {8,114,88,75,0,0,0,0};//8
    public static int[] Settings_tmp = new int[8] { 8, 81, 88, 75, 0, 0, 0, 0 };//8
    /*/public static void InitDefault()
    {
        Settings
    }*/
}
unsafe public class Loader
{
    bool exit = File.Exists(@"sav/settings.conf");
    string pathSettings = "sav/settings.conf";
    

    //FileStream fileGameSaving = new File
    public bool LoadSettings()
    {   
        FileStream fileSettings = new FileStream(pathSettings, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
        byte[] buffer = new byte[4*8];//8
        byte* p;
        if (exit)
        {
            BinaryReader reader = new BinaryReader(fileSettings);
            buffer = reader.ReadBytes(8*4);//8
            fixed (int* tmp = &(Configs.Settings[0]))
            {
                p = (byte*)tmp;
            }
            for(int i = 0;i < 4*8;i++)//8
            {
                *(p + i) = buffer[i];
            }

            reader.Close();
            for (int i = 0;i < 8;i++)//8
            {
                Configs.Settings_tmp[i] = Configs.Settings[i];
            }
        }
        else
        {
            BinaryWriter writer = new BinaryWriter(fileSettings);
            fixed (int* tmp = &(Configs.Settings[0])) {
                p = (byte*)tmp;
            }
            for (int i = 0;i < 4*8;i++)//8
            {
                buffer[i] = *(p + i);
            }
            writer.Write(buffer);
            writer.Close();
        }
        return true;
    }
    public bool LoadGameSaving(string storyName)
    {
        return true;
    }
    public bool LoadStorySaving(string storyName)
    {
        return true;
    }
    public bool LoadStorySavings()
    {
        return true;
    }
}
