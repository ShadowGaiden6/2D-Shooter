using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    private AudioSource _audioSource;

    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        Destroy(this.gameObject, 3f);
        _audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
