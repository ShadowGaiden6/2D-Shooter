using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MegaLaser : MonoBehaviour
{
    private float _speed = 3f;
    private GameObject _target;
    // Start is called before the first frame update
 

    // Update is called once per frame
    void Update()
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

        if (transform.position.y <= -4.5f)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();
            if(player != null)
            {
                player.MegaLaser();
            }
            Destroy(this.gameObject);
        }

    }
}
