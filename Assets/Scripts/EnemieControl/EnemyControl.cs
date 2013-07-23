using UnityEngine;
using System.Collections;

public class EnemyControl : MonoBehaviour {
	
	//vida que tendra
	public int m_iLife;
	public GameObject e_gBullet;
	//tiempo entre disparos
	public float m_fShootPeriod;
	//velocidad de la bala
	public float m_fBulletSpeed;
	
	//el dios de los enemigos que se encarga de crearlos
	private GodCreation e_gMyGod;
	private Transform m_tMyParentTransform;
	private Transform e_tPrincipalTransform;
	private IA m_iaBehave;
	private float m_fTimeSinceLastShoot;
	
	// Use this for initialization
	void Start () {
		m_tMyParentTransform = transform.parent;
		//asignamos la nave principal
		e_tPrincipalTransform = GameObject.FindGameObjectWithTag("ShipParent").transform;
		m_iaBehave = new Avoid(transform, e_tPrincipalTransform, this);
		m_fTimeSinceLastShoot = m_fShootPeriod;
		//le asignamos a su Dios
		e_gMyGod = (GodCreation)GameObject.FindWithTag("EvilGod").GetComponent("GodCreation");
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		m_iaBehave.Behave();
		m_iaBehave.Move();
	}
	
	void HitNormalBullet()
	{
		m_iLife--;
		if(m_iLife <= 0.0f)
		{
			//Informamos a Dios de que hemos muerto
			e_gMyGod.BasicEnemieDown();
			Destroy(transform.parent.gameObject);
		}
	}
	
	public void Shoot()
	{		
		m_fTimeSinceLastShoot += Time.deltaTime;
		
		if(m_fTimeSinceLastShoot >= m_fShootPeriod)
		{
			GameObject bullet;
			//le sumamos transform.forward * 0.4f para que la bala salga de delante de la nave
        	bullet = (GameObject)Instantiate(e_gBullet, transform.position + transform.forward * 0.4f, Quaternion.Euler(Vector3.zero));
			Physics.IgnoreCollision(collider, bullet.collider);
			Physics.IgnoreLayerCollision(bullet.layer, bullet.layer);
			Physics.IgnoreLayerCollision(bullet.layer, LayerMask.NameToLayer("CameraWall"));
        	//le damos velocidad a la bala
        	bullet.rigidbody.AddForce(transform.forward  * m_fBulletSpeed);
			m_fTimeSinceLastShoot = 0.0f;
		}
	}
}
