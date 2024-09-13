using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    [SerializeField]
    private Text _scoreText;
    [SerializeField]
    private Text _gameOverText;
    [SerializeField]
    private Sprite[] _livesSprites;
    [SerializeField]
    private Image _LivesImage;
    [SerializeField]
    private Text _restartText;
    [SerializeField]
    private Text _ammoCount;
    private GameManager _gameManager;
    [SerializeField]
    private Sprite[] _shieldSprites;
    [SerializeField]
    private Image _ShieldsImage;
    public int shieldLife = 3;
    [SerializeField]
    private GameObject _shieldsDisplay;
    [SerializeField]
    private Slider _thrusterBar;
    [SerializeField]
    private Text _waveCounter;
    //private int _shieldLife = 3;
    // Start is called before the first frame update
    void Start()
    {
        _scoreText.text = "Score: " + 0;
        _gameOverText.gameObject.SetActive(false);
        _ammoCount.text = 15.ToString();
        _shieldsDisplay.SetActive(false);
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        _waveCounter.gameObject.SetActive(false);
        _thrusterBar.value = 1f;
        if(_gameManager == null)
        {
            Debug.LogError("GameManager is NULL");
        }
    }
    public void UpdateWaves(int currentWave)
    {
        if (currentWave == 4)
        {
            _waveCounter.gameObject.SetActive(true);
            _waveCounter.text = "Wave: Boss";
            StartCoroutine(WaveDown());
        }
        else
        {
            _waveCounter.gameObject.SetActive(true);
            _waveCounter.text = "Wave: " + currentWave.ToString();
            StartCoroutine(WaveDown());
        }
    }
    private IEnumerator WaveDown()
    {
        while(true)
        {
            yield return new WaitForSeconds(6.0f);
            _waveCounter.gameObject.SetActive(false);
        }
    }

    public void UpdateScore(int playerScore)
    {
        _scoreText.text = "Score: " + playerScore.ToString();
    }

    public void UpdateShieldLife(int shieldLife)
    {
        _shieldsDisplay.SetActive(true);
        _ShieldsImage.sprite = _shieldSprites[shieldLife];
    }

    public void UpdateLives(int currentLives)
    {
        _LivesImage.sprite = _livesSprites[currentLives];
        if(currentLives <= 0)
        {
            GameOverSequence();
        }
    }

    public void UpdateAmmo(int currentAmmoCount)
    {
        _ammoCount.text = currentAmmoCount.ToString();
    }

    void GameOverSequence()
    {
        _gameManager.GameOver();
        _restartText.gameObject.SetActive(true);
        StartCoroutine(GameOverFlickerRoutine());
    }

    IEnumerator GameOverFlickerRoutine()
    {
        while(true)
        {
            _gameOverText.gameObject.SetActive(true);
            yield return new WaitForSeconds(1f);
            _gameOverText.gameObject.SetActive(false);
            yield return new WaitForSeconds(1f);
        }
    }

    public void UpdateThrusterBar(float value)
    {
        
        if(value < 0)
        {
            _thrusterBar.value = 0;
        }
        else if(value > 10)
        {
            _thrusterBar.value = 10;
        }
        else
        {
            _thrusterBar.value = value;
        }

    }  
}
