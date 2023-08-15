using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
using Cinemachine;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [HideInInspector] public bool isPlay = false;
    public TextMeshProUGUI bestText;
    public RectTransform[] uis;
    public float[] Out;
    public CinemachineVirtualCamera mainCam;
    AudioSource aud;
    private void Awake()
    {
        if (Instance == null) Instance = this;
        aud = GetComponent<AudioSource>();
        Application.targetFrameRate = 130;
    }
    private void Start()
    {
        if (!PlayerPrefs.HasKey("Best")) PlayerPrefs.SetFloat("Best", 0);
        bestText.text = PlayerPrefs.GetFloat("Best").ToString("0.00m");
    }
    public void OnClickStart()
    {
        isPlay = true;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        aud.Stop();
        for (int i = 1; i < uis.Length-1; i++)
        {
            uis[i].DOAnchorPosY(Out[i - 1], 0.8f);
        }
        uis[5].DOAnchorPosX(-2246, 0.8f);
        uis[0].gameObject.SetActive(true);
        mainCam.Priority = 5;
    }
}
