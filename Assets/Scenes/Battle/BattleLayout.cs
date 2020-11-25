// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public struct WindowCB //窗体控制的真值结构体
// {
//     public bool cvPause,cvInfoMenu,cvScoreMenu;
// }
// static public class State{
//     public static WindowCB isFadingOut;
//     public static WindowCB isFadingIn;
// }
// public class BattleLayout : MonoBehaviour
// {
//     public GameObject cvPause;
//     int count;
//     int windowFadeInAndOutTime;
//     float EaseOut(int frame)
//     {
//         if (frame < windowFadeInAndOutTime / 3.0)
//         {
//             return (-2.0 / windowFadeInAndOutTime * frame + 1);
//         }
//         else
//         {
//             return 0.0;
//         }
//     }
//     // Start is called before the first frame update
//     void Start()
//     {
//         cvPause.gameObject.SetActive(false);
//         State.isFadingOut.cvPause = true;
//         count = 0;
//     }

//     // Update is called once per frame
//     void Update()
//     {
//         if (State.isFadingOut.cvPause)
//         {

//         }
//     }
// }
