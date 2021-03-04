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

	private HurtHUD hurtUI;

	private void Awake()
	{
		AudioListener.pause = false;
		hurtUI = this.gameObject.AddComponent<HurtHUD>();
		hurtUI.Setup(canvas, hurtPrefab, decayFactor, this.transform);
		maxHealth = health;
	}

	public override void TakeDamage(Vector3 location, Vector3 direction, float damage, Collider bodyPart, GameObject origin)
	{
		health -= damage;
		UpdateHealth();

		if (hurtPrefab && canvas)
			hurtUI.DrawHurtUI(origin.transform, origin.GetHashCode());
	}

	void UpdateHealth()
    {
		if (health > 0f)
		{
			healthDisplay.color = Color.Lerp(Color.white,Color.red,Mathf.Lerp(1,0,health/maxHealth));
			healthDisplay.text= health.ToString();
		}
		else if (!dead)
		{
			dead = true;
			StartCoroutine("ReloadScene");
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

		SceneManager.LoadScene(0);
	}
}
