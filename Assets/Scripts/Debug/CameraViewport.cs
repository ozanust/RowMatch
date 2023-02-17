using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraViewport : MonoBehaviour
{
	[SerializeField]
	private Camera cam;
	// Start is called before the first frame update
	void Start()
	{
		cam = Camera.main;
	}

	// Update is called once per frame
	void Update()
	{
		float ratio = cam.aspect;
		Debug.Log("orthographicSize = " + cam.orthographicSize);
		Debug.Log("ratio = " + ratio);
		Debug.Log("camera view world width = " + cam.aspect * (cam.orthographicSize * 2));
		Debug.Log("camera view left bound = " + (cam.transform.position.x - (cam.aspect * cam.orthographicSize)));
		Debug.Log("camera view right bound = " + (cam.transform.position.x + (cam.aspect * cam.orthographicSize)));
	}
}
