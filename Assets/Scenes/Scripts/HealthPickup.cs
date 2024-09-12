using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    private float _speed = 3f;
    [SerializeField]
    private AudioClip _clip;
    private GameObject _target;

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

        if (transform.position.y < -4.5f)
        {
            Destroy(this.gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();
            AudioSource.PlayClipAtPoint(_clip, transform.position);
            if (player != null)
            {
                player.AddLives();
            }
            Destroy(this.gameObject);
        }
    }
}
