using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleAnimation : MonoBehaviour
{
	public GameObject title;
	public GameObject left;
	public GameObject right;
	public GameObject bt1;
	public GameObject bt2;
	public GameObject bt3;
	public GameObject bdg;

	public void ScaleUp()
	{
		iTween.ScaleTo(bdg, iTween.Hash("scale", new Vector3(12, 12, 1), "time", 0.5f, "islocal", true, "EaseType", "easeInOutBack", "oncomplete", "MoveBt1", "oncompletetarget", gameObject));
	}
	public void MoveBt1()
	{
		iTween.MoveTo(bt1, iTween.Hash("position", new Vector3(-2, -54, 0), "time", 0.5f, "islocal", true, "EaseType", "easeOutBack", "oncomplete", "MoveBt2", "oncompletetarget", gameObject));
	}
	public void MoveBt2()
	{
		iTween.MoveTo(bt2, iTween.Hash("position", new Vector3(-2, -160, 0), "time", 0.5f, "islocal", true, "EaseType", "easeOutBack", "oncomplete", "MoveBt3", "oncompletetarget", gameObject));
	}
	public void MoveBt3()
	{
		iTween.MoveTo(bt3, iTween.Hash("position", new Vector3(-2, -266, 0), "time", 0.5f, "islocal", true, "EaseType", "easeOutBack"));
	}

    void Start()
    {
		iTween.MoveTo(title, iTween.Hash("position", new Vector3(14, 213, 0), "time", 2f, "islocal", true, "EaseType", "easeOutCirc"));
		iTween.MoveTo(left, iTween.Hash("position", new Vector3(-513, -54, 0), "time", 2f, "islocal", true, "EaseType", "easeInOutBack"));
		iTween.MoveTo(right, iTween.Hash("position", new Vector3(451, -104, 0), "time", 4f, "islocal", true, "EaseType", "easeOutQuad", "oncomplete", "ScaleUp", "oncompletetarget",gameObject));
    }

    void Update()
    {

    }
}
