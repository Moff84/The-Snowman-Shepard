using UnityEngine;
using System.Collections;

public class TriggerNarritive : MonoBehaviour {

	void OnTriggerEnter(Collider col){		// when the snowman enters the safe zone collider run the method in AITarget
		if (col.gameObject.tag == "Snowman") {
			col.enabled = false; //stops it playing more than once on each snowman
			AITarget aiScript = col.gameObject.GetComponent<AITarget>();
			aiScript.GoToSafeZone();
		}

	}
}
