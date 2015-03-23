using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AITarget : MonoBehaviour {
	public Canvas myCanvas;
	public bool inSafeZone;
	public GameObject player;
	public AudioClip[] spottedPlayerBarks,lostPlayerBarks, safeSounds;
	public static int spottedProgress,lostPlayerProgress;
	AudioSource aiSound;
	public Vector3 targetScale,mySpawnPoint;
	float meltTimer = 1,timeSinceLostPlayer,timeSinceSpottedPlayer;
	public float distanceToPlayer=20;
	NavMeshAgent myNavMesh;
	int randomBark;
	Animator anim;
	public enum AIState{
		idle,
		seeingPlayer,
		nearPlayer,
		safe
	}
	void OnEnable(){
		ResetSnowman ();
		targetScale = Vector3.one;
	}
	AIState ai = AIState.idle;
	void Start(){
		//mySpawnPoint = transform.position;
		inSafeZone = false;
		myCanvas.enabled = false;
		targetScale = Vector3.one;
		anim = GetComponent<Animator> ();
		player = GameObject.Find ("Player");
		anim.SetBool ("isWalking", false);
		myNavMesh = GetComponent<NavMeshAgent> ();
		aiSound = gameObject.GetComponent<AudioSource> ();
	}

	void Update(){
		distanceToPlayer = Vector3.Distance (player.transform.position, this.transform.position);
		switch (ai) {
		case AIState.idle:					//     IDLE PLAYER
			timeSinceLostPlayer+=Time.deltaTime;
			if(timeSinceLostPlayer>1){
				if(distanceToPlayer<20){	//		go to seeing player
					GoToSeeingPlayer();
				}
			}
			break;
		case AIState.nearPlayer:			//   NEAR PLAYER
			Melt();
			myCanvas.transform.LookAt(player.transform.position);
			if(distanceToPlayer>10){
				GoToSeeingPlayer();

			}
			break;
		case AIState.seeingPlayer:			//    SEEING PLAYER
			myNavMesh.destination = player.transform.position;
			if(distanceToPlayer >20){		//    Go to idle
				GoToIdle();
			}else if(distanceToPlayer <5){
				GoToNearPlayer();
			}
			break;
		case AIState.safe:
			meltTimer +=Time.deltaTime;
			if(meltTimer>=3){
				ResetSnowman();
				gameObject.SetActive(false);

			}
			targetScale = Vector3.one;
			break;
		}
	}
	void SetClip(AudioClip clip){
		aiSound.clip = clip;
		if (aiSound.isPlaying == false)
			aiSound.PlayOneShot (clip);

	}

	void Melt(){
		meltTimer -= Time.deltaTime;
		if (meltTimer < 0) {
			targetScale-=new Vector3(-0.05f,0.05f,-0.05f);
			aiSound.pitch-=0.05f;
			meltTimer=1;
		}
		transform.localScale = Vector3.Lerp (transform.localScale, targetScale, Time.deltaTime);
		if (transform.localScale.y < 0.5f) {
			GameManager.snowMenMelted++;
			ResetSnowman();
			gameObject.SetActive(false);
		}
	}

	void GoToIdle(){
		myCanvas.enabled = false;
		anim.SetBool ("isWalking", false);
		myNavMesh.destination = this.transform.position;
		timeSinceLostPlayer = 0;
		SetClip(lostPlayerBarks[Random.Range(0,lostPlayerBarks.Length)]);
		ai = AIState.idle;
	}
	void GoToNearPlayer(){
		myCanvas.enabled = true;
		anim.SetBool ("isWalking", false);
		myNavMesh.destination = this.transform.position;
		SetClip(spottedPlayerBarks[Random.Range(0,spottedPlayerBarks.Length)]);
		ai = AIState.nearPlayer;

	}
	public void GoToSafeZone(){
		ai = AIState.safe;
		GameManager.snowMenSaved++;
		anim.SetBool("isWalking",false);
		myNavMesh.destination = this.transform.position;
		SetClip(safeSounds[Random.Range(0,safeSounds.Length)]);

	}
	void GoToSeeingPlayer(){
		meltTimer = 0;
		myCanvas.enabled = false;
		anim.SetBool("isWalking",true);
		myNavMesh.destination = player.transform.position;
		ai = AIState.seeingPlayer;
	}
	void ResetSnowman(){
		if(anim)
		anim.SetBool ("isWalking", false);
		ai = AIState.idle;
		myCanvas.enabled = false;
		targetScale = Vector3.one;
		transform.localScale = Vector3.one;
		transform.position = mySpawnPoint;
		//gameObject.SetActive (false);
	}
}
