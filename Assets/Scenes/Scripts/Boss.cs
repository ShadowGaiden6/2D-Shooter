using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField]
    private GameObject _beamPrefab;
    [SerializeField]
    private GameObject _laserPrefab;
    private float _fireRate = 3.0f;
    private float _canFire = -1;
    private GameObject _target;
    [SerializeField]
    private int _life = 100;
    private Player _player;
    [SerializeField]
    private GameObject _damageVisual1;
    [SerializeField]
    private GameObject _damageVisual2;
    [SerializeField]
    private GameObject _damageVisual3;
    [SerializeField]
    private GameObject _explosion1;
    [SerializeField]
    private GameObject _explosion2;
    [SerializeField]
    private GameObject _explosion3;
    [SerializeField]
    private GameObject _explosion4;
    [SerializeField]
    private GameObject _explosion5;
    private float _speed = 0.25f;
    private float _canMegaFire = -1;
    private bool _isBossDead = false;
    private SpawnManager _spawnManager;

    // Start is called before the first frame update
    void Start()
    {
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _target = GameObject.FindGameObjectWithTag("Player");
        _player = GameObject.Find("Player").GetComponent<Player>();
        _canFire = Time.time + 2f;
        _canMegaFire = Time.time + 10f;
        FireLaser();
    }

    // Update is called once per frame
    void Update()
    {
        if (_isBossDead == false)
        {
            CalculateMovement();
            if (_target == null || _player == null)
            {
                Debug.LogError("Player is Dead");
            }
        }
    }

    void CalculateMovement()
    {
        transform.Translate((Vector3.down + _target.transform.position) * _speed * Time.deltaTime);

        if (transform.position.y <= 2.75f)
        {
            transform.position = new Vector3(transform.position.x, 2.75f, 0);
        }

        if (transform.position.x >= 9)
        {
            transform.position = new Vector3(-9, transform.position.y, 0);
        }
        else if (transform.position.x <= -9)
        {
            transform.position = new Vector3(9, transform.position.y, 0);
        }
    }
    void FireLaser()
    {
        
        StartCoroutine(FireLaserRoutine());
    }

    IEnumerator FireLaserRoutine()
    {
        while (_life > 0)
        {
            if (Time.time > _canFire)
            {
                BasicFire1();
                BasicFire2();
                _fireRate = Random.Range(3f, 5f);
                _canFire = Time.time + _fireRate;
            }
            MegaLaserFire();
            yield return null;
        }
    }

    void BasicFire1()
    {
        var offset1 = transform.position + new Vector3(-0.7f, -1f, 0f);
        GameObject enemyLaser = Instantiate(_laserPrefab, offset1, Quaternion.identity);
        Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();
        for (int i = 0; i < lasers.Length; i++)
        {
            lasers[i].AssignEnemyLaser();
        }

    }

    void BasicFire2()
    {
        var offset4 = transform.position + new Vector3(-1.2f, -1f, 0f);
        GameObject enemyLaser = Instantiate(_laserPrefab, offset4, Quaternion.identity);
        Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();
        for (int i = 0; i < lasers.Length; i++)
        {
            lasers[i].AssignEnemyLaser();
        }

    }


    void MegaLaserFire()
    {
        if (Time.time > _canMegaFire)
        {
            var offset = new Vector3(0f, -1f, 0f);
            _fireRate = 10f;
            _canMegaFire = _fireRate + Time.time;
            GameObject enemyBeam = Instantiate(_beamPrefab, transform.position + offset, Quaternion.identity);
            Beam[] beams = enemyBeam.GetComponentsInChildren<Beam>();
            foreach (Beam v in beams)
            {
                v.AssignEnemyBeam();
            }
        }
    }

    void LifeCheck()
    {
        if(_life < 75)
        {
            _damageVisual1.SetActive(true);
        }

        if (_life < 50)
        {
            _damageVisual2.SetActive(true);
        }

        if(_life < 25)
        {
            _damageVisual3.SetActive(true);
        }
        if (_life == 0 && _isBossDead == false)
        {
            _isBossDead = true;
            _damageVisual1.SetActive(false);
            _damageVisual2.SetActive(false);
            _damageVisual3.SetActive(false);
            StartCoroutine(DeathCallRoutine());
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.tag == "Player")
        {
            Destroy(other.gameObject);
            return;
        }
        if (other.tag == "Collectable")
        {
            return;
        }
        if (other.tag == "Laser")
        {
            _life -= 1;
            Destroy(other.gameObject);
            LifeCheck();
            return;
        }

        if (other.tag == "Mega_Laser")
        {
            _life -= 3;
            Destroy(other.gameObject);
            LifeCheck();
            return;
        }
    }
    IEnumerator DeathCallRoutine()
    {
        if(_life == 0 && _isBossDead == true)
        {
            _player.AddScore(250);
            _speed = 0f;
            _explosion1.SetActive(true);
            yield return new WaitForSeconds(1f);
            _explosion2.SetActive(true);
            yield return new WaitForSeconds(1f);
            _explosion3.SetActive(true);
            yield return new WaitForSeconds(1f);
            _explosion4.SetActive(true);
            yield return new WaitForSeconds(1f);
            _explosion5.SetActive(true);
            yield return new WaitForSeconds(1f);
            _spawnManager._bossRemaining -= 1;
            Destroy(this.gameObject);
        }
    }
}