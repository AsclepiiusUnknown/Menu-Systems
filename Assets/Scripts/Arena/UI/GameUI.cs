using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameUI : MonoBehaviour
{
    public Image fadePlane;
    public GameObject gameOverUI;

    public RectTransform newWaveBanner;
    public Text newWaveTitle;
    public Text newWaveEnemyCount;

    public Spawner spawner;
    public Player player;

    public GameObject scoreCount;
     TextMeshProUGUI scoreCountText;
    public TextMeshProUGUI GameOverScoreCount;
    public RectTransform healthBar;
    public string[] smartAssComments;
    public GameObject gameUI;
    public GameObject pauseUI;
    

    public bool isPaused = false;

    private void Start()
    {
        player = FindObjectOfType<Player>();
        FindObjectOfType<Player>().OnDeath += GameOver;
    }

    private void Awake()
    {
        if(scoreCount.GetComponent<TextMeshProUGUI>() != null)
        {
            scoreCountText = scoreCount.GetComponent<TextMeshProUGUI>();
        }
        spawner = FindObjectOfType<Spawner>();
        spawner.OnNewWave += OnNewWave;
    }

    private void Update()
    {
        if (scoreCount.GetComponent<TextMeshProUGUI>() != null && scoreCountText != scoreCount.GetComponent<TextMeshProUGUI>())
        {
            scoreCountText = scoreCount.GetComponent<TextMeshProUGUI>();
        }
        if (scoreCountText != null)
        {
            scoreCountText.text = "Score: " + ScoreKeeper.score.ToString("D6");
        }

        float healthPercent = 0;
        if (player != null)
        {
            healthPercent = player.health / player.startingHealth;
        }
        healthBar.localScale = new Vector3(healthPercent, 1, 1);
    }

    void OnNewWave(int wavenumber)
    {
        string[] numbers = { "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Eleven" };
        newWaveTitle.text = "- Wave " + numbers[wavenumber-1] + " -";
        string enemyCountString = ((spawner.waves[wavenumber - 1].infinite) ? "Infinite" : spawner.waves[wavenumber - 1].enemyCount + "");
        newWaveEnemyCount.text = "Enemies: " + enemyCountString;

        StopCoroutine("AnimateNewWaveBanner");
        StartCoroutine("AnimateNewWaveBanner");
    }

    IEnumerator AnimateNewWaveBanner()
    {
        float delayTime = 1.5f;
        float speed = 3;
        float animatePercent = 0;
        int dir = 1;
        float endDelayTime = Time.time + 1 / speed + delayTime;

        while (animatePercent >= 0)
        {
            animatePercent += Time.deltaTime * speed * dir;

            if (animatePercent >= 1)
            {
                animatePercent = 1;
                if (Time.time > endDelayTime)
                {
                    dir = -1;
                }
            }

            newWaveBanner.anchoredPosition = Vector2.up * Mathf.Lerp(-450, -230, animatePercent);
            yield return null;
        }
    }

    void GameOver()
    {
        player.crossHairsGO.SetActive(false);
        Cursor.visible = true;
        StartCoroutine(Fade(Color.clear, new Color(0, 0, 0, .5f), 1));
        string SAComment = smartAssComments[Random.Range(0, smartAssComments.Length)];
        scoreCount.SetActive(false);
        GameOverScoreCount.text = "You scored " + ScoreKeeper.score.ToString() + ". " + SAComment;
        gameOverUI.SetActive(true);
    }

    IEnumerator Fade(Color from, Color to, float time)
    {
        float speed = 1 / time;
        float percent = 0;

        while (percent < 1)
        {
            percent += Time.deltaTime * speed;
            fadePlane.color = Color.Lerp(from, to, percent);
            yield return null;
        }
    }

    public void StartNewGame()
    {
        SceneManager.LoadScene("Arena Game");
    }
}
