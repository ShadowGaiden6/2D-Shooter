using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    private float _speed = 5f;
    private GameObject _target;
    public Vector3 direction;
    private Quaternion rotatetoTarget;
    private float rotationSpeed = 3.0f;
    // Start is called before the first frame update
    void Start()
    {
        
        _target = GameObject.FindGameObjectWithTag("Enemy");

        if (_target == null)
        {
            _target = GameObject.FindGameObjectWithTag("Boss");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_target == null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            CalculateMovement();
        }
    }

    void CalculateMovement()
    {
        direction = (_target.transform.position - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        rotatetoTarget = Quaternion.AngleAxis(angle - 90, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotatetoTarget, Time.deltaTime * rotationSpeed);
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
}
