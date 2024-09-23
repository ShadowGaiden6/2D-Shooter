using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRadar : MonoBehaviour
{
    Enemy enemy; 
    void Start()
    {
        enemy = transform.parent.GetComponent<Enemy>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Collectable" && other.transform == this.transform)
        {
            enemy.PowerupDetection();
        }
    }
}
