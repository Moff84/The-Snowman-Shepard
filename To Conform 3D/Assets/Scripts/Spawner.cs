using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//Not in use
public class Spawner : MonoBehaviour {
	public GameObject player,snowman;
	void Start(){
		InvokeRepeating ("CreateSnowman", 5, 5);
	}
	
	// Update is called once per frame
	void Update () {
		transform.RotateAround (player.transform.position, Vector3.up, 2);
	}

	void CreateSnowman(){
		GameObject obj = (GameObject)Instantiate(snowman);
		obj.transform.position = this.transform.position;
		obj.transform.rotation = this.transform.rotation;
		obj.transform.localScale = Vector3.one;
		}
}
