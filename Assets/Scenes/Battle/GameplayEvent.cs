using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;

using static ReactLayoutBattle;

public static class StateGameplayEvent //无法get set，用完手动置false
{
	public static bool[] common = new bool[8];
	/*
	 * 0 棋盘onclick
	 *
	 */
	public static bool[,] weapon = new bool[8, 3];
	/*
	 * 1 武器1物理攻击onclick
	 * 2 武器2魔法攻击onclick
	 * 3 武器3？？？
	 */

	public static bool[,] tool = new bool[8, 3];
	/*
	 * 1 道具1
	 * 2 道具2
	 * 3 道具3
	 */
	public static bool[,] skill = new bool[8, 3];
	/*？？？*/
	public static bool[] roundMenu = new bool[5];//环形菜单 0:Attack 1:Defend 2:Wait

}

public class Character
{
	//人物编号
	public int Num;
	//图片，是否死亡，是否行动完毕，防御状态，等级，经验，最大经验值，最大生命，生命，攻击，防御，最大科技，科技，敏捷，移动，幸运，负重，载具
	private int Role, Die, Over, Pro, Lv, Mexp, Exp, Mgpa, Gpa, Atk, Def, Mmag, Mag, Tec, Agi, Mov, Luk, Wgt, Rid;
	private int X, Z;
	public Item[] bag;
	public Weapon[] arsenal;
	public Weapon weapon;
	public int num
	{
		get { return Num; }
		set { Num = value; }
	}
	public int role
	{
		get { return Role; }
		set { Role = value; }
	}
	//0->没死
	//1->死了
	public int die
	{
		get { return Die; }
		set { Die = value; }
	}
	public int pro
	{
		get { return Pro; }
		set { Pro = value; }
	}
	public int over
	{
		get { return Over; }
		set { Over = value; }
	}
	public int lv
	{
		get { return Lv; }
		set { Lv = value; }
	}
	public int mexp
	{
		get { return Mexp; }
		set { Mexp = value; }
	}
	public int exp
	{
		get { return Exp; }
		set { Exp = value; }
	}
	public int mgpa
	{
		get { return Mgpa; }
		set { Mgpa = value; }
	}
	public int gpa
	{
		get { return Gpa; }
		set { Gpa = value; }
	}
	public int atk
	{
		get { return Atk; }
		set { Atk = value; }
	}
	public int def
	{
		get { return Def; }
		set { Def = value; }
	}
	public int mmag
	{
		get { return Mmag; }
		set { Mmag = value; }
	}
	public int mag
	{
		get { return Mag; }
		set { Mag = value; }
	}
	public int tec
	{
		get { return Tec; }
		set { Tec = value; }
	}
	public int agi
	{
		get { return Agi; }
		set { Agi = value; }
	}
	public int mov
	{
		get { return Mov; }
		set { Mov = value; }
	}
	public int luk
	{
		get { return Luk; }
		set { Luk = value; }
	}
	public int wgt
	{
		get { return Wgt; }
		set { Wgt = value; }
	}
	//0->步行
	//1->飞行
	//2->骑自行车
	public int rid
	{
		get { return Rid; }
		set { Rid = value; }
	}
	public int x
	{
		get { return X; }
		set { X = value; }
	}
	public int z
	{
		get { return Z; }
		set { Z = value; }
	}
	public int trueAgi
	{
		get { return Math.Max(0, agi + Math.Min(0, wgt - sumWgt())); }
	}
	private int sumWgt()
	{
		return bag[0].wgt + bag[1].wgt + bag[2].wgt + arsenal[0].wgt + arsenal[1].wgt + arsenal[2].wgt;
	}
	public void Levelup()
	{
		int sum = mgpa + atk + def + mmag + tec + agi + luk;
		int p;
		System.Random rd = new System.Random();
		//7项能力值，每升一级期望增加4点能力值
		p = 400 * mgpa / sum;
		if (lv > 10)
		{
			p = p * 3 / 2;
		}
		//最大生命值不能超过33
		while (mgpa < 33 && rd.Next(100) < p && p > 0)
		{
			mgpa += 1;
			p -= 100;
			Debug.Log("mgpa++");
		}
		p = 400 * atk / sum;
		while (rd.Next(100) < p && p > 0)
		{
			atk += 1;
			p -= 100;
			Debug.Log("atk++");
		}
		p = 400 * def / sum;
		while (rd.Next(100) < p && p > 0)
		{
			def += 1;
			p -= 100;
			Debug.Log("def++");
		}
		p = 400 * mgpa / sum;
		while (rd.Next(100) < p && p > 0)
		{
			mmag += 1;
			p -= 100;
			Debug.Log("mmag++");
		}
		p = 400 * mgpa / sum;
		while (rd.Next(100) < p && p > 0)
		{
			tec += 1;
			p -= 100;
			Debug.Log("tec++");
		}
		p = 400 * mgpa / sum;
		while (rd.Next(100) < p && p > 0)
		{
			agi += 1;
			p -= 100;
			Debug.Log("agi++");
		}
		p = 400 * mgpa / sum;
		while (rd.Next(100) < p && p > 0)
		{
			luk += 1;
			p -= 100;
			Debug.Log("luk++");
		}
	}
	//获取坐标
	public Vector3Int GetPosition()
	{
		return new Vector3Int(x, 0, z);
	}
	//道具的可用状态
	//三个值分别为：是否可用，是否可卸下，是否可丢弃
	public bool[] ItemAvailable(int index)
	{
		bool[] avail = new bool[3];
		if (bag[index].on == true)
		{
			avail[0] = false;
			avail[1] = true;
			avail[2] = false;
			return avail;
		}
		if (bag[index].wearable == true)
		{
			avail[0] = true;
			avail[1] = false;
			avail[2] = true;
			return avail;
		}
		if (bag[index].gpa != 0 && gpa == mgpa)
		{
			avail[0] = false;
			avail[1] = false;
			avail[2] = true;
			return avail;
		}
		if (bag[index].mag != 0 && bag[index].mmag == 0 && mag == mmag)
		{
			avail[0] = false;
			avail[1] = false;
			avail[2] = true;
			return avail;
		}
		else
		{
			avail[0] = true;
			avail[1] = false;
			avail[2] = false;
			return avail;
		}
	}
	//Gpa, Mmag, Mag, Atk, Def, Agi
	//Character A
	//A.UseItem(1);
	//使用道具，并将该道具换为装备状态或将其置空
	public void UseItem(int index)
	{
		Debug.Log("Item used");
		if (bag[index].wearable == false)
		{
			gpa = Math.Min(gpa + bag[index].gpa, mgpa);
			mmag += bag[index].mmag;
			mag += bag[index].mag;
			atk += bag[index].atk;
			def += bag[index].def;
			agi += bag[index].agi;
			bag[index] = new Item();
		}
		else if (bag[index].on == false)
		{
			mmag += bag[index].mmag;
			atk += bag[index].atk;
			def += bag[index].def;
			agi += bag[index].agi;
			bag[index].on = true;
		}
		else
		{
			mmag -= bag[index].mmag;
			mag = Math.Max(mag, mmag);
			atk -= bag[index].atk;
			def -= bag[index].def;
			agi -= bag[index].agi;
			bag[index].on = false;
		}

	}
	//丢弃道具，置空
	public void DiscardItem(int index)
	{
		bag[index] = new Item();
	}
	//交换武器，交换后应当注意刷新攻击范围
	public void ChangeWeapon(int index)
	{
		//始终保证arsenal[0]是现在正在装备的武器
		weapon = arsenal[index];
		Weapon temp = arsenal[0];
		arsenal[0] = arsenal[index];
		arsenal[index] = temp;
	}
}

public class Weapon
{
	public int Atk;
	//物理攻击/魔法攻击(isMag)
	private bool IsMag;
	//攻击力(atk),攻击范围(rng),重量(wgt),命中率(hr),暴击率(cr)
	private int Rng, Wgt, Hr, Cr;
	//武器描述和图像
	public string des = "WeaponDescription", img = "0";

	public Weapon()
	{
		IsMag = false;
		Atk = 0;
		Rng = 1;
		Wgt = 0;
		hr = 0;
		Cr = 0;

	}
	public int atk
	{
		get { return Atk; }
		set { Atk = value; }
	}
	public int rng
	{
		get { return Rng; }
		set { Rng = value; }
	}
	public int wgt
	{
		get { return Wgt; }
		set { Wgt = value; }
	}
	public int hr
	{
		get { return Hr; }
		set { Hr = value; }
	}
	public int cr
	{
		get { return Cr; }
		set { Cr = value; }
	}
	public bool isMag
	{
		get { return IsMag; }
		set { IsMag = value; }
	}
}

public class Item
{
	public int Gpa;
	//该道具对各项属性的加成
	private int Mmag, Mag, Atk, Def, Agi;
	//该道具重量
	private int Wgt;
	//该道具是否可取消使用
	private bool Wearable, On;
	//道具描述和图片
	public string des = "ItemDescription", img = "0";
	public Item()
	{
		Gpa = 0;
		Mmag = 0;
		Mag = 0;
		Atk = 0;
		Def = 0;
		Agi = 0;
		Wgt = 0;
		Wearable = false;
		On = false;
		
	}
	public int gpa
	{
		get { return Gpa; }
		set { Gpa = value; }
	}
	public int mmag
	{
		get { return Mmag; }
		set { Mmag = value; }
	}
	public int mag
	{
		get { return Mag; }
		set { Mag = value; }
	}
	public int atk
	{
		get { return Atk; }
		set { Atk = value; }
	}
	public int def
	{
		get { return Def; }
		set { Def = value; }
	}
	public int agi
	{
		get { return Agi; }
		set { Agi = value; }
	}
	public int wgt
	{
		get { return Wgt; }
		set { Wgt = value; }
	}
	public bool wearable
	{
		get { return Wearable; }
		set { Wearable = value; }
	}
	public bool on
	{
		get { return On; }
		set { On = value; }
	}
}

unsafe public class GameplayEvent : MonoBehaviour
{
	//定位，显示人物与环形菜单
	public Tilemap grid;
	public GameObject prefabCharacter;
	public GameObject cvMenuEncirclChess;

	//显示坐标相关
	public GameObject prefabText;
	public GameObject canvas;

	//显示范围相关
	public GameObject prefabAvailable;
	public GameObject prefabAttack;
	public GameObject prefabFriend;

	//棋盘基本参数
	public int length;
	public int width;
	public int offset;
	public int maxDis;

	//总人数、总道具数、总关卡数
	public int numPeople;
	public int numItem;
	public int numLevel;
	public int numWeapon;
	public int uLevel;

	//有限状态自动机
	public int status;

	//回合开始提示
	public GameObject playerphase;
	public GameObject enemyphase;

	//玩家
	public Character[] CharacterList;
	GameObject[] CharacterSprites;

	//敌方
	public Character[] EnemyList;
	GameObject[] EnemySprites;

	//敌我初位置
	Vector3Int[,] CharacterPosition;
	Vector3Int[,] EnemyPosition;

	//道具表
	Item[] ItemList;
	Weapon[] WeaponList;

	//地形，距离，该位置的棋子编号
	public int[,] MapCells;
	int[,] Distance;
	public int[,] Owner;

	//正在显示的移动范围和攻击范围
	List<GameObject> MoveRange;
	List<GameObject> AttackRange;

	Vector3Int selectedTile;

	//这几个值用上面的表算出来，不用存
	int enemymoved;
	int totlenemy;
	int totlplayer;
	Character globalEnemy;
	Character globalCharacter;

	//计分版
	public int score;
	public int round;
	public GameObject gameover;
	public GameObject obstruct;

	//以下变量用于角色的移动和环形菜单
	int startTime;
	int timeCount;
	float stepTime;//from settings undone
	int totlMoveTime;
	public Vector3Int globalDestination;
	public Vector3Int globalOldPosition;
	bool flagCvMenuEncirclChess;
	bool flagattack;

	//攻击敌人时的选择
	int atkselected;
	int atktemp;

	//获取点击格子转换后的坐标
	Vector3Int ClickToCoord()
	{
		Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		Vector3Int cellPosition = grid.WorldToCell(worldPosition);
		cellPosition.z = 0;
		return TransCoord(cellPosition);
	}

	//变换坐标后，六边形的邻接表，只使用了x和z
	List<Vector3Int> Directions = new List<Vector3Int>
	{
		new Vector3Int(-1,0,1),
		new Vector3Int(1,0,-1),
		new Vector3Int(-1,0,0),
		new Vector3Int(0,0,-1),
		new Vector3Int(0,0,1),
		new Vector3Int(1,0,0),
	};
	//当攻击距离为2时的邻接表
	List<Vector3Int> AttackDirections = new List<Vector3Int>
	{
		new Vector3Int(-2,0,0),
		new Vector3Int(-1,0,-1),
		new Vector3Int(0,0,-2),
		new Vector3Int(-2,0,1),
		new Vector3Int(1,0,-2),
		new Vector3Int(-2,0,2),
		new Vector3Int(2,0,-2),
		new Vector3Int(-1,0,2),
		new Vector3Int(2,0,-1),
		new Vector3Int(0,0,2),
		new Vector3Int(1,0,1),
		new Vector3Int(2,0,0),
	};

	//把地形存在MapCells中
	//地形		步行	飞行	骑行	效果		
	//0->平地	1		1		1		
	//1->小树林	1.5		1		2		闪避+20%
	//2->废墟	2		1		2.5		闪避+10%
	//3->补给点	1.5		1		2		闪避+25% 每回合开始时恢复20%Mgpa
	//4->水		5		1		inf		闪避-20%
	//5->墙		inf		1		inf
	//6->室内墙	inf		inf		inf
	//7->火		1.5		1		2		闪避-15% 每回合开始时损失20%Mgpa
	//8->丘陵	1.5		1		1
	//飞行单位不受地形buff/debuff
	//inf不可通过(视为100，乘2等于200)

	//不用double，把所有移动力乘2得到整数
	int[,] movecost = new int[9, 3]
	{
		{2,2,2},
		{3,2,4},
		{4,2,5},
		{3,2,4},
		{10,2,200},
		{200,2,200},
		{200,200,200},
		{3,2,4},
		{3,2,2}
	};

	//地形对命中率的修正
	float[] CellHitRate = new float[9]
	{
		0f, 0.2f, 0.1f, 0.25f, -0.2f, 0f, 0f, -0.15f, 0f
	};

	//地形加成对估值的影响
	int[] MapCellValue = new int[] { 0, 8, 5, 20, -8, 0, 0, -20, 0 };

	//返回移动至该格花费
	//rid:	0->步行		1->飞行		2->骑自行车
	//turn: 0->玩家移动 1->敌方移动
	int MoveCost(int x, int z, int rid, int turn)
	{
		if (Owner[x, z] < 0 && turn == 0)   //有人在该位置，返回inf
		{
			return 200;
		}
		else if (Owner[x, z] > 0 && turn == 1)
		{
			return 200;
		}
		return movecost[MapCells[x, z], rid];
	}

	//变量数组链表初始化
	void ListsInitial()
	{
		score = 0;
		round = 1;

		//刚开始先放音乐
		status = -2;

		length = 100;
		width = 100;
		offset = (length + width) / 4;
		maxDis = 50;

		numPeople = 20;
		numItem = 20;
		numLevel = 2;
		numWeapon = 10;

		totlMoveTime = 60;

		CharacterList = new Character[numPeople];
		CharacterSprites = new GameObject[numPeople];

		EnemyList = new Character[numPeople];
		EnemySprites = new GameObject[numPeople];

		CharacterPosition = new Vector3Int[numLevel, numPeople];
		EnemyPosition = new Vector3Int[numLevel, numPeople];

		ItemList = new Item[numItem];
		WeaponList = new Weapon[numWeapon];

		MapCells = new int[length, width];
		Distance = new int[length, width];
		Owner = new int[length, width];

		//显示范围
		MoveRange = new List<GameObject>();
		AttackRange = new List<GameObject>();

		StateLayoutBattle.offset = offset;

		//初始化人物表
		for (int i = 0; i < numPeople; i++)
		{
			CharacterList[i] = new Character();
			EnemyList[i] = new Character();
		}

		ItemInitial();
		WeaponInitial();

		//人物初始位置初始化
		{
			totlplayer = 3;
			CharacterPosition[0, 1] = new Vector3Int(57, 0, 35);
			CharacterPosition[0, 2] = new Vector3Int(57, 0, 37);
			CharacterPosition[0, 3] = new Vector3Int(59, 0, 35);

			totlenemy = 5;
			EnemyPosition[0, 1] = new Vector3Int(62, 0, 32);
			EnemyPosition[0, 2] = new Vector3Int(51, 0, 44);
			EnemyPosition[0, 3] = new Vector3Int(52, 0, 40);
			EnemyPosition[0, 4] = new Vector3Int(62, 0, 30);
			EnemyPosition[0, 5] = new Vector3Int(50, 0, 50);
		}

		//人物信息初始化
		{
			Character hero;
			hero = new Character()
			{
				num = 0,
				bag = new Item[3] { ItemList[0], ItemList[0], ItemList[0] },
				arsenal = new Weapon[3] { WeaponList[0], WeaponList[0], WeaponList[0] },
				weapon = WeaponList[0],
			};
			CharacterList[0] = hero;
			//李天梭，刺客
			hero = new Character()
			{
				num = 1,
				role = 1,
				die = 0,
				lv = 1,
				exp = 0,
				mexp = 100,
				mgpa = 16,
				gpa = 16,
				atk = 7,
				def = 3,
				mmag = 6,
				mag = 6,
				tec = 10,
				agi = 9,
				mov = 6,
				luk = 8,
				wgt = 8,
				rid = 2,
				x = CharacterPosition[uLevel, 1].x,
				z = CharacterPosition[uLevel, 1].z,
				bag = new Item[3] { ItemList[1], ItemList[3], ItemList[0] },
				arsenal = new Weapon[3] { WeaponList[2], WeaponList[8], WeaponList[0] },
				weapon = WeaponList[2],
			};
			CharacterList[1] = hero;
			//陈伟坤，坦克
			hero = new Character()
			{
				num = 2,
				role = 2,
				die = 0,
				lv = 1,
				exp = 0,
				mexp = 100,
				mgpa = 33,
				gpa = 33,
				atk = 5,
				def = 5,
				mmag = 4,
				mag = 4,
				tec = 6,
				agi = 2,
				mov = 4,
				luk = 7,
				wgt = 10,
				rid = 0,
				x = CharacterPosition[uLevel, 2].x,
				z = CharacterPosition[uLevel, 2].z,
				bag = new Item[3] { ItemList[2], ItemList[5], ItemList[0] },
				arsenal = new Weapon[3] { WeaponList[1], WeaponList[7], WeaponList[0] },
				weapon = WeaponList[1],
			};
			CharacterList[2] = hero;
			//灵梦，法师
			hero = new Character()
			{
				num = 3,
				role = 3,
				die = 0,
				lv = 2,
				exp = 80,
				mexp = 100,
				mgpa = 13,
				gpa = 13,
				atk = 4,
				def = 2,
				mmag = 12,
				mag = 12,
				tec = 5,
				agi = 9,
				mov = 7,
				luk = 10,
				wgt = 6,
				rid = 1,
				x = CharacterPosition[uLevel, 3].x,
				z = CharacterPosition[uLevel, 3].z,
				bag = new Item[3] { ItemList[3], ItemList[0], ItemList[0] },
				arsenal = new Weapon[3] { WeaponList[4], WeaponList[3], WeaponList[0] },
				weapon = WeaponList[4],
			};
			CharacterList[3] = hero;

			hero = new Character()
			{
				num = 0,
				bag = new Item[3] { ItemList[0], ItemList[0], ItemList[0] },
				arsenal = new Weapon[3] { WeaponList[0], WeaponList[0], WeaponList[0] },
				weapon = WeaponList[0],
			};
			EnemyList[0] = hero;
			//小鬼
			hero = new Character()
			{
				num = 1,
				role = 4,
				die = 0,
				lv = 1,
				exp = 0,
				mexp = 100,
				mgpa = 20,
				gpa = 20,
				atk = 7,
				def = 5,
				mmag = 3,
				mag = 3,
				tec = 2,
				agi = 7,
				mov = 5,
				luk = 7,
				wgt = 7,
				rid = 0,
				x = EnemyPosition[uLevel, 1].x,
				z = EnemyPosition[uLevel, 1].z,
				bag = new Item[3] { ItemList[0], ItemList[0], ItemList[0] },
				arsenal = new Weapon[3] { WeaponList[1], WeaponList[0], WeaponList[0] },
				weapon = WeaponList[1],
			};
			EnemyList[1] = hero;
			//飞行弓箭小鬼
			hero = new Character()
			{
				num = 2,
				role = 5,
				die = 0,
				lv = 1,
				exp = 0,
				mexp = 100,
				mgpa = 14,
				gpa = 14,
				atk = 4,
				def = 2,
				mmag = 5,
				mag = 5,
				tec = 9,
				agi = 8,
				mov = 6,
				luk = 9,
				wgt = 6,
				rid = 1,
				x = EnemyPosition[uLevel, 2].x,
				z = EnemyPosition[uLevel, 2].z,
				bag = new Item[3] { ItemList[0], ItemList[0], ItemList[0] },
				arsenal = new Weapon[3] { WeaponList[5], WeaponList[0], WeaponList[0] },
				weapon = WeaponList[5],
			};
			EnemyList[2] = hero;
			//重甲小鬼
			hero = new Character()
			{
				num = 3,
				role = 6,
				die = 0,
				lv = 3,
				exp = 0,
				mexp = 100,
				mgpa = 27,
				gpa = 27,
				atk = 6,
				def = 6,
				mmag = 1,
				mag = 1,
				tec = 4,
				agi = 4,
				mov = 4,
				luk = 7,
				wgt = 15,
				rid = 0,
				x = EnemyPosition[uLevel, 3].x,
				z = EnemyPosition[uLevel, 3].z,
				bag = new Item[3] { ItemList[0], ItemList[0], ItemList[0] },
				arsenal = new Weapon[3] { WeaponList[2], WeaponList[0], WeaponList[0] },
				weapon = WeaponList[2],
			};
			EnemyList[3] = hero;
			//骑士小鬼
			hero = new Character()
			{
				num = 4,
				role = 7,
				die = 0,
				lv = 2,
				exp = 0,
				mexp = 100,
				mgpa = 19,
				gpa = 19,
				atk = 5,
				def = 3,
				mmag = 2,
				mag = 2,
				tec = 8,
				agi = 8,
				mov = 7,
				luk = 8,
				wgt = 7,
				rid = 2,
				x = EnemyPosition[uLevel, 4].x,
				z = EnemyPosition[uLevel, 4].z,
				bag = new Item[3] { ItemList[0], ItemList[0], ItemList[0] },
				arsenal = new Weapon[3] { WeaponList[1], WeaponList[0], WeaponList[0] },
				weapon = WeaponList[1],
			};
			EnemyList[4] = hero;
			//Boss小鬼
			hero = new Character()
			{
				num = 5,
				role = 8,
				die = 0,
				lv = 4,
				exp = 0,
				mexp = 100,
				mgpa = 30,
				gpa = 30,
				atk = 1,
				def = 4,
				mmag = 10,
				mag = 10,
				tec = 5,
				agi = 5,
				mov = 4,
				luk = 9,
				wgt = 10,
				rid = 0,
				x = EnemyPosition[uLevel, 5].x,
				z = EnemyPosition[uLevel, 5].z,
				bag = new Item[3] { ItemList[0], ItemList[0], ItemList[0] },
				arsenal = new Weapon[3] { WeaponList[5], WeaponList[0], WeaponList[0] },
				weapon = WeaponList[5],
			};
			EnemyList[5] = hero;
		}
	}

	//从文件读地图信息
	bool LoadMapFromFile(int num_map)
	{
		int[,] mapStds = new int[,] { { 42, 42, 17, 22 } };
		int TF = mapStds[num_map, 2], TL = mapStds[num_map, 3];
		int size = TF * TL;
		byte[] buffer = new byte[size * 4];
		int[,] tmpInt = new int[TF, TL];
		byte* pTmp;
		fixed (int* tmp = &tmpInt[0, 0])
		{
			pTmp = (byte*)tmp;
		}
		try
		{
			BinaryReader br = new BinaryReader(new FileStream("sav/map/map" + num_map.ToString(),
				FileMode.Open, FileAccess.Read, FileShare.None));
			buffer = br.ReadBytes(size * 4);
			for (int i = 0; i < TF; i++)
			{
				for (int j = 0; j < TL * 4; j++)
				{
					pTmp[i * TL * 4 + j] = buffer[i * TL * 4 + j];
				}
			}
			for (int i = 0; i < TF; i++)
			{
				for (int j = 0; j < TL; j++)
				{
					if (i % 2 == 0)
					{
						MapCells[i / 2 + j + mapStds[num_map, 0], i / 2 - j + mapStds[num_map, 1]] = tmpInt[i, j];
					}
					else
					{
						MapCells[(i - 1) / 2 + j + mapStds[num_map, 0] + 1, i / 2 - j + mapStds[num_map, 1]] = tmpInt[i, j];
					}
				}
			}
			//好习惯
			br.Close();
			return true;
		}
		//Debug
		catch (IOException e)
		{
			return false;
		}
	}


	void MapInitial()
	{
		StateLayoutBattle.offset = offset;
		StateLayoutBattle.grid = grid;
		Sprite[] roleSprites = Configs.CharacterSprites;
		//存地形
		for (int i = 0; i < length; i++)
		{
			for (int j = 0; j < width; j++)
			{
				MapCells[i, j] = 6;
			}
		}
		if (LoadMapFromFile(uLevel))//地图mapcells读取成功
		{
			//显示双方棋子
			foreach (Character uchar in CharacterList)
			{
				if (uchar.num == 0 || uchar.die == 1)
				{
					continue;
				}
				CharacterSprites[uchar.num] = Instantiate(prefabCharacter);
				CharacterSprites[uchar.num].GetComponent<SpriteRenderer>().sprite = LoadSpriteFromFile("Assets/Resources/img/walk/" + uchar.role.ToString() + ".png", 360, 360);
				Vector3Int zero = uchar.GetPosition();
				Vector3 globalZero = grid.CellToWorld(OldCoord(zero));
				Vector3 upos = CharacterSprites[uchar.num].transform.position;
				CharacterSprites[uchar.num].transform.Translate(globalZero - upos, Space.World);
				Debug.Log("uchar.num" + uchar.num.ToString());
				//注意改颜色
				if (uchar.over == 1)
				{
					CharacterSprites[uchar.num].GetComponent<SpriteRenderer>().color = new Color32(168, 168, 168, 255);
				}
				RefreshChessSlider(uchar.num);
			}
			foreach (Character uchar in EnemyList)
			{
				if (uchar.num == 0 || uchar.die == 1)
				{
					continue;
				}
				EnemySprites[uchar.num] = Instantiate(prefabCharacter);
				EnemySprites[uchar.num].GetComponent<SpriteRenderer>().sprite = LoadSpriteFromFile("Assets/Resources/img/walk/" + uchar.role.ToString() + ".png", 360, 360);
				Vector3Int zero = uchar.GetPosition();
				Vector3 globalZero = grid.CellToWorld(OldCoord(zero));
				Vector3 upos = EnemySprites[uchar.num].transform.position;
				EnemySprites[uchar.num].transform.Translate(globalZero - upos, Space.World);
				//不能在敌方回合存档，所以不用改颜色
				RefreshChessSlider(-uchar.num);
			}

			//存哪些格子上有人
			//0			没人
			//+1*num	玩家
			//-1*num	敌军
			foreach (Character uchar in CharacterList)
			{
				if (uchar.num == 0 || uchar.die == 1)
				{
					continue;
				}
				Owner[uchar.x, uchar.z] = uchar.num;
			}
			foreach (Character uchar in EnemyList)
			{
				if (uchar.num == 0 || uchar.die == 1)
				{
					continue;
				}
				Owner[uchar.x, uchar.z] = -uchar.num;
			}

		}
		else
		{
			ReactLayoutBattle.Warn("地图读取错误！请检查/sav/map/目录下的文件");
			ReactLayoutBattle.Fade(2, 0);
			Debug.Log("File error");
		}
	}

	//道具表初始化
	void ItemInitial()
	{
		//空
		ItemList[0] = new Item
		{

		};
		//数理基础
		ItemList[1] = new Item
		{
			gpa = 8,
			wgt = 2,
			des = "数理基础:\nGpa+8\n重量 1",
			img = "1"
		};
		//坚实的数理基础
		ItemList[2] = new Item
		{
			gpa = 15,
			wgt = 3,
			des = "坚实的数理基础:\nGpa+15\n重量 3",
			img = "2"
		};
		//大题典
		ItemList[3] = new Item
		{
			mag = 3,
			wgt = 3,
			des = "大题典:\n科技+3\n重量 3",
			img = "3"
		};
		//吉米多维奇
		ItemList[4] = new Item
		{
			mmag = 2,
			mag = 2,
			wgt = 4,
			des = "吉米多维奇:\n最大科技+2\n科技+2\n重量 4",
			img = "4"
		};
		//要石
		ItemList[5] = new Item
		{
			def = 3,
			atk = 1,
			agi = -3,
			wearable = true,
			wgt = 5,
			des = "要石:\n装备\n防御+3\n攻击+1\n敏捷-3\n重量 5",
			img = "5"
		};
		//月人装甲
		ItemList[6] = new Item
		{
			agi = 4,
			atk = -1,
			def = -2,
			wearable = true,
			wgt = 2,
			des = "月人装甲:\n装备\n敏捷+4\n攻击-1\n防御-2\n重量 2",
			img = "6"
		};
	}

	//武器初始化
	void WeaponInitial()
	{
		//空
		WeaponList[0] = new Weapon
		{
			isMag = false,
			atk = 0,
			rng = 0,
			wgt = 0,
			hr = 0,
			cr = 0,
		};
		//富光杯
		WeaponList[1] = new Weapon
		{
			isMag = false,
			atk = 3,
			rng = 1,
			wgt = 2,
			hr = 70,
			cr = 0,
			des = "富光杯\n攻击力 3\n近战\n命中率 70\n暴击率 0\n重量 2",
			img = "1",
		};
		//物理学圣剑
		WeaponList[2] = new Weapon
		{
			isMag = false,
			atk = 4,
			rng = 1,
			wgt = 3,
			hr = 60,
			cr = 25,
			des = "物理学圣剑\n攻击力 4\n近战\n命中率 60\n暴击率 25\n重量 3",
			img = "2",
		};
		//短弓
		WeaponList[3] = new Weapon
		{
			isMag = false,
			atk = 3,
			rng = 2,
			wgt = 2,
			hr = 55,
			cr = 5,
			des = "短弓\n攻击力 3\n远程\n命中率 55\n暴击率 5\n重量 2",
			img = "3",
		};
		//御币
		WeaponList[4] = new Weapon
		{
			isMag = true,
			atk = 2,
			rng = 2,
			wgt = 3,
			hr = 80,
			cr = 0,
			des = "御币\n科学\n攻击力 2\n命中率 80\n暴击率 0\n重量 3",
			img = "4",
		};
		//希尔文
		WeaponList[5] = new Weapon
		{
			isMag = false,
			atk = 4,
			rng = 2,
			wgt = 4,
			hr = 120,
			cr = 15,
			des = "希尔文\n攻击力 4\n远程\n命中率 120\n暴击率 15\n重量 4",
			img = "5",
		};
		//封印的冈格尼尔
		WeaponList[6] = new Weapon
		{
			isMag = true,
			atk = 3,
			rng = 2,
			wgt = 6,
			hr = 18,
			cr = 45,
			des = "封印的冈格尼尔\n科学\n攻击力 3\n命中率 18\n暴击率 45\n重量 6",
			img = "6",
		};
		//超电磁手炮
		WeaponList[7] = new Weapon
		{
			isMag = true,
			atk = 1,
			rng = 2,
			wgt = 2,
			hr = 120,
			cr = 15,
			des = "超电磁手炮\n科学\n攻击力 1\n命中率 120\n暴击率 15\n重量 2",
			img = "7",
		};
		//痛苦面具
		WeaponList[8] = new Weapon
		{
			isMag = true,
			atk = 2,
			rng = 2,
			wgt = 2,
			hr = 50,
			cr = 35,
			des = "痛苦面具\n科学\n攻击力 2\n命中率 50\n暴击率 35\n重量 2",
			img = "8",
		};
	}

	//变换坐标，使得邻接表更好看,offset防止数组负下标
	Vector3Int TransCoord(Vector3Int pos)
	{
		pos.x = pos.x - (pos.y - (pos.y & 1)) / 2;
		pos.z = -pos.x - pos.y;
		pos.y = 0;
		pos.x += offset;
		pos.z += offset;
		return pos;
	}

	//变回原坐标，仅在移动棋子的时候调用，其他时候用变换后的坐标就好
	Vector3Int OldCoord(Vector3Int pos)
	{
		pos.x -= offset;
		pos.z -= offset;
		pos.y = -pos.x - pos.z;
		pos.x = pos.x + (pos.y - (pos.y & 1)) / 2;
		pos.z = 0;
		return pos;
	}

	//寻路，以该角色为起点计算到地图各处的距离
	//不用double，把所有移动力乘2得到整数
	void Dijkstra(Vector3Int pos)
	{
		int x = pos.x;
		int z = pos.z;
		int rid, turn;
		if (Owner[x, z] > 0)
		{
			rid = CharacterList[Owner[x, z]].rid;
			turn = 0;
		}
		else if (Owner[x, z] < 0)
		{
			rid = EnemyList[-Owner[x, z]].rid;
			turn = 1;
		}
		else
		{
			//这个地方没人！
			return;
		}
		//初始化为全不通行
		for (int i = 0; i < length; i++)
		{
			for (int j = 0; j < width; j++)
			{
				Distance[i, j] = 200;
			}
		}
		Distance[x, z] = 0;

		bool[,] Visit = new bool[length, width];
		//Dijkstra算法主体
		while (!Visit[x, z])
		{
			if (Distance[x, z] > maxDis)
			{
				break;
			}
			Visit[x, z] = true;
			foreach (Vector3Int dir in Directions)
			{
				if (x + dir.x >= length || x + dir.x < 0 || z + dir.z >= width || z + dir.z < 0)
				{
					continue;
				}
				Distance[x + dir.x, z + dir.z] =
					Math.Min(Distance[x + dir.x, z + dir.z], Distance[x, z] + MoveCost(x + dir.x, z + dir.z, rid, turn));
			}
			int minDis = 200;
			for (int i = 0; i < length; i++)
			{
				for (int j = 0; j < width; j++)
				{
					if (!Visit[i, j] && Distance[i, j] < minDis)
					{
						x = i;
						z = j;
						minDis = Distance[i, j];
					}
				}
			}

		}
	}

	//求完单源最短路之后，找到起点到目标的一条路径
	//假DFS
	List<Vector3Int> Dfs(Vector3Int pos, int rid, int turn)
	{
		List<Vector3Int> Path = new List<Vector3Int>();
		Path.Insert(0, pos);
		while (Distance[pos.x, pos.z] > 0)
		{
			foreach (Vector3Int dir in Directions)
			{
				Vector3Int upos = pos + dir;
				//判断这个格子是否可走，防止移动动画不合理
				if (Distance[pos.x, pos.z] - Distance[upos.x, upos.z] == MoveCost(pos.x, pos.z, rid, turn))
				{
					Path.Insert(0, upos);
					pos = upos;
					break;
				}
			}
		}
		return Path;
	}

	//让编号为num的sprite移动到dir的位置
	//dir为转化过的坐标
	public void Move(int num, Vector3[] OldPath, int step)
	{
		totlMoveTime = Convert.ToInt32(step * stepTime * Configs.settings[3]);
		if (num > 0)
		{
			iTween.MoveTo(CharacterSprites[num], iTween.Hash("path", OldPath, "time", step * stepTime, "islocal", true, "EaseType", "linear"));
		}
		else
		{
			iTween.MoveTo(EnemySprites[-num], iTween.Hash("path", OldPath, "time", step * stepTime, "islocal", true, "EaseType", "linear"));
		}
	}


	//单次伤害计算函数
	public int Dam(Character A, Character B)
	{
		if (A.weapon.isMag)   //魔法攻击
		{
			return Math.Max((Math.Max(A.mag - B.mag * 2, 0) + A.lv) * A.weapon.atk, 0);
		}
		else    //物理攻击
		{
			return Math.Max(A.atk - B.def + A.weapon.atk, 0);
		}
	}

	//命中率计算函数
	public float Hrate(Character A, Character B)
	{
		float hr;
		if (A.weapon.isMag)   //魔法攻击	
		{
			hr = (float)A.weapon.hr / 100;
		}
		else    //物理攻击
		{
			hr = (A.weapon.hr + A.tec * 2 + A.luk - B.luk + A.trueAgi - B.trueAgi) / 100f;
		}
		//飞行单位不受影响
		if (B.rid == 1)
		{
			return Math.Max(hr, 0);
		}
		hr += CellHitRate[MapCells[B.x, B.z]];
		hr = Math.Max(hr, 0);
		return hr;
	}


	//暴击率计算函数
	public float Crate(Character A, Character B)
	{
		if (A.weapon.isMag) return (float)A.weapon.cr / 100f;
		return Math.Max((0.0f + A.weapon.cr + A.tec + A.luk - B.luk) / 100f, 0);
	}

	//估值函数，A为当前选中棋子，B为敌方棋子
	public int EnemyValue(Character A, Character B)
	{
		return (int)(((B.mgpa - B.gpa) / (float)B.mgpa) * 10 +
			 Dam(A, B) * Hrate(A, B) * (1 + Crate(A, B)) * 8 -
			 Dam(B, A) * Hrate(B, A) * (1 + Crate(B, A)) * 4);
	}

	//用于攻击的估值函数
	public int AttackValue(Character A, Character B)
	{
		return (int)(((B.mgpa - B.gpa) / (float)B.mgpa) * 8 +
			 Dam(A, B) * Hrate(A, B) * (1 + Crate(A, B)) * 5);
	}

	//敌人行动
	public void EnemyBehaviour()
	{
		foreach (var nowEnemy in EnemyList)
		{
			if (enemymoved >= totlenemy)
			{
				return;
			}
			if (nowEnemy.num == 0 || nowEnemy.die == 1 || nowEnemy.over == 1)
			{
				continue;
			}
			enemymoved++;
			EnemyList[nowEnemy.num].over = 1;
			Dijkstra(new Vector3Int(nowEnemy.x, 0, nowEnemy.z));
			List<Vector3Int> Available = new List<Vector3Int>();

			//所有可移动的位置
			for (int i = 0; i < length; i++)
			{
				for (int j = 0; j < width; j++)
				{
					if (Owner[i, j] == -nowEnemy.num || (Distance[i, j] <= nowEnemy.mov * 2 && Owner[i, j] == 0))
					{
						Available.Add(new Vector3Int(i, 0, j));
					}
				}
			}

			int maxValue = 0;
			Vector3Int Bestpos = new Vector3Int(nowEnemy.x, 0, nowEnemy.z);

			//遍历可移动的位置，寻找估值最大的可移动位置
			foreach (Vector3Int pos in Available)
			{
				int nowValue = 0;
				int Enemycount1 = 0, Enemycount2 = 0;

				nowValue += MapCellValue[MapCells[pos.x, pos.z]];

				//第一圈
				foreach (var dir in Directions)
				{
					int xx = pos.x + dir.x;
					int zz = pos.z + dir.z;
					if (xx >= length || xx < 0 || zz >= width || zz < 0)
					{
						continue;
					}
					if (Owner[xx, zz] > 0)  //该位置存在英雄角色
					{
						Enemycount1++;
						if (nowEnemy.weapon.rng == 1 || nowEnemy.weapon.isMag == true)
						{
							nowValue += EnemyValue(nowEnemy, CharacterList[Owner[xx, zz]]);
						}
					}
				}

				//第二圈
				foreach (var dir in AttackDirections)
				{
					int xx = pos.x + dir.x;
					int zz = pos.z + dir.z;
					if (xx >= length || xx < 0 || zz >= width || zz < 0)
					{
						continue;
					}
					if (Owner[xx, zz] > 0)  //该位置存在英雄角色
					{
						Enemycount2++;
						if (nowEnemy.weapon.rng == 2 || nowEnemy.weapon.isMag == true)
						{
							nowValue += EnemyValue(nowEnemy, CharacterList[Owner[xx, zz]]);
						}
					}
				}

				//不要莽，不要使劲儿往人堆里冲
				if (Enemycount1 + Enemycount2 > 1)
				{
					nowValue = Math.Max(0, (int)(nowValue * (1.0 - (2 * Enemycount1 + Enemycount2) / 6.0)));
				}
				//Debug.Log($"({pos.x}, {pos.z}): {nowValue}");

				if (nowValue == maxValue)
				{
					System.Random rnd = new System.Random();
					if (rnd.Next() % 2 == 0)
					{
						Bestpos = pos;
					}
				}
				else if (nowValue > maxValue)
				{
					maxValue = nowValue;
					Bestpos = pos;
				}

			}

			//移动棋子
			if (Bestpos.x == nowEnemy.x && Bestpos.z == nowEnemy.z)
			{
				continue;
			}

			List<Vector3Int> TransPath = Dfs(Bestpos, nowEnemy.rid, 1);
			int len = TransPath.Count;
			Vector3[] GlobalPath = new Vector3[len];
			for (int i = 0; i < len; i++)
			{
				GlobalPath[i] = grid.CellToWorld(OldCoord(TransPath[i]));
			}
			Owner[Bestpos.x, Bestpos.z] = Owner[nowEnemy.x, nowEnemy.z];
			Owner[nowEnemy.x, nowEnemy.z] = 0;
			EnemyList[-Owner[Bestpos.x, Bestpos.z]].x = Bestpos.x;
			EnemyList[-Owner[Bestpos.x, Bestpos.z]].z = Bestpos.z;
			Move(Owner[Bestpos.x, Bestpos.z], GlobalPath, len);

			//攻击
			Vector3Int nowpos = new Vector3Int(nowEnemy.x, 0, nowEnemy.z);

			int Besthero = -1;
			int BestValue = 0;

			if (nowEnemy.weapon.rng == 1 || nowEnemy.weapon.isMag == true)
			{
				foreach (var dir in Directions)
				{
					int xx = nowpos.x + dir.x;
					int zz = nowpos.z + dir.z;
					if (xx >= length || xx < 0 || zz >= width || zz < 0)
					{
						continue;
					}
					if (Owner[xx, zz] > 0)  //该位置存在英雄角色
					{
						if (AttackValue(nowEnemy, CharacterList[Owner[xx, zz]]) > BestValue)
						{
							BestValue = AttackValue(nowEnemy, CharacterList[Owner[xx, zz]]);
							Besthero = Owner[xx, zz];
						}
					}

				}
			}

			if (nowEnemy.weapon.rng == 2 || nowEnemy.weapon.isMag == true)
			{
				foreach (var dir in AttackDirections)
				{
					int xx = nowpos.x + dir.x;
					int zz = nowpos.z + dir.z;
					if (xx >= length || xx < 0 || zz >= width || zz < 0)
					{
						continue;
					}
					if (Owner[xx, zz] > 0)  //该位置存在英雄角色
					{
						if (AttackValue(nowEnemy, CharacterList[Owner[xx, zz]]) > BestValue)
						{
							BestValue = AttackValue(nowEnemy, CharacterList[Owner[xx, zz]]);
							Besthero = Owner[xx, zz];
						}
					}

				}
			}

			globalEnemy = nowEnemy;

			if (Besthero == -1) //没人可以打！
			{
				EnemySprites[nowEnemy.num].GetComponent<SpriteRenderer>().color = new Color32(168, 168, 168, 255);
				if (nowEnemy.mag < nowEnemy.mmag / 2)
				{
					nowEnemy.mag = Math.Min(nowEnemy.mag + 2, nowEnemy.mmag);
				}
				else
				{
					nowEnemy.pro = 1;
					nowEnemy.def += 2;
				}
				EnemyList[nowEnemy.num] = nowEnemy;
				return;
			}

			globalCharacter = CharacterList[Besthero];

			flagattack = true;

			return;
		}
	}
	public void EnemyAttack(Character A, Character B)
	{
		System.Random rd = new System.Random();
		int dam = 0;
		int exp = 0;

		//攻击方式，与动画有关
		int tool1 = A.weapon.isMag ? 2 : (A.weapon.rng == 2 ? 1 : 0);
		int tool2 = B.weapon.isMag ? 2 : (B.weapon.rng == 2 ? 1 : 0);

		//A先攻击B
		if (Hrate(A, B) * 100 <= rd.Next(100))
		{
			exp += 5;
			dam = 0;
			GameObject.Find("Main Camera").GetComponent<AttackAnimation>().AddAnimation(1, tool1, 2);
		}
		else
		{
			if (Crate(A, B) * 100 <= rd.Next(100))
			{
				//未暴击
				dam = Dam(A, B);
				GameObject.Find("Main Camera").GetComponent<AttackAnimation>().AddAnimation(1, tool1, 0);
			}
			else
			{
				dam = Dam(A, B) * 2;
				GameObject.Find("Main Camera").GetComponent<AttackAnimation>().AddAnimation(1, tool1, 1);
			}
			B.gpa -= dam;
			if (B.gpa <= 0)
			{
				MakeDie(B.num);
			}
		}
		if (A.weapon.isMag == true)
		{
			//使用魔法耗蓝
			A.mag = Math.Max(A.mag - A.weapon.atk * 2, 0);
		}
		//B攻击A之前先判定能不能攻击到
		bool isback = false;
		if (B.weapon.isMag)
		{
			isback = true;
		}
		else if (B.weapon.rng == 1)
		{
			foreach (Vector3Int dir in Directions)
			{
				if (A.GetPosition() == B.GetPosition() + dir)
				{
					isback = true;
					break;
				}
			}
		}
		else if (B.weapon.rng == 2)
		{
			foreach (Vector3Int dir in AttackDirections)
			{
				if (A.GetPosition() == B.GetPosition() + dir)
				{
					isback = true;
					break;
				}
			}
		}
		if (B.die == 0 && isback)
		{
			//B攻击A
			if (Hrate(B, A) * 100 <= rd.Next(100))
			{
				dam = 0;
				GameObject.Find("Main Camera").GetComponent<AttackAnimation>().AddAnimation(0, tool2, 2);
			}
			else
			{
				if (Crate(B, A) * 100 <= rd.Next(100))
				{
					//未暴击
					exp += 5;
					dam = Dam(B, A);
					GameObject.Find("Main Camera").GetComponent<AttackAnimation>().AddAnimation(0, tool2, 0);
				}
				else
				{
					exp += 10;
					dam = Dam(B, A) * 2;
					GameObject.Find("Main Camera").GetComponent<AttackAnimation>().AddAnimation(0, tool2, 1);
				}
				A.gpa -= dam;
				if (A.gpa <= 0)
				{
					exp += 50;
					MakeDie(-A.num);
					B.exp += exp;
					while (B.exp >= 100 && B.lv < 30)
					{
						B.exp -= 100;
						B.lv++;
						B.Levelup();
					}
					if (B.exp > 100)
					{
						B.exp = 100;
					}
					GameObject.Find("Main Camera").GetComponent<AttackAnimation>().Display(B.role, A.role, 1);
					GameObject.Find("Main Camera").GetComponent<AttackAnimation>().ActAnimation();
					CharacterList[B.num] = B;
					return;
				}
			}
			if (B.weapon.isMag == true)
			{
				B.mag = Math.Max(B.mag - B.weapon.atk * 2, 0);
			}
		}
		//A速度快的话再打一次
		if (B.die == 0 && A.trueAgi >= B.trueAgi * 2)
		{
			if (Hrate(A, B) * 100 <= rd.Next(100))
			{
				exp += 5;
				dam = 0;
				GameObject.Find("Main Camera").GetComponent<AttackAnimation>().AddAnimation(1, tool1, 2);
			}
			else
			{
				if (Crate(A, B) * 100 <= rd.Next(100))
				{
					//未暴击
					dam = Dam(A, B);
					GameObject.Find("Main Camera").GetComponent<AttackAnimation>().AddAnimation(1, tool1, 0);
				}
				else
				{
					dam = Dam(A, B) * 2;
					GameObject.Find("Main Camera").GetComponent<AttackAnimation>().AddAnimation(1, tool1, 1);
				}
				B.gpa -= dam;
				if (B.gpa <= 0)
				{
					MakeDie(B.num);
				}
			}
			if (A.weapon.isMag == true)
			{
				A.mag = Math.Max(A.mag - A.weapon.atk * 2, 0);
			}
		}
		GameObject.Find("Main Camera").GetComponent<AttackAnimation>().Display(B.role, A.role, 1);
		GameObject.Find("Main Camera").GetComponent<AttackAnimation>().ActAnimation();
		//攻击完成
		A.over = 1;
		CharacterList[B.num] = B;
		if (A.gpa > 0)
		{
			EnemyList[A.num] = A;
			EnemySprites[A.num].GetComponent<SpriteRenderer>().color = new Color32(168, 168, 168, 255);

		}
		if (B.gpa > 0)
		{
			B.exp += exp;
			while (B.exp >= 100 && B.lv < 30)
			{
				B.exp -= 100;
				B.lv++;
				B.Levelup();
			}
			if (B.exp > 100)
			{
				B.exp = 100;
			}
			CharacterList[B.num] = B;
		}
	}

	//玩家攻击
	public void PlayerAttack()
	{
		Character A = globalCharacter;
		Character B = globalEnemy;
		bool isback = false;
		System.Random rd = new System.Random();
		int dam = 0;
		int exp = 5;
		int tool1 = A.weapon.isMag ? 2 : (A.weapon.rng == 2 ? 1 : 0);
		int tool2 = B.weapon.isMag ? 2 : (B.weapon.rng == 2 ? 1 : 0);
		if (Hrate(A, B) * 100 <= rd.Next(100))
		{
			GameObject.Find("Main Camera").GetComponent<AttackAnimation>().AddAnimation(0, tool1, 2);
			dam = 0;
		}
		else
		{
			if (Crate(A, B) * 100 <= rd.Next(100))
			{
				//未暴击
				exp += 5;
				dam = Dam(A, B);
				GameObject.Find("Main Camera").GetComponent<AttackAnimation>().AddAnimation(0, tool1, 0);
			}
			else
			{
				exp += 10;
				dam = Dam(A, B) * 2;
				GameObject.Find("Main Camera").GetComponent<AttackAnimation>().AddAnimation(0, tool1, 1);
			}
			B.gpa -= dam;
			if (B.gpa <= 0)
			{
				exp += 50;
				MakeDie(-B.num);
			}
		}
		if (A.weapon.isMag == true)
		{
			//使用魔法耗蓝
			A.mag = Math.Max(A.mag - A.weapon.atk * 2, 0);
		}
		//B攻击A之前先判定能不能攻击到
		if (B.weapon.isMag)
		{
			isback = true;
		}
		else if (B.weapon.rng == 1)
		{
			foreach (Vector3Int dir in Directions)
			{
				if (A.GetPosition() == B.GetPosition() + dir)
				{
					isback = true;
					break;
				}
			}
		}
		else if (B.weapon.rng == 2)
		{
			foreach (Vector3Int dir in AttackDirections)
			{
				if (A.GetPosition() == B.GetPosition() + dir)
				{
					isback = true;
					break;
				}
			}
		}
		if (B.die == 0 && isback)
		{
			//B攻击A
			if (Hrate(B, A) * 100 <= rd.Next(100))
			{
				exp += 5;
				dam = 0;
				GameObject.Find("Main Camera").GetComponent<AttackAnimation>().AddAnimation(1, tool2, 2);
			}
			else
			{
				if (Crate(B, A) * 100 <= rd.Next(100))
				{
					//未暴击
					dam = Dam(B, A);
					GameObject.Find("Main Camera").GetComponent<AttackAnimation>().AddAnimation(1, tool2, 0);
				}
				else
				{
					dam = Dam(B, A) * 2;
					GameObject.Find("Main Camera").GetComponent<AttackAnimation>().AddAnimation(1, tool2, 1);
				}
				A.gpa -= dam;
				if (A.gpa <= 0)
				{
					MakeDie(A.num);
					atkselected = 0;
					status = 13;
					GameObject.Find("Main Camera").GetComponent<AttackAnimation>().Display(A.role, B.role, 0);
					GameObject.Find("Main Camera").GetComponent<AttackAnimation>().ActAnimation();
					return;
				}
			}
			if (B.weapon.isMag == true)
			{
				B.mag = Math.Max(B.mag - B.weapon.atk * 2, 0);
			}
		}
		//A速度快的话再打一次
		if (B.die == 0 && A.trueAgi >= B.trueAgi * 2)
		{
			if (Hrate(A, B) * 100 <= rd.Next(100))
			{
				dam = 0;
				GameObject.Find("Main Camera").GetComponent<AttackAnimation>().AddAnimation(0, tool1, 2);
			}
			else
			{
				if (Crate(A, B) * 100 <= rd.Next(100))
				{
					//未暴击
					exp += 5;
					dam = Dam(A, B);
					GameObject.Find("Main Camera").GetComponent<AttackAnimation>().AddAnimation(0, tool1, 0);
				}
				else
				{
					exp += 10;
					dam = Dam(A, B) * 2;
					GameObject.Find("Main Camera").GetComponent<AttackAnimation>().AddAnimation(0, tool1, 1);
				}
				B.gpa -= dam;
				if (B.gpa <= 0)
				{
					exp += 50;
					MakeDie(-B.num);
				}
			}
		}
		if (A.weapon.isMag == true)
		{
			A.mag = Math.Max(A.mag - A.weapon.atk * 2, 0);
		}
		status = 13;
		GameObject.Find("Main Camera").GetComponent<AttackAnimation>().Display(A.role, B.role, 0);
		GameObject.Find("Main Camera").GetComponent<AttackAnimation>().ActAnimation();
		//攻击完成
		A.exp += exp;
		while (A.exp >= 100 && A.lv < 30)
		{
			A.exp -= 100;
			A.lv++;
			A.Levelup();
		}
		if (A.exp > 100)
		{
			A.exp = 100;
		}
		A.over = 1;
		if (A.die == 0)
		{
			CharacterSprites[A.num].GetComponent<SpriteRenderer>().color = new Color32(168, 168, 168, 255);
			ReactCharaInfo.Close();
		}
		atkselected = 0;
		if (A.gpa > 0)
		{
			//如果死了，那就得不到经验，也无法升级
			CharacterList[A.num] = A;
			RefreshChessSlider(A.num);
			ReactCharaInfo.Refresh(A.num, MapCells[A.x, A.z]);
		}
		if (B.gpa > 0)
		{
			EnemyList[B.num] = B;
			RefreshChessSlider(-B.num);
		}
	}

	//使死亡
	public void MakeDie(int num)
	{
		if (num > 0)
		{
			CharacterList[num].die = 1;
			Destroy(CharacterSprites[num]);
			Owner[CharacterList[num].x, CharacterList[num].z] = 0;
			totlplayer--;
			if (totlplayer == 0)
            {
				GameOver();
            }
			return;
		}
		else
		{
			EnemyList[-num].die = 1;
			Destroy(EnemySprites[-num]);
			Owner[EnemyList[-num].x, EnemyList[-num].z] = 0;
			totlenemy--;
			score++;
			System.Random rd = new System.Random();
			int uid = rd.Next(3);
			if (CharacterList[uid].die == 1)
			{
				return;
			}
			//奖励一个随机道具
			for (int i = 0; i < 3; i++)
			{
				if (CharacterList[uid].bag[i].wgt == 0)
				{
					CharacterList[uid].bag[i] = ItemList[rd.Next(6) + 1];
					return;
				}
			}
			ReactCharaInfo.RefreshRound();
			return;
		}
	}

	//连战模式，用于在敌人回合结束时刷新敌人列表
	public void Revive()
	{
		foreach (Character uchar in EnemyList)
		{
			if (uchar.num == 0 || uchar.die == 0)
			{
				continue;
			}
			//复活该角色并让其升级
			if (uchar.lv < 30)
			{
				uchar.Levelup();
				uchar.lv++;
			}
			uchar.die = 0;
			uchar.gpa = uchar.mgpa;
			uchar.mag = uchar.mmag;
			System.Random rd = new System.Random();
			uchar.x = rd.Next(width);
			uchar.z = rd.Next(length);
			while (MapCells[uchar.x, uchar.z] > 3 || Owner[uchar.x, uchar.z] != 0)
			{
				uchar.x = rd.Next(width);
				uchar.z = rd.Next(length);
			}
			Owner[uchar.x, uchar.z] = -uchar.num;
			EnemySprites[uchar.num] = Instantiate(prefabCharacter);
			Vector3Int zero = uchar.GetPosition();
			Vector3 globalZero = grid.CellToWorld(OldCoord(zero));
			Vector3 upos = EnemySprites[uchar.num].transform.position;
			EnemySprites[uchar.num].transform.Translate(globalZero - upos, Space.World);
			EnemySprites[uchar.num].GetComponent<SpriteRenderer>().sprite = LoadSpriteFromFile("Assets/Resources/img/walk/" + uchar.role.ToString() + ".png", 360, 360);
			RefreshChessSlider(-uchar.num);
			EnemyList[uchar.num] = uchar;
			totlenemy++;
		}
	}

	public void RefreshChessSlider(int n)
	{
		GameObject[] gos;
		int N;
		Character[] cls;
		if (n > 0)
		{
			gos = CharacterSprites;
			cls = CharacterList;
			N = n;
		}
		else
		{
			gos = EnemySprites;
			cls = EnemyList;
			N = -n;
		}
		gos[N].transform.GetChild(1).transform.localScale = new Vector3(41.24f * ((cls[N].gpa + 0f) / cls[N].mgpa), 3, 1);
		gos[N].transform.GetChild(3).transform.localScale = new Vector3(41.24f * ((cls[N].mag + 0f) / cls[N].mmag), 3, 1);
	}

	public void DisplayRange(int num, int state)
	{
		if (num > 0 && state == 0)
		{
			int x = CharacterList[num].x;
			int z = CharacterList[num].z;
			Vector3Int[] Available = new Vector3Int[maxDis * maxDis];
			bool[,] Visit = new bool[length, width];
			Available[0] = new Vector3Int(x, 0, z);
			Visit[x, z] = true;
			//假队列BFS
			for (int i = 0, j = 0; i <= j; i++)
			{
				foreach (Vector3Int dir in Directions)
				{
					int xx = Available[i].x + dir.x;
					int zz = Available[i].z + dir.z;
					if (xx >= length || xx < 0 || zz >= width || zz < 0)
					{
						continue;
					}
					if (Visit[xx, zz] == false && Distance[xx, zz] <= CharacterList[num].mov * 2)
					{
						Available[++j] = new Vector3Int(xx, 0, zz);
						Visit[xx, zz] = true;
					}
				}
			}
			//显示移动范围
			foreach (Vector3Int dir in Available)
			{
				GameObject usprite;
				if (Owner[dir.x, dir.z] > 0 && (x != dir.x || z != dir.z))
				{
					usprite = Instantiate(prefabFriend);
				}
				else
				{
					usprite = Instantiate(prefabAvailable);
				}
				Vector3Int ucoord = OldCoord(new Vector3Int(dir.x, 0, dir.z));
				Vector3 uglobal = grid.CellToWorld(ucoord);
				Vector3 upos = usprite.transform.position;
				usprite.transform.Translate(uglobal - upos);
				MoveRange.Add(usprite);
			}
			//显示攻击范围
			foreach (Vector3Int place in Available)
			{
				if (CharacterList[num].weapon.rng == 1 || CharacterList[num].weapon.isMag == true)
				{
					foreach (Vector3Int dir in Directions)
					{
						int xx = place.x + dir.x;
						int zz = place.z + dir.z;
						if (xx >= length || xx < 0 || zz >= width || zz < 0)
						{
							continue;
						}
						if (Visit[xx, zz] == false && MapCells[xx, zz] != 6)
						{
							GameObject usprite = Instantiate(prefabAttack);
							Vector3Int ucoord = OldCoord(new Vector3Int(xx, 0, zz));
							Vector3 uglobal = grid.CellToWorld(ucoord);
							Vector3 upos = usprite.transform.position;
							usprite.transform.Translate(uglobal - upos);
							AttackRange.Add(usprite);
							Visit[xx, zz] = true;
						}
					}
				}
				if (CharacterList[num].weapon.rng == 2 || CharacterList[num].weapon.isMag == true)
				{
					foreach (Vector3Int dir in AttackDirections)
					{
						int xx = place.x + dir.x;
						int zz = place.z + dir.z;
						if (xx >= length || xx < 0 || zz >= width || zz < 0)
						{
							continue;
						}
						if (Visit[xx, zz] == false && MapCells[xx, zz] != 6)
						{
							GameObject usprite = Instantiate(prefabAttack);
							Vector3Int ucoord = OldCoord(new Vector3Int(xx, 0, zz));
							Vector3 uglobal = grid.CellToWorld(ucoord);
							Vector3 upos = usprite.transform.position;
							usprite.transform.Translate(uglobal - upos);
							AttackRange.Add(usprite);
							Visit[xx, zz] = true;
						}
					}
				}
			}
		}
		if (num < 0 && state == 0)
		{
			int x = EnemyList[-num].x;
			int z = EnemyList[-num].z;
			Vector3Int[] Available = new Vector3Int[maxDis * maxDis];
			bool[,] Visit = new bool[length, width];
			Available[0] = new Vector3Int(x, 0, z);
			Visit[x, z] = true;
			//假队列BFS
			for (int i = 0, j = 0; i <= j; i++)
			{
				foreach (Vector3Int dir in Directions)
				{
					int xx = Available[i].x + dir.x;
					int zz = Available[i].z + dir.z;
					if (xx >= length || xx < 0 || zz >= width || zz < 0)
					{
						continue;
					}
					if (Visit[xx, zz] == false && Distance[xx, zz] <= EnemyList[-num].mov * 2)
					{
						Available[++j] = new Vector3Int(xx, 0, zz);
						Visit[xx, zz] = true;
					}
				}
			}
			//显示移动范围
			foreach (Vector3Int dir in Available)
			{
				GameObject usprite;
				if (Owner[dir.x, dir.z] < 0 && x != dir.x && z != dir.z)
				{
					usprite = Instantiate(prefabFriend);
				}
				else
				{
					usprite = Instantiate(prefabAvailable);
				}
				Vector3Int ucoord = OldCoord(new Vector3Int(dir.x, 0, dir.z));
				Vector3 uglobal = grid.CellToWorld(ucoord);
				Vector3 upos = usprite.transform.position;
				usprite.transform.Translate(uglobal - upos);
				MoveRange.Add(usprite);
			}
			foreach (Vector3Int place in Available)
			{
				if (EnemyList[-num].weapon.rng == 1 || EnemyList[-num].weapon.isMag == true)
				{
					foreach (Vector3Int dir in Directions)
					{
						int xx = place.x + dir.x;
						int zz = place.z + dir.z;
						if (xx >= length || xx < 0 || zz >= width || zz < 0 || MapCells[xx, zz] == 6)
						{
							continue;
						}
						if (Visit[xx, zz] == false && MapCells[xx, zz] != 6)
						{
							GameObject usprite = Instantiate(prefabAttack);
							Vector3Int ucoord = OldCoord(new Vector3Int(xx, 0, zz));
							Vector3 uglobal = grid.CellToWorld(ucoord);
							Vector3 upos = usprite.transform.position;
							usprite.transform.Translate(uglobal - upos);
							AttackRange.Add(usprite);
							Visit[xx, zz] = true;
						}
					}
				}
				if (EnemyList[-num].weapon.rng == 2 || EnemyList[-num].weapon.isMag == true)
				{
					foreach (Vector3Int dir in AttackDirections)
					{
						int xx = place.x + dir.x;
						int zz = place.z + dir.z;
						if (xx >= length || xx < 0 || zz >= width || zz < 0 || MapCells[xx, zz] == 6)
						{
							continue;
						}
						if (Visit[xx, zz] == false && MapCells[xx, zz] != 6)
						{
							GameObject usprite = Instantiate(prefabAttack);
							Vector3Int ucoord = OldCoord(new Vector3Int(xx, 0, zz));
							Vector3 uglobal = grid.CellToWorld(ucoord);
							Vector3 upos = usprite.transform.position;
							usprite.transform.Translate(uglobal - upos);
							AttackRange.Add(usprite);
							Visit[xx, zz] = true;
						}
					}
				}
			}
		}
		else if (num > 0 && state == 1)
		{
			int x = CharacterList[num].x;
			int z = CharacterList[num].z;
			if (CharacterList[num].weapon.rng == 1 || CharacterList[num].weapon.isMag == true)
			{
				foreach (Vector3Int dir in Directions)
				{
					int xx = x + dir.x;
					int zz = z + dir.z;
					if (xx >= length || xx < 0 || zz >= width || zz < 0 || MapCells[xx, zz] == 6)
					{
						continue;
					}
					GameObject usprite = Instantiate(prefabAttack);
					Vector3Int ucoord = OldCoord(new Vector3Int(xx, 0, zz));
					Vector3 uglobal = grid.CellToWorld(ucoord);
					Vector3 upos = usprite.transform.position;
					usprite.transform.Translate(uglobal - upos);
					AttackRange.Add(usprite);
				}
			}
			if (CharacterList[num].weapon.rng == 2 || CharacterList[num].weapon.isMag == true)
			{
				foreach (Vector3Int dir in AttackDirections)
				{
					int xx = globalDestination.x + dir.x;
					int zz = globalDestination.z + dir.z;
					if (xx >= length || xx < 0 || zz >= width || zz < 0 || MapCells[xx, zz] == 6)
					{
						continue;
					}
					GameObject usprite = Instantiate(prefabAttack);
					Vector3Int ucoord = OldCoord(new Vector3Int(xx, 0, zz));
					Vector3 uglobal = grid.CellToWorld(ucoord);
					Vector3 upos = usprite.transform.position;
					usprite.transform.Translate(uglobal - upos);
					AttackRange.Add(usprite);
				}
			}
		}
		else if (num > 0 && state == 2)
		{
			int x = CharacterList[num].x;
			int z = CharacterList[num].z;
			foreach (Vector3Int dir in Directions)
			{
				int xx = x + dir.x;
				int zz = z + dir.z;
				if (xx >= length || xx < 0 || zz >= width || zz < 0 || MapCells[xx, zz] == 6)
				{
					continue;
				}
				GameObject usprite = Instantiate(prefabFriend);
				Vector3Int ucoord = OldCoord(new Vector3Int(xx, 0, zz));
				Vector3 uglobal = grid.CellToWorld(ucoord);
				Vector3 upos = usprite.transform.position;
				usprite.transform.Translate(uglobal - upos);
				AttackRange.Add(usprite);
			}
		}
	}

	public void HideRange()
	{
		foreach (GameObject usprite in MoveRange)
		{
			Destroy(usprite);
		}
		MoveRange.Clear();
		foreach (GameObject usprite in AttackRange)
		{
			Destroy(usprite);
		}
		AttackRange.Clear();
	}


	public int AtkSelect()
	{
		Vector3Int atkCoord = ClickToCoord();
		//先判断目标格子是否有敌人
		if (Owner[atkCoord.x, atkCoord.z] >= 0)
		{
			return 0;
		}
		//判断目标格子是否在攻击范围内
		Character uchar = CharacterList[Owner[globalDestination.x, globalDestination.z]];
		if (uchar.weapon.rng == 2 || uchar.weapon.isMag)
		{
			foreach (Vector3Int dir in AttackDirections)
			{
				int xx = uchar.x + dir.x;
				int zz = uchar.z + dir.z;
				if (xx == atkCoord.x && zz == atkCoord.z)
				{
					return Owner[xx, zz];
				}
			}
		}
		if (uchar.weapon.rng == 1 || uchar.weapon.isMag)
		{
			foreach (Vector3Int dir in Directions)
			{
				int xx = uchar.x + dir.x;
				int zz = uchar.z + dir.z;
				if (xx == atkCoord.x && zz == atkCoord.z)
				{
					return Owner[xx, zz];
				}
			}
		}
		return 0;
	}

	//显示现在是谁的回合
	public void ShowPhase(int num)
	{
		totlMoveTime = 60;//0.8 * 75
		startTime = 0;
		if (num == 0)
		{
			playerphase.gameObject.SetActive(true);
			status = 15;
		}
		else
		{
			enemyphase.gameObject.SetActive(true);
			status = 16;
		}
	}

	//呼出游戏结束面板
	public void GameOver()
	{
		gameover.gameObject.SetActive(true);
		obstruct.gameObject.SetActive(true);
		status = 18;
	}

	//保存存档
	public int[] CollectSav(int nMe, int nEne)
	{
		Debug.Log("CollectSav");
		int po = 2;
		int* p;

		int[] sav = new int[2 + (nMe + nEne + 2) * (22 + 8 * 3 + 6 * 3) + 4];//0

		Character[] CList;
		int n;
		for (int t = 0; t < 2; t++)
		{
			CList = (t == 0 ? CharacterList : EnemyList);
			n = (t == 0 ? nMe : nEne);
			for (int i = 0; i <= n; i++)
			{
				fixed (int* tmp = &CList[i].Num)
				{
					p = tmp;
				}
				for (int j = 0; j < 22; j++)
				{
					sav[po + j] = *(p + j);
					Debug.Log((po + j).ToString());
				}
				po += 22;

				for (int j = 0; j < 3; j++)
				{
					fixed (int* tmp = &CList[i].arsenal[j].Atk)
					{
						p = tmp;
					}
					for (int k = 0; k < 6; k++)
					{
						sav[po + k] = *(p + k);
					}
					po += 6;
					fixed (int* tmp = &CList[i].bag[j].Gpa)
					{
						p = tmp;
					}
					for (int k = 0; k < 8; k++)
					{
						sav[po + k] = *(p + k);
					}
					po += 8;
				}


			}

		}
		sav[0] = nMe;
		sav[1] = nEne;
		sav[po + 0] = score;
		sav[po + 1] = round;
		sav[po + 2] = uLevel;
		sav[po + 3] = ReactLayoutBattle.diaProcs;
		Debug.Log("ROUND  " + (po + 1).ToString() + "  " + round.ToString());
		return sav;
	}//取得游戏要存档的int[]

	//读取存档
	public bool IssueSav(int[] sav)
	{
		int po = 2;
		int nMe = sav[0];
		int nEne = sav[1];
		int* p;

		Character[] CList;
		int n;
		for (int t = 0; t < 2; t++)
		{
			CList = (t == 0 ? CharacterList : EnemyList);
			n = (t == 0 ? nMe : nEne);
			for (int i = 0; i <= n; i++)
			{
				fixed (int* tmp = &CList[i].Num)
				{
					p = tmp;
				}
				for (int j = 0; j < 22; j++)
				{
					*(p + j) = sav[po + j];
					Debug.Log((po + j).ToString());
				}
				po += 22;

				for (int j = 0; j < 3; j++)
				{
					fixed (int* tmp = &CList[i].arsenal[j].Atk)
					{
						p = tmp;
					}
					for (int k = 0; k < 6; k++)
					{
						*(p + k) = sav[po + k];
					}
					po += 6;
					fixed (int* tmp = &CList[i].bag[j].Gpa)
					{
						p = tmp;
					}
					for (int k = 0; k < 8; k++)
					{
						*(p + k) = sav[po + k];
					}
					po += 8;
				}
			}
		}
		score = sav[po + 0];
		round = sav[po + 1];
		Debug.Log("ROUND  " + (po + 1).ToString() + "  " + round.ToString());
		uLevel = sav[po + 2];
		ReactLayoutBattle.diaProcs = sav[po + 3];
		return true;
	}//按存档初始化

	//开始游戏初始化一下
	//开public用来读档的时候调用
	public void Start()
	{
		stepTime = Configs.settings[4] / 100f;
		if (Configs.isFromSavIn)
		{
        	foreach (Character uchar in CharacterList)
        	{
        		if (uchar.num == 0 || uchar.die == 1)
        		{
        			continue;
        		}
        		Destroy(CharacterSprites[uchar.num]);
        	}
        	foreach (Character uchar in EnemyList)
        	{
        		if (uchar.num == 0 || uchar.die == 1)
        		{
        			continue;
        		}
        		Destroy(EnemySprites[uchar.num]);
        	}
        }
		ListsInitial();
		//从存档进入不用播放剧情
        if (Configs.isFromSavIn || Configs.isFromSavOut)
        {
			status = 0;
			IssueSav(Configs.loader.LoadGameSaving(Configs.storyName));
			totlplayer = 0;
			totlenemy = 0;
			foreach (Character uchar in CharacterList)
			{
				if (uchar.num !=0 && uchar.die == 0)
				{
					totlplayer++;
				}
			}
			foreach (Character uchar in EnemyList)
			{
				if (uchar.num !=0 && uchar.die == 0)
				{
					totlenemy++;
				}
			}
			if (!GameObject.Find("Main Camera").GetComponent<Settings>().source.isPlaying)
			{
				GameObject.Find("Main Camera").GetComponent<Settings>().source.Play();
			}
        }
        else
        {
			ReactLayoutBattle.diaProcs = -1;
        }
		MapInitial();
	}

	// Update is called once per frame
	void Update()
	{
		//正在执行对话
		if (status == -2)
		{
			
		}
		//对话结束放音乐
		if (status == -1)
		{
			if (!GameObject.Find("Main Camera").GetComponent<Settings>().source.isPlaying)
			{
				GameObject.Find("Main Camera").GetComponent<Settings>().source.Play();
			}
			ShowPhase(0);
			ReactCharaInfo.Refresh(0, 0);
			ReactCharaInfo.RefreshRound();
			status = 15;
		}
		//初状态
		if ((status == 0 || status == 1 || status == 3) && StateGameplayEvent.common[0])
		{
			if (status != 0)
            {
				HideRange();
			}
			if (totlplayer == 0)
			{
				GameOver();
			}
			StateGameplayEvent.common[0] = false;
			selectedTile = ClickToCoord();
			Debug.Log(selectedTile);
			int x = selectedTile.x;
			int z = selectedTile.z;
			ReactCharaInfo.Open(Owner[x, z], MapCells[x, z]);
			if (Owner[x, z] == 0 || (Owner[x, z] > 0 && CharacterList[Owner[x, z]].over == 1))
			{
				//只显示地形
				status = 1;
				Debug.Log(status);
				return;
			}
			if (Owner[x, z] > 0 && CharacterList[Owner[x, z]].over == 0)
			{
				//显示地形、玩家方棋子信息和移动范围，并准备开始移动
				status = 2;
				Dijkstra(selectedTile);
				//显示移动范围
				DisplayRange(Owner[x, z], 0);
				globalOldPosition = new Vector3Int(x, 0, z);
				Debug.Log(status);
				return;
			}
			if (Owner[x, z] < 0)
			{
				//显示地形、敌方棋子信息和移动范围
				status = 3;
				Dijkstra(selectedTile);
				//显示移动范围
				DisplayRange(Owner[x, z], 0);
				Debug.Log(status);
				return;
			}
		}
		//选择棋子时，立刻执行Dijkstra
		//选择棋子后，再次左键单击移动棋子
		//所以此处不需要再求最短路
		if (status == 2 && StateGameplayEvent.common[0])
		{
			StateGameplayEvent.common[0] = false;
			Vector3Int destination = ClickToCoord();
			globalDestination = destination;
			Debug.Log(destination);
			HideRange();
			if (destination == selectedTile)
			{
				status = 4;
				totlMoveTime = 1;
				return;
			}
			if (Owner[destination.x, destination.z] != 0 || Distance[destination.x, destination.z] > CharacterList[Owner[selectedTile.x, selectedTile.z]].mov * 2)
			{
				status = 1;
				selectedTile = destination;
				return;
			}
			List<Vector3Int> TransPath = Dfs(destination, CharacterList[Owner[selectedTile.x, selectedTile.z]].rid, 0);
			int len = TransPath.Count;
			Vector3[] GlobalPath = new Vector3[len];
			for (int i = 0; i < len; i++)
			{
				GlobalPath[i] = grid.CellToWorld(OldCoord(TransPath[i]));
			}
			Owner[destination.x, destination.z] = Owner[selectedTile.x, selectedTile.z];
			Owner[selectedTile.x, selectedTile.z] = 0;
			CharacterList[Owner[destination.x, destination.z]].x = destination.x;
			CharacterList[Owner[destination.x, destination.z]].z = destination.z;
			Move(Owner[destination.x, destination.z], GlobalPath, len);
			status = 4;
			return;
		}

		//显示环形菜单，进行角色操作
		if (status == 4)
		{
			if (!flagCvMenuEncirclChess)
			{
				startTime++;
			}
			if (startTime >= totlMoveTime)
			{
				startTime = 0;
				flagCvMenuEncirclChess = true;
				Debug.Log(status);
				ReactLayoutBattle.EncirclChess(globalDestination, cvMenuEncirclChess, true, true, true);
				DisplayRange(Owner[globalDestination.x, globalDestination.z], 1);
				status = 5;
			}

		}

		//等待玩家进行操作（攻击防御待机取消）
		if (status == 5)
		{
			atktemp = 0;
			atkselected = 0;
			flagCvMenuEncirclChess = false;
			HideRange();
		}

		//按下攻击按钮后
		if (status == 6 && StateGameplayEvent.common[0])
		{
			StateGameplayEvent.common[0] = false;
			atktemp = AtkSelect();
			Character A, B;
			if (atktemp == 0)
			{
				status = 5;
				ReactLayoutBattle.EncirclChess(globalDestination, cvMenuEncirclChess, true, true, true);
				DisPredictAtk();
				return;
			}
			if (atkselected != atktemp)
			{
				atkselected = atktemp;
				A = CharacterList[Owner[globalDestination.x, globalDestination.z]];
				B = EnemyList[-atkselected];
				Vector3Int enemypos = B.GetPosition();
				bool isback = false;
				if (B.weapon.isMag)
				{
					isback = true;
				}
				else if (B.weapon.rng == 1)
				{
					foreach (Vector3Int dir in Directions)
					{
						if (globalDestination + dir == enemypos)
						{
							isback = true;
							break;
						}
					}
				}
				else
				{
					foreach (Vector3Int dir in AttackDirections)
					{
						if (globalDestination + dir == enemypos)
						{
							isback = true;
							break;
						}
					}
				}
				PredictAtk(grid.CellToWorld(OldCoord(new Vector3Int(B.x, 0, B.z - 1))), A.role, B.role, Dam(A, B), (int)(Hrate(A, B) * 100), (int)(Crate(A, B) * 100), Dam(B, A), (int)(Hrate(B, A) * 100), (int)(Crate(B, A) * 100), A.gpa, B.gpa, A.trueAgi >= B.trueAgi * 2, isback);
				return;
			}
			globalCharacter = CharacterList[Owner[globalDestination.x, globalDestination.z]];
			globalEnemy = EnemyList[-atkselected];
			HideRange();
			DisPredictAtk();
			PlayerAttack();
		}

		//按下防御按钮后
		//进入防御状态，防御力+2，下回合解除
		if (status == 7)
		{
			CharacterList[Owner[globalDestination.x, globalDestination.z]].def += 2;
			CharacterList[Owner[globalDestination.x, globalDestination.z]].pro = 1;
			CharacterList[Owner[globalDestination.x, globalDestination.z]].over = 1;
			CharacterSprites[Owner[globalDestination.x, globalDestination.z]].GetComponent<SpriteRenderer>().color = new Color32(168, 168, 168, 255);
			ReactCharaInfo.Refresh(Owner[globalDestination.x, globalDestination.z], MapCells[globalDestination.x, globalDestination.z]);
			status = 0;
		}
		//按下取消按钮后，回到原位，当作无事发生
		if (status == 8)
		{
			ReactCharaInfo.Close();
			//移动到自己的位置后点取消，这么做的人都是傻逼
			if (globalDestination == globalOldPosition)
			{
				status = 1;
				return;
			}
			CharacterList[Owner[globalDestination.x, globalDestination.z]].x = globalOldPosition.x;
			CharacterList[Owner[globalDestination.x, globalDestination.z]].z = globalOldPosition.z;
			Owner[globalOldPosition.x, globalOldPosition.z] = Owner[globalDestination.x, globalDestination.z];
			Owner[globalDestination.x, globalDestination.z] = 0;
			CharacterSprites[Owner[globalOldPosition.x, globalOldPosition.z]].transform.position = grid.CellToWorld(OldCoord(globalOldPosition));
			status = 1;
		}
		//按下待机按钮后
		//恢复1点魔力
		if (status == 9)
		{
			CharacterList[Owner[globalDestination.x, globalDestination.z]].over = 1;
			CharacterSprites[Owner[globalDestination.x, globalDestination.z]].GetComponent<SpriteRenderer>().color = new Color32(168, 168, 168, 255);
			CharacterList[Owner[globalDestination.x, globalDestination.z]].mag =
				Math.Min(CharacterList[Owner[globalDestination.x, globalDestination.z]].mag + 2, CharacterList[Owner[globalDestination.x, globalDestination.z]].mmag);
			ReactCharaInfo.Refresh(Owner[globalDestination.x, globalDestination.z], MapCells[globalDestination.x, globalDestination.z]);
			status = 0;
		}
		//敌人回合
		if (status == 10)
		{
			//敌人回合开始，解除防御状态，并受到地形影响
			ReactCharaInfo.Close();
			HideRange();
			enemymoved = 0;
			flagattack = false;
			globalEnemy = new Character();
			foreach (Character uchar in CharacterList)
			{
				if (uchar.num == 0 || uchar.die == 1)
				{
					continue;
				}
				CharacterSprites[uchar.num].GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 255);
			}
			foreach (Character uchar in EnemyList)
			{
				if (uchar.num == 0 || uchar.die == 1)
				{
					continue;
				}
				if (uchar.pro == 1)
				{
					uchar.def -= 2;
					uchar.pro = 0;
				}
				uchar.over = 0;
				if (MapCells[uchar.x, uchar.z] == 3 && uchar.rid != 1)
				{
					uchar.gpa = Math.Min(uchar.gpa + uchar.mgpa / 5, uchar.mgpa);
				}
				else if (MapCells[uchar.x, uchar.z] == 7 && uchar.rid != 1)
				{
					uchar.gpa -= uchar.mgpa / 5;
				}
				EnemyList[uchar.num] = uchar;
				RefreshChessSlider(-uchar.num);
				if (uchar.gpa <= 0)
				{
					MakeDie(-uchar.num);
				}
			}
			status = 11;
		}
		//等待敌人移动完毕
		if (status == 11)
		{
			timeCount++;
			if (timeCount >= totlMoveTime && flagattack)
			{
				EnemyAttack(globalEnemy, globalCharacter);
				RefreshChessSlider(-globalEnemy.num);
				RefreshChessSlider(globalCharacter.num);
				status = 14;
				flagattack = false;
			}
			//攻击完成后等50f再进行下一个移动
			if (timeCount >= totlMoveTime + 50)
			{
				if (totlplayer == 0)
				{
					GameOver();
				}
				if (enemymoved >= totlenemy)
				{
					status = 12;
					return;
				}
				timeCount = 0;
				totlMoveTime = 0;
				EnemyBehaviour();
			}
		}
		//敌方结束，我方开始
		if (status == 12)
		{
			//我方回合开始，解除防御状态，并受到地形影响
			round++;
			ReactCharaInfo.RefreshRound();
			foreach (Character uchar in EnemyList)
			{
				if (uchar.num == 0 || uchar.die == 1)
				{
					continue;
				}
				EnemySprites[uchar.num].GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 255);
			}
			foreach (Character uchar in CharacterList)
			{
				if (uchar.num == 0 || uchar.die == 1)
				{
					continue;
				}
				if (uchar.pro == 1)
				{
					uchar.def -= 2;
					uchar.pro = 0;
				}
				uchar.over = 0;
				if (MapCells[uchar.x, uchar.z] == 3 && uchar.rid != 1)
				{
					uchar.gpa = Math.Min(uchar.gpa + uchar.mgpa / 5, uchar.mgpa);
				}
				else if (MapCells[uchar.x, uchar.z] == 7 && uchar.rid != 1)
				{
					uchar.gpa -= uchar.mgpa / 5;
				}
				CharacterList[uchar.num] = uchar;
				RefreshChessSlider(uchar.num);
				if (uchar.gpa <= 0)
				{
					MakeDie(uchar.num);
				}
			}
			Revive();
			ShowPhase(0);
			status = 15;
		}

		//等待我方动画结束
		if (status == 13)
		{

		}

		//等待敌方动画结束
		if (status == 14)
		{

		}

		//等待我方回合切换动画结束
		if (status == 15)
		{
			startTime++;
			if (startTime >= totlMoveTime)
			{
				startTime = totlMoveTime = 0;
				playerphase.gameObject.SetActive(false);
				status = 0;
			}
		}

		//等待敌方回合切换动画结束
		if (status == 16)
		{
			startTime++;
			if (startTime >= totlMoveTime)
			{
				startTime = totlMoveTime = 0;
				enemyphase.gameObject.SetActive(false);
				status = 10;
			}
		}

		//按下推按钮后，推动临近的一个人
		if (status == 17 && StateGameplayEvent.common[0])
		{
			StateGameplayEvent.common[0] = false;
			int x = globalDestination.x;
			int z = globalDestination.z;
			Vector3Int ucoord = ClickToCoord();
			foreach (Vector3Int dir in Directions)
			{
				if (ucoord.x + dir.x > 0 && ucoord.z + dir.z > 0 && ucoord.x + dir.x < width && ucoord.z + dir.z < length && Owner[ucoord.x, ucoord.z] != 0 && x + dir.x == ucoord.x && z + dir.z == ucoord.z && Owner[ucoord.x + dir.x, ucoord.z + dir.z] == 0 && movecost[MapCells[ucoord.x + dir.x, ucoord.z + dir.z], 0] < 10)
				{
					Owner[ucoord.x + dir.x, ucoord.z + dir.z] = Owner[ucoord.x, ucoord.z];
					Owner[ucoord.x, ucoord.z] = 0;
					if (Owner[ucoord.x + dir.x, ucoord.z + dir.z] > 0)
					{
						CharacterList[Owner[ucoord.x + dir.x, ucoord.z + dir.z]].x = ucoord.x + dir.x;
						CharacterList[Owner[ucoord.x + dir.x, ucoord.z + dir.z]].z = ucoord.z + dir.z;
						CharacterSprites[Owner[ucoord.x + dir.x, ucoord.z + dir.z]].transform.position = grid.CellToWorld(OldCoord(ucoord + dir));
						CharacterList[Owner[globalDestination.x, globalDestination.z]].over = 1;
						status = 0;
						CharacterSprites[Owner[globalDestination.x, globalDestination.z]].GetComponent<SpriteRenderer>().color = new Color32(168, 168, 168, 255);
						HideRange();
						return;
					}
					if (Owner[ucoord.x + dir.x, ucoord.z + dir.z] < 0)
					{
						EnemyList[-Owner[ucoord.x + dir.x, ucoord.z + dir.z]].x = ucoord.x + dir.x;
						EnemyList[-Owner[ucoord.x + dir.x, ucoord.z + dir.z]].z = ucoord.z + dir.z;
						EnemySprites[-Owner[ucoord.x + dir.x, ucoord.z + dir.z]].transform.position = grid.CellToWorld(OldCoord(ucoord + dir));
						CharacterList[Owner[globalDestination.x, globalDestination.z]].over = 1;
						status = 0;
						CharacterSprites[Owner[globalDestination.x, globalDestination.z]].GetComponent<SpriteRenderer>().color = new Color32(168, 168, 168, 255);
						HideRange();
						return;
					}
				}
			}
			ReactLayoutBattle.EncirclChess(globalDestination, cvMenuEncirclChess, true, true, true);
			HideRange();
			status = 5;
		}

		//游戏结束
		if (status == 18)
		{

		}
	}
}
