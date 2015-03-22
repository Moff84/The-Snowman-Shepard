using UnityEngine;
using System.Collections;

public class PlayerShoot : MonoBehaviour {
	GameObject prefab;
	// Use this for initialization
	void Start () {
		prefab = Resources.Load ("snowball") as GameObject;
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButtonDown(0)){
			GameObject projectile = (GameObject)Instantiate(prefab);
			projectile.transform.position = Camera.main.transform.position+Vector3.forward*2;
			Rigidbody rb = projectile.GetComponent<Rigidbody>();
			rb.velocity = Camera.main.transform.forward*50;
		}
	}
}
