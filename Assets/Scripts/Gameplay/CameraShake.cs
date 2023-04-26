using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public Transform transform;
    public float shakeDuration;
    public AnimationCurve curve;

    // easy way to test shake in editor
    [SerializeField] bool start = false;

    IEnumerator Shaking()
    {
        Vector3 startPosition = transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < shakeDuration)
        {
            elapsedTime += Time.deltaTime;
            float strength = curve.Evaluate(elapsedTime / shakeDuration);
            startPosition = transform.position;
            transform.position = startPosition + Random.insideUnitSphere * strength;
            
            yield return null;
        }

        transform.position = startPosition;
    }

    void Awake()
    {
        if (transform == null)
        {
            transform = GetComponent(typeof(Transform)) as Transform;
        }
    }


    void Update()
    {
        if (start)
        {
            start = false;
            StartCoroutine(Shaking());
        }

    }

    public void Shake()
    {
        start = false;
        StartCoroutine(Shaking());
    }

    public void Shake(float shakeDuration)
    {
        this.shakeDuration = shakeDuration;
        start = false;
        StartCoroutine(Shaking());
    }
}
