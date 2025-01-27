using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class MainObjectBehaviour : MonoBehaviour
{
    private static readonly int Color1 = Shader.PropertyToID("_Color");

    //support variables
    int selected_torso_model = 0;
    int selected_waist_model = 0;
    const string HEAD_ANCHOR = "Head_Anchor";
    const string RIGHT_ANCHOR = "R_Anchor";
    const string LEFT_ANCHOR = "L_Anchor";
    const string WAIST_ANCHOR = "Waist_Anchor";
    const string COLOR_TAG = "Has_Color";

    /*
    // colors
    private Color _orange = new Color(1f, 0.5f, 0.3f);
    private Color _purple = new Color(0.4f, 0.3f, 0.6f);
    private Color _black = new Color(0f, 0f, 0f);

    // default colors
    private Color _gundamBlue = new Color(0.207f, 0.191f, 0.535f, 1f);
    private Color _gundamWhite = new Color(0.749f, 0.749f, 0.749f, 1f);
    private Color _gundamRed = new Color(0.406f, 0.047f, 0f, 1f);
    private Color _gundamOrange = new Color(0.786f, 0.483f, 0.193f, 1f);
    private Color _exiaMainBlue = new Color(0.207f, 0.191f, 0.535f, 1f);
    private Color _exiaDetailBlue = new Color(0f, 0.151f, 0.680f, 1f);
    private Color _exiaGreen = new Color(0.028f, 0.240f, 0.054f, 1f);
    private Color _exiaWhite = new Color(0.906f, 0.906f, 0.906f, 1f);
    private Color _exiaRed = new Color(0.693f, 0.082f, 0.117f, 1f);


    // materials name
    private readonly string _gundamBlueName = "BLUE.002";
    private readonly string _gundamWhiteName = "Material.013";
    private readonly string _gundamRedName = "Material.022";
    private readonly string _gundamOrangeName = "Material.020";
    private readonly string _exiaMainBlueName = "BLUE";
    private readonly string _exiaDetailBlueName = "Plavsky crystal";
    private readonly string _exiaGreenName = "GN composite";
    private readonly string _exiaWhiteName = "BOOSTER";
    private readonly string _exiaRedName = "red";
    */

    private ColorHandler _colorHandler;
    private NewBehaviourScript _menuHandler;

    // custom scales for the models
    const float G_G_HEAD_SCALE = 90f;
    const float G_E_UPPER_SCALE = 0.3f;
    const float G_E_WAIST_SCALE = 0.18f;
    const float G_E_LEGS_SCALE = 1.55f;
    const float G_E_LENGHT_SCALE = 1.7f;

    //legs scale based on waist
    const float GWAIST_E_LEGS_SCALE = 0.31f;
    const float GWAIST_G_LEGS_SCALE = 1f;
    const float EWAIST_G_LEGS_SCALE = 5f;
    const float EWAIST_E_LEGS_SCALE = 1.55f;

    const float E_E_SCALE = 1.5f;
    const float E_G_BODY_SCALE = 5f;
    const float E_G_HEAD_SCALE = 500f;
    const float E_G_LEGS_SCALE = 1f;

    // materials
    private Material _redMetallicMaterial;
    private Material _tealMaterial;
    private Material _blueMaterial;
    private Material _whiteMaterial;
    private Material _redMaterial;
    private Material _orangeMaterial;
    private Material _exiaMainBlueNameMaterial;
    private Material _exiaDetailBlueNameMaterial;
    private Material _exiaGreenMaterial;
    private Material _exiaRedMaterial;
    private Material _exiaWhiteMaterial;

    // gameobjects and prefabs
    private GameObject imageTarget;
    private GameObject head_cube_prefab;
    private GameObject head_sphere_prefab;
    private GameObject torso_cube_prefab;
    private GameObject arm_cube_prefab;
    private GameObject arm_sphere_prefab;
    private GameObject waist_cube_prefab;
    private GameObject leg_cube_prefab;
    private GameObject leg_sphere_prefab;

    // EXIA prefabs
    private GameObject head_exia_prefab;
    private GameObject torso_exia_prefab;
    private GameObject R_arm_exia_prefab;
    private GameObject L_arm_exia_prefab;
    private GameObject waist_exia_prefab;
    private GameObject R_leg_exia_prefab;
    private GameObject L_leg_exia_prefab;

    // GRAMPS prefabs
    private GameObject head_gramps_prefab;
    private GameObject torso_gramps_prefab;
    private GameObject arm_gramps_prefab;
    private GameObject waist_gramps_prefab;
    private GameObject leg_gramps_prefab;

    // anchors
    [SerializeField] private GameObject anchor_head,
        anchor_torso,
        anchor_waist,
        anchor_R_arm,
        anchor_L_arm,
        anchor_R_leg,
        anchor_L_leg;

    private GameObject instantiated_head;
    private GameObject instantiated_torso;
    private GameObject instantiated_waist;
    private GameObject instantiated_R_arm;
    private GameObject instantiated_L_arm;
    private GameObject instantiated_R_leg;
    private GameObject instantiated_L_leg;

    private GameObject scripts;

    private bool roundHead;
    private bool roundUpper;
    private bool roundArms;
    private bool roundLegs;


    private void Start()
    {
        scripts = GameObject.Find("Scripts");
        _colorHandler = scripts.GetComponent<ColorHandler>();
        _menuHandler = scripts.GetComponent<NewBehaviourScript>();
        LoadPrefabs();
        FindAnchors();
        FindStartingGOs();
        LoadMaterials();
        roundHead = false;
        roundUpper = false;
        roundArms = false;
        roundLegs = false;
    }

    private void LoadPrefabs()
    {
        imageTarget = Resources.Load("Sphere_leg") as GameObject;
        torso_cube_prefab = Resources.Load("Cube_torso") as GameObject;
        waist_cube_prefab = Resources.Load("Cube_waist") as GameObject;
        head_cube_prefab = Resources.Load("Cube_head") as GameObject;
        arm_cube_prefab = Resources.Load("Cube_arm") as GameObject;
        leg_cube_prefab = Resources.Load("Cube_leg") as GameObject;
        head_sphere_prefab = Resources.Load("Sphere_head") as GameObject;
        arm_sphere_prefab = Resources.Load("Sphere_arm") as GameObject;
        leg_sphere_prefab = Resources.Load("Sphere_leg") as GameObject;

        // EXIA prefabs
        head_exia_prefab = Resources.Load("Exia_prefab/Exia_head") as GameObject;
        torso_exia_prefab = Resources.Load("Exia_prefab/Exia_torso") as GameObject;
        R_arm_exia_prefab = Resources.Load("Exia_prefab/Exia_right_arm") as GameObject;
        L_arm_exia_prefab = Resources.Load("Exia_prefab/Exia_left_arm") as GameObject;
        waist_exia_prefab = Resources.Load("Exia_prefab/Exia_waist") as GameObject;
        R_leg_exia_prefab = Resources.Load("Exia_prefab/Exia_right_leg") as GameObject;
        L_leg_exia_prefab = Resources.Load("Exia_prefab/Exia_left_leg") as GameObject;

        // GRAMPS prefabs
        head_gramps_prefab = Resources.Load("Gramps_prefab/Gramps_head") as GameObject;
        torso_gramps_prefab = Resources.Load("Gramps_prefab/Gramps_torso") as GameObject;
        arm_gramps_prefab = Resources.Load("Gramps_prefab/Gramps_arm") as GameObject;
        waist_gramps_prefab = Resources.Load("Gramps_prefab/Gramps_waist") as GameObject;
        leg_gramps_prefab = Resources.Load("Gramps_prefab/Gramps_leg") as GameObject;
    }

    private void LoadMaterials()
    {
        _redMetallicMaterial = Resources.Load("Materials/Red") as Material;
        _tealMaterial = Resources.Load("Materials/Teal") as Material;

        // gundam materials
        _colorHandler.blueMaterial =
            FindMaterialFromPrefabRecursive(torso_gramps_prefab, COLOR_TAG, _colorHandler._gundamBlueName);
        _colorHandler.redMaterial =
            FindMaterialFromPrefabRecursive(torso_gramps_prefab, COLOR_TAG, _colorHandler._gundamRedName);
        _colorHandler.whiteMaterial =
            FindMaterialFromPrefabRecursive(arm_gramps_prefab, COLOR_TAG, _colorHandler._gundamWhiteName);
        _colorHandler.orangeMaterial =
            FindMaterialFromPrefabRecursive(torso_gramps_prefab, COLOR_TAG, _colorHandler._gundamOrangeName);

        // exia materials
        _colorHandler.exiaMainBlueNameMaterial =
            FindMaterialFromPrefabRecursive(torso_exia_prefab, COLOR_TAG, _colorHandler._exiaMainBlueName);
        _colorHandler.exiaDetailBlueNameMaterial =
            FindMaterialFromPrefabRecursive(torso_exia_prefab, COLOR_TAG, _colorHandler._exiaDetailBlueName);
        _colorHandler.exiaWhiteMaterial =
            FindMaterialFromPrefabRecursive(torso_exia_prefab, COLOR_TAG, _colorHandler._exiaWhiteName);
        _colorHandler.exiaRedMaterial =
            FindMaterialFromPrefabRecursive(torso_exia_prefab, COLOR_TAG, _colorHandler._exiaRedName);
        _colorHandler.exiaGreenMaterial =
            FindMaterialFromPrefabRecursive(torso_exia_prefab, COLOR_TAG, _colorHandler._exiaGreenName);
    }

    private Material FindMaterialFromPrefabRecursive(GameObject prefab, string colorTag, string materialName)
    {
        Material result = null;
        var childList = FindTransformListFromTag(prefab, COLOR_TAG);
        foreach (var element in childList)
        {
            if (element.gameObject.GetComponent<Renderer>() != null)
            {
                foreach (var material in element.gameObject.GetComponent<Renderer>().sharedMaterials)
                {
                    if (material.name != materialName) continue;
                    result = material;
                    break;
                }

                if (result != null) break;
            }
            else if (element.CompareTag(colorTag))
            {
                result = FindMaterialFromPrefabRecursive(element.gameObject, colorTag, materialName);
                if (result != null) break;
            }
        }

        return result;
    }

    private void FindAnchors()
    {
        anchor_head = GameObject.Find("Anchor_head");
        anchor_torso = GameObject.Find("Anchor_torso");
        anchor_waist = GameObject.Find("Anchor_waist");
        anchor_R_arm = GameObject.Find("Anchor_R_arm");
        anchor_L_arm = GameObject.Find("Anchor_L_arm");
        anchor_R_leg = GameObject.Find("Anchor_R_leg");
        anchor_L_leg = GameObject.Find("Anchor_L_leg");
    }

    //find prefabs instantiated at start
    private void FindStartingGOs()
    {
        instantiated_head = GameObject.Find("Anchor_head/Gramps_head");
        instantiated_torso = GameObject.Find("Anchor_torso/Gramps_torso");
        instantiated_waist = GameObject.Find("Anchor_torso/Gramps_waist");
        instantiated_R_arm = GameObject.Find("Anchor_R_arm/Gramps_arm_right");
        instantiated_L_arm = GameObject.Find("Anchor_L_arm/Gramps_arm_left");
        instantiated_R_leg = GameObject.Find("Anchor_R_leg/Gramps_leg_right");
        instantiated_L_leg = GameObject.Find("Anchor_L_leg/Gramps_leg_left");
    }

    //final model change methods for Exia
    public void SetExiaHead(GameObject menu)
    {
        if (selected_torso_model == 0)
        {
            instantiated_head = InstantiatePrefab(head_exia_prefab,
                FindTransformFromTag(instantiated_torso, HEAD_ANCHOR), instantiated_head,
                new Vector3(G_E_UPPER_SCALE, G_E_UPPER_SCALE, G_E_UPPER_SCALE));
        }
        else if (selected_torso_model == 1)
        {
            instantiated_head = InstantiatePrefab(head_exia_prefab,
                FindTransformFromTag(instantiated_torso, HEAD_ANCHOR), instantiated_head,
                new Vector3(E_E_SCALE, E_E_SCALE, E_E_SCALE));
        }

        menu.SetActive(false);
    }

    public void SetExiaTorso(GameObject menu)
    {
        instantiated_torso = InstantiatePrefab(torso_exia_prefab, anchor_torso.transform, instantiated_torso,
            new Vector3(1, 1, 1));
        menu.SetActive(false);
    }

    public void SetExiaRArm(GameObject menu)
    {
        if (selected_torso_model == 0)
        {
            instantiated_R_arm = InstantiatePrefab(R_arm_exia_prefab,
                FindTransformFromTag(instantiated_torso, RIGHT_ANCHOR), instantiated_R_arm,
                new Vector3(G_E_UPPER_SCALE, G_E_UPPER_SCALE, G_E_UPPER_SCALE));
        }
        else if (selected_torso_model == 1)
        {
            instantiated_R_arm = InstantiatePrefab(R_arm_exia_prefab,
                FindTransformFromTag(instantiated_torso, RIGHT_ANCHOR), instantiated_R_arm,
                new Vector3(E_E_SCALE, E_E_SCALE, E_E_SCALE));
        }

        menu.SetActive(false);
    }

    public void SetExiaLArm(GameObject menu)
    {
        if (selected_torso_model == 0)
        {
            instantiated_L_arm = InstantiatePrefab(L_arm_exia_prefab,
                FindTransformFromTag(instantiated_torso, LEFT_ANCHOR), instantiated_L_arm,
                new Vector3(G_E_UPPER_SCALE, G_E_UPPER_SCALE, G_E_UPPER_SCALE));
        }
        else if (selected_torso_model == 1)
        {
            instantiated_L_arm = InstantiatePrefab(L_arm_exia_prefab,
                FindTransformFromTag(instantiated_torso, LEFT_ANCHOR), instantiated_L_arm,
                new Vector3(E_E_SCALE, E_E_SCALE, E_E_SCALE));
        }

        menu.SetActive(false);
        //instantiated_L_arm = InstantiatePrefab(L_arm_exia_prefab, findTorsoAnchor(instantiated_torso, "L_Anchor"), instantiated_L_arm, new Vector3(1, 1, 1));
    }

    public void SetExiaWaist(GameObject menu)
    {
        if (selected_torso_model == 0)
        {
            instantiated_waist = InstantiatePrefab(waist_exia_prefab,
                FindTransformFromTag(instantiated_torso, WAIST_ANCHOR), instantiated_waist,
                new Vector3(G_E_WAIST_SCALE, G_E_WAIST_SCALE, G_E_WAIST_SCALE));
            selected_waist_model = 1;
        }
        else if (selected_torso_model == 1)
        {
            instantiated_waist = InstantiatePrefab(waist_exia_prefab,
                FindTransformFromTag(instantiated_torso, WAIST_ANCHOR), instantiated_waist,
                new Vector3(1, 1, 1));
            selected_waist_model = 1;
        }

        menu.SetActive(false);
        //instantiated_waist = InstantiatePrefab(waist_exia_prefab, anchor_torso.transform, instantiated_waist, new Vector3(2, 2, 2));
    }

    public void SetExiaRLeg(GameObject menu)
    {
        if (selected_waist_model == 0)
        {
            instantiated_R_leg = InstantiatePrefab(R_leg_exia_prefab,
                FindTransformFromTag(instantiated_waist, RIGHT_ANCHOR), instantiated_R_leg,
                new Vector3(GWAIST_E_LEGS_SCALE, GWAIST_E_LEGS_SCALE, GWAIST_E_LEGS_SCALE));
        }
        else if (selected_waist_model == 1)
        {
            instantiated_R_leg = InstantiatePrefab(R_leg_exia_prefab,
                FindTransformFromTag(instantiated_waist, RIGHT_ANCHOR), instantiated_R_leg,
                new Vector3(EWAIST_E_LEGS_SCALE, EWAIST_E_LEGS_SCALE, EWAIST_E_LEGS_SCALE));
        }

        menu.SetActive(false);
        //instantiated_R_leg = InstantiatePrefab(R_leg_exia_prefab, anchor_R_leg.transform, instantiated_R_leg, new Vector3(2, 2, 2));
    }

    public void SetExiaLLeg(GameObject menu)
    {
        if (selected_waist_model == 0)
        {
            instantiated_L_leg = InstantiatePrefab(L_leg_exia_prefab,
                FindTransformFromTag(instantiated_waist, LEFT_ANCHOR), instantiated_L_leg,
                new Vector3(GWAIST_E_LEGS_SCALE, GWAIST_E_LEGS_SCALE, GWAIST_E_LEGS_SCALE));
        }
        else if (selected_waist_model == 1)
        {
            instantiated_L_leg = InstantiatePrefab(L_leg_exia_prefab,
                FindTransformFromTag(instantiated_waist, LEFT_ANCHOR), instantiated_L_leg,
                new Vector3(EWAIST_E_LEGS_SCALE, EWAIST_E_LEGS_SCALE, EWAIST_E_LEGS_SCALE));
        }

        menu.SetActive(false);
        //instantiated_L_leg = InstantiatePrefab(L_leg_exia_prefab, anchor_L_leg.transform, instantiated_L_leg, new Vector3(2, 2, 2));
    }

    //final model change methods for Gramps
    public void SetGrampsHead(GameObject menu)
    {
        if (selected_torso_model == 0)
        {
            instantiated_head = InstantiatePrefab(head_gramps_prefab,
                FindTransformFromTag(instantiated_torso, HEAD_ANCHOR), instantiated_head,
                new Vector3(G_G_HEAD_SCALE, G_G_HEAD_SCALE, G_G_HEAD_SCALE));
        }
        else if (selected_torso_model == 1)
        {
            instantiated_head = InstantiatePrefab(head_gramps_prefab,
                FindTransformFromTag(instantiated_torso, HEAD_ANCHOR), instantiated_head,
                new Vector3(E_G_HEAD_SCALE, E_G_HEAD_SCALE, E_G_HEAD_SCALE));
        }

        menu.SetActive(false);
        //instantiated_head = InstantiatePrefab(head_gramps_prefab, anchor_head.transform, instantiated_head, new Vector3(2, 2, 2));
    }

    public void SetGrampsTorso(GameObject menu)
    {
        instantiated_torso = InstantiatePrefab(torso_gramps_prefab, anchor_torso.transform, instantiated_torso,
            new Vector3(6, 6, 6));
    }

    public void SetGrampsLArm(GameObject menu)
    {
        if (selected_torso_model == 0)
        {
            instantiated_L_arm = InstantiatePrefab(arm_gramps_prefab,
                FindTransformFromTag(instantiated_torso, LEFT_ANCHOR), instantiated_L_arm,
                new Vector3(-1, 1, 1));
        }
        else if (selected_torso_model == 1)
        {
            instantiated_L_arm = InstantiatePrefab(arm_gramps_prefab,
                FindTransformFromTag(instantiated_torso, LEFT_ANCHOR), instantiated_L_arm,
                new Vector3(-E_G_BODY_SCALE, E_G_BODY_SCALE, E_G_BODY_SCALE));
        }

        menu.SetActive(false);
        //instantiated_L_arm = InstantiatePrefab(arm_gramps_prefab, anchor_L_arm.transform, instantiated_L_arm, new Vector3(-arm_gramps_prefab.transform.localScale.x, arm_gramps_prefab.transform.localScale.y, arm_gramps_prefab.transform.localScale.z));
    }

    public void SetGrampsRArm(GameObject menu)
    {
        if (selected_torso_model == 0)
        {
            instantiated_R_arm = InstantiatePrefab(arm_gramps_prefab,
                FindTransformFromTag(instantiated_torso, RIGHT_ANCHOR), instantiated_R_arm,
                new Vector3(1, 1, 1));
        }
        else if (selected_torso_model == 1)
        {
            instantiated_R_arm = InstantiatePrefab(arm_gramps_prefab,
                FindTransformFromTag(instantiated_torso, RIGHT_ANCHOR), instantiated_R_arm,
                new Vector3(E_G_BODY_SCALE, E_G_BODY_SCALE, E_G_BODY_SCALE));
        }

        menu.SetActive(false);
        //instantiated_R_arm = InstantiatePrefab(arm_gramps_prefab, anchor_R_arm.transform, instantiated_R_arm, arm_gramps_prefab.transform.localScale);
    }

    public void SetGrampsWaist(GameObject menu)
    {
        if (selected_torso_model == 0)
        {
            instantiated_waist = InstantiatePrefab(waist_gramps_prefab,
                FindTransformFromTag(instantiated_torso, WAIST_ANCHOR), instantiated_waist,
                new Vector3(1, 1, 1));
            selected_waist_model = 0;
        }
        else if (selected_torso_model == 1)
        {
            instantiated_waist = InstantiatePrefab(waist_gramps_prefab,
                FindTransformFromTag(instantiated_torso, WAIST_ANCHOR), instantiated_waist,
                new Vector3(E_G_BODY_SCALE, E_G_BODY_SCALE, E_G_BODY_SCALE));
            selected_waist_model = 0;
        }

        menu.SetActive(false);
        //instantiated_waist = InstantiatePrefab(waist_gramps_prefab, anchor_torso.transform, instantiated_waist, waist_gramps_prefab.transform.localScale);
    }

    public void SetGrampsRLeg(GameObject menu)
    {
        if (selected_waist_model == 0)
        {
            instantiated_R_leg = InstantiatePrefab(leg_gramps_prefab,
                FindTransformFromTag(instantiated_waist, RIGHT_ANCHOR), instantiated_R_leg,
                new Vector3(GWAIST_G_LEGS_SCALE, GWAIST_G_LEGS_SCALE, GWAIST_G_LEGS_SCALE));
        }
        else if (selected_waist_model == 1)
        {
            instantiated_R_leg = InstantiatePrefab(leg_gramps_prefab,
                FindTransformFromTag(instantiated_waist, RIGHT_ANCHOR), instantiated_R_leg,
                new Vector3(EWAIST_G_LEGS_SCALE, EWAIST_G_LEGS_SCALE, EWAIST_G_LEGS_SCALE));
        }

        menu.SetActive(false);
        //instantiated_R_leg = InstantiatePrefab(leg_gramps_prefab, anchor_R_leg.transform, instantiated_R_leg, leg_gramps_prefab.transform.localScale);
    }

    public void SetGrampsLLeg(GameObject menu)
    {
        if (selected_waist_model == 0)
        {
            instantiated_L_leg = InstantiatePrefab(leg_gramps_prefab,
                FindTransformFromTag(instantiated_waist, LEFT_ANCHOR), instantiated_L_leg,
                new Vector3(GWAIST_G_LEGS_SCALE, GWAIST_G_LEGS_SCALE, GWAIST_G_LEGS_SCALE));
        }
        else if (selected_waist_model == 1)
        {
            instantiated_L_leg = InstantiatePrefab(leg_gramps_prefab,
                FindTransformFromTag(instantiated_waist, LEFT_ANCHOR), instantiated_L_leg,
                new Vector3(EWAIST_G_LEGS_SCALE, EWAIST_G_LEGS_SCALE, EWAIST_G_LEGS_SCALE));
        }

        menu.SetActive(false);
        //instantiated_L_leg = InstantiatePrefab(leg_gramps_prefab, anchor_L_leg.transform, instantiated_L_leg, leg_gramps_prefab.transform.localScale);
    }

    public void InstantiateGramps(GameObject menu)
    {
        selected_torso_model = 0;
        selected_waist_model = 0;
        SetGrampsTorso(menu);
        SetGrampsWaist(menu);
        SetGrampsHead(menu);
        SetGrampsLArm(menu);
        SetGrampsRArm(menu);
        SetGrampsLLeg(menu);
        SetGrampsRLeg(menu);
        _menuHandler.PopulateColorDropdown();
    }

    public void InstantiateExia(GameObject menu)
    {
        selected_torso_model = 1;
        selected_waist_model = 1;
        SetExiaTorso(menu);
        SetExiaWaist(menu);
        SetExiaHead(menu);
        SetExiaLArm(menu);
        SetExiaRArm(menu);
        SetExiaLLeg(menu);
        SetExiaRLeg(menu);
        _menuHandler.PopulateColorDropdown();
    }

    public void changeGlobalColor(string colorName, bool reset)
    {
        switch (colorName)
        {
            case "blue":
                _blueMaterial.color = _colorHandler.Color;
                if (reset) _blueMaterial.color = _colorHandler.GundamBlue;
                break;
            case "white":
                _whiteMaterial.color = _colorHandler.Color;
                if (reset) _whiteMaterial.color = _colorHandler._gundamWhite;
                break;
            case "red":
                _redMaterial.color = _colorHandler.Color;
                if (reset) _redMaterial.color = _colorHandler._gundamRed;
                break;
            case "orange":
                _orangeMaterial.color = _colorHandler.Color;
                if (reset) _orangeMaterial.color = _colorHandler._gundamOrange;
                break;
            case "exia_main_blue":
                _exiaMainBlueNameMaterial.color = _colorHandler.Color;
                if (reset) _exiaMainBlueNameMaterial.color = _colorHandler._exiaMainBlue;
                break;
            case "exia_detail_blue":
                _exiaDetailBlueNameMaterial.color = _colorHandler.Color;
                if (reset) _exiaDetailBlueNameMaterial.color = _colorHandler._exiaDetailBlue;
                break;
            case "exia_white":
                _exiaWhiteMaterial.color = _colorHandler.Color;
                if (reset) _exiaWhiteMaterial.color = _colorHandler._exiaWhite;
                break;
            case "exia_red":
                _exiaRedMaterial.color = _colorHandler.Color;
                if (reset) _exiaRedMaterial.color = _colorHandler._exiaRed;
                break;
            case "exia_green":
                _exiaGreenMaterial.color = _colorHandler.Color;
                if (reset) _exiaGreenMaterial.color = _colorHandler._exiaGreen;
                break;
        }
    }

    public void ChangePartColor(string partName, string colorName)
    {
        switch (partName)
        {
            case "head":
                Renderer[] headRenderers = instantiated_head.GetComponentsInChildren<Renderer>();
                setPropertyBlockOnRenderers(headRenderers, colorName);
                break;
            case "torso":
                Renderer[] torsoRenderers = instantiated_torso.GetComponentsInChildren<Renderer>();
                setPropertyBlockOnRenderers(torsoRenderers, colorName);
                break;
            case "left arm":
                Renderer[] lARMRenderers = instantiated_L_arm.GetComponentsInChildren<Renderer>();
                setPropertyBlockOnRenderers(lARMRenderers, colorName);
                break;
            case "right arm":
                Renderer[] rARMRenderers = instantiated_R_arm.GetComponentsInChildren<Renderer>();
                setPropertyBlockOnRenderers(rARMRenderers, colorName);
                break;
            case "waist":
                Renderer[] waistRenderers = instantiated_waist.GetComponentsInChildren<Renderer>();
                setPropertyBlockOnRenderers(waistRenderers, colorName);
                break;
            case "left leg":
                Renderer[] lLegRenderers = instantiated_L_leg.GetComponentsInChildren<Renderer>();
                setPropertyBlockOnRenderers(lLegRenderers, colorName);
                break;
            case "right leg":
                Renderer[] rLegRenderers = instantiated_R_leg.GetComponentsInChildren<Renderer>();
                setPropertyBlockOnRenderers(rLegRenderers, colorName);
                break;
            default:
                Renderer[] headRenderersGlobal = instantiated_head.GetComponentsInChildren<Renderer>();
                setPropertyBlockOnRenderers(headRenderersGlobal, colorName);
                Renderer[] torsoRenderersGlobal = instantiated_torso.GetComponentsInChildren<Renderer>();
                setPropertyBlockOnRenderers(torsoRenderersGlobal, colorName);
                Renderer[] lARMRenderersGlobal = instantiated_L_arm.GetComponentsInChildren<Renderer>();
                setPropertyBlockOnRenderers(lARMRenderersGlobal, colorName);
                Renderer[] rARMRenderersGlobal = instantiated_R_arm.GetComponentsInChildren<Renderer>();
                setPropertyBlockOnRenderers(rARMRenderersGlobal, colorName);
                Renderer[] waistRenderersGlobal = instantiated_waist.GetComponentsInChildren<Renderer>();
                setPropertyBlockOnRenderers(waistRenderersGlobal, colorName);
                Renderer[] lLegRenderersGlobal = instantiated_L_leg.GetComponentsInChildren<Renderer>();
                setPropertyBlockOnRenderers(lLegRenderersGlobal, colorName);
                Renderer[] rLegRenderersGlobal = instantiated_R_leg.GetComponentsInChildren<Renderer>();
                setPropertyBlockOnRenderers(rLegRenderersGlobal, colorName);
                break;
        }
    }

    private void setPropertyBlockOnRenderers(Renderer[] renderers, string colorName)
    {
        MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();
        propertyBlock.SetColor(Color1, _colorHandler.Color);

        foreach (var element in renderers)
        {
            foreach (var material in element.sharedMaterials)
            {
                if (material != _colorHandler.ColorNameToMaterial(colorName)) continue;
                
                element.SetPropertyBlock(propertyBlock);

                break;
            }
        }
    }

    private Transform FindTransformFromTag(GameObject go, string tag)
    {
        Transform result = null;
        for (int i = 0; i < go.transform.childCount; ++i)
        {
            Transform child = go.transform.GetChild(i);
            if (child.CompareTag(tag))
            {
                result = child;
            }
        }

        return result;
    }

    private List<Transform> FindTransformListFromTag(GameObject go, string tagName)
    {
        var result = new List<Transform>();
        for (int i = 0; i < go.transform.childCount; ++i)
        {
            var child = go.transform.GetChild(i);
            if (child.CompareTag(tagName))
            {
                result.Add(child);
            }
        }

        return result;
    }

    public Renderer[] GetInstancedRenderers(string partName)
    {
        List<Renderer> result = new List<Renderer>();
        switch (partName)
        {
            case "head":
                result.AddRange(instantiated_head.GetComponentsInChildren<Renderer>());
                break;
            case "torso":
                result.AddRange(instantiated_torso.GetComponentsInChildren<Renderer>());
                break;
            case "L_arm":
                result.AddRange(instantiated_L_arm.GetComponentsInChildren<Renderer>());
                break;
            case "R_arm":
                result.AddRange(instantiated_R_arm.GetComponentsInChildren<Renderer>());
                break;
            case "waist":
                result.AddRange(instantiated_waist.GetComponentsInChildren<Renderer>());
                break;
            case "L_leg":
                result.AddRange(instantiated_L_leg.GetComponentsInChildren<Renderer>());
                break;
            case "R_leg":
                result.AddRange(instantiated_R_leg.GetComponentsInChildren<Renderer>());
                break;
            default:
                result.AddRange(instantiated_head.GetComponentsInChildren<Renderer>());
                result.AddRange(instantiated_torso.GetComponentsInChildren<Renderer>());
                result.AddRange(instantiated_L_arm.GetComponentsInChildren<Renderer>());
                result.AddRange(instantiated_R_arm.GetComponentsInChildren<Renderer>());
                result.AddRange(instantiated_waist.GetComponentsInChildren<Renderer>());
                result.AddRange(instantiated_L_leg.GetComponentsInChildren<Renderer>());
                result.AddRange(instantiated_R_leg.GetComponentsInChildren<Renderer>());
                break;
        }

        return result.ToArray();
    }

    private GameObject InstantiatePrefab(GameObject prefab, Transform anchor, GameObject instantiatedPart,
        Vector3 scale)
    {
        GameObject go = Instantiate(prefab, anchor) as GameObject;
        go.transform.SetPositionAndRotation(anchor.position, anchor.rotation);
        go.transform.localScale = scale;

        //check if is waist to avoid null value for legs when waist is deleted
        if (instantiatedPart != null && instantiatedPart.CompareTag("Is_Waist"))
        {
            GameObject left_leg = FindTransformFromTag(instantiatedPart, LEFT_ANCHOR).GetChild(0).gameObject;
            GameObject right_leg = FindTransformFromTag(instantiatedPart, RIGHT_ANCHOR).GetChild(0).gameObject;

            //find new anchors and place the legs in the appropirate position
            Transform newLeftAnchor = FindTransformFromTag(go, LEFT_ANCHOR);
            Transform newRightAnchor = FindTransformFromTag(go, RIGHT_ANCHOR);
            left_leg.transform.SetParent(newLeftAnchor, true);
            right_leg.transform.SetParent(newRightAnchor, true);
            instantiated_L_leg = left_leg;
            instantiated_R_leg = right_leg;
        }

        Destroy(instantiatedPart);
        return go;
    }
}