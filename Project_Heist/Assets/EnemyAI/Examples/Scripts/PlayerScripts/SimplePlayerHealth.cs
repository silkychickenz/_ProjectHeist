using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

// This class is created for the example scene. There is no support for this script.
public class SimplePlayerHealth : HealthManager
{
	public float health = 100f;
	float maxHealth;

	public Transform canvas;
	public GameObject hurtPrefab;
	public TMP_Text healthDisplay;
	public float decayFactor = 0.8f;
	public float HealthRegenStartTime = 2f, healthRegenPerSec = 10f;
	private float healthRegenTimer = 2, healthInt;

	private HurtHUD hurtUI;
	private float lastHealth;
	private void Awake()
	{
		AudioListener.pause = false;
		hurtUI = this.gameObject.AddComponent<HurtHUD>();
		hurtUI.Setup(canvas, hurtPrefab, decayFactor, this.transform);
		maxHealth = health;
		healthRegenTimer = HealthRegenStartTime;


	}

	public override void TakeDamage(Vector3 location, Vector3 direction, float damage, Collider bodyPart, GameObject origin)
	{
		
		health -= damage;
		healthRegenTimer = 0;
		
		UpdateHealth();
		
		
		
		if (hurtPrefab && canvas)
			hurtUI.DrawHurtUI(origin.transform, origin.GetHashCode());

	}

	void UpdateHealth()
    {
		if (health > 0f)
		{
			Mathf.RoundToInt(health);
			healthDisplay.color = Color.Lerp(Color.white,Color.red,Mathf.Lerp(1,0,health/maxHealth));
			healthInt = (int)health;
			healthDisplay.text= healthInt.ToString();
			
		}
		else if (!dead)
		{
			dead = true;
			StartCoroutine("ReloadScene");
		}
	}

	public void HealthRegen()
	{	
		// timer until health regen starts
		if (healthRegenTimer <= HealthRegenStartTime)
		{
			healthRegenTimer +=  Time.deltaTime;
		}
		
		// start the health regen
		if (healthRegenTimer >= HealthRegenStartTime && health <= maxHealth)
		{
			Debug.Log(healthRegenTimer);
			health += healthRegenPerSec * Time.deltaTime;
			
			UpdateHealth();
		}
	}

	private IEnumerator ReloadScene()
	{
		SendMessage("PlayerDead", SendMessageOptions.DontRequireReceiver);
		yield return new WaitForSeconds(0.5f);
		canvas.gameObject.SetActive(false);
		AudioListener.pause = true;
		Camera.main.clearFlags = CameraClearFlags.SolidColor;
		Camera.main.backgroundColor = Color.black;
		Camera.main.cullingMask = LayerMask.GetMask();

		yield return new WaitForSeconds(1);

		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	
}
