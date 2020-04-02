using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GAMEMANAGER : MonoBehaviour
{
    // VARIABLES --------------------------------------------------------------
    public static GAMEMANAGER access;
    [HideInInspector] public List<PlayerScript> players;
    [HideInInspector] public List<MotherShipScript> motherships;
    [HideInInspector] public bool isPlaying;
    [HideInInspector] public bool isPaused;
    [HideInInspector] public int score;
    float displayedScore;

    [Header("MUSICS")]
    public AudioClip musicMenu;
    public AudioClip musicGame;
    public AudioClip musicVictory;
    public AudioClip musicGameOver;
    AudioSource music;

    public Slider musicVolume;
    public Slider soundVolume;

    [Header("INTERFACE")]
    public Text scoreText;
    Transform canvas;
    GameObject gameUI;

    [Header("Game ref")]
    public GameObject spawner = null;


    // ---------------------------------------------------------------


    // DEBUT DE PARTIE --------------------------------------------------------------
    void Awake()
    {
        if (access == null) access = this; else Destroy(this);
        players = new List<PlayerScript>();
        motherships = new List<MotherShipScript>();
        music = GetComponentInChildren<AudioSource>();
        canvas = GameObject.Find("CANVAS_MENUS").GetComponent<Canvas>().transform;
        gameUI = GameObject.Find("CANVAS_UI");
        canvas.gameObject.SetActive(true);
        isPaused = false;
        isPlaying = false;
        if (!musicVolume)
        musicVolume = GameObject.Find("MusicSlider").GetComponent<Slider>();
        if (!soundVolume)
        soundVolume = GameObject.Find("SoundSlider").GetComponent<Slider>();
    }

    void Start()
    {
        musicVolume.value = .5f;
        soundVolume.value = .5f;
        PlayMusic(musicMenu);
        ShowMenu(canvas.Find("START_MENU"));
    }


    public void StartNewGame()
    {
        Debug.Log("STARTING NEW GAME");
        score = 0;
        displayedScore = 0;
        UpdateScore();
        isPlaying = true;
        PlayMusic(musicGame);
        gameUI.SetActive(true);
        HideMenus();
        spawner.SetActive(true);

        foreach (PlayerScript player in players)
        {
            player.StartNewGame();
        }

        foreach (MotherShipScript mothership in motherships)
        {
            mothership.StartNewGame();
        }

        StartCoroutine(WaitForMotherShips());
    }

    IEnumerator WaitForMotherShips()
    {
        bool ready = false;

        while (ready == false)
        {
            ready = true;
            foreach (MotherShipScript mothership in motherships)
            {
                if (mothership.alive == false) ready = false;
            }
            yield return null;
        }

        foreach (PlayerScript player in players)
        {
            player.shootScript.canShoot = true;

            
        }

        
    }

    // ---------------------------------------------------------------


    // METTRE EN PAUSE --------------------------------------------------------------
    void Update()
    {
        if (isPlaying && Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused) ResumeGame();
            else PauseGame();
        }

        foreach (PlayerScript player in players)
        {
            if (Input.GetKeyDown(KeyCode.M) && player.shootScript.fireRate != 8)
            {
                player.shootScript.fireRate = 8;
                player.shootScript.maxMissilesOnScreen = 2;
                SoundManager.Instance.Play("mathilda");
                player.SwapMatSprite();
            }
            if (Input.GetKeyDown(KeyCode.L) && player.shootScript.fireRate != 4)
            {
                player.shootScript.fireRate = 4;
                player.shootScript.maxMissilesOnScreen = 1;
                SoundManager.Instance.Play("leon");
                player.SwapLeonControllerSprite();
            }
        }


            UpdateScore();
    }
    // ----------------------------------------------------------------




    public void CheckGameOver()
    {
        bool gameOver = true;
        foreach (PlayerScript player in players)
        {
            if (player.extraLives >= 0) gameOver = false;
        }

        if (gameOver) GameOver();
    }


    public void GameOver()
    {
        if (isPlaying == false) return;
        Debug.Log("GAME OVER");
        isPlaying = false;
        foreach (PlayerScript player in players)
        {
            player.extraLives = -1;
            player.Die();
        }
        PlayMusic(musicGameOver);
        ShowMenu(canvas.Find("GAMEOVER_MENU"));
        SoundManager.Instance.Play("dead");
    }

    public void CheckVictory()
    {
        if (motherships.Count < 1) Victory();
    }

    public void Victory()
    {
        Debug.Log("VICTORY");
        isPlaying = false;
        PlayMusic(musicGameOver);
        ShowMenu(canvas.Find("VICTORY_MENU"));
        SoundManager.Instance.Play("victory");
    }



    // PAUSE --------------------------------------------------------------
    public void PauseGame()
    {
        Debug.Log("PAUSE");
        if (isPlaying && isPaused == false)
        {
            isPaused = true;
            ShowMenu(canvas.Find("PAUSE_MENU"));
            Time.timeScale = 0;
        }
    }

    public void ResumeGame()
    {
        Debug.Log("RESUME");
        if (isPlaying && isPaused)
        {
            isPaused = false;
            Time.timeScale = 1f;
            HideMenus();
        }
    }
    // --------------------------------------------------------------





    void PlayMusic(AudioClip desiredMusic)
    {
        if (desiredMusic == null) Debug.Log("Now Playing: ERROR (missing music)"); else Debug.Log("Now Playing: " + desiredMusic.name);
        music.Stop();
        music.clip = desiredMusic;
        if (desiredMusic != null) music.Play();
    }



    // MENUS --------------------------------------------------------------
    public void ShowMenu(Transform desiredMenu)
    {
        Debug.Log("Showing: " + desiredMenu.name);
        HideMenus();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        canvas.Find(desiredMenu.name).gameObject.SetActive(true);
    }


    public void HideMenus()
    {
        Debug.Log("Hiding Menus");
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        foreach (Transform menu in canvas)
        {
            menu.gameObject.SetActive(false);
        }
    }
    // --------------------------------------------------------------



    // SCORE --------------------------------------------------------------
    public void AddScore(int amount)
    {
        score += amount;
        if (score < 0) score = 0;
        Debug.Log("Score=" + score);
    }

    void UpdateScore()
    {

        if (displayedScore < score)
            displayedScore += 50f * Time.deltaTime + (score - displayedScore) / 10f;

        if (displayedScore > score) displayedScore = score;

        if (scoreText) scoreText.text = displayedScore.ToString("F0");
    }



    // --------------------------------------------------------------


    public void QuitApp()
    {
        Application.Quit();
    }





} // FIN DU SCRIPT



// INTERFACES --------------------------------------------------
public interface IKillable
{
    void LoseLife();
}

public interface IDamageable
{
    void Spawn();
    void TakeDamage(int amount);
    void Die();
}

public interface IStartable
{
    void StartNewGame();
}

public interface IDetectOffscreen
{
    void OffScreen();
}

// ----------------------------------------------------------------
