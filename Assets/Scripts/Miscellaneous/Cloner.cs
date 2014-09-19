using UnityEngine;
using System.Collections;

public class Cloner : MonoBehaviour 
{
	public bool animOffset = false;
	public int moveSpeed, rotSpeed;
	float timer = 0.5f;
	
	
	// Use this for initialization
	void Start () 
	{
		if(animOffset)
			StartCoroutine(Offset());
	}
	
	IEnumerator Offset()
	{
		yield return new WaitForSeconds(Random.Range(0.2f,0.8f));
		animation.PlayAnimation("Wave");
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(timer <= 0)
			SpawnCopy();
		
		timer -= Time.deltaTime;
		
		transform.Rotate(0,Time.deltaTime*rotSpeed,0);
		transform.position += new Vector3(Time.deltaTime*moveSpeed,0,0);
	}
	
	void SpawnCopy()
	{
		GameObject copy = (GameObject)Instantiate(gameObject, transform.position, transform.rotation);
		copy.animation[copy.animation.clip.name].time = animation[animation.clip.name].time;
		copy.animation.Stop();
		//copy.animation.Play();
		copy.GetComponent<Cloner>().moveSpeed = 0;
		Destroy(copy.GetComponent<Cloner>());
		timer = 0.5f;
	}
}
