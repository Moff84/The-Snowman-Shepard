using UnityEngine;
using System.Collections;
//Not in use
public class shoot : MonoBehaviour {
	LineRenderer line;
	Light light;
	AudioSource sound;
	// Use this for initialization
	void Start () {
		light = GetComponent<Light> ();
		line = GetComponent<LineRenderer> ();
		sound = GetComponent<AudioSource> ();
		sound.enabled = false;
		line.enabled = false;
		light.enabled = false;
		Screen.lockCursor = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown ("Fire1")) {
			StopCoroutine("fire");
			StartCoroutine("fire");
		}
	}
	//transform.forward*5,hit.point
	IEnumerator fire(){
		line.enabled = true;
		sound.enabled = true;
		light.enabled = true;
		while (Input.GetButton("Fire1")) {
			line.GetComponent<Renderer>().material.mainTextureOffset = new Vector2(0,Time.time);
			Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
			RaycastHit hit;
			line.SetPosition(0,ray.origin);

			if(Physics.Raycast(ray,out hit)){
				line.SetPosition(1,hit.point);
				Debug.Log(hit.collider.gameObject);
				if(hit.rigidbody){
					hit.rigidbody.AddExplosionForce(10,hit.point,4);
					Debug.Log(hit.rigidbody.gameObject.name);
					GameManager.score++;
					hit.rigidbody.gameObject.SetActive(false);
				}
			}
			else
				line.SetPosition(1,ray.GetPoint(100));

			yield return null;
				}
		line.enabled = false;
		light.enabled = false;
		sound.enabled = false;

	}
}
