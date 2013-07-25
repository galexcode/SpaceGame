using UnityEngine;
using System.Collections;

public class RocketControl : MonoBehaviour {

	private Vector3 m_vInitialPos;
	private Transform m_tParentTransform;
	private GameObject e_gObjective;
	
	// Use this for initialization
	void Start () {
		m_tParentTransform = transform.parent;
		m_vInitialPos = m_tParentTransform.position;
		e_gObjective = FindNearestEnemie();
	}
	
	void FixedUpdate () {
		
		//movemos el cohete hacia el objetivo		
		m_tParentTransform.position += m_tParentTransform.forward * Time.deltaTime * 10.0f;

		if(e_gObjective != null)
		{
			m_tParentTransform.forward += Vector3.Lerp(m_tParentTransform.forward, (e_gObjective.transform.position - m_tParentTransform.position), Time.deltaTime * 3.0f);
			//m_tParentTransform.forward = (e_gObjective.transform.position - m_tParentTransform.position).normalized;
		}
		
		if( (m_vInitialPos - transform.position).magnitude > 10.0f )
			Destroy(m_tParentTransform.gameObject);
	}
	
	void OnTriggerEnter(Collider col)
	{
		//comprobamos que no este chocando con quien envia el misil
		Destroy(gameObject);
		col.SendMessage("HitNormalBullet");
	}
	
	GameObject FindNearestEnemie()
	{
		GameObject[] Enemies = GameObject.FindGameObjectsWithTag("Enemies");

		int closestEnemie = 0;
		
		if(Enemies.Length >= 1)
		{
			//buscamos el enemigo mas cercano
			for(int index=0; index < Enemies.Length; index++)
			{
				if(Enemies[index].transform.position.z < Enemies[closestEnemie].transform.position.z)
					closestEnemie = index;
			}
		}
		else
			return null;
		
		//devolvemos el hijo, que es la nave, el gameobject parent solo es para la rotacion
		return Enemies[closestEnemie].transform.GetChild(0).gameObject;
	}
}
