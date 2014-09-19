using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RuneManager : MonoBehaviour 
{
	string[] ulysses = {"I was a Flower", "of the mountain", "yes, when?",
						"I put the rose in my hair", "like the Andalusian girls", 
						"or shall I wear a red", "how he kissed me", "under the Moorish wall", 
						"I thought well as well", "him as another", "asked him with my eyes", 
						"ask again", "he asked", "mountain flower", "put my arms", "around him yes",
						"drew him down", "all perfume", "his heart", "I said yes", "I will Yes"};

	public GameObject textPrefab;

	public Transform leftEye, rightEye;
	bool runeHeld = false;

	public LayerMask runeLayer; // Which layer should the spawn raycast look at

	public GameObject[] runes;
	Transform rune = null, rune2 = null;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(Player.Instance.firstPerson)
		{
			RaycastHit hit;
			if(Physics.SphereCast(transform.position, 10, transform.forward, out hit, 100, runeLayer))
			{
				hit.collider.transform.parent = transform;

				if(hit.collider.transform != rune)
				{
					//If this is the second rune we're picking up
					if(runeHeld)
					{	
						SpawnText(hit.transform.position);

						//Save ref to rune2
						rune2 = hit.collider.transform;

						KillRune(ref rune);
						KillRune(ref rune2);

						runeHeld = false;

						StartCoroutine(SwapLevel());
					}
					else
					{
						rune = hit.collider.transform;
						rune.collider.enabled = false;
						runeHeld = true;
					}
				}
			}
		}
		else
		{
			ResetRune(ref rune);
			ResetRune(ref rune2);
			runeHeld = false;
		}
	}

	void ResetRune(ref Transform resetRune)
	{
		if(resetRune != null)
		{
			StartCoroutine(resetRune.GetComponent<Rune>().Reset());
			resetRune = null;
		}
	}

	void KillRune(ref Transform killRune)
	{
		Destroy(killRune.gameObject);
	}

	void SpawnText(Vector3 spawnPos)
	{
		//Spawn text object
		GameObject text = (GameObject)Instantiate(textPrefab, spawnPos, Quaternion.identity);

		//Give it a text mesh
		TextMesh text3D = text.GetComponent<TextMesh>();
		text3D.text = ulysses[Random.Range(0,ulysses.Length)];

		//Face it towards the player
		text.transform.LookAt(transform);
		text.transform.rotation *= Quaternion.Euler(0, 180, 0);
	}

	IEnumerator SwapLevel()
	{
		yield return new WaitForSeconds(3.5f);
		GameManager.Instance.ChangeLevel(1);
	}
}
