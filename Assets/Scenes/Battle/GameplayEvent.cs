using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public static class StateGameplayEvent
{
	public static bool[] tmp = new bool[3];
}
public class Character
{
	private int Num,Role,Die,Lv,Exp,Mgpa,Gpa,Atk,Def,Mag,Tec,Agi,Mov,Luk,Wgt,Rid;
	private int X,Z;
	public int[] bag;
	public Character()
	{
		num = 0;
		role = 0;
		die = 0;
		lv = 1;
		exp = 0;
		mgpa = 10;
		gpa = 10;
		atk =5;
		def = 2;
		mag = 2;
		tec = 3;
		agi = 3;
		mov = 5;
		luk = 5;
		wgt = 10;
		rid = 1;
		bag = new int[3];
	} 
	public int num
	{
		get{return Num;}
		set{Num=value;}
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
		get{return Die;}
		set{Die=value;}
	}
	public int lv
	{
		get{return Lv;}
		set{Lv=value;}
	}
	public int exp
	{
		get{return Exp;}
		set{Exp=value;}
	}
	public int mgpa
	{
		get{return Mgpa;}
		set{Mgpa=value;}
	}
	public int gpa
	{
		get{return Gpa;}
		set{Gpa=value;}
	}
	public int atk
	{
		get{return Atk;}
		set{Atk=value;}
	}
	public int def
	{
		get{return Def;}
		set{Def=value;}
	}
	public int mag
	{
		get{return Mag;}
		set{Mag=value;}
	}
	public int tec
	{
		get{return Tec;}
		set{Tec=value;}
	}
	public int agi
	{
		get{return Agi;}
		set{Agi=value;}
	}
	public int mov
	{
		get{return Mov;}
		set{Mov=value;}
	}
	public int luk
	{
		get{return Luk;}
		set{Luk=value;}
	}
	public int wgt
	{
		get{return Wgt;}
		set{Wgt=value;}
	}
	//0->步行
	//1->飞行
	//2->骑自行车
	public int rid
	{
		get{return Rid;}
		set{Rid=value;}
	}
	public int x
	{
		get{return X;}
		set{X=value;}
	}
	public int z
	{
		get{return Z;}
		set{Z=value;}
	}
	public Vector3Int GetPosition()
    {
		return new Vector3Int(x,0,z);
    }
}

public class GameplayEvent : MonoBehaviour
{
	public Tilemap grid;
	public GameObject prefab;
	public GameObject cvMenuEncirclChess;

	int status;

	int length;
	int width;
	int offset;
	int maxDis;
	int numPeople;

	Character[] CharacterList;
	GameObject[] CharacterSprites;

	Character[] EnemyList;
	GameObject[] EnemySprites;

	int[,] MapCells;
	int[,] Distance;
	int[,] Owner;

	Vector3Int selectedTile;

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
	    new Vector3Int(-1,0,0),
	    new Vector3Int(0,0,-1),
	    new Vector3Int(-1,0,1),
	    new Vector3Int(1,0,-1),
	    new Vector3Int(0,0,1),
	    new Vector3Int(1,0,0),
	};

	//把地形存在MapCells中
	//地形		步行	飞行	骑行	效果		
	//0->平地	1		1		1		
	//1->小树林	1.5		1		2		闪避+20%
	//2->废墟	2		1		2.5		闪避+10%
	//3->补给点	1.5		1		2		闪避+25% 每回合开始时恢复10%Mgpa
	//4->水		5		1		inf		闪避-20%
	//5->墙		inf		1		inf
	//6->室内墙	inf		inf		inf
	//7->火		1.5		1		2		闪避-10% 每回合开始时损失10%Mgpa
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

	//返回移动至该格花费
	//rid:	0->步行		1->飞行		2->骑自行车
	int MoveCost(int x, int z, int rid)
	{
		if (Owner[x, z] != 0)   //有人在该位置，返回inf
		{
		    return 200;
		}
		return movecost[MapCells[x, z], rid];
	}

	void ListsInitial()
	{
		status = 0;

		length = 100;
		width = 100;
		offset = (length + width) / 4;
		maxDis = 50;
		numPeople = 20;

		CharacterList = new Character[numPeople];
		CharacterSprites = new GameObject[numPeople];

		EnemyList = new Character[numPeople];
		EnemySprites = new GameObject[numPeople];

		MapCells = new int[length, width];
		Distance = new int[length, width];
		Owner = new int[length, width];

		StateLayoutBattle.offset = offset;
		for (int i = 0; i < numPeople; i++)
		{
			CharacterList[i] = new Character();
			EnemyList[i] = new Character();
		}
	}


    void MapInitial()
    {
		//存地形
		for (int i = 0; i < length; i++)
		{
		    for (int j = 0; j < width; j++)
		    {
		        MapCells[i, j] = 0;
		    }
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
			Owner[uchar.x, uchar.z] = - uchar.num;
		}
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
		int rid;
		if (Owner[x, z] > 0)
		{
		    rid = CharacterList[Owner[x, z]].rid;
		}
		else if (Owner[x, z] < 0)
		{
		    rid = EnemyList[Owner[x, z] * (-1)].rid;
		}
		else
		{
		    //这个地方没人！
		    return;
		}
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
			//如只需获得到该角色距离不超过maxDis的位置，取消下几行注释
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
					Mathf.Min(Distance[x + dir.x, z + dir.z], Distance[x, z] + MoveCost(x + dir.x, z + dir.z, rid));
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
    List<Vector3Int> Dfs(Vector3Int pos)
    {
        List<Vector3Int> Path = new List<Vector3Int>();
        Path.Insert(0, pos);
        while (Distance[pos.x, pos.z] > 0)
        {
            foreach (Vector3Int dir in Directions)
            {
				Vector3Int upos = pos + dir;
				if (Distance[pos.x, pos.z] > Distance[upos.x, upos.z])
				{
					Path.Insert(0, upos);
					pos = upos;
				}
			}
		}
		return Path;
	}

	//让编号为num的sprite移动到dir的位置
	//dir为转化过的坐标
	void Move(int num, Vector3[] OldPath, int newStatus)
	{
		
		iTween.MoveTo(CharacterSprites[num], iTween.Hash("path", OldPath, "time", 2.3f, "islocal", true, "EaseType", "linear"));
	}
	int startTime;
	Vector3Int globalDestination;
	bool flagCvMenuEncirclChess;
    void Start()
    {
		StateLayoutBattle.grid = grid;
    	ListsInitial();
        //按照棋子List来实例化Prefab，并将其移动至(0,0,0)处
        //可以考虑再建一个数组存每一关每个棋子的初始位置
        //把下面的zero矢量换掉

        //Test
        Character hero = new Character();
        hero.num = 1;
        hero.x = TransCoord(grid.WorldToCell(new Vector3(0, 0, 0))).x;
        hero.z = TransCoord(grid.WorldToCell(new Vector3(0, 0, 0))).z;
        CharacterList[1] = hero;
        //Test

        foreach (Character uchar in CharacterList)
        {
        	if (uchar.num == 0 || uchar.die == 1)
        	{
        		continue;
        	}

        	CharacterSprites[uchar.num] = Instantiate(prefab);
        	Vector3 globalZero = new Vector3(0, 0, 0);
        	Vector3Int zero = grid.WorldToCell(globalZero);
        	globalZero = grid.CellToWorld(zero);
        	Vector3 upos = CharacterSprites[uchar.num].transform.position;

        	CharacterSprites[uchar.num].transform.Translate(globalZero - upos, Space.World);

        }
        MapInitial();
    }


    // Update is called once per frame
    void Update()
    {
		//初状态
		if ((status == 0 || status == 1) && StateGameplayEvent.tmp[0])
		{
			StateGameplayEvent.tmp[0] = false;
			selectedTile = ClickToCoord();
			Debug.Log("/*/Cell Relative coo "+selectedTile);
			int x = selectedTile.x;
			int z = selectedTile.z;
			// ViewCell(selectedTile);**
			if (Owner[x, z] == 0)
			{
				//只显示地形  
				status = 1;
				Debug.Log("Void Void VVV/*/ Cell Status "+status);
				return;
			}
			if (Owner[x, z] > 0)
			{
				//显示地形、玩家方棋子信息和移动范围，并准备开始移动
				//己方棋子选中
				status = 2;
				Dijkstra(selectedTile);
				
				Debug.Log("Cell Status "+status);
				return;
			}
			if (Owner[x, z] < 0)
			{
				//显示地形、敌方棋子信息和移动范围
				status = 3;
				Dijkstra(selectedTile);
				Debug.Log("Cell Status " + status);
				return;
			}
			
		}

		//选择棋子时，立刻执行Dijkstra
		//选择棋子后，再次左键单击移动棋子
		//所以此处不需要再求最短路
		if (status == 2 && StateGameplayEvent.tmp[0])
		{
			Vector3Int destination = ClickToCoord();
			Debug.Log(destination);
			if (destination == selectedTile)
			{
				status = 1;
				return;
			}
			List<Vector3Int> TransPath = Dfs(destination);
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
			//移动后选择进行UI操作，status变为4
			//临时测试，把statue改成了1
			status = 1;
			ReactCharaInfo.Open(CharacterList[Owner[destination.x, destination.z]]);
			return;
		}


		//显示地形和敌人攻击范围
		if (status == 3)
		{

		}

        //显示游戏内UI，进行角色操作
        
		if (status == 4)
		{
			if (!flagCvMenuEncirclChess&&(DateTime.Now.Second-startTime+60)%60 == 3)
			{
				ReactLayoutBattle.EncirclChess(globalDestination, cvMenuEncirclChess);
				flagCvMenuEncirclChess = true;
			}
			
		}
    }
}
