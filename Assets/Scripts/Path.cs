using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;

public class Path : MonoBehaviour
{
    private string width;
    private Cube cube;

    public enum Materials { NoMaterial, Ice, Metal, Rubber, Wood };

    #region Getters and Setters
    public string Width
    {
        get { return width; }
        set { width = value; }
    }
    #endregion

    /// <summary>
    /// Sets scale path.
    /// </summary>
    public void SetScale(Cube cube)
    {
        if (!width.Equals(string.Empty) && !width.Equals("."))
        {
            if (width.Split('.').Length - 1 <= 1 && float.Parse(width) > cube.gameObject.transform.localScale.x)
            {      
                this.transform.localScale = new Vector3(float.Parse(width), 1, 1);
            }
        }
    }

    /// <summary>
    /// Configure the material and the physic material of the Cube Path.
    /// </summary>
    /// <param name="srcPathMaterial">Source path of the material</param>
    /// <param name="srcPathPhysicMaterial">Source path of the physic material</param>
    public void SetPhysicMaterial(string srcPathMaterial, string srcPathPhysicMaterial)
    {
        this.GetComponent<BoxCollider>().material =  (PhysicMaterial)Resources.Load(srcPathPhysicMaterial);
        this.GetComponent<MeshRenderer>().material = (Material)Resources.Load(srcPathMaterial);
    }
}