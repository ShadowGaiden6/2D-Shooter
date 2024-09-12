using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beam : MonoBehaviour
{
    private bool _isEnemyBeam = false;
    private float _speed = 15f;
    public void Update()
    {
        MoveDown();
        StartCoroutine(BeamPowerDownRoutine());
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
    public void AssignEnemyBeam()
    {
        _isEnemyBeam = true;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && _isEnemyBeam == true)
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                player.Damage();
            }
        }
    }
    IEnumerator BeamPowerDownRoutine()
    {
        yield return new WaitForSeconds(10f);
        Destroy(this.gameObject);
    }
}
