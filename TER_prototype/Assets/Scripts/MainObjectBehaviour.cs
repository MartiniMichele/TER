using UnityEditor;
using UnityEngine;

public class MainObjectBehaviour : MonoBehaviour
{
    //support variables
    int selected_torso_model = 0;
    int selected_waist_model = 0;
    const string HEAD_ANCHOR = "Head_Anchor";
    const string RIGHT_ANCHOR = "R_Anchor";
    const string LEFT_ANCHOR = "L_Anchor";
    const string WAIST_ANCHOR = "Waist_Anchor";
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
    private Material redMetallicMaterial;
    private Material tealMaterial;
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
    [SerializeField]
    private GameObject anchor_head, anchor_torso, anchor_waist, anchor_R_arm, anchor_L_arm, anchor_R_leg, anchor_L_leg;

    private GameObject instantiated_head;
    private GameObject instantiated_torso;
    private GameObject instantiated_waist;
    private GameObject instantiated_R_arm;
    private GameObject instantiated_L_arm;
    private GameObject instantiated_R_leg;
    private GameObject instantiated_L_leg;

    private bool roundHead;
    private bool roundUpper;
    private bool roundArms;
    private bool roundLegs;


    private void Start()
    {
        LoadMaterials();
        LoadPrefabs();
        FindAnchors();
        FindStartingGOs();
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
        redMetallicMaterial = Resources.Load("Red") as Material;
        tealMaterial = Resources.Load("Teal") as Material;
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
            instantiated_head = InstantiatePrefab(head_exia_prefab, findAnchor(instantiated_torso, HEAD_ANCHOR), instantiated_head,
                new Vector3(G_E_UPPER_SCALE, G_E_UPPER_SCALE, G_E_UPPER_SCALE));
        }
        else if (selected_torso_model == 1)
        {
            instantiated_head = InstantiatePrefab(head_exia_prefab, findAnchor(instantiated_torso, HEAD_ANCHOR), instantiated_head,
                new Vector3(E_E_SCALE, E_E_SCALE, E_E_SCALE));
        }
        menu.SetActive(false);

    }
    public void SetExiaTorso(GameObject menu)
    {
        instantiated_torso = InstantiatePrefab(torso_exia_prefab, anchor_torso.transform, instantiated_torso, new Vector3(1, 1, 1));
        menu.SetActive(false);

    }
    public void SetExiaRArm(GameObject menu)
    {
        if (selected_torso_model == 0)
        {
            instantiated_R_arm = InstantiatePrefab(R_arm_exia_prefab, findAnchor(instantiated_torso, RIGHT_ANCHOR), instantiated_R_arm,
                new Vector3(G_E_UPPER_SCALE, G_E_UPPER_SCALE, G_E_UPPER_SCALE));
        }
        else if (selected_torso_model == 1)
        {
            instantiated_R_arm = InstantiatePrefab(R_arm_exia_prefab, findAnchor(instantiated_torso, RIGHT_ANCHOR), instantiated_R_arm,
                new Vector3(E_E_SCALE, E_E_SCALE, E_E_SCALE));
        }
        menu.SetActive(false);

    }
    public void SetExiaLArm(GameObject menu)
    {
        if (selected_torso_model == 0)
        {
            instantiated_L_arm = InstantiatePrefab(L_arm_exia_prefab, findAnchor(instantiated_torso, LEFT_ANCHOR), instantiated_L_arm,
                new Vector3(G_E_UPPER_SCALE, G_E_UPPER_SCALE, G_E_UPPER_SCALE));
        }
        else if (selected_torso_model == 1)
        {
            instantiated_L_arm = InstantiatePrefab(L_arm_exia_prefab, findAnchor(instantiated_torso, LEFT_ANCHOR), instantiated_L_arm,
                new Vector3(E_E_SCALE, E_E_SCALE, E_E_SCALE));
        }
        menu.SetActive(false);
        //instantiated_L_arm = InstantiatePrefab(L_arm_exia_prefab, findTorsoAnchor(instantiated_torso, "L_Anchor"), instantiated_L_arm, new Vector3(1, 1, 1));
    }
    public void SetExiaWaist(GameObject menu)
    {
        if (selected_torso_model == 0)
        {
            instantiated_waist = InstantiatePrefab(waist_exia_prefab, findAnchor(instantiated_torso, WAIST_ANCHOR), instantiated_waist,
                new Vector3(G_E_WAIST_SCALE, G_E_WAIST_SCALE, G_E_WAIST_SCALE));
            selected_waist_model = 1;
        }
        else if (selected_torso_model == 1)
        {
            instantiated_waist = InstantiatePrefab(waist_exia_prefab, findAnchor(instantiated_torso, WAIST_ANCHOR), instantiated_waist,
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
            instantiated_R_leg = InstantiatePrefab(R_leg_exia_prefab, findAnchor(instantiated_waist, RIGHT_ANCHOR), instantiated_R_leg,
                new Vector3(GWAIST_E_LEGS_SCALE, GWAIST_E_LEGS_SCALE, GWAIST_E_LEGS_SCALE));
        }
        else if (selected_waist_model == 1)
        {
            instantiated_R_leg = InstantiatePrefab(R_leg_exia_prefab, findAnchor(instantiated_waist, RIGHT_ANCHOR), instantiated_R_leg,
                new Vector3(EWAIST_E_LEGS_SCALE, EWAIST_E_LEGS_SCALE, EWAIST_E_LEGS_SCALE));
        }
        menu.SetActive(false);
        //instantiated_R_leg = InstantiatePrefab(R_leg_exia_prefab, anchor_R_leg.transform, instantiated_R_leg, new Vector3(2, 2, 2));

    }
    public void SetExiaLLeg(GameObject menu)
    {
        if (selected_waist_model == 0)
        {
            instantiated_L_leg = InstantiatePrefab(L_leg_exia_prefab, findAnchor(instantiated_waist, LEFT_ANCHOR), instantiated_L_leg,
                new Vector3(GWAIST_E_LEGS_SCALE, GWAIST_E_LEGS_SCALE, GWAIST_E_LEGS_SCALE));
        }
        else if (selected_waist_model == 1)
        {
            instantiated_L_leg = InstantiatePrefab(L_leg_exia_prefab, findAnchor(instantiated_waist, LEFT_ANCHOR), instantiated_L_leg,
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
            instantiated_head = InstantiatePrefab(head_gramps_prefab, findAnchor(instantiated_torso, HEAD_ANCHOR), instantiated_head,
                new Vector3(G_G_HEAD_SCALE, G_G_HEAD_SCALE, G_G_HEAD_SCALE));
        }
        else if (selected_torso_model == 1)
        {
            instantiated_head = InstantiatePrefab(head_gramps_prefab, findAnchor(instantiated_torso, HEAD_ANCHOR), instantiated_head,
                new Vector3(E_G_HEAD_SCALE, E_G_HEAD_SCALE, E_G_HEAD_SCALE));
        }
        menu.SetActive(false);
        //instantiated_head = InstantiatePrefab(head_gramps_prefab, anchor_head.transform, instantiated_head, new Vector3(2, 2, 2));

    }
    public void SetGrampsTorso(GameObject menu)
    {
        instantiated_torso = InstantiatePrefab(torso_gramps_prefab, anchor_torso.transform, instantiated_torso, new Vector3(6, 6, 6));

    }

    public void SetGrampsLArm(GameObject menu)
    {
        if (selected_torso_model == 0)
        {
            instantiated_L_arm = InstantiatePrefab(arm_gramps_prefab, findAnchor(instantiated_torso, LEFT_ANCHOR), instantiated_L_arm,
                new Vector3(-1, 1, 1));
        }
        else if (selected_torso_model == 1)
        {
            instantiated_L_arm = InstantiatePrefab(arm_gramps_prefab, findAnchor(instantiated_torso, LEFT_ANCHOR), instantiated_L_arm,
                new Vector3(-E_G_BODY_SCALE, E_G_BODY_SCALE, E_G_BODY_SCALE));
        }
        menu.SetActive(false);
        //instantiated_L_arm = InstantiatePrefab(arm_gramps_prefab, anchor_L_arm.transform, instantiated_L_arm, new Vector3(-arm_gramps_prefab.transform.localScale.x, arm_gramps_prefab.transform.localScale.y, arm_gramps_prefab.transform.localScale.z));

    }
    public void SetGrampsRArm(GameObject menu)
    {
        if (selected_torso_model == 0)
        {
            instantiated_R_arm = InstantiatePrefab(arm_gramps_prefab, findAnchor(instantiated_torso, RIGHT_ANCHOR), instantiated_R_arm,
                new Vector3(1, 1, 1));
        }
        else if (selected_torso_model == 1)
        {
            instantiated_R_arm = InstantiatePrefab(arm_gramps_prefab, findAnchor(instantiated_torso, RIGHT_ANCHOR), instantiated_R_arm,
                new Vector3(E_G_BODY_SCALE, E_G_BODY_SCALE, E_G_BODY_SCALE));
        }
        menu.SetActive(false);
        //instantiated_R_arm = InstantiatePrefab(arm_gramps_prefab, anchor_R_arm.transform, instantiated_R_arm, arm_gramps_prefab.transform.localScale);

    }
    public void SetGrampsWaist(GameObject menu)
    {
        if (selected_torso_model == 0)
        {
            instantiated_waist = InstantiatePrefab(waist_gramps_prefab, findAnchor(instantiated_torso, WAIST_ANCHOR), instantiated_waist,
                new Vector3(1, 1, 1));
            selected_waist_model = 0;
        }
        else if (selected_torso_model == 1)
        {
            instantiated_waist = InstantiatePrefab(waist_gramps_prefab, findAnchor(instantiated_torso, WAIST_ANCHOR), instantiated_waist,
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
            instantiated_R_leg = InstantiatePrefab(leg_gramps_prefab, findAnchor(instantiated_waist, RIGHT_ANCHOR), instantiated_R_leg,
                new Vector3(GWAIST_G_LEGS_SCALE, GWAIST_G_LEGS_SCALE, GWAIST_G_LEGS_SCALE));
        }
        else if (selected_waist_model == 1)
        {
            instantiated_R_leg = InstantiatePrefab(leg_gramps_prefab, findAnchor(instantiated_waist, RIGHT_ANCHOR), instantiated_R_leg,
                new Vector3(EWAIST_G_LEGS_SCALE, EWAIST_G_LEGS_SCALE, EWAIST_G_LEGS_SCALE));
        }
        menu.SetActive(false);
        //instantiated_R_leg = InstantiatePrefab(leg_gramps_prefab, anchor_R_leg.transform, instantiated_R_leg, leg_gramps_prefab.transform.localScale);

    }
    public void SetGrampsLLeg(GameObject menu)
    {
        if (selected_waist_model == 0)
        {
            instantiated_L_leg = InstantiatePrefab(leg_gramps_prefab, findAnchor(instantiated_waist, LEFT_ANCHOR), instantiated_L_leg,
                new Vector3(GWAIST_G_LEGS_SCALE, GWAIST_G_LEGS_SCALE, GWAIST_G_LEGS_SCALE));
        }
        else if (selected_waist_model == 1)
        {
            instantiated_L_leg = InstantiatePrefab(leg_gramps_prefab, findAnchor(instantiated_waist, LEFT_ANCHOR), instantiated_L_leg,
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

    }

    private Transform findAnchor(GameObject go, string tag)
    {
        Transform result = null;
        for (int i = 0; i < go.transform.childCount; ++i)
        {
            Transform child = go.transform.GetChild(i);
            var tag_child = child.tag;
            if (child.tag == tag)
            {
                result = child;
            }
        }
        return result;
    }

    private GameObject InstantiatePrefab(GameObject prefab, Transform anchor, GameObject instantiatedPart, Vector3 scale)
    {
        GameObject go = Instantiate(prefab, anchor) as GameObject;
        go.transform.SetPositionAndRotation(anchor.position, anchor.rotation);
        go.transform.localScale = scale;

        //check if is waist to avoid null value for legs when waist is deleted
        if (instantiatedPart != null && instantiatedPart.CompareTag("Is_Waist"))
        {
            GameObject left_leg = findAnchor(instantiatedPart, LEFT_ANCHOR).GetChild(0).gameObject;
            GameObject right_leg = findAnchor(instantiatedPart, RIGHT_ANCHOR).GetChild(0).gameObject;

            //find new anchors and place the legs in the appropirate position
            Transform newLeftAnchor = findAnchor(go, LEFT_ANCHOR);
            Transform newRightAnchor = findAnchor(go, RIGHT_ANCHOR);
            left_leg.transform.SetParent(newLeftAnchor, true);
            right_leg.transform.SetParent(newRightAnchor, true);
            instantiated_L_leg = left_leg;
            instantiated_R_leg = right_leg;
        }
        Destroy(instantiatedPart);
        return go;

    }

    public void ChangeHeadColor()
    {
        // pass true in order to also include disabled or inactive child Renderer
        foreach (var rend in anchor_head.GetComponentsInChildren<Renderer>(false))
        {
            if (rend.sharedMaterial == tealMaterial)
            {
                rend.material = redMetallicMaterial;
            }
            else
            {
                rend.material = tealMaterial;
            }
        }
    }

    public void ChangeHead()
    {
        if (!roundHead)
        {
            GameObject go = Instantiate(head_sphere_prefab, anchor_head.transform) as GameObject;
            go.transform.SetPositionAndRotation(anchor_head.transform.position, anchor_head.transform.rotation);
            Destroy(instantiated_head);
            instantiated_head = go;
            roundHead = true;
        }
        else
        {
            GameObject go = Instantiate(head_cube_prefab, anchor_head.transform) as GameObject;
            go.transform.SetPositionAndRotation(anchor_head.transform.position, anchor_head.transform.rotation);
            Destroy(instantiated_head);
            instantiated_head = go;
            roundHead = false;
        }

    }

    public void ChangeUpperColor()
    {
        ChangeTorsoColor();
        ChangeWaistColor();
    }
    public void ChangeArmsColor()
    {
        ChangeArmColor();
    }
    public void ChangeLegsColor()
    {
        ChangeLegColor();
    }

    public void ChangeArms()
    {
        ChangeArmsPrefab();
    }
    public void ChangeLegs()
    {
        ChangeLegsPrefab();
    }
    private void ChangeTorsoColor()
    {
        // pass true in order to also include disabled or inactive child Renderer
        foreach (var rend in anchor_torso.GetComponentsInChildren<Renderer>(false))
        {
            if (rend.sharedMaterial == tealMaterial)
            {
                rend.material = redMetallicMaterial;
            }
            else
            {
                rend.material = tealMaterial;
            }
        }
    }
    private void ChangeWaistColor()
    {

        // pass true in order to also include disabled or inactive child Renderer
        foreach (var rend in anchor_waist.GetComponentsInChildren<Renderer>(false))
        {
            if (rend.sharedMaterial == tealMaterial)
            {
                rend.material = redMetallicMaterial;
            }
            else
            {
                rend.material = tealMaterial;
            }
        }
    }
    private void ChangeArmColor()
    {
        // pass true in order to also include disabled or inactive child Renderer
        foreach (var rend in anchor_R_arm.GetComponentsInChildren<Renderer>(false))
        {
            if (rend.sharedMaterial == tealMaterial)
            {
                rend.material = redMetallicMaterial;
            }
            else
            {
                rend.material = tealMaterial;
            }
        }

        foreach (var rend in anchor_L_arm.GetComponentsInChildren<Renderer>(false))
        {
            if (rend.sharedMaterial == tealMaterial)
            {
                rend.material = redMetallicMaterial;
            }
            else
            {
                rend.material = tealMaterial;
            }
        }
    }
    private void ChangeArmsPrefab()
    {
        if (!roundArms)
        {
            GameObject go_L = Instantiate(arm_sphere_prefab, anchor_L_arm.transform) as GameObject;
            go_L.transform.SetPositionAndRotation(anchor_L_arm.transform.position, anchor_L_arm.transform.rotation);
            Destroy(instantiated_L_arm);
            instantiated_L_arm = go_L;
            GameObject go_R = Instantiate(arm_sphere_prefab, anchor_R_arm.transform) as GameObject;
            go_R.transform.SetPositionAndRotation(anchor_R_arm.transform.position, anchor_R_arm.transform.rotation);
            Destroy(instantiated_R_arm);
            instantiated_R_arm = go_R;
            roundArms = true;
        }
        else
        {
            GameObject go_L = Instantiate(arm_cube_prefab, anchor_L_arm.transform) as GameObject;
            go_L.transform.SetPositionAndRotation(anchor_L_arm.transform.position, anchor_L_arm.transform.rotation);
            Destroy(instantiated_L_arm);
            instantiated_L_arm = go_L;
            GameObject go_R = Instantiate(arm_cube_prefab, anchor_R_arm.transform) as GameObject;
            go_R.transform.SetPositionAndRotation(anchor_R_arm.transform.position, anchor_R_arm.transform.rotation);
            Destroy(instantiated_R_arm);
            instantiated_R_arm = go_R;
            roundArms = false;
        }
    }
    private void ChangeLegsPrefab()
    {
        if (!roundLegs)
        {
            GameObject go_L = Instantiate(leg_sphere_prefab, anchor_L_leg.transform) as GameObject;
            go_L.transform.SetPositionAndRotation(anchor_L_leg.transform.position, anchor_L_leg.transform.rotation);
            Destroy(instantiated_L_leg);
            instantiated_L_leg = go_L;
            GameObject go_R = Instantiate(leg_sphere_prefab, anchor_R_leg.transform) as GameObject;
            go_R.transform.SetPositionAndRotation(anchor_R_leg.transform.position, anchor_R_leg.transform.rotation);
            Destroy(instantiated_R_leg);
            instantiated_R_leg = go_R;
            roundLegs = true;
        }
        else
        {
            GameObject go_L = Instantiate(leg_cube_prefab, anchor_L_leg.transform) as GameObject;
            go_L.transform.SetPositionAndRotation(anchor_L_leg.transform.position, anchor_L_leg.transform.rotation);
            Destroy(instantiated_L_leg);
            instantiated_L_leg = go_L;
            GameObject go_R = Instantiate(leg_cube_prefab, anchor_R_leg.transform) as GameObject;
            go_R.transform.SetPositionAndRotation(anchor_R_leg.transform.position, anchor_R_leg.transform.rotation);
            Destroy(instantiated_R_leg);
            instantiated_R_leg = go_R;
            roundLegs = false;
        }
    }
    private void ChangeLegColor()
    {
        // pass true in order to also include disabled or inactive child Renderer
        foreach (var rend in anchor_R_leg.GetComponentsInChildren<Renderer>(false))
        {
            if (rend.sharedMaterial == tealMaterial)
            {
                rend.material = redMetallicMaterial;
            }
            else
            {
                rend.material = tealMaterial;
            }
        }

        foreach (var rend in anchor_L_leg.GetComponentsInChildren<Renderer>(false))
        {
            if (rend.sharedMaterial == tealMaterial)
            {
                rend.material = redMetallicMaterial;
            }
            else
            {
                rend.material = tealMaterial;
            }
        }
    }
}
