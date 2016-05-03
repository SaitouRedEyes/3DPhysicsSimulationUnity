using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class GameManager : MonoBehaviour
{
    public static int width, height;
    public Text textForce, textTravelledDistance, textMaxDistance;
    public InputField inputFieldMaxDistance;
    public Button[] buttonsMaterial;

    private Cube cube;
    private Path path;

    void Awake()
    {
        width = Screen.width;
        height = Screen.height;

        path = InstantiateObject<Path>("Prefabs/Path", "Path").GetComponent<Path>();
        cube = InstantiateObject<Cube>("Prefabs/Cube", "Cube").GetComponent<Cube>();

        path.Width = inputFieldMaxDistance.text = "35";
    }

    void Update()
    {
        GUIManager();
        KeyboardInputHandler();
        CameraFollowCube();

        path.SetScale(cube);
        cube.UpdateCubeStartPoint(path);
    }

    private void GUIManager()
    {
        textForce.text = "Força: " + cube.Force + " N";
        textTravelledDistance.text = cube.CouldCalculate ? "Distância percorrida: " + cube.TravelledDistance + " M" : string.Empty;

        if (cube.CouldApplyForce)
        {
            textMaxDistance.gameObject.SetActive(true);
            inputFieldMaxDistance.gameObject.SetActive(true);
            ChangeButtonsMaterialState(true);
        }
        else
        {
            textMaxDistance.gameObject.SetActive(false);
            inputFieldMaxDistance.gameObject.SetActive(false);
            ChangeButtonsMaterialState(false);
        }
    }

    /// <summary>
    /// Method accessed by Max Path input field event onValueChange
    /// </summary>
    public void UpdatePathSize()
    {
        path.Width = inputFieldMaxDistance.text;
    }

    /// <summary>
    /// Set the path physic material
    /// </summary>
    /// <param name="pathMaterial">Path material</param>
    public void SetPathMaterial(int pathMaterial)
    {
        switch (pathMaterial)
        {
            case (int)Path.Materials.NoMaterial: path.SetPhysicMaterial("Materials/Default", null); break;
            case (int)Path.Materials.Ice: path.SetPhysicMaterial("Materials/Ice", "Physic Materials/Ice"); break;
            case (int)Path.Materials.Metal: path.SetPhysicMaterial("Materials/Metal", "Physic Materials/Metal"); break;
            case (int)Path.Materials.Rubber: path.SetPhysicMaterial("Materials/Rubber", "Physic Materials/Rubber"); break;
            case (int)Path.Materials.Wood: path.SetPhysicMaterial("Materials/Wood", "Physic Materials/Wood"); break;
        }
    }

    /// <summary>
    /// Set the cube force
    /// </summary>
    /// <param name="force">Cube Force</param>
    public void SetForce(int force)
    {
        cube.Force = force;
    }

    private void ChangeButtonsMaterialState(bool active)
    {
        foreach (Button b in buttonsMaterial)
        {
            b.gameObject.SetActive(active);
        }
    }

    /// <summary>
    /// Instatiate a generic object
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="resourcePath"></param>
    /// <param name="name"></param>
    private GameObject InstantiateObject<T>(string resourcePath, string name) where T : Component
    {
        GameObject go;
        go = (GameObject)Instantiate(Resources.Load(resourcePath));
        go.name = name;
        go.AddComponent<T>();

        return go;
    }

    /// <summary>
    /// Keyboard input handler.
    /// </summary>
    private void KeyboardInputHandler()
    {
        if (Input.GetKeyUp(KeyCode.Space)) cube.KeyboardInputHandler(KeyCode.Space);
        else if (Input.GetKeyUp(KeyCode.R)) cube.KeyboardInputHandler(KeyCode.R);
        else if (Input.GetKeyUp(KeyCode.Escape)) Application.Quit();
    }

    /// <summary>
    /// Camera follow the cube.
    /// </summary>
    private void CameraFollowCube()
    {
        Camera.main.transform.position = new Vector3((cube.transform.position.x - 2.0f),
                                                           Camera.main.transform.position.y,
                                                           Camera.main.transform.position.z);
    }
}