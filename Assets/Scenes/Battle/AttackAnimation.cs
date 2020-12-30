using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

using static ReactLayoutBattle;

public class AttackAnimation : MonoBehaviour
{
	public GameObject PrefabAttackRole;

	public GameObject cv;

	public GameObject background0;
	public GameObject background1;

	GameObject[] attackBackground = new GameObject[2];

	GameObject[] AttackRole = new GameObject[3];
	Vector3[] RolePosition = new Vector3[4]
	{
		new Vector3(-332, 0, 40),
		new Vector3(354, 0, 40),
		new Vector3(-492, 0, 40),
		new Vector3(514, 0, 40)
	};

	bool flaganimation;
	bool flagdestroy;
	bool flagfinal;
	int timeCounter = 0;
	int totltime = 0;

	Queue<Vector3Int> AnimationQueue = new Queue<Vector3Int>();

	//战斗动画入口，传入两个图片编号
	public void Display(int role1, int role2, int turn)
	{
		//应当保证立绘图片600x600，不然可能出现拉伸变形现象
		attackBackground[turn].gameObject.SetActive(true);
		AttackRole[0] = Instantiate(PrefabAttackRole);
		AttackRole[1] = Instantiate(PrefabAttackRole);
		AttackRole[0].transform.SetParent(cv.transform);
		AttackRole[1].transform.SetParent(cv.transform);
		AttackRole[0].transform.localScale = new Vector3(1, 1, 1);
		AttackRole[1].transform.localScale = new Vector3(1, 1, 1);
		AttackRole[0].GetComponent<Image>().sprite = LoadSpriteFromFile("Assets/Resources/img/atk/" + role1.ToString() + ".png", 600, 600);
		AttackRole[1].GetComponent<Image>().sprite = LoadSpriteFromFile("Assets/Resources/img/atk/" + role2.ToString() + ".png", 600, 600);
		AttackRole[0].transform.localPosition = RolePosition[0];
		AttackRole[1].transform.localPosition = RolePosition[1];
	}

	//战斗动画结束
	public void Hide()
	{
		attackBackground[0].gameObject.SetActive(false);
		attackBackground[1].gameObject.SetActive(false);
		Destroy(AttackRole[0]);
		Destroy(AttackRole[1]);
		AnimationQueue.Clear();
	}

	//A攻击B
	public void Atk1(int role)
	{
		if (role == 0)
		{
			iTween.MoveTo(AttackRole[0], iTween.Hash("position", RolePosition[1], "time", 1f, "islocal", true, "EaseType", "easeOutBack", "oncomplete", "Back1", "oncompletetarget", gameObject));
		}
		else
		{
			flagdestroy = true;
			AttackRole[2] = Instantiate(PrefabAttackRole);
			AttackRole[2].transform.SetParent(cv.transform);
			AttackRole[2].transform.localScale = new Vector3(1, 1, 1);
			AttackRole[2].GetComponent<Image>().sprite = LoadSpriteFromFile("Assets/Resources/img/atk/tele" + role.ToString() + ".png", 600, 600);
			AttackRole[2].transform.localPosition = RolePosition[0];
			iTween.MoveTo(AttackRole[2], iTween.Hash("position", RolePosition[1], "time", 1f, "islocal", true, "EaseType", "linear", "oncomplete", "Back1", "oncompletetarget", gameObject));
		}
	}
	//B攻击A
	public void Atk2(int role)
	{
		if (role == 0)
		{
			iTween.MoveTo(AttackRole[1], iTween.Hash("position", RolePosition[0], "time", 1f, "islocal", true, "EaseType", "easeOutBack", "oncomplete", "Back2", "oncompletetarget", gameObject));
		}
		else
		{
			flagdestroy = true;
			AttackRole[2] = Instantiate(PrefabAttackRole);
			AttackRole[2].transform.SetParent(cv.transform);
			AttackRole[2].transform.localScale = new Vector3(1, 1, 1);
			AttackRole[2].GetComponent<Image>().sprite = LoadSpriteFromFile("Assets/Resources/img/atk/tele" + role.ToString() + ".png", 600, 600);
			AttackRole[2].transform.localPosition = RolePosition[1];
			AttackRole[2].transform.localRotation = new Quaternion(0, 0, 180, 0);
			iTween.MoveTo(AttackRole[2], iTween.Hash("position", RolePosition[0], "time", 1f, "islocal", true, "EaseType", "linear", "oncomplete", "Back2", "oncompletetarget", gameObject));
		}
	}

	//A闪避
	public void Miss1()
	{
		iTween.MoveTo(AttackRole[0], iTween.Hash("position", RolePosition[2], "time", 1f, "islocal", true, "EaseType", "linear", "oncomplete", "Back1", "oncompletetarget", gameObject));
	}

	//B闪避
	public void Miss2()
	{
		iTween.MoveTo(AttackRole[1], iTween.Hash("position", RolePosition[3], "time", 1f, "islocal", true, "EaseType", "easeOutBack", "oncomplete", "Back2", "oncompletetarget", gameObject));
	}

	//A暴击
	public void Critic1(int role)
	{
		iTween.ScaleTo(AttackRole[0], iTween.Hash("scale", new Vector3(1.2f, 1.2f, 1.2f), "time", 1f, "islocal", true, "EaseType", "linear", "oncomplete", "Atk1", "oncompletetarget", gameObject, "oncompleteparams", role));
	}

	//B暴击
	public void Critic2(int role)
	{
		iTween.ScaleTo(AttackRole[1], iTween.Hash("scale", new Vector3(1.2f, 1.2f, 1.2f), "time", 1f, "islocal", true, "EaseType", "linear", "oncomplete", "Atk2", "oncompletetarget", gameObject, "oncompleteparams", role));
	}

	//A复位
	public void Back1()
	{
		iTween.ScaleTo(AttackRole[0], iTween.Hash("scale", new Vector3(1f, 1f, 1f), "time", 1f, "islocal", true, "EaseType", "linear"));
		iTween.MoveTo(AttackRole[0], iTween.Hash("position", RolePosition[0], "time", 1f, "islocal", true, "EaseType", "linear"));
		Vanish();
	}

	//B复位
	public void Back2()
	{
		iTween.ScaleTo(AttackRole[1], iTween.Hash("scale", new Vector3(1f, 1f, 1f), "time", 1f, "islocal", true, "EaseType", "linear"));
		iTween.MoveTo(AttackRole[1], iTween.Hash("position", RolePosition[1], "time", 1f, "islocal", true, "EaseType", "linear"));
		Vanish();
	}

	//法球消失
	public void Vanish()
	{
		if (flagdestroy == true)
		{
			Destroy(AttackRole[2]);
			flagdestroy = false;
		}
	}

	//state:0	1	2
	//		命中 暴击 未命中
	public float HandleAimation(Vector3Int request)
	{
		int turn = request.x;
		int tool = request.y;
		int state = request.z;
		if (turn == 0 && state == 0)
		{
			Atk1(tool);
			return 2f;
		}
		else if (turn == 0 && state == 1)
		{
			Critic1(tool);
			return 3f;
		}
		else if (turn == 0 && state == 2)
		{
			Atk1(tool);
			Miss2();
			return 2f;
		}
		else if (turn == 1 && state == 0)
		{
			Atk2(tool);
			return 2f;
		}
		else if (turn == 1 && state == 1)
		{
			Critic2(tool);
			return 3f;
		}
		else if (turn == 1 && state == 2)
		{
			Atk2(tool);
			Miss1();
			return 2f;
		}
		return 3f;
	}

	public void AddAnimation(int turn, int tool, int state)
	{
		AnimationQueue.Enqueue(new Vector3Int(turn, tool, state));
	}

	public void ActAnimation()
	{
		flaganimation = true;
		Debug.Log("Actttttt!" + AnimationQueue.Count.ToString());
	}

    // Start is called before the first frame update
    void Start()
    {
        flaganimation = false;
        flagfinal = false;
        AnimationQueue.Clear();
        totltime = 0;
        attackBackground[0] = background0;
        attackBackground[1] = background1;
    }

    // Update is called once per frame
    void Update()
    {
        if (AnimationQueue.Count == 0 && flaganimation == true && timeCounter >= totltime && flagfinal == false)
        {
        	flaganimation = false;
        	Hide();
        	if (GameObject.Find("Main Camera").GetComponent<GameplayEvent>().status == 13)
        	{
        		GameObject.Find("Main Camera").GetComponent<GameplayEvent>().status = 0;
        	}
        	else
        	{
        		GameObject.Find("Main Camera").GetComponent<GameplayEvent>().status = 11;
        	}
        }
        if (flaganimation == true)
        {
        	timeCounter++;
        }
        if (flaganimation == true && timeCounter >= totltime && AnimationQueue.Count > 0)
        {
        	totltime = (int)(HandleAimation(AnimationQueue.Dequeue()) * Configs.settings[3]);
        	Debug.Log("totltime=" + totltime.ToString());
        	if (AnimationQueue.Count == 0)
        	{
        		flagfinal = true;
        	}
        	timeCounter = 0;
        }
        if (timeCounter >= totltime)
        {
        	flagfinal = false;
        }
    }
}
