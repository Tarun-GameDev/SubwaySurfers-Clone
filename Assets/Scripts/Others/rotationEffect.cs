using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotationEffect : MonoBehaviour
{

    [SerializeField]
    float durationTime;
    [SerializeField]
    Vector3 rotationatlAxis;
    [SerializeField]
    float smooth;

    [SerializeField]
    float height = 0.5f;
    [SerializeField]
    float moveSpeed = 1f;

    private void Update()
    {
        smooth = Time.deltaTime * durationTime * 200f;
        transform.Rotate(rotationatlAxis * smooth);

        Vector3 pos = transform.localPosition;
        float newY = Mathf.Sin(Time.time * moveSpeed);
        transform.localPosition = new Vector3(pos.x, (newY * height) + height, pos.z);
    }
}
