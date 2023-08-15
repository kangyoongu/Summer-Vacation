using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CameraTurn : MonoBehaviour
{
    public float camSpeed = 9.0f;
    public Transform player;
    private float yaw = 0.0f;
    private float pitch = 0.0f;
    public Transform cam;
    public LayerMask layer;
    public Transform gunaim;
    void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    void Update()
    {
        if (GameManager.Instance.isPlay)
        {
            yaw = camSpeed * Input.GetAxis("Mouse X");
            pitch += camSpeed * Input.GetAxis("Mouse Y");

            pitch = Mathf.Clamp(pitch, -70f, 90f);

            transform.localEulerAngles = new Vector3(-pitch, 0, 0);
            player.Rotate(Vector3.up * yaw);
            cam.position = transform.position;
            cam.eulerAngles = new Vector3(-pitch, player.eulerAngles.y, 0);
            RaycastHit hit;
            if (Physics.Raycast(cam.position, cam.forward, out hit, 500, layerMask: layer))
            {
                gunaim.position = hit.point;
            }
            else
            {
                gunaim.position = transform.forward * 500;
            }
        }
    }
}