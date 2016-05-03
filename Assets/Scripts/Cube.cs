using UnityEngine;
using System;
using System.Collections;

public class Cube : MonoBehaviour
{
    public float startTime;
    
    private bool couldCalculate;
    private bool couldRestart;
    private bool couldApplyForce;
    private float force;
    private Vector3 initialosPos;
    private Rigidbody myRigidbody;

    #region getters and setters
    public float Force
    {
        get { return force; }
        set { if (value >= -5 && value <= 5) force += value; }
    }

    public double TravelledDistance
    {
        get { return Math.Round(this.transform.position.x - initialosPos.x, 2); }
    }

    public bool CouldCalculate
    {
        get { return couldCalculate; }
    }

    public bool CouldApplyForce
    {
        get { return couldApplyForce; }
    }
    #endregion

    // Use this for initialization
    void Start()
    {
        force = 1400.0f; // m * a

        couldCalculate = false;
        couldRestart = false;
        couldApplyForce = true;

        myRigidbody = this.gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        CouldRestart();
    }

    /// <summary>
    /// Keyboard handler
    /// </summary>
    /// <param name="code"> Keyboard code </param>
    public void KeyboardInputHandler(KeyCode code)
    {
        switch (code)
        {
            case KeyCode.Space: ApplyForce(); break;
            case KeyCode.R: RestartCube(); break;
        }
    }

    /// <summary>
    /// Updates the cube start point.
    /// </summary>
    public void UpdateCubeStartPoint(Path path)
    {
        if (couldApplyForce)
        {
            this.transform.position = new Vector3(path.transform.position.x - ((path.transform.localScale.x / 2) - (this.transform.localScale.x / 2)),
                                                  this.transform.position.y,
                                                  this.transform.position.z);
        }
    }

    /// <summary>
    /// Applies force in the cube.
    /// </summary>
    private void ApplyForce()
    {
        if (couldApplyForce)
        {
            startTime = Time.time;
            initialosPos = this.transform.position;

            myRigidbody.AddForce(Vector3.right * force, ForceMode.Force);

            couldApplyForce = false;
            couldCalculate = true;
        }
    }

    /// <summary>
    /// Restarts the cube.
    /// </summary>
    private void RestartCube()
    {
        if (couldRestart)
        {
            this.transform.localPosition = initialosPos;
            myRigidbody.velocity = Vector3.zero;

            couldApplyForce = true;
            couldRestart = false;
            couldCalculate = false;
        }
    }

    /// <summary>
    /// Could restart?
    /// </summary>
    private void CouldRestart()
    {
        couldRestart = this.gameObject.GetComponent<Rigidbody>().velocity.x == 0 && couldCalculate ? true : false;  

        if (myRigidbody.velocity.y < 0) couldRestart = true;
    }
}