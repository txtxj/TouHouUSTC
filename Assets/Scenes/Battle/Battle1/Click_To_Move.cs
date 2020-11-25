using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Click_To_Move : MonoBehaviour
{
	public GameObject Selected_Character;
	Vector3[] Position=new Vector3[2];
	int Corner=0;
	// Start is called before the first frame update
	void Start()
	{
		
	}
	// Only called in function Move_To_Destination()
	void Move()
	{
		if (Corner<2)
		{
			iTween.MoveTo(Selected_Character,iTween.Hash("position",Position[Corner],"easeType","linear","speed",12f,"oncomplete","Move","oncompletetarget",gameObject));
			Corner++;
		}
	}
	void Move_To_Destination()
	{
		if (Input.GetMouseButtonDown(0))
		{
			Ray ray=GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			Physics.Raycast(ray,out hit);
			// 先横着走再竖着走
			Position[0]=new Vector3(hit.point.x,Selected_Character.transform.position.y,-3);
			Position[1]=new Vector3(hit.point.x,hit.point.y,-3);
			Corner=0;
			Move();
		}
	}
	// Update is called once per frame
	void Update()
	{
		Move_To_Destination();
	}
}
