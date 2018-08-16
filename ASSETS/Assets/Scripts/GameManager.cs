using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { set; get; }
    private List<Vegetable> veggies = new List<Vegetable>();

    private Vector3 lastMousePos;

    private float lastSpawn;
    private float deltaSpawn = 1.0f;

    private const float requiredsliceforce = 8f;

    public GameObject vegetablePrefab;
    public Transform trail;

    private Collider2D[] veggiesCols;

    //UI part of the Game
    private int score;
    private int highscore;
    public int lifepoint;

    public Text scoreText;
    public Text highscoreText;

    public Image[] lifepoints;

    public GameObject pauseMenu;
    public GameObject deathMenu;

    public bool isPaused;
    
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        veggiesCols = new Collider2D[0];
        NewGame();

    }

    public void NewGame()
    {
        SoundManager.Instance.PlaySound(0);
        score = 0;
        lifepoint = 3;
        pauseMenu.SetActive(false);
        scoreText.text = score.ToString();
        highscore = PlayerPrefs.GetInt("Score");
        highscoreText.text = "BEST: " + highscore.ToString();
        Time.timeScale = 1;
        isPaused = false;

        foreach (Image i in lifepoints)
            i.enabled = true;

        foreach (Vegetable v in veggies)
            Destroy(v.gameObject);
        veggies.Clear();

        deathMenu.SetActive(false);

    }

    private void Update()
    {

        if (isPaused)
            return;

        if(Time.time - lastSpawn > deltaSpawn)
        {
            Vegetable v = GetVegetable();
            float randomX = Random.Range(-1.65f,1.65f);
            v.LaunchVegetable(Random.Range(1.85f,2.75f),randomX,-randomX);

            lastSpawn = Time.time;

        }

        if(Input.GetMouseButton(0))
        {
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pos.z = -1;
            trail.position = pos;
            Collider2D[] thisFramesVeggie = Physics2D.OverlapPointAll(new Vector2(pos.x, pos.y),LayerMask.GetMask("Vegetable"));
            
            if ((Input.mousePosition - lastMousePos).sqrMagnitude > requiredsliceforce)
            {

                foreach (Collider2D c2 in thisFramesVeggie)
                {
                    for (int i = 0; i < veggiesCols.Length; i++)
                    {
                        if (c2 == veggiesCols[i])
                        {
                            c2.GetComponent<Vegetable>().Slice();
                        }
                    }
                }
            }
            veggiesCols = thisFramesVeggie;
            lastMousePos = Input.mousePosition;


        }
    }

    private Vegetable GetVegetable()
    {
        Vegetable v = veggies.Find(x => !x.IsActive);

        if(v == null)
        {
            v = Instantiate(vegetablePrefab).GetComponent<Vegetable>();
            veggies.Add(v);
        }

        return v;

    }

    public void IncrementScore(int scoreAmount)
    {
        score += scoreAmount;
        scoreText.text = score.ToString();

        if(score > highscore)
        {
            highscore = score;
            highscoreText.text = "BEST: " + highscore.ToString();
            PlayerPrefs.SetInt("Score", highscore); 
        }
    }

    public void LoseLP()
    {
        if (lifepoint == 0)
            return;
        lifepoint--;
        lifepoints[lifepoint].enabled = false;
        if (lifepoint == 0)
            Death();
    }

    public void Death()
    {
        isPaused = true;
        
        deathMenu.SetActive(true);
    }

    public void PauseGame()
    {
        SoundManager.Instance.PlaySound(0);
        pauseMenu.SetActive(!pauseMenu.activeSelf);
        isPaused = pauseMenu.activeSelf;
        Time.timeScale = (Time.timeScale == 0) ? 1 : 0;
    }

    public void ToMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
