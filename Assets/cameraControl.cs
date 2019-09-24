using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraControl : MonoBehaviour
{
    [Range(0.0f, 10.0f)]
    public float verticalSpeed;
    public Transform player;
    Vector3 displacement;

    void Start()
    {
        displacement = transform.position - player.position;
    }

    void Update()
    {
        transform.position = player.position + displacement;

        float v = verticalSpeed * Input.GetAxis("Mouse Y");
        transform.Rotate(v, 0, 0);

        if (Mathf.Abs(transform.rotation.eulerAngles.y - player.rotation.eulerAngles.y) > 90)
        {
            transform.Rotate(-v, 0, 0);
        }

        transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.eulerAngles.x, player.rotation.eulerAngles.y, transform.rotation.eulerAngles.z));
    }
}
