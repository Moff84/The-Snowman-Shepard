using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {
	public bool gameOver = false; //end game if true
	public Text menuText,snowMenMeltedText,snowMenSavedText; // different UI texts
	Player playerScript;
	public List<GameObject> snowMen; //my 10 snowmen that are in the game
	public static int score, snowMenMelted,snowMenSaved; //holding my scores. score only used by shoot. no longer in use
	public static GameObject instance;
	public GameObject snowman, player;
	public float startTime,timeInGame;
	public AudioSource gameSoundSource, gameMusicSource;
	public AudioClip startGame,menuMusic,inGameMusic,gameOverMusic,pauseSound;
	enum GameState{ // a state machine that will switch between the game and the menus
		mainMenu,
		game,
		gameOver,
		paused
	}
	GameState currentState = GameState.mainMenu;
	void Start(){
		snowMenMelted = 0;					//zeroing the scores
		snowMenSaved = 0;					//		"""""""
		Cursor.lockState = CursorLockMode.Locked;  //unity 5 lock screen method
		Cursor.visible = false;						// unity 5 hide cursor
		if (instance == null)					//if there's more than one of these scripts kill any other object with the script
			instance = this.gameObject;
		else 
			Destroy (this.gameObject);
		DontDestroyOnLoad (instance);
		playerScript = player.GetComponent<Player> ();	//get the player script
	}
	void Update(){
		switch (currentState) {								//menu state machine. see diagram
		case GameState.mainMenu:
			player.transform.position = Vector3.up;			//player spawn point happens to be (0,1,0)
			snowMenMeltedText.text = "";					//no score text in main menu
			snowMenSavedText.text = "";						//          """"""""""
			menuText.text = "The Snowman Shepard\nPress Enter To Start";//menu text
			snowMenMelted = 0;				//zeroing the scores
			snowMenSaved = 0;				//   """"""""""
			if(Input.GetKeyDown(KeyCode.Return)){ //go to game
				PlayClip(inGameMusic,gameMusicSource); //start the game music
				currentState = GameState.game;	
			}
			break;

		case GameState.game:
			GameOver();							//looks for gameOver condition
			if(snowMenMelted>0){											//show UI
				snowMenMeltedText.text = "Snowmen Melted: "+snowMenMelted;
			}
			if(snowMenSaved>0){
				snowMenSavedText.text = "Snowmen Saved: "+snowMenSaved;
			}

			menuText.text = "";
			Zoom();
			if(Input.GetKeyDown(KeyCode.Escape)){
				Time.timeScale = 0;
				currentState = GameState.paused;
				PlayClip(pauseSound,gameSoundSource);
				gameMusicSource.Pause();
			}
			if(gameOver){
				PlayClip(gameOverMusic,gameMusicSource);
				currentState = GameState.gameOver;
			}
			break;

		case GameState.paused:
			snowMenMeltedText.text = "";
			menuText.text = "Paused";
			if(Input.GetKeyDown(KeyCode.Escape)){
				Time.timeScale = 1;
				gameMusicSource.UnPause();
				currentState = GameState.game;
			}
			if(Input.GetKeyDown(KeyCode.Return)){
				Time.timeScale = 1;
				gameOver = true;
				menuText.text = "GAME OVER";
				currentState = GameState.gameOver;
			}
			break;


		case GameState.gameOver:
			Camera.main.fieldOfView = 60;
			if(Input.GetKeyDown(KeyCode.Return)){
				ResetGame();
				PlayClip(menuMusic,gameMusicSource);
				currentState = GameState.mainMenu;
			}
			break;

		}
	}

	void Zoom(){
		if (Input.GetMouseButton (1)) {
			Camera.main.fieldOfView = Mathf.Lerp (Camera.main.fieldOfView, 40, Time.deltaTime*5);
		} else
			Camera.main.fieldOfView = Mathf.Lerp (Camera.main.fieldOfView, 60, Time.deltaTime*6);
	}
	void GameOver(){
		if (snowMenSaved+snowMenMelted >= 10) {		//once all ten snowmen are dealt with end game and display text
			if(snowMenMelted!=0&&snowMenSaved!=0)
			menuText.text = "Game Over\n"+"Your Score: "+(snowMenSaved/snowMenMelted*100+timeInGame);
			else if (snowMenSaved==0)
				menuText.text = "GameOver\n"+"Why did you kill them all?";
			else if (snowMenMelted == 0)
				menuText.text = "Game Over\n"+"Perfect Game In "+timeInGame+" Secs";

			gameOver = true;
		}
	}

	void ResetGame(){
		snowMenSaved = 0;
		snowMenMelted = 0;
		for (int i=0; i<snowMen.Count; i++) {
			if(!snowMen[i].activeInHierarchy){
				snowMen[i].SetActive(true);
//				AITarget snowManScript = snowMen[i].GetComponent<AITarget>();
//				snowManScript.targetScale = Vector3.one;
//				snowMen[i].transform.position = snowManScript.mySpawnPoint.position;
//				snowMen[i].transform.localScale = Vector3.one;
//				snowMen[i].transform.rotation = snowManScript.mySpawnPoint.rotation;
//				snowMen[i].SetActive(true);
			}else{
				Debug.Log("Thought snowman"+i+" was alive");
//				AITarget snowManScript = snowMen[i].GetComponent<AITarget>();
//				snowMen[i].transform.position = snowManScript.mySpawnPoint.position;
//				snowMen[i].transform.localScale = Vector3.one;
//				snowMen[i].transform.rotation = snowManScript.mySpawnPoint.rotation;
			}
		}
		gameOver = false;
	}
	public void PlayClip(AudioClip clip,AudioSource source){
		source.clip = clip;
		source.Play ();
	}
}
