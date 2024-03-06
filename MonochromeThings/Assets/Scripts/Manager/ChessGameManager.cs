using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ChessPiece;
using ClassTemp;
using Data;
using UnityEngine;
using ScriptableObject;
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
		private PlayerPiece _player;
		private readonly List<EnemyPiece> _enemyPieces = new ();

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
			_player.isSelected = false;
		}

		public Point GetPlayerPos()
		{
			return _player.pos;
		}
		
		private void PlayerTurn(Point p)
		{
			if (_turn == PieceType.Enemy)
				return;
			if (Point.Dist(_player.pos, p) > 1)
				return;
			PlayerMove(p);
			CheckEnemyEncounter();
			StartCoroutine(EnemyTurn());
		}

		private void CheckEnemyEncounter()
		{
			EnemyPiece collidedEnemy = null;
			foreach (var enemy in _enemyPieces.Where(enemy => enemy.pos == _player.pos))
				collidedEnemy = enemy;
			if (collidedEnemy != null)
			{
				_enemyPieces.Remove(collidedEnemy);
				collidedEnemy.Destroy();
				MapManager.Instance.ResetTileColor();
				HighlightEnemyPathTile();
			}
		}
		
		private void PlayerMove(Point p)
		{
			_player.Move(CalWorldPos(p));
			_player.pos = p;
		}

		private void InitEnemy()
		{
			MapManager.Instance.ResetTileAvailability();
			for (int i = 0; i < 5; i++)
			{
				CreateEnemy(0);
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
			var enemy = Instantiate(enemyPrefab).GetComponent<EnemyPiece>();
			enemy.Init(enemyID, p);
			_enemyPieces.Add(enemy);
		}

		IEnumerator EnemyTurn()
		{
			yield return new WaitForSeconds(0.5f);
			MapManager.Instance.ResetTileColor();
			foreach (var enemy in _enemyPieces)
			{
				enemy.Action();
			}
			yield return new WaitForSeconds(0.5f);

			CheckEnemyEncounter();
			EnemySpawn();
			HighlightEnemyPathTile();
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

		public void HighlightEnemyPathTile()
		{
			foreach (var enemy in  _enemyPieces)
			{
				enemy.HighlightAvailableTile();
			}
		}

		public void TileSelect(Point p)
		{
			MapManager.Instance.ResetTileColor();
			HighlightEnemyPathTile();
			if (_player.isSelected)
			{
				_player.isSelected = false;
				PlayerTurn(p);
			}
		}

		public void OnPlayerClick()
		{
			_player.isSelected = true;
			MapManager.Instance.HighlightAvailableTile(_player.pos);
		}

		public static Point CalWorldPos(Point p)
		{
			return (new Point(-(MapManager.MapSize / 2) + p.x, MapManager.MapSize / 2 - p.y));
		}
	}
}