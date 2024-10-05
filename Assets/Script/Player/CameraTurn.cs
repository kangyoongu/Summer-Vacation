using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CameraTurn : MonoBehaviour
{
    public Transform player;
    private float yaw = 0.0f;
    private float pitch = 0.0f;
    public Transform cam;
    public LayerMask layer;
    public Transform gunaim;
    public Slider slider;
    void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        if (!PlayerPrefs.HasKey("Sens"))
        {
            PlayerPrefs.SetFloat("Sens", 4f);
        }
        slider.value = PlayerPrefs.GetFloat("Sens");
    }

    void Update()
    {
        if (GameManager.Instance.isPlay)
        {
            yaw = PlayerPrefs.GetFloat("Sens") * Input.GetAxis("Mouse X");
            pitch += PlayerPrefs.GetFloat("Sens") * Input.GetAxis("Mouse Y");

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
    public void SetSens(float value)
    {
        PlayerPrefs.SetFloat("Sens", value);
    }
}