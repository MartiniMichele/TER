using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NewBehaviourScript : MonoBehaviour
{
    public Image image;

    private void Start()
    {
        image = GameObject.Find("Canvas").GetComponent<Image>();
    }
    public void LoadARScene()
    {
        SceneManager.LoadScene("AR_Scene");
    }

    public void ModelSelected(int i)
    {
    }

    public void LoadMenuScene()
    {
        SceneManager.LoadScene("Menu");
    }

    public void changeAlpha()
    {
        var tempColor = image.color;
        if (tempColor.a == 1f)
        {
            tempColor.a = 0f;
            image.color = tempColor;
        }
        else
        {
            tempColor.a = 1f;
            image.color = tempColor;
        }
    }
}
