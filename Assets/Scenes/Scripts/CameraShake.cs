using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private bool _isShaking = false;
    private float _camShakeTime = 0.2f;
    [SerializeField]
    private AnimationCurve cameraShakeCurve;
  IEnumerator ShakeCameraRoutine()
    {
        _isShaking = true;

        Vector3 originalPos = transform.position;

        float timeRunning = 0;

        while(timeRunning < _camShakeTime)
        {
            timeRunning += Time.deltaTime;
            float strength = cameraShakeCurve.Evaluate(timeRunning / _camShakeTime);
            transform.position = originalPos + Random.insideUnitSphere * strength;
            yield return null;
        }
        transform.position = originalPos;
        _isShaking = false;

    }

    public void ShakeCamera()
    {
        StartCoroutine(ShakeCameraRoutine());
        if(_isShaking)
        {
            _isShaking = false;
            StopCoroutine(ShakeCameraRoutine());
        }
    }
}
