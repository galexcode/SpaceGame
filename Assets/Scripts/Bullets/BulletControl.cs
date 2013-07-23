using UnityEngine;
using System.Collections;

public class BulletControl : MonoBehaviour {
	
	private Vector3 m_vInitialPos;
	// Use this for initialization
	void Start () {
		m_vInitialPos = transform.position;
	}
	
	void FixedUpdate () {
		Debug.DrawRay(transform.position, transform.forward);
		if( (m_vInitialPos - transform.position).magnitude > 10.0f )
			Destroy(gameObject);
	}
	
	void OnTriggerEnter(Collider col)
	{
		//comprobamos que no este chocando con quien envia el misil
		Destroy(gameObject);
		col.SendMessage("HitNormalBullet");
	}
}
