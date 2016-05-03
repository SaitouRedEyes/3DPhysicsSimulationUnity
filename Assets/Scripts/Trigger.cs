using UnityEngine;
using System;
using System.Collections;

public class Trigger : MonoBehaviour 
{
	private bool collided;
	private float colliderTime;
	private Cube cube;

    #region Getters and Setters
    public bool Collided
    {
        get { return collided; }
    }

    public float ColliderTime
    {
        get { return colliderTime; }
    }
    #endregion

    // Use this for initialization
	void Start () 
	{	
		collided = false;
		colliderTime = 0;
		
		cube = (Cube)GameObject.FindObjectOfType(typeof(Cube));
	}
	
	// Update is called once per frame
	void Update () 
	{
        if (cube.CouldApplyForce) collided = false;
	}
	
	void OnTriggerEnter(Collider c)
	{
		collided = true;
		colliderTime = (float)Math.Round(Time.time - cube.startTime, 2);
	}
}