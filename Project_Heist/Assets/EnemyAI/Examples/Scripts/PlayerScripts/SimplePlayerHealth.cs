using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// This class is created for the example scene. There is no support for this script.
public class SimplePlayerHealth : HealthManager
{
	public float health = 100f;

	public Transform canvas;
	public Text healthDisplay;
	public GameObject hurtPrefab;
	public float decayFactor = 0.8f;


	private HurtHUD hurtUI;

	private void Awake()
	{
		AudioListener.pause = false;
		hurtUI = this.gameObject.AddComponent<HurtHUD>();
		hurtUI.Setup(canvas, hurtPrefab, decayFactor, this.transform);
		healthDisplay.text = health.ToString();
	}

	public override void TakeDamage(Vector3 location, Vector3 direction, float damage, Collider bodyPart, GameObject origin)
	{
		health -= damage;
		UpdateHealth();

		if (hurtPrefab && canvas)
			hurtUI.DrawHurtUI(origin.transform, origin.GetHashCode());
	}

	public void UpdateHealth()
    {
		if (health > 0f)
		{
			healthDisplay.text = health.ToString();
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
