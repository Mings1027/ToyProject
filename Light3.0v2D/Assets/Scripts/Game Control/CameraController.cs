using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;

    [SerializeField] Transform target;
    [SerializeField] Vector3 offset;
    [SerializeField][Range(1, 10)] float smoothness;

    private float shakeTime, shakePower, shakeFadeTime, shakeRotation;
    private float rotationMultiplier;

    private void Awake()
    {
        instance = this;
    }
    private void FixedUpdate()
    {
        Follow();
    }
    private void Follow()
    {
        Vector3 targetPosition = target.position + offset;
        Vector3 smoothPosition = Vector3.Lerp(transform.position, targetPosition, smoothness * Time.fixedDeltaTime);
        transform.position = smoothPosition;
    }
    private void LateUpdate()
    {
        if (shakeTime > 0)
        {
            shakeTime -= Time.deltaTime;

            float x = Random.Range(-1f, 1f) * shakePower;
            float y = Random.Range(-1f, 1f) * shakePower;
            transform.position += new Vector3(x, y, 0);

            shakePower = Mathf.MoveTowards(shakePower, 0, shakeFadeTime * Time.deltaTime);
            shakeRotation = Mathf.MoveTowards(shakeRotation, 0, shakeFadeTime * rotationMultiplier * Time.deltaTime);
        }
        transform.rotation = Quaternion.Euler(0, 0, shakeRotation * Random.Range(-1f, 1f));
    }

    public void StartShake(float length, float power)
    {
        shakeTime = length;
        shakePower = power;

        shakeFadeTime = power / length;
        shakeRotation = power * rotationMultiplier;
    }




}
