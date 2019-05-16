using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	public static GameManager Instance { get; set; }

    public bool isDead { get; set; }
    private bool isGameStarted = false;
    private PlayerController playerController;

    //UI and the UI fields
    public Animator gameCanvas, menuAnim, diamondAnim;
    public Text scoreText, coinText, modifierText, hiScoreText;
    private float score, coinScore, modifierScore;
    private int lastScore;

    //Death Menu
    public Animator deathMenuAnim;
    public Text deadScoreText, deadCoinText;

    private void Awake()
    {
        Instance = this;
        modifierScore = 1;
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        modifierText.text = "x" + modifierScore.ToString("0.0");
        coinText.text = coinScore.ToString("0");
        scoreText.text = scoreText.text = score.ToString("0");

        hiScoreText.text = PlayerPrefs.GetInt("Hiscore").ToString();
    }

    private void Update()
    {
        if (MobileInput.Instance.Tap && !isGameStarted)
        {
            isGameStarted = true;
            playerController.StartRunning();
            FindObjectOfType<BackgroundSpawner>().IsScrolling = true;
            FindObjectOfType<CameraController>().IsMoving = true;
            gameCanvas.SetTrigger("Show");
            menuAnim.SetTrigger("Hide");

        }

        if (isGameStarted && !isDead)
        {
            //score up
            score += (Time.deltaTime * modifierScore);
            if (lastScore != (int)score)
            {
                lastScore = (int)score;
                scoreText.text = score.ToString("0");
            }
        }
    }

    public void GetCoin()
    {
        diamondAnim.SetTrigger("Collect");
        coinScore ++;
        coinText.text = coinScore.ToString("0");
        score += Constant.COIN_SCORE_AMOUNT;
        scoreText.text = scoreText.text = score.ToString("0");
    }

    //public void UpdateScores()
    //{
    //    scoreText.text = score.ToString();
    //    coinText.text = coinScore.ToString();
    //    modifierText.text = "x" + modifierScore.ToString("0.0");
    //}

    public void UpdateModifier(float modifierAmount)
    {
        modifierScore = 1.0f + modifierAmount;
        modifierText.text = "x" + modifierScore.ToString("0.0");
    }

    public void OnPlayButton()
    {
        print("clicked");
        UnityEngine.SceneManagement.SceneManager.LoadScene("GermHunter");
    
    }

    public void OnDeath()
    {
        isDead = true;
        FindObjectOfType<BackgroundSpawner>().IsScrolling = false;
        deadScoreText.text = score.ToString("0");
        deadCoinText.text = coinScore.ToString("0");
        deathMenuAnim.SetTrigger("Dead");
        gameCanvas.SetTrigger("Hide");

        //check if this is a highscore
        if (score > PlayerPrefs.GetInt("Hiscore"))
        {
            float s = score;
            if (s % 1 == 0)
                s += 1;
            PlayerPrefs.SetInt("Hiscore", (int)s);
        }
    }
}
