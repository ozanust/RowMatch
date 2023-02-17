using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneGrid : MonoBehaviour
{
	private Camera cam;
	float cameraSize;

	void Start()
	{
		cam = Camera.main;
		cameraSize = cam.orthographicSize;
	}


}
