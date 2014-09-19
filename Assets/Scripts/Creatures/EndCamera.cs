using UnityEngine;
using System.Collections;

public class EndCamera : MonoBehaviour 
{
	public Vector2 rotateSpeed;

	Vector3 camEuler;

	// Use this for initialization
	void Start () 
	{
		camEuler = transform.eulerAngles;
	}
	
	// Update is called once per frame
	void Update () 
	{
		CameraControl();
	}

	void CameraControl()
	{
		//Lock cursor
		if(Input.GetMouseButtonDown(0))
			Screen.lockCursor = !Screen.lockCursor;
		
		//Rotate transformera based on mouse move
		camEuler.y += Input.GetAxis("Mouse X")*rotateSpeed.x;//Horizontal control
		camEuler.x += -Input.GetAxis("Mouse Y")*rotateSpeed.y;//Vertical control

		if(camEuler.x > 85 && camEuler.x < 200) camEuler.x = 85;
		if(camEuler.x < 300 && camEuler.x > 130) camEuler.x = 300;
		
		transform.eulerAngles = camEuler;
	}
}
