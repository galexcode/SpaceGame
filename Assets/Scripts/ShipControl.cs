using UnityEngine;
using System.Collections;

public class ShipControl : MonoBehaviour {
	
	//velocidad de la nave
	public float m_fSpeed;
	//cubo que tenemos como referencia
	public GameObject e_gCube;
	//maximo angulo de cabeceo lateral
	public float m_fMaxAngle;
	//es el gameobject dentro del cual va la camara, controla el movimiento del mundo.
	public GameObject e_gBossMovement;
	//distancia minima a la que la nave estara de la camara (de lo que ve el usuario)
	public float m_fMinDistanceToBoss;
	//distancia maxima a la que la nave estara de la camara
	public float m_fMaxDistanceToBoss;
	//velocidad a la que la nave vuelve a su rotacion original
	public float m_fRotationSpeed;
	//angulo maximo para apuntar hacia los lados
	public float m_fMaxHeadAngle;
	//las balas
	public GameObject Bullet;
	//cohetes
	public GameObject Rocket;
	//velocidad de las balas
	public float m_fBulletSpeed;
	//textura del cursor
	public Texture2D e_tCursorTexture;
	//modo de manejo de la nave: cabeceo lateral o por raton
	public bool m_bMouseController;
	//periodo para disparar
	public float m_fShootPeriod;
	//vida de la nave
	public int m_iLife;
	
	//transform que modificaremos (el del padre)
	private Transform m_gMyParentTransform;
	//Vector direccion que apunta hacia donde tenemos que ir
	private Vector3 m_vHeading;
	//puntos laterales
	private float m_fMaxRightPos;
	private float m_fMaxLeftPos;
	//indica si la nave mira a izquierda o derecha
	private int m_iLookingTo;
	//m_iLookingTo tendra uno de estos valores, segun hacia donde mire
	private const int left = -1;
	private const int right = 1;
	//para controlar la distancia con el cubo
	private RaycastHit m_rHit;
	//tiempo desde el ultimo disparo
	private float m_fTimeSinceLastShoot;
	
	
	// Use this for initialization
	void Start () {
		m_gMyParentTransform = transform.parent;
		m_vHeading = Vector3.zero;
		m_fMaxRightPos = 2.2f;
		m_fMaxLeftPos = -2.1f;
		//establecemos el cursor si estamos en ese modo
		if(m_bMouseController)
			Cursor.SetCursor(e_tCursorTexture, Vector2.zero, CursorMode.Auto);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void FixedUpdate()
	{
		SetDir();
		Move();
		Shoot();
	}
	
	void Shoot()
	{
		m_fTimeSinceLastShoot += Time.deltaTime;
		if(Input.GetButtonDown("Fire1") && m_fTimeSinceLastShoot >= m_fShootPeriod)
		{
			GameObject bullet;
        	bullet = (GameObject)Instantiate(Bullet, transform.position + transform.forward * 0.4f, Quaternion.Euler(Vector3.zero));
			Physics.IgnoreCollision(collider, bullet.collider);
			Physics.IgnoreLayerCollision(bullet.layer, bullet.layer);
        	//le damos velocidad a la bala
        	bullet.rigidbody.AddForce(transform.forward * m_fBulletSpeed);
			m_fTimeSinceLastShoot = 0.0f;
		}
		if(Input.GetButtonDown("Fire2") && m_fTimeSinceLastShoot >= m_fShootPeriod)
		{
			GameObject rocket;
        	rocket = (GameObject)Instantiate(Rocket, transform.position + transform.forward * 0.4f, transform.rotation);
			rocket = rocket.transform.GetChild(0).gameObject;
			Physics.IgnoreCollision(collider, rocket.collider);
			Physics.IgnoreLayerCollision(rocket.layer, rocket.layer);
        	//al cohete no le damos velocidad porque lleva su propio sistema de control y movimiento
        	
			m_fTimeSinceLastShoot = 0.0f;
		}
	}
	
	void SetDir()
	{
		SetM_vHeading();
		//controlamos la nave segun el modo en que estemos
		if(m_bMouseController)
			SetYRotation();
		else
			SetZRotation();
	}
	
	void SetYRotation()
	{	
		Plane playerPlane = new Plane(Vector3.up, m_gMyParentTransform.position);
    	Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
    	float hitdist = 0.0f;
 
   	 	if (playerPlane.Raycast(ray, out hitdist)) 
		{
        	Vector3 targetPoint = ray.GetPoint(hitdist);
        	Quaternion targetRotation = Quaternion.LookRotation(targetPoint - m_gMyParentTransform.position);
 			float angle = targetRotation.eulerAngles.y;
			
			//limitamos el angulo de movimiento
			if( ( angle > ( 360.0f - m_fMaxHeadAngle ) ) ||  ( angle < m_fMaxHeadAngle  ) )
        		transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10.0f); //10.0f es la velocidad de rotacion
		}

	}
	
	void SetM_vHeading()
	{
		m_vHeading.Set(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
	}
	
	void SetZRotation()
	{
		float Vertical = m_vHeading.z;
		
		m_vHeading.z = 0.0f;
		
		//el vector perpendicular tendra direccion z. Si rota hacia la izquierda, el sentido sera
		//positivo, si rota hacia la derecha, sera negativo
		m_iLookingTo = ( Vector3.Cross(Vector3.up, transform.up).z > 0.0f ) ? left : right;
		
		float Angle = m_iLookingTo * Vector3.Angle(Vector3.up, transform.up);
				
		if( ( m_vHeading.x > 0.0f && Angle <= m_fMaxAngle ) || ( m_vHeading.x < 0.0f && Angle >= -m_fMaxAngle ) )
			//si el angulo que pasamos es positivo, gira hacia la izquierda
			transform.RotateAround(Vector3.forward, Time.deltaTime * - m_vHeading.x);
		
		//Volvemos a poner la nave en su rotacion original si no hay entrada del usuario. Dejamos un margen, ya que
		//el angulo no volvera a ser cero exactamente
		if( m_vHeading.x == 0.0f && Mathf.Abs(Angle) > 5.0f)
			transform.RotateAround(Vector3.forward, Time.deltaTime * m_iLookingTo * m_fRotationSpeed);
		
		m_vHeading.z = Vertical;
		
	}
	
	void Move()
	{
		Physics.Raycast(m_gMyParentTransform.position, -m_gMyParentTransform.forward, out m_rHit, 10.0f);
		Vector3 posToAdd = m_fSpeed * m_vHeading * Time.deltaTime;
		Vector3 newPos = m_gMyParentTransform.position + posToAdd;
		//limitamos el movimiento lateral de la nave
		if( newPos.x > m_fMaxRightPos || newPos.x < m_fMaxLeftPos )
			posToAdd.x = 0.0f;
		
		//limitamos el movimiento hacia delante y hacia atras de la nave
		if( ( m_rHit.distance <= m_fMinDistanceToBoss && posToAdd.z < 0.0f ) || ( m_rHit.distance >= m_fMaxDistanceToBoss && posToAdd.z > 0.0f ) )
			posToAdd.z = 0.0f;
			
		m_gMyParentTransform.position += posToAdd;
	}
	
	void HitNormalBullet()
	{
		m_iLife--;
		if(m_iLife <= 0.0f)
			Destroy(gameObject);
	}
}
