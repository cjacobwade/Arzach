using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour 
{
	public Sounder[] sounders;
	Sounder currentSound, targetSound;
	
	private static MusicManager _instance;
	public static MusicManager Instance
	{
		get{ return _instance; }
	}
	
	float changeTimer = 2;

	// Use this for initialization
	void Start () 
	{
		_instance = this;

		for (int i = 0; i < sounders.Length; i++) 
		{
			sounders[i].source = gameObject.AddComponent<AudioSource>();
			sounders[i].source.clip = sounders[i].song;
			sounders[i].source.playOnAwake = true;
			sounders[i].source.loop = true;
			sounders[i].source.volume = 0;
		}
		
		currentSound = sounders[0];
		targetSound = sounders[0];
		
		ChangeMusic(sounders[0]);
		
		currentSound.source.enabled = false;
		currentSound.source.enabled = true;
		
		currentSound.source.volume = currentSound.targetVolume;
	}

	public void ChangeMusic(Sounder sound)
	{
		targetSound = sound;
		if (targetSound != currentSound) 
		{
			while(currentSound.source.volume > 0.1f)
			{
				currentSound.source.volume = Mathf.Lerp (currentSound.source.volume, 0, Time.deltaTime);
				targetSound.source.volume = Mathf.Lerp (targetSound.source.volume, targetSound.targetVolume, Time.deltaTime);
			}
			currentSound.source.volume = 0;
			
			targetSound.source.enabled = false;
			targetSound.source.enabled = true;
			targetSound.source.volume = targetSound.targetVolume;
			
			currentSound = targetSound;
		}
	}
	
	void OnTriggerStay(Collider other)
	{
		if(changeTimer > 0)
			changeTimer-=Time.deltaTime;
		
//		if(changeTimer <= 0)
//		{
//			for(int i = 0; i < sounders.Length; i++)
//			{
//				if(other == sounders[i].hit)
//					ChangeMusic(sounders[i]);
//			}
//		}
	}
	
	void OnTriggerExit()
	{
		changeTimer = 2;	
	}
}

[System.Serializable]
public class Sounder
{
	public string name;	//just to make things easier in the inspector
	public AudioClip song;
	public Collider hit;
	public float targetVolume = 0.5f;
	internal AudioSource source;
}
