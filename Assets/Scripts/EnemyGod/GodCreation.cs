using UnityEngine;
using System.Collections;

public class GodCreation : MonoBehaviour {
	
	public GameObject e_gBasicEnemie;
	//la nave principal que controla el jugador
	public GameObject e_gPrincipalShip;
	
	//numero de enemigos basicos en pantalla
	private int m_iNumberOfBasicEnemies;
	private GameObject e_eBasicUnitEnemie;
	
	// Use this for initialization
	void Start () {
		StartCoroutine(EnemieCreator());
	}
	
	// Update is called once per frame
	void Update () {
		/*
		if(m_iNumberOfBasicEnemies < 1)
			CreateBasicEnemie();
			*/
	}
	
	void CreateBasicEnemie()
	{
		e_eBasicUnitEnemie = (GameObject)Instantiate(e_gBasicEnemie, new Vector3(e_gPrincipalShip.transform.position.x, e_gPrincipalShip.transform.position.y,
			e_gPrincipalShip.transform.position.z + 10.0f), Quaternion.Euler(Vector3.zero));
		m_iNumberOfBasicEnemies++;
	}
	
	//con rutinas asi no tenemos que comprobar en cada update si se ha destruido el enemigo,
	//lo comprobamos cada 0.1 segundos
	IEnumerator EnemieCreator()
	{
		while(true)
		{
			if(m_iNumberOfBasicEnemies < 1)
				CreateBasicEnemie();
			yield return new WaitForSeconds(0.1f);
		}
	}
	//cada vez que una nave sea destruida, invocara esta funcion.
	public void BasicEnemieDown()
	{
		m_iNumberOfBasicEnemies--;
	}
}
