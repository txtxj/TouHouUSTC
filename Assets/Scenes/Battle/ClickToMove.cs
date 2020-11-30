using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickToMove: MonoBehaviour
{
	public GameObject SelectedCharacter;
	Vector3[] Position = new Vector3[2];
	int pace = 0;
	// Start is called before the first frame update
	void Start()
	{

	}
	// Only called in function Move_To_Destination()
	void Move()
	{
		if (pace < 2)
		{
			iTween.MoveTo(SelectedCharacter, iTween.Hash("position", Position[pace], "easeType", "linear", "speed", 12f, "oncomplete", "Move", "oncompletetarget", gameObject));
			pace++;
		}
	}
	void Move_To_Destination()
	{
		if (Input.GetMouseButtonDown(0))
		{
			Ray ray = GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			Physics.Raycast(ray, out hit);
			// 先横着走再竖着走
			Position[0] = new Vector3(hit.point.x, SelectedCharacter.transform.position.y, -3);
			Position[1] = new Vector3(hit.point.x, hit.point.y, -3);
			pace = 0;
			Move();
		}
	}
	// Update is called once per frame
	void Update()
	{
		Move_To_Destination();
	}
}
