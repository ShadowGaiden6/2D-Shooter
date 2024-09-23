using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    private float _speed = 8f;
    public bool _isEnemyLaser = false;
    private bool _backFire = false;


    // Update is called once per frame
    void Update()
    {
        if (_isEnemyLaser == false || _backFire == true)
        {
            MoveUp();
        }
        else if (_isEnemyLaser == true)
        {
            MoveDown();
        }
    }
    void MoveUp()
    {
        transform.Translate(Vector3.up * _speed * Time.deltaTime);

        if (transform.position.y >= 6)
        {
            Destroy(this.gameObject);
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
        }
    }

    void MoveDown()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y <= -6)
        {
            Destroy(this.gameObject);
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
        }
    }

    public void AssignEnemyLaser()
    {
        _isEnemyLaser = true;
    }

    public void BackFireLaser()
    {
        _isEnemyLaser = true;
        _backFire = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player" && _isEnemyLaser == true || _backFire == true)
        {
            Player player = other.GetComponent<Player>();
            if(player != null)
            {
                player.Damage();
            }
        }
        else if (other.tag == "Enemy" && _isEnemyLaser == true)
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.FriendlyFire();
            }
        }

        if (other.tag == "Collectable" && _isEnemyLaser == true)
        {
            Powerup powerup = other.transform.GetComponent<Powerup>();
            if(powerup != null)
            {
                Destroy(other.gameObject);
            }
        }
    }
}
