using UnityEngine;
using System.Collections;

public class TriggerNarritive : MonoBehaviour {
	public AudioClip clip;
	public AudioSource sound;
	public GameObject player;
	public static bool canPlay;
	bool hasPlayed;
	GameManager gameScript;
	// Use this for initialization
	void Start () {
		hasPlayed = false;
		canPlay = false;
		gameScript = player.GetComponent<GameManager> ();
		if (gameScript) {
		}
	}
	void OnTriggerEnter(Collider col){
		if (col.gameObject.tag == "Snowman") {
			col.enabled = false;
			AITarget aiScript = col.gameObject.GetComponent<AITarget>();
			aiScript.GoToSafeZone();
		}

	}
}
