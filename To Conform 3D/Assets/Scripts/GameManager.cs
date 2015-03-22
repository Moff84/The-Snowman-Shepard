using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {
	public bool gameOver = false;
	List<GameObject> bullets;
	public Text paused,snowMenMeltedText,snowMenSavedText;
	Player playerScript;
	public static int score, snowMenMelted,snowMenSaved;
	public static GameObject instance,b;
	public GameObject bulletPrefab, player;
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
		snowMenMelted = 0;

		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
		if (instance == null)
			instance = this.gameObject;
		else 
			Destroy (this.gameObject);
		DontDestroyOnLoad (instance);
		playerScript = player.GetComponent<Player> ();
	}
	void Update(){
		switch (currentState) {
		case GameState.mainMenu:
			snowMenMeltedText.text = "";
			paused.text = "The Snowman Shepard\nPress Enter To Start";
			Time.timeScale = 0;
			snowMenMelted = 0;
			if(Input.GetKeyDown(KeyCode.Return)){
				Time.timeScale = 1;

				//PlayClip(startGame,gameSoundSource);
				PlayClip(inGameMusic,gameMusicSource);
				currentState = GameState.game;
			}
			break;

		case GameState.game:
			GameOver();
			if(snowMenMelted>0){
				snowMenMeltedText.text = "Snowmen Melted: "+snowMenMelted;
			}

			paused.text = "I\n---+---\nI";
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
			paused.text = "Paused";
			if(Input.GetKeyDown(KeyCode.Escape)){
				Time.timeScale = 1;
				gameMusicSource.UnPause();
				currentState = GameState.game;
			}
			if(Input.GetKeyDown(KeyCode.Return)){
				gameOver = true;
			}
			break;


		case GameState.gameOver:
			paused.text = "GAME OVER";
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
		if (snowMenSaved+snowMenMelted >= 10) {
			paused.text = "Game Over\n"+"Your Score: "+(snowMenSaved/snowMenMelted*100+timeInGame);
			gameOver = true;
		}
	}

	void ResetGame(){
		snowMenSaved = 0;
		snowMenMelted = 0;
		Application.LoadLevel (Application.loadedLevel);
		gameOver = false;
	}
	public void PlayClip(AudioClip clip,AudioSource source){
		source.clip = clip;
		source.Play ();
	}
}
