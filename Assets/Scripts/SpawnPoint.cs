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
