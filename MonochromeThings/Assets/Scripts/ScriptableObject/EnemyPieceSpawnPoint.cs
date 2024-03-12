using System.Collections.Generic;
using Data;
using UnityEngine;

namespace ScriptableObject
{
	[CreateAssetMenu(fileName = "EnemySpawnPoint", menuName = "Scriptable Object/Enemy Spawn Point",
		order = int.MaxValue)]
	[System.Serializable]
	public class EnemyPieceSpawnPoint : UnityEngine.ScriptableObject
	{
		[SerializeField] private List<Point> spawnPoints;
		public List<Point> SpawnPoints => spawnPoints;
	}
}