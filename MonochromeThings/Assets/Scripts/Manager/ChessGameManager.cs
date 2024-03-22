using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ChessPiece;
using ClassTemp;
using Data;
using UnityEngine;
using ScriptableObject;
using UnityEditor;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Manager
{
	public class ChessGameManager : MonoBehaviourSingleton<ChessGameManager>
	{
		[SerializeField] private GameObject playerPrefab;
		[SerializeField] private GameObject enemyPrefab;
		[FormerlySerializedAs("enemySpawnPoint")] [SerializeField] private EnemyPieceSpawnPoint enemyPieceSpawnPoint;

		private PieceType _turn;
		private bool _isPlayerSelected;
		private PlayerPiece _player;
		private readonly List<EntityPiece> _enemyPieces = new ();

		private int _turnToSpawn;
		private Point _nextEnemySpawnPoint;

		private void Start()
		{
			InitPlayer();
			InitEnemy();
			GameObject.Find("MapUIManager").GetComponent<MapUIManager>().SetTurnToSpawn(_turnToSpawn);
		}

		private void InitPlayer()
		{
			_turn = PieceType.Player;
			_player = Instantiate(playerPrefab, this.transform).GetComponent<PlayerPiece>();
			_player.pos = new Point(MapManager.MapSize / 2, MapManager.MapSize / 2);
			_isPlayerSelected = false;
		}

		private void InitEnemy()
		{
			MapManager.Instance.ResetTileAvailability();
			for (int i = 0; i < 5; i++)
			{
				CreateEnemy(4);
			}
			_turnToSpawn = -1;
		}

		private void CreateEnemy(int enemyID)
		{
			var p = enemyPieceSpawnPoint.SpawnPoints[Random.Range(0, enemyPieceSpawnPoint.SpawnPoints.Capacity - 1)];
			while (MapManager.Instance.IsMapAvailable(p) == false)
				p = enemyPieceSpawnPoint.SpawnPoints[Random.Range(0, enemyPieceSpawnPoint.SpawnPoints.Capacity - 1)];
			CreateEnemy(enemyID, p);
		}

		private void CreateEnemy(int enemyID, Point p)
		{
			if (MapManager.Instance.IsMapAvailable(p) == false)
				return;
			var enemy = Instantiate(enemyPrefab).GetComponent<EntityPiece>();
			enemy.Init(p);
			_enemyPieces.Add(enemy);
		}

		public void TileSelect(Point p)
		{
			if (_turn == PieceType.Enemy)
				return;
			if (_isPlayerSelected)
				PlayerAction(p);
			MapManager.Instance.ResetTileColor();
		}
		
		public void OnPlayerClick()
		{
			if (_turn == PieceType.Enemy)
				return;
			_isPlayerSelected = true;
			MapManager.Instance.ResetTileColor();
			_player.HighlightAvailableTile();
		}

		public void OnEntityClick(EntityPiece piece)
		{
			if (_turn == PieceType.Enemy)
				return;
			if (_isPlayerSelected)
				TileSelect(piece.pos);
			else
			{
				MapManager.Instance.ResetTileColor();
				piece.HighlightAvailableTile();
			}
		}

		public Point GetPlayerPos()
		{
			return _player.pos;
		}

		private void PlayerAction(Point p)
		{
			_isPlayerSelected = false;
			if (Point.Dist(_player.pos, p) > 1)
				return;
			PlayerMove(p);
			var encounter = CheckEnemyEncounter();
			if (encounter != null)
			{
				_enemyPieces.Remove(encounter);
				encounter.Destroy();
			}
			StartCoroutine(EnemyTurn());
		}

		private EntityPiece CheckEnemyEncounter()
		{
			EntityPiece encounter = null;
			foreach (var enemy in _enemyPieces.Where(enemy => enemy.pos == _player.pos))
				encounter = enemy;
			return encounter;
		}

		private EntityPiece EntityMove(int entityID)
		{
			EntityPiece encounter = null;

			foreach (var enemy in _enemyPieces)
			{
				if (enemy.GetEntityName() != EntityDataManager.Instance.GetEntityData(0).Name)
					enemy.Action();
				encounter = CheckEnemyEncounter();
				if (encounter != null)
					MapManager.Instance.SetTileAvailability(encounter.pos, false);
			}
			return encounter;
		}
		
		IEnumerator EnemyTurn()
		{
			EntityPiece encounter = null;
			yield return new WaitForSeconds(0.5f);
			MapManager.Instance.ResetTileColor();

			encounter = EntityMove(0);
			encounter = EntityMove(2);
			encounter = EntityMove(4);
			encounter = EntityMove(1);
			encounter = EntityMove(5);
			encounter = EntityMove(3);
			yield return new WaitForSeconds(0.5f);

			//전투
			if (encounter != null)
			{
				MapManager.Instance.SetTileAvailability(encounter.pos, true);
				_enemyPieces.Remove(encounter);
				encounter.Destroy();
			}
			EnemySpawn();
		}
		private void PlayerMove(Point p)
		{
			_player.Move(CalWorldPos(p));
			_player.pos = p;
		}


		private Point CalNextSpawnPoint()
		{
			_nextEnemySpawnPoint = enemyPieceSpawnPoint.SpawnPoints[Random.Range(0, enemyPieceSpawnPoint.SpawnPoints.Capacity - 1)];
			while (MapManager.Instance.IsMapAvailable(_nextEnemySpawnPoint) == false) 
				_nextEnemySpawnPoint = enemyPieceSpawnPoint.SpawnPoints[Random.Range(0, enemyPieceSpawnPoint.SpawnPoints.Capacity - 1)];
			return _nextEnemySpawnPoint;
		}
		
		private void EnemySpawn()
		{
			if (_turnToSpawn == -1)
			{
				_turnToSpawn = 4;
				_nextEnemySpawnPoint = CalNextSpawnPoint();
				MapManager.Instance.SetTileAvailability(_nextEnemySpawnPoint, false);
				MapManager.Instance.SetTileWarning(_nextEnemySpawnPoint, true);
			}
			_turnToSpawn--;
			if (_turnToSpawn == 0)
			{
				MapManager.Instance.SetTileAvailability(_nextEnemySpawnPoint, true);
				MapManager.Instance.SetTileWarning(_nextEnemySpawnPoint, false);
				//나중에 수정
				CreateEnemy(0, _nextEnemySpawnPoint);

				_turnToSpawn = 4;
				_nextEnemySpawnPoint = CalNextSpawnPoint();
				MapManager.Instance.SetTileAvailability(_nextEnemySpawnPoint, false);
				MapManager.Instance.SetTileWarning(_nextEnemySpawnPoint, true);
			}
			GameObject.Find("MapUIManager").GetComponent<MapUIManager>().SetTurnToSpawn(_turnToSpawn);
		}

		public static Point CalWorldPos(Point p)
		{
			return (new Point(-(MapManager.MapSize / 2) + p.x, MapManager.MapSize / 2 - p.y));
		}
	}
}