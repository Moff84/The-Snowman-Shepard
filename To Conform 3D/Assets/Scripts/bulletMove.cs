using UnityEngine;
using System.Collections;

public class bulletMove : MonoBehaviour {
	public float bulletSpeed;

	void Update () {
		transform.Translate(Vector3.forward*bulletSpeed);
		float placeOnMap = Terrain.activeTerrain.SampleHeight (transform.position);
		if (transform.position.y <= placeOnMap + this.transform.position.y) {
			gameObject.SetActive(false);
		}
	}
	void OnCollisionEnter(Collision col){
		Debug.Log (col.gameObject.name);
		if(col.gameObject.tag == "Snowman"){
			col.transform.localScale +=new Vector3(0.01f,0.01f,0.01f);
		}
		this.gameObject.SetActive (false);
	}

}
