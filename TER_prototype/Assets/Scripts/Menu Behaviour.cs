using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class NewBehaviourScript : MonoBehaviour
{
    public Image image;
    public TMP_Dropdown colorDropdown;
    public TMP_Dropdown partDropdown;
    public GameObject PaintButton;
    
    private GameObject scripts;
    private ColorHandler _colorHandler;
    private MainObjectBehaviour _mainObjectBehaviour;

    private void Start()
    {
        image = GameObject.Find("Canvas").GetComponent<Image>();
        scripts = GameObject.Find("Scripts");
        _colorHandler = scripts.GetComponent<ColorHandler>();
        _mainObjectBehaviour = scripts.GetComponent<MainObjectBehaviour>();
    }

    public void PopulateColorDropdown()
    {
        colorDropdown.ClearOptions();

        switch (partDropdown.captionText.text.ToLower())
        {
            case "head":
                colorDropdown.AddOptions(_colorHandler.GetCurrentColors(_mainObjectBehaviour.GetInstancedRenderers("head")));
                break;
            case "torso":
                colorDropdown.AddOptions(_colorHandler.GetCurrentColors(_mainObjectBehaviour.GetInstancedRenderers("torso")));
                break;
            case "left arm":
                colorDropdown.AddOptions(_colorHandler.GetCurrentColors(_mainObjectBehaviour.GetInstancedRenderers("L_arm")));
                break;
            case "right arm":
                colorDropdown.AddOptions(_colorHandler.GetCurrentColors(_mainObjectBehaviour.GetInstancedRenderers("R_arm")));
                break;
            case "waist":
                colorDropdown.AddOptions(_colorHandler.GetCurrentColors(_mainObjectBehaviour.GetInstancedRenderers("waist")));
                break;
            case "left leg":
                colorDropdown.AddOptions(_colorHandler.GetCurrentColors(_mainObjectBehaviour.GetInstancedRenderers("L_leg")));
                break;
            case "right leg":
                colorDropdown.AddOptions(_colorHandler.GetCurrentColors(_mainObjectBehaviour.GetInstancedRenderers("R_leg")));
                break;
            case "global":
                colorDropdown.AddOptions(_colorHandler.GetCurrentColors(_mainObjectBehaviour.GetInstancedRenderers("global")));
                break;
        }
    }

    public void ChangeColorConfirmPressed()
    {
        _mainObjectBehaviour.ChangePartColor(partDropdown.captionText.text.ToLower(), colorDropdown.captionText.text.ToLower());
        PaintButton.SetActive(true);
    }

    public string GetColorSelected()
    {
        return colorDropdown.captionText.text;
    }
    
    public string GetPartSelected()
    {
        return partDropdown.captionText.text;
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
        if (Mathf.Approximately(tempColor.a, 1f))
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