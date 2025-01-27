using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class ColorHandler : MonoBehaviour
{
    [SerializeField]
    private FlexibleColorPicker colorPicker;
    
    public Color Color { get; set; }
    
    // default colors
    public readonly Color GundamBlue = new Color(0.207f, 0.191f, 0.535f, 1f);
    public readonly Color _gundamWhite = new Color(0.749f, 0.749f, 0.749f, 1f); 
    public readonly Color _gundamRed = new Color(0.406f, 0.047f, 0f, 1f); 
    public readonly Color _gundamOrange = new Color(0.786f, 0.483f, 0.193f, 1f); 
    public readonly Color _exiaMainBlue = new Color(0.207f, 0.191f, 0.535f, 1f); 
    public readonly Color _exiaDetailBlue = new Color(0f, 0.151f, 0.680f, 1f); 
    public readonly Color _exiaGreen = new Color(0.028f, 0.240f, 0.054f, 1f); 
    public readonly Color _exiaWhite = new Color(0.906f, 0.906f, 0.906f, 1f); 
    public readonly Color _exiaRed = new Color(0.693f, 0.082f, 0.117f, 1f);
    
    // shared materials
    public Material blueMaterial;
    public Material whiteMaterial;
    public Material redMaterial;
    public Material orangeMaterial;
    public Material exiaMainBlueNameMaterial;
    public Material exiaDetailBlueNameMaterial;
    public Material exiaGreenMaterial;
    public Material exiaRedMaterial;
    public Material exiaWhiteMaterial;
    
    
    // materials readonly names
    public readonly string _gundamBlueName = "BLUE.002";
    public readonly string _gundamWhiteName = "Material.013";
    public readonly string _gundamRedName = "Material.022";
    public readonly string _gundamOrangeName = "Material.020";
    public readonly string _exiaMainBlueName = "BLUE";
    public readonly string _exiaDetailBlueName = "Plavsky crystal";
    public readonly string _exiaGreenName = "GN composite";
    public readonly string _exiaWhiteName = "BOOSTER";
    public readonly string _exiaRedName = "red";
        
    // materials constant names
    private const string _gundamBlueName_const = "BLUE.002";
    private const string _gundamWhiteName_const = "Material.013";
    private const string _gundamRedName_const = "Material.022";
    private const string _gundamOrangeName_const = "Material.020";
    private const string _exiaMainBlueName_const = "BLUE";
    private const string _exiaDetailBlueName_const = "Plavsky crystal";
    private const string _exiaGreenName_const = "GN composite";
    private const string _exiaWhiteName_const = "BOOSTER";
    private const string _exiaRedName_const = "red";
    


    
    // Start is called before the first frame update
    private void Awake()
    {
        Debug.Log("awake called:", gameObject);
        colorPicker.onColorChange.AddListener(colorPicker_OnChanged);
    }

    private void colorPicker_OnChanged(Color chosenColor)
    {
        Color = chosenColor;
    }

    public string ColorNameToMaterialName(string colorName)
    {
        return colorName switch
        {
            "blue" => _gundamBlueName,
            "white" => _gundamWhiteName,
            "red" => _gundamRedName,
            "orange" => _gundamOrangeName,
            "exia_main_blue" => _exiaMainBlueName,
            "exia_detail_blue" => _exiaDetailBlueName,
            "exia_white" => _exiaWhiteName,
            "exia_red" => _exiaRedName,
            "exia_green" => _exiaGreenName,
            _ => null
        };
    }

    private string MaterialNameToColorName(string colorName)
    {
        return colorName switch
        {
            _gundamBlueName_const => "Blue",
            _gundamWhiteName_const => "White",
            _gundamRedName_const => "Red",
            _gundamOrangeName_const => "Orange",
            _exiaMainBlueName_const => "Exia Main Blue",
            _exiaDetailBlueName_const => "Exia Detail Blue",
            _exiaWhiteName_const => "Exia White",
            _exiaRedName_const => "Exia Red",
            _exiaGreenName_const => "Exia Green",
            _ => null
        };
    }
    
    public Color ColorNameToRGB(string colorName)
    {
        return colorName switch
        {
            "blue" => GundamBlue,
            "white" => _gundamWhite,
            "red" => _gundamRed,
            "orange" => _gundamOrange,
            "exia_main_blue" => _exiaMainBlue,
            "exia_detail_blue" => _exiaDetailBlue,
            "exia_white" => _exiaWhite,
            "exia_red" => _exiaRed,
            "exia_green" => _exiaGreen,
            _ => Color.magenta
        };
    }
    
    public Material ColorNameToMaterial(string colorName)
    {
        return colorName switch
        {
            "blue" => blueMaterial,
            "white" => whiteMaterial,
            "red" => redMaterial,
            "orange" => orangeMaterial,
            "exia main blue" => exiaMainBlueNameMaterial,
            "exia detail blue" => exiaDetailBlueNameMaterial,
            "exia white" => exiaWhiteMaterial,
            "exia red" => exiaRedMaterial,
            "exia green" => exiaGreenMaterial,
        };
    }

    public List<string> GetCurrentColors(Renderer[] renderers)
    {
        HashSet<string> colors = new HashSet<string>();
        foreach (Renderer element in renderers)
        {
            foreach (var material in element.sharedMaterials)
            {
                colors.Add(MaterialNameToColorName(material.name));
            }
            
        }
        colors.RemoveWhere(entry => entry == null);
        return colors.ToList();
    }
}
