using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NegativePowerUp : MonoBehaviour
{
    private GameObject _target;
    private float _speed = 0.85f;
    // Update is called once per frame
    private void Awake()
    {
        _target = GameObject.FindGameObjectWithTag("Player");

    }

    void Update()
    {
        if (_target == null)
        {
            Debug.LogError("The Player Is Dead");
        }
        transform.Translate(_target.transform.position * _speed * Time.deltaTime);
        if (transform.position.y < -4.5f)
        {
            Destroy(this.gameObject);
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
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();
            if(player != null)
            {
                player.FireLock();
            }
            else
            {
                Debug.LogError("The Spawn Manager is NULL");
            }
            Destroy(this.gameObject);
        }
    }
}
