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
    float distance;
    float nearestDistance = 1000000f;
    public GameObject[] AllObjects;
    public GameObject NearestObject;
    // Start is called before the first frame update
    void Start()
    {
        AllObjects = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i = 0; i < AllObjects.Length; i++)
        {
            distance = Vector3.Distance(this.transform.position, AllObjects[i].transform.position);
            if (distance < nearestDistance)
            {
                NearestObject = AllObjects[i];
                nearestDistance = distance;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
    }

    void CalculateMovement()
    {
        _target = NearestObject;
        if (_target != null)
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
        else
        {
            CalculateDistance();
        }
    }

    void CalculateDistance()
    {
        AllObjects = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i = 0; i < AllObjects.Length; i++)
        {
            distance = Vector3.Distance(this.transform.position, AllObjects[i].transform.position);
            if (distance < nearestDistance)
            {
                NearestObject = AllObjects[i];
                nearestDistance = distance;
            }
        }
    }
}
