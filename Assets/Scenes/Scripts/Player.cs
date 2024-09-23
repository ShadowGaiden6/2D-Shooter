using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // public or private reference
    //data type (int, float, bool, string)
    //every variable has a name
    //optional value assigned
    private float _speed = 5f;
    private float _thrustMultiplier = 1;
    private float _boostMultiplier = 1;
    private float _boostValue = 3;
    private float _thrustValue = 2;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private float _fireRate = 0.15f;
    private float _canFire = -1f;
    [SerializeField]
    private int _lives = 3;
    private SpawnManager _spawnManager;
    [SerializeField]
    private bool _isTripleShotActive = false;
    private bool _speedBoostActive = false;
    private bool _shieldsActive = false;
    private bool _fireLockActive = false;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    [SerializeField]
    private GameObject _shieldVisual;
    [SerializeField]
    private int _score;
    private UI_Manager _uiManager;
    [SerializeField]
    private GameObject[] _playerDamage; //0 is Left Engine, 1 is Right Engine
    private AudioSource _audioSource1;
    [SerializeField]
    private AudioSource _audioSource2;
    [SerializeField]
    private GameObject _shieldsDisplay;
    public int shieldLife = 3;
    [SerializeField]
    private int _ammo = 15;
    [SerializeField]
    private GameObject _megaLaserPrefab;
    private bool _isMegaLaserActive = false; 
    [SerializeField]
    private float _thrusterValue = 10f;
    private bool _thrustersActive = false;
    private bool _thrusterEmpty = false;
    private CameraShake _camShake;
    [SerializeField]
    private bool _isHomingActive = false;
    [SerializeField]
    private GameObject _homingPrefab;
    
    // Start is called before the first frame update
    void Start()
    {
        //take the current position = new position (0, 0, 0)
        transform.position = new Vector3(0, -3.2f, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("UI_Manager").GetComponent<UI_Manager>();
        _camShake = GameObject.Find("Main Camera").GetComponent<CameraShake>();
        _audioSource1 = GetComponent<AudioSource>();
        if (_spawnManager == null)
        {
            Debug.LogError("The Spawn Manager is NULL");
        }

        if (_uiManager == null)
        {
            Debug.Log("The UI Manager is NULL");
        }

        if (_audioSource1 == null)
        {
            Debug.LogError("Player audio source is NULL");
        }

        StartCoroutine(ThrustersPowerDown());

    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
        Thrusters();
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            if(_ammo == 0)
            {
                _audioSource2.Play();
                return;
            }
            else if(_fireLockActive == true) //Negative powerup that prevents firing
            {
                return;
            }
            else if(_isMegaLaserActive == true) //Keeps regular lasers from firing whilst MegaLaser is active
            {
                return;
            }
            FireLaser();
        }
    }

    //Basic Player Movement
    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");

        float verticalInput = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

        transform.Translate(_speed * _thrustMultiplier * _boostMultiplier * Time.deltaTime * direction);
    
        if (transform.position.y >= 0)
        {
            transform.position = new Vector3(transform.position.x, 0, 0);
        }
        else if (transform.position.y <= -4.3f)
        {
            transform.position = new Vector3(transform.position.x, -4.3f, 0);
        }

        if (transform.position.x >= 10)
        {
            transform.position = new Vector3(-10, transform.position.y, 0);
        }
        else if (transform.position.x <= -10)
        {
            transform.position = new Vector3(10, transform.position.y, 0);
        }

    }

    //Thruster Function
    void Thrusters()
    {
        if (_thrusterEmpty == false)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift) && _thrusterValue > 0)
            {
                _thrustersActive = true;
                _thrustMultiplier = _thrustValue;
            }
            else if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                _thrustersActive = false;
                _thrustMultiplier = 1;
            }

        }

        if (_thrusterValue <= 0)
        {
            _thrusterValue = 0;
            _thrustersActive = false;
            _thrustMultiplier = 1;
            _thrusterEmpty = true;
        }
    }  
    
    IEnumerator ThrustersPowerDown()
    {
        while(true)
        {
          if(_thrustersActive == true)
            {
                _thrusterValue -= Time.deltaTime;
            }

          if(_thrustersActive == false && _thrusterValue < 10)
            {
                _thrusterValue += Time.deltaTime;
                if(_thrusterValue > 10)
                {
                    _thrusterValue = 10;
                    _thrusterEmpty = false;
                }
            }
            _uiManager.UpdateThrusterBar(_thrusterValue);
            yield return null;
        }
    }

    //Player Basic Attack
    void FireLaser()
    {
        _ammo -= 1;
        _uiManager.UpdateAmmo(_ammo);
        _canFire = Time.time + _fireRate;
        if (_isTripleShotActive == true)
        {
            Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
        }
        else if(_isMegaLaserActive == true)
        {
            MegaLaser();
        }
        else if(_isHomingActive == true)
        {
            Instantiate(_homingPrefab, transform.position + new Vector3(0, 0.6f, 0), Quaternion.identity);
        }
        else
        {
            Instantiate(_laserPrefab, transform.position + new Vector3(0, 0.6f, 0), Quaternion.identity);
        }
        _audioSource1.Play();
    }

    //Player Damage
    public void Damage()
    {
        if (_shieldsActive == true && shieldLife < 4)
        {
            shieldLife -= 1;
            _uiManager.UpdateShieldLife(shieldLife);
            if (shieldLife <= 0)
            {
                _shieldsActive = false;
                _shieldVisual.SetActive(false);
            }
            return;
        }
        _lives -= 1;
        _camShake.ShakeCamera();
        if (_lives == 2)
        {
            _playerDamage[0].SetActive(true);
        }
        else if(_lives == 1)
        {
            _playerDamage[1].SetActive(true);
        }
        _uiManager.UpdateLives(_lives);
        if (_lives < 1)
        {
            _spawnManager.OnPlayerDeath();
            Destroy(this.gameObject);
        }
    }

    public void AddLives()
    {
        if (_lives < 3)
        {
            _lives += 1;
            _uiManager.UpdateLives(_lives);
            if (_lives == 3 && _playerDamage[0] == true)
            {
                _playerDamage[0].SetActive(false);
            }
            else if (_lives == 2 && _playerDamage[1] == true)
            {
                _playerDamage[1].SetActive(false);
            }
        }  
    }

    //Triple Shot Powerup
    public void TripleShotActive()
    {
        _isTripleShotActive = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }

    //Mega Laser Powerup
    public void MegaLaser()
    {
        _megaLaserPrefab.SetActive(true);
        _isMegaLaserActive = true;
        StartCoroutine(MegaLaserPowerDown());
    }

    IEnumerator MegaLaserPowerDown()
    {
        while (_isMegaLaserActive == true)
        {
            yield return new WaitForSeconds(8f);
            _isMegaLaserActive = false;
            _megaLaserPrefab.SetActive(false);
        }
    }

    //Powerup Active Time
    IEnumerator TripleShotPowerDownRoutine()
    {
        while (_isTripleShotActive == true)
        {
            yield return new WaitForSeconds(8f);
            _isTripleShotActive = false;
        }
    }

    public void SpeedBoostActive()
    {
        _speedBoostActive = true;
        _boostMultiplier = _boostValue;
        StartCoroutine(SpeedBoostPowerDownRoutine());
    }

    IEnumerator SpeedBoostPowerDownRoutine()
    {
        if (_speedBoostActive == true)
        {
            yield return new WaitForSeconds(5f);
            _speedBoostActive = false;
            _boostMultiplier = 1;
        }
    }

    IEnumerator FireLockPowerdownRoutine()
    {
        if(_fireLockActive == true)
        {
            yield return new WaitForSeconds(5.0f);
            _fireLockActive = false;
        }
    }

    public void ShieldsActive()
    {
        _shieldsActive = true;
        _shieldVisual.SetActive(true);
        shieldLife = 3;
        _uiManager.UpdateShieldLife(shieldLife);
    }

    public void FireLock()
    {
        _fireLockActive = true;
        StartCoroutine(FireLockPowerdownRoutine());
    }

    public void AddScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }

    public void AddAmmo()
    {
        _ammo += 15;
        _uiManager.UpdateAmmo(_ammo);
    }

    public void HomingShotActive()
    {
        _isHomingActive = true;
        StartCoroutine(HomingPowerDownRoutine());
    }

    IEnumerator HomingPowerDownRoutine()
    {
        if (_isHomingActive == true)
        {
            yield return new WaitForSeconds(10f);
            _isHomingActive = false;
        }
    }


}