using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour{
	public float health;
	public float playerHeight;
	// Use this for initialization
	void Start () {
		health = 100;
	}
	
	// Update is called once per frame
	void Update () {
		//StandOnTerrain ();
	}
	void StandOnTerrain(){
		float placeOnMap = Terrain.activeTerrain.SampleHeight (transform.position);
		if (transform.position.y <= placeOnMap + playerHeight) {
			transform.position = new Vector3(this.transform.position.x,placeOnMap+playerHeight,this.transform.position.z);
		}
	}
}
