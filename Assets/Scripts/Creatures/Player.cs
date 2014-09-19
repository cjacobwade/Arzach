using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour 
{
	private static Player _instance;
	public static Player Instance
	{
		get{ return _instance; }
		set{ _instance = value; }
	}

	//Model
	public Transform model, headModel;
	public Animation anim;
	
	//Movement
	int moveSpeed = 5;
	Vector3 moveVec, lastVelocity, velChange;
	internal Vector3 lastGroundPos = Vector3.zero;
	float maxChange = 10;
	
	//Camera controls
	public Camera camObj;
	float camLerpSpeed = 2f;
	float camSlerpSpeed = 2f;
	
	public Transform cam;
	Vector3 camOffset;
	
	//Gravity
	bool isGrounded = false;
	float gravityRate = 5f, maxGravity = -25f;
	float currentGravity = 0;

	//Effects
	ParticleSystem trail;

	//First person stuff
	internal bool firstPerson = false;
	float edgeLerpSpeed = 1f;

	public Transform headTransform, headPivot;
	float headLerpSpeed = 100f;
	float headSlerpSpeed = 4f;

	public EdgeDetectEffectNormals headEffect;
	public Color[] headColors;

	bool active = true;
	public GameObject endCam;

	// Use this for initialization
	void Start () 
	{
		_instance = this;

		trail = GetComponentInChildren<ParticleSystem>();
		camOffset = transform.position - cam.position;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(active)
		{
			Movement();
			CameraControl();
		}
	}
	
	void Movement()
	{
		Vector3 targetVelocity = Vector3.zero;

		if(!firstPerson)
        	targetVelocity = new Vector3(Input.GetAxis("Horizontal"), -1, Input.GetAxis("Vertical"));

		if(targetVelocity == -Vector3.up)
		{
			trail.emissionRate = 0;
			if(!anim["Activate"].enabled)
				anim.Stop("Walk");
		}
		else 
		{
			if(!anim["Activate"].enabled)
				anim.PlayAnimation("Walk");
			
			lastVelocity = targetVelocity;
			trail.emissionRate = 15;
			
			lastVelocity.y = 0;
			Quaternion camRot = Quaternion.Euler(0, cam.eulerAngles.y, cam.eulerAngles.z);
			model.transform.rotation = 	Quaternion.Lerp(model.transform.rotation, 
														Quaternion.LookRotation(camRot*lastVelocity),
														Time.deltaTime*7);
		}
		
		if(Mathf.Abs(targetVelocity.x) > 0.15f && Mathf.Abs(targetVelocity.z) > 0.15f)
   			targetVelocity *= 0.66f;
		
		targetVelocity = transform.TransformDirection(targetVelocity);
		targetVelocity *= moveSpeed; 
		
        // Apply a force that attempts to reach our target velocity
        velChange = Vector3.Lerp(velChange, targetVelocity - rigidbody.velocity, Time.deltaTime*4);
        velChange.x = Mathf.Clamp(velChange.x, -maxChange, maxChange);
        velChange.z = Mathf.Clamp(velChange.z, -maxChange, maxChange);
        velChange.y = 0;
        rigidbody.AddForce(velChange, ForceMode.VelocityChange);
	
		//Gravity
		isGrounded = Physics.Raycast(transform.position, -transform.up, 1f);
		
		if(!isGrounded)
		{
			if(currentGravity < maxGravity)
				currentGravity -= gravityRate;
		}
		else
			lastGroundPos = transform.position;
		
		rigidbody.AddForce(-Vector3.up*gravityRate*moveSpeed);
		
		model.transform.position = Vector3.Lerp(model.transform.position, transform.position, Time.deltaTime*7);
	}
	
	void CameraControl()
	{
		//Lock cursor
		if(Input.GetMouseButtonDown(0))
			Screen.lockCursor = !Screen.lockCursor;
		
		//Rotate camera based on mouse move
		cam.RotateAround(transform.position, transform.right, -Input.GetAxis("Mouse Y"));
		cam.RotateAround(transform.position, transform.up, Input.GetAxis("Mouse X"));

		//Clamp camera xrot
		Vector3 camEuler = cam.eulerAngles;

		//Pick from the possible colors each time you use fps mode
		if(Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Space))
		{
			//Search array of colors until a non-matching color is found
			Color lastColor = headEffect.edgesOnlyBgColor;
			while(lastColor == headEffect.edgesOnlyBgColor)
				headEffect.edgesOnlyBgColor = headColors[Random.Range(0, headColors.Length)];
		}

		firstPerson = Input.GetMouseButton(1) || Input.GetKey(KeyCode.Space);
		
		if(firstPerson)
		{
			if(camEuler.x > 85 && camEuler.x < 200) camEuler.x = 85;
			if(camEuler.x < 300 && camEuler.x > 130) camEuler.x = 300;

			model.gameObject.SetActive(false);
			headModel.gameObject.SetActive(true);
			
			cam.position = Vector3.Lerp(cam.position, headTransform.position, Time.deltaTime * 30);
			camObj.transform.position = Vector3.Lerp(camObj.transform.position, cam.position, 10 * Time.deltaTime);

			headEffect.edgesOnly = Mathf.Lerp(headEffect.edgesOnly, 1f, edgeLerpSpeed * Time.deltaTime);
			headEffect.sampleDist = Mathf.Lerp(headEffect.sampleDist, 2.1f, edgeLerpSpeed * Time.deltaTime);
		}
		else
		{
			if(camEuler.x > 70 && camEuler.x < 300) camEuler.x = 70;
			if(camEuler.x < 350 && camEuler.x > 100) camEuler.x = 350;

			model.gameObject.SetActive(true);
			headModel.gameObject.SetActive(false);
			
			cam.position = Vector3.Lerp(cam.position, transform.position - cam.rotation*camOffset, Time.deltaTime*5);
			camObj.transform.position = Vector3.Lerp(camObj.transform.position, cam.position, camLerpSpeed * Time.deltaTime);
			
			headEffect.edgesOnly = Mathf.Lerp(headEffect.edgesOnly, 0f, edgeLerpSpeed * Time.deltaTime);
			headEffect.sampleDist = Mathf.Lerp(headEffect.sampleDist, 0f, edgeLerpSpeed * Time.deltaTime);
		}

		cam.eulerAngles = camEuler;

		//Player rot matches camera rot
		Vector3 rot = transform.eulerAngles;
		rot.y = camEuler.y;
		transform.rotation = Quaternion.Euler(rot);
		
		headPivot.position = Vector3.Lerp(headPivot.position, camObj.transform.position, headLerpSpeed * Time.deltaTime);
		headPivot.rotation = Quaternion.Slerp(headPivot.rotation, camObj.transform.rotation, headSlerpSpeed * Time.deltaTime);
		
		cam.rigidbody.velocity = Vector3.zero;
		
		//Should always be the same
		camObj.transform.rotation = Quaternion.Slerp(camObj.transform.rotation, cam.rotation, camSlerpSpeed * Time.deltaTime);
	}

	public void Setup(LevelInfo currentLevel)
	{
		if(!currentLevel.lastPos.HasValue)
			return;

		transform.position = currentLevel.lastPos.Value;
		cam.position = currentLevel.lastCamPos;
		cam.rotation = currentLevel.lastCamRot;
		CamSetup();
	}

	void Reset()
	{
		transform.position = lastGroundPos;
		CamSetup();
	}

	void CamSetup()
	{
		cam.position = transform.position - cam.rotation*camOffset;
		camObj.transform.position = cam.position;
		
		headPivot.position = cam.position;
		headPivot.rotation = cam.rotation;
		
		cam.rigidbody.velocity = Vector3.zero;

		camObj.transform.rotation = cam.rotation;
	}
	
	void OnTriggerEnter(Collider other)
	{
		switch(other.tag)
		{
		case "Act":
			other.GetComponent<Animation>().Play();
			break;
		case "Reset":
			Reset();
			break;
		case "toDesert":
			GameManager.Instance.ChangeLevel(0);
			other.gameObject.SetActive(false);
			break;
		case "toEnd":
			active = false;
			other.gameObject.SetActive(false);
			camObj.camera.enabled = false;
			endCam.SetActive(true);
			break;
		}
	}
}
