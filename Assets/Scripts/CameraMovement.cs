using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {
	
	//velocidad a la que avanza la camara
	public float m_fSpeed;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Move();
	}
	
	void Move()
	{
		transform.position += m_fSpeed * transform.forward * Time.deltaTime;
	}
}
