using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class TutorialManager : MonoBehaviour
{
    public GameObject[] image;
    public RectTransform border;
    public float[] updown;
    int index = 0;
    public void Up()
    {
        image[index].SetActive(true);
        image[7].SetActive(false);
        border.DOAnchorPosX(updown[0], 0.7f);
        index = 0;
    }
    private void Update()
    {
        if (GameManager.Instance.isPlay == true)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
            if (Cursor.visible == true)
            {
                if (Input.GetMouseButtonUp(0))
                {
                    Cursor.visible = false;
                    Cursor.lockState = CursorLockMode.Locked;
                }
            }
        }
    }
    public void OnClickNext()
    {
        if (index <= 6)
        {
            image[index].SetActive(false);
            index++;
            image[index].SetActive(true);
        }
        else
        {
            border.DOAnchorPosX(updown[1], 0.7f);
            index = 0;
        }
    }
    public void OnClickOut()
    {
        print("asfdasf");
        Application.Quit();
    }
}
