using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
	public static void TeleportParticle(string fileName, GameObject gameObject)
	{
		ParticleSystem loadedParticles01 = LoadTeleportFromFile(fileName);
		Instantiate(loadedParticles01, gameObject.transform.position, loadedParticles01.transform.rotation);
	}

	public static ParticleSystem LoadTeleportFromFile(string fileName)
	{
		ParticleSystem loadedTeleport = Resources.Load<ParticleSystem>(fileName);
		if (loadedTeleport == null)
		{
			throw new FileNotFoundException("This file wasn't found");
		}
		return loadedTeleport;
	}

	public static void ChangeMaterial(Renderer renderer, Material material)
	{
		if (renderer.material != null)
		{
			renderer.material = material;
		}
		else
		{
			Debug.LogError("gameObjectMaterial was empty");
		}

	}

}
