using UnityEngine;
using System.Collections;


public interface IA
{
	//controlara la direccion
	void Behave();
	//controlara el movimiento
	void Move();
}
public class Avoid :  IA {
	
	//gameobject que modela la bala
	public GameObject Bullet;
	
	private Transform m_tTransform;
	//transform de la nave principal
	private Transform e_tTransform;
	//si hay que disparar
	private bool haveToShoot;
	//clase de la nave
	private EnemyControl objClass;
	//maximas posiciones laterales
	private	float m_fMaxRightPos = 2.0f;
	private float m_fMaxLeftPos = -1.9f;
	
	public Avoid(Transform toControlTransform, Transform enemyTransform, EnemyControl objectClass)
	{
		m_tTransform = toControlTransform;
		e_tTransform = enemyTransform;
		objClass = objectClass;
	}
	
	public void Behave()
	{
		//que mire hacia donde tiene que disparar
		m_tTransform.LookAt(e_tTransform.GetChild(0).GetChild(0));
	}
	
	public void Move()
	{
		float speed = 0.0f;
		float distance = m_tTransform.position.z - e_tTransform.position.z;
		
		speed = (distance - 7.0f);
		
		if(distance < 6.0f)
			objClass.Shoot();
		
		Vector3 posToAdd = m_tTransform.forward * Time.deltaTime * speed;
		Vector3 newPos = m_tTransform.position + posToAdd;
		
		if( newPos.x > m_fMaxRightPos || newPos.x < m_fMaxLeftPos )
			posToAdd.x = 0.0f;
		
		m_tTransform.position += posToAdd;
	}
	

}
