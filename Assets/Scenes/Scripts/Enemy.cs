using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 1f;
    private Player _player;
    private Animator _animator;
    private AudioSource _audioSource;
    [SerializeField]
    private GameObject _laserPrefab;
    private float _fireRate = 3.0f;
    private float _canFire = -1;
    [SerializeField]
    private int _drifterChance; //drifters that seek the player
    [SerializeField]
    public int _floaterChance; //floaters that fire lasers from the top of the screen
    [SerializeField]
    private int _shieldChance;
    [SerializeField]
    private int _ramChance;
    private GameObject _target1;
    private SpawnManager _spawnManager;
    public int _rng;
    [SerializeField]
    private GameObject _beamPrefab;
    [SerializeField]
    private GameObject _shieldVisual;
    private bool _shieldsActive = false;
    public int shieldLife = 1;
    private int _backfireChance;
    private bool _isBelowPlayer = false;
    private int _avoidShot;
    private float _laserDistance;
    private float _detectionRange = 3.0f;
    private Vector3 _avoidDirection;

    private void Awake()
    {
        _drifterChance = Random.Range(0, 1);
        _shieldChance = Random.Range(2, 3);
        _floaterChance = Random.Range(4, 5);
        _ramChance = Random.Range(6, 7);
        _backfireChance = Random.Range(8, 9);
        _avoidShot = Random.Range(10, 11);
        _rng = Random.Range(0, 11);
        if (_rng == _drifterChance || _rng == _floaterChance || _rng == _ramChance || _rng == _backfireChance)
        {
            _target1 = GameObject.FindGameObjectWithTag("Player");
        }
        else
        {
            _target1 = null;
        }
    }

    private void Start()
    {
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _player = GameObject.Find("Player").GetComponent<Player>();
        _audioSource = GetComponent<AudioSource>();
        if (_player == null)
        {
            Debug.LogError("The Player is NULL");
        }
        _animator = GetComponent<Animator>();
        if (_animator == null)
        {
            Debug.LogError("Animator is NULL");
        }
    }

    void Update()
    {
        CalculateMovement();
        StartCoroutine(FloaterLaserRoutine());
        ShieldsActive();
        AvoidShot();
    }

    public void ShieldsActive()
    {
        if (_shieldChance == _rng)
        {
            _shieldsActive = true;
            _shieldVisual.SetActive(true);
        }

        if (shieldLife == 0)
        {
            _shieldsActive = false;
            _shieldVisual.SetActive(false);
        }
    }

    void CalculateMovement()
    {
        //Basic movement logics
        if (_target1 != null && _rng == _drifterChance) //Drifter Enemy type movement
        {
            transform.Translate(_target1.transform.position * _speed * Time.deltaTime);
        }
        else if (_target1 != null && _rng == _floaterChance) //Floater Enemy type movement
        {
            transform.Translate((Vector3.down + _target1.transform.position) * 0.5f * Time.deltaTime);
        }
        else //Basic Enemy type movement
        {
            transform.Translate(Vector3.down * _speed * Time.deltaTime);
        }
        //Y axis wrap around for the enemies
        if (transform.position.y <= -6)
        {
            _isBelowPlayer = false;
            transform.position = new Vector3(Random.Range(-8f, 8f), 7, 0);
        }
        else if (transform.position.y <= 3.75f && _rng == _floaterChance)
        {
            transform.position = new Vector3(transform.position.x, 3.75f, 0);
        }

        //X axis wrap around for the enemies
        if (transform.position.x >= 9)
        {
            transform.position = new Vector3(-9, transform.position.y, 0);
        }
        else if (transform.position.x <= -9)
        {
            transform.position = new Vector3(9, transform.position.y, 0);
        }

        if (_target1 != null && _rng == _ramChance)
        {
            var _distance = Vector3.Distance(transform.position, _target1.transform.position);
            var _step = _speed * Time.deltaTime;
            if (_distance < 3)
            {
                _speed = 3f;
                transform.position = Vector3.MoveTowards(transform.position, _target1.transform.position, _step);
            }
        }
    }

    void AvoidShot()
    {
        if (_rng == _avoidShot)
        {
            GameObject _laser = GameObject.Find("Laser(Clone)");
            if (_laser != null)
            {
                _laserDistance = Vector3.Distance(_laser.transform.position, this.transform.position);

                if (_laserDistance <= _detectionRange)
                {
                    _avoidDirection = this.transform.position - _laser.transform.position;
                    _avoidDirection = _avoidDirection.normalized;
                    this.transform.position += _avoidDirection * Time.deltaTime * (_speed * 4);
                }
            }
        }
    }

    IEnumerator FloaterLaserRoutine()
    {
        if (_rng == _floaterChance)
        {
            yield return new WaitForSeconds(10f);
            FireLaser();
        }
        else
        {
            FireLaser();
        }
    }

    void FireLaser()
    {
        if (Time.time > _canFire && _rng == _floaterChance)
        {
            var offset = new Vector3(0, 2.5f, 0f);
            _fireRate = 15f;
            _canFire = _fireRate + Time.time;
            GameObject enemyBeam = Instantiate(_beamPrefab, transform.position + offset, Quaternion.identity);
            Beam[] beams = enemyBeam.GetComponentsInChildren<Beam>();
            foreach (Beam v in beams)
            {
                v.AssignEnemyBeam();
            }
        }
        else if (_rng == _backfireChance)
        {
            var _distance = Vector3.Distance(transform.position, _target1.transform.position);
            var _offset = transform.position + new Vector3(0, 3f, 0);
            if (_distance < 3 && transform.position.y < _target1.transform.position.y)
            {
                BackfireCheck();
                if (Time.time > _canFire)
                {
                    Debug.Log("Backfire");
                    _fireRate = 3f;
                    _canFire = _fireRate + Time.time;
                    GameObject enemyLaser = Instantiate(_laserPrefab, _offset, Quaternion.identity);
                    Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();
                    for (int i = 0; i < lasers.Length; i++)
                    {
                        lasers[i].BackFireLaser();
                    }
                }
            }
        }
        else if (Time.time > _canFire)
        {
            _fireRate = Random.Range(3f, 7f);
            _canFire = Time.time + _fireRate;
            GameObject enemyLaser = Instantiate(_laserPrefab, transform.position, Quaternion.identity);
            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();
            for (int i = 0; i < lasers.Length; i++)
            {
                lasers[i].AssignEnemyLaser();
            }
        }
    }

    private void BackfireCheck()
    {
        if (_isBelowPlayer == false)
        {
            _canFire = Time.time + 0.5f;
            _isBelowPlayer = true;
        }
    }

    public void PowerupDetection()
    {
        GameObject enemyLaser = Instantiate(_laserPrefab, transform.position, Quaternion.identity);
        Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();
        for (int i = 0; i < lasers.Length; i++)
        {
            lasers[i].AssignEnemyLaser();
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && _shieldsActive == true)
        {
            shieldLife -= 1;
            return;
        }
        else if (other.tag == "Player" && _shieldsActive == false)
        {
            Player player = other.transform.GetComponent<Player>();
            if (player != null)
            {
                player.Damage();
                if (_rng == _ramChance)
                {
                    _spawnManager._enemiesRemaining--;
                    _animator.SetTrigger("OnEnemyDeath");
                    _speed = 0;
                    _audioSource.Play();
                    Destroy(this.gameObject, 1.5f);
                }
                return;
            }
            DeathCall();
        }

        if (other.tag == "Laser" && _shieldsActive == true)
        {
            shieldLife -= 1;
            Destroy(other.gameObject);
            _shieldsActive = false;
            return;
        }
        else if (other.tag == "Laser" && _shieldsActive == false)
        {
            Destroy(other.gameObject);
            if (_player != null)
            {
                _player.AddScore(10);
            }
            Destroy(GetComponent<Collider2D>());
            DeathCall();
        }

        if (other.tag == "Mega_Laser" && _shieldsActive == true)
        {
            shieldLife -= 1;
            return;
        }
        else if (other.tag == "Mega_Laser" && _shieldsActive == false)
        {
            if (_player != null)
            {
                _player.AddScore(10);
            }
            DeathCall();
        }
    }
    private void DeathCall()
    {
        transform.tag = "";
        _spawnManager._enemiesRemaining--;
        _animator.SetTrigger("OnEnemyDeath");
        _speed = 0;
        _audioSource.Play();
        Destroy(this.gameObject, 1.5f);
    }
}