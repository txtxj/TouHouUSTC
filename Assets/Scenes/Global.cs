using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Configs
{
    /*public static int timeWindowFadeInAndOut = 8;
    public static int timeTransSceneInAndOut = 81;
    public static int scaleMinWindowFadeInAndOut = 0.88;//乘一百取int
    public static int frameRate = 75;*/
    public static Loader loader = new Loader();
    public static int[] settings = new int[8] {8,114,88,75,25,0,0,0};//8
    public static int[] settings_tmp = new int[8] { 8, 81, 88, 75, 25, 0, 0, 0 };//8
    public static Sprite[] CharacterSprites = new Sprite[10];//
    public static Sprite[] LevelSprites = new Sprite[6];//
    public static Sprite[] WeaponSprites = new Sprite[10];
    public static Sprite[] ItemSprites = new Sprite[10];
    public static int[] stageDiaNum_tmp = new int[8];
    public static string[] dialogs_tmp = new string[92];
    public static string[] diaPics_tmp = new string[92];
    public static string storyName = "test";//
    public static bool isFromSavOut = false;
    public static bool isFromSavIn = false;
    public static int uLevel = 1;//
    /*/public static void InitDefault()
    {
        settings
    }*/
}

public static class Story
{

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
            fixed (int* tmp = &(Configs.settings[0]))
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
                Configs.settings_tmp[i] = Configs.settings[i];
            }
        }
        else
        {
            BinaryWriter writer = new BinaryWriter(fileSettings);
            fixed (int* tmp = &(Configs.settings[0])) {
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
    }//将文件读入设置文件 sav/settings.config 读入Configs.settings和Configs.settings_tmp

    public int[] LoadGameSaving(string storyName)
    {
        int[] sav = new int[2];
        int nMe,nEne,len;
        byte[] buffer = new byte[8];
        byte* bI;
        FileStream fs = new FileStream("sav/game/" + storyName, FileMode.Open, FileAccess.Read, FileShare.None);
        fs.Read(buffer, 0, 8);
        fixed(int* tmp = &sav[0])
        {
            bI = (byte*)tmp;
        }
        for (int i = 0;i < 8;i++)
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
        for (int i = 0;i < len * 4;i++)
        {
            *(bI + i) = buffer[i];
        }
        return sav;
    }//加载战局

    public bool SaveGameSaving(string storyName,int[] sav)
    {
        Debug.Log("HAOYE -- Save");
        //Debug.Log("nMe " + sav[0].ToString() + " nENE" + sav[1].ToString());
        int len = sav.Length;
        byte[] buffer = new byte[len * 4];
        byte* bI;
        FileStream fs = new FileStream("sav/game/" + storyName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None);
        fixed (int* tmp = &sav[0])
        {
            bI = (byte*)tmp;
        }
        for (int i = 0; i < len * 4; i++)
        {
            buffer[i] = *(bI + i);
        }
        Debug.Log("nMe " + buffer[0].ToString() + " nENE" + buffer[4].ToString());
        fs.Write(buffer, 0, len * 4);
        fs.Close();
        return true;
    }//保存战局

    public bool LoadStory(int scene)
    {
        StreamReader sr = new StreamReader("Assets/Resources/Story/scene" + scene.ToString(), Encoding.Default);
        String line;
        int count = 0, b = 0, i = 0;
        bool isP = false; 
        while ((line = sr.ReadLine()) != null)
        {
            if (line == "-end-")
            {
                if (!isP)
                {
                    Configs.stageDiaNum_tmp[b] = count / 2;
                    b++;
                }
                count = 0;
            }
            else
            {
                if (isP)
                {
                    Configs.diaPics_tmp[i / 2] = line;
                }
                else
                {
                    Configs.dialogs_tmp[i / 2] = line;
                }
                count++;
                i++;
            }
            isP = !isP;
        }
        Debug.Log("/////");
        Debug.Log(Configs.stageDiaNum_tmp[0]);
        Debug.Log(Configs.stageDiaNum_tmp[1]);
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
    public bool LoadRoleSpriteFromFile(int n)
    {
        Texture2D texture;
        FileStream fileStream;
        Sprite sprite;
        Sprite[] CharacterSprites = Configs.CharacterSprites;
        Sprite[] LevelSprites = Configs.LevelSprites;
        try
        {
            for (int i = 0; i < n; i++)
            {
                fileStream = new FileStream("Assets/Resources/role/walk/" + i.ToString() + ".png", FileMode.Open, FileAccess.Read);
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
                int width = 160;
                int height = 160;
                texture = new Texture2D(width, height);
                texture.LoadImage(bytes);
                sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                CharacterSprites[i] = sprite;
            }
            for (int i = 1; i < 7; i++)
            {
                fileStream = new FileStream("Assets/Resources/role/lv" + i.ToString() + ".png", FileMode.Open, FileAccess.Read);
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
                int width = 160;
                int height = 160;
                texture = new Texture2D(width, height);
                texture.LoadImage(bytes);
                sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                LevelSprites[i-1] = sprite;
            }
            return true;
        }
        catch (IOException e)
        {
            Debug.Log("LoadRoleSpriteFromFile  F A I L ");
            return false;
        }
    }
    public bool LoadWeaponAndItemSpriteFromFile()
    {
        Texture2D texture;
        FileStream fileStream;
        Sprite sprite;
        Sprite[] wSprites = Configs.WeaponSprites;
        Sprite[] iSprites = Configs.ItemSprites;
        try
        {
            for (int i = 0; i < 9; i++)
            {
                fileStream = new FileStream("Assets/Resources/weapon/" + i.ToString() + ".png", FileMode.Open, FileAccess.Read);
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
                int width = 160;
                int height = 160;
                texture = new Texture2D(width, height);
                texture.LoadImage(bytes);
                sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                wSprites[i] = sprite;
            }
            for (int i = 0; i < 9; i++)
            {
                fileStream = new FileStream("Assets/Resources/item/" + i.ToString() + ".png", FileMode.Open, FileAccess.Read);
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
                int width = 160;
                int height = 160;
                texture = new Texture2D(width, height);
                texture.LoadImage(bytes);
                sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                iSprites[i] = sprite;
            }
            return true;
        }
        catch (IOException e)
        {
            Debug.Log("LoadRoleSpriteFromFile  F A I L ");
            return false;
        }
    }
}
