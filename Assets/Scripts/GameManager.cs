using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Vuforia;

public class GameManager : DefaultTrackableEventHandler
{

    public static GameManager instance;

    public GameObject[] shells;

    public Text scoreCountText, livesCountText, roundTextTargetText, roundTextNumber, gameOverScoreText;

    public int round = 1;
    public int playerScore = 0;
    public int shotsPerRound = 3;
    private int _lives = 2;

    // -- Show hide GameObjects
    public GameObject GUIScoreText, GUILivesText, GUICenterTarget, GUIFireButton, GUIDog, GUIRoundText, GUIGameOverPanel, GUIGun;
    public GameObject startPanel;
    public GameObject terrain;

    AudioSource audioSource;

    public AudioClip[] clips;


    // -- Round Rules
    private int _roundTargetScore = 3;
    private int _scoreIncrement = 2;
    public int roundScore = 0;
    public bool _playerStarted = false;

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        playerScore = int.Parse(scoreCountText.text);
        ShowStartPanel();
        audioSource = GetComponent<AudioSource>();
        livesCountText.text = _lives.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (DefaultTrackableEventHandler.trueFalse)
        {
            HideStartPanel();
            ShowItems();
            if (!_playerStarted)
            {
                StartCoroutine(PlayRound());
            }
            _playerStarted = true;
        }
        else
        {
            ShowStartPanel();
            HideItems();
        }

        if (roundScore == _roundTargetScore)
        {
            PlayEffects(0);
            StartCoroutine(NewRound());
            roundScore = 0;
            _roundTargetScore = _roundTargetScore + _scoreIncrement;
        }

        if (shotsPerRound == 0)
        {
            shells[0].SetActive(false);
            StartCoroutine(LoseLife());
            shotsPerRound = 3;
        }

        scoreCountText.text = playerScore.ToString();
    }

    public void ShowItems()
    {
        GUIScoreText.SetActive(true);
        GUILivesText.SetActive(true);
        GUICenterTarget.SetActive(true);
        GUIFireButton.SetActive(true);
        terrain.SetActive(true);
        GUIGun.SetActive(true);
        ShowShells();
    }

    public void HideItems()
    {
        GUIScoreText.SetActive(false);
        GUILivesText.SetActive(false);
        GUICenterTarget.SetActive(false);
        GUIFireButton.SetActive(false);
        terrain.SetActive(false);
        GUIGun.SetActive(false);
    }

    private void ShowStartPanel()
    {
        startPanel.SetActive(true);
    }

    private void HideStartPanel()
    {
        startPanel.SetActive(false);
    }

    public IEnumerator PlayRound()
    {
        yield return new WaitForSeconds(2.0f);
        roundTextTargetText.text = "SHOOT " + _roundTargetScore.ToString() + " DUCKS";
        GUIRoundText.SetActive(true);
        PlayEffects(0);
        StartCoroutine(HideRoundText());
    }

    private void PlayEffects(int sound)
    {
        audioSource.clip = clips[sound];
        audioSource.Play();
    }

    public IEnumerator HideRoundText()
    {
        yield return new WaitForSeconds(4.0f);
        GUIRoundText.SetActive(false);
    }

    public void ShowShells()
    {
        if (shotsPerRound == 3)
        {
            shells[0].SetActive(true);
            shells[1].SetActive(true);
            shells[2].SetActive(true);
        }
        else if (shotsPerRound == 2)
        {
            shells[0].SetActive(true);
            shells[1].SetActive(true);
            shells[2].SetActive(false);
        }
        else if (shotsPerRound == 1)
        {
            shells[0].SetActive(true);
            shells[1].SetActive(false);
            shells[2].SetActive(false);
        }
    }

    public IEnumerator LoseLife()
    {
        _lives--;
        if (_lives == 0)
        {
            GUIFireButton.SetActive(false);
            PlayEffects(1);
            GUIGameOverPanel.SetActive(true);
            gameOverScoreText.text = playerScore.ToString();
            _lives = 0;
        }
        else
        {
            GUIFireButton.SetActive(true);
            PlayEffects(2);
            GUIDog.SetActive(true);
            yield return new WaitForSeconds(3.0f);
            GUIDog.SetActive(false);
            GUIFireButton.SetActive(true);
            shotsPerRound = 3;
        }
        yield return new WaitForSeconds(0.2f);
        livesCountText.text = _lives.ToString();
    }

    public void Quit()
    {
        //Application.Quit();
        SceneManager.LoadScene("Intro");
    }

    public IEnumerator NewRound()
    {
        yield return new WaitForSeconds(3.0f);
        round++;
        GUIRoundText.SetActive(true);
        roundTextTargetText.text = "SHOOT " + _roundTargetScore + " DUCKS";
        roundTextNumber.text = round.ToString();
        StartCoroutine(HideRoundText());
    }

    public void Restart()
    {
        HideItems();
        _lives = 2;
        livesCountText.text = _lives.ToString();
        playerScore = 0;
        scoreCountText.text = playerScore.ToString();
        _roundTargetScore = 3;
        roundScore = 0;
        gameOverScoreText.text = "0";
        round = 1;
        roundTextNumber.text = round.ToString();
        GUIGameOverPanel.SetActive(false);
        StartCoroutine(PlayRound());
    }
}