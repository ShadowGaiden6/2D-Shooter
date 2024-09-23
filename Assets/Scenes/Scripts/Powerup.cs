using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3f;
    [SerializeField] //0 = Triple Shot, 1 = Speed, 2 = Shields, 3 = Homing Missiles
    private int powerupID;
    [SerializeField]
    private AudioClip _clip;
    private GameObject _target;
    Player player;
    //Powerup movement
    void Update()
    {
        CalculateMovement();
    }

    void CalculateMovement()
    {
        _target = GameObject.FindGameObjectWithTag("Player");
        if (Input.GetKey(KeyCode.C))
        {
            transform.position = Vector3.MoveTowards(transform.position, _target.transform.position, 0.1f);
        }
        else
        {
            transform.Translate(Vector3.down * _speed * Time.deltaTime);
        }

        if (transform.position.y < -4.5f)
        {
            Destroy(this.gameObject);
        }
    }
    //Collection of powerup
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            player = other.transform.GetComponent<Player>();
            AudioSource.PlayClipAtPoint(_clip, transform.position);
            if (player != null)
            {
                switch (powerupID)
                {
                    case 0:
                        player.TripleShotActive();

                        break;
                    case 1:
                        player.SpeedBoostActive();

                        break;
                    case 2:
                        player.ShieldsActive();

                        break;
                    case 3:
                        player.HomingShotActive();

                        break;
                    default:
                        Debug.Log("Default Vaule");

                        break;
                }   
            }
            Destroy(this.gameObject);
        }
        
        if (other.tag == "Enemy") //Makes it so nothing happens when the enemies collide with the powerups//
        {
            Enemy enemy = other.transform.GetComponent<Enemy>();
            if (enemy != null)
            {
                return;
            }
        }
    }
}
