using UnityEngine;

[System.Serializable]
public class SpawnPoint
{
	public static float spawnPointCooldown;
	public Transform transform;
	float nextSpawnTime = Time.time;
	
	public void Spawn(GameObject character)
	{
		GameObject spawnedCharacter = GameObject.Instantiate(character, transform.position, transform.rotation);
		if (character.tag == "Player")
		{
			GameObject deathcam = GameObject.Find("DeathCam");
			if (deathcam != null)
				deathcam.GetComponent<Camera>().enabled = false;
				deathcam.GetComponent<AudioListener>().enabled = false;
		}
		spawnedCharacter.name = spawnedCharacter.name.Replace("(Clone)", "");
		nextSpawnTime = Time.time + spawnPointCooldown;
	}

	public bool OnCooldown()
	{
		if(Time.time < nextSpawnTime)
		{
			return true;
		}
		return false;
	}
}
