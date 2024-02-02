using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ChessPiece;
using ClassTemp;
using Data;
using UnityEngine;
using DG.Tweening;
using ScriptableObject;
using Random = UnityEngine.Random;

namespace Manager
{
	public class ChessGameManager : MonoBehaviourSingleton<ChessGameManager>
	{
		[SerializeField] private GameObject playerPrefab;
		[SerializeField] private GameObject enemyPrefab;
		[SerializeField] private EnemySpawnPoint enemySpawnPoint;

		private PieceType _turn;
		private PlayerPiece _player;
		private readonly List<EnemyPiece> _enemyPieces = new ();

		private int _turnToSpawn;

		private void Start()
		{
			InitPlayer();
			InitEnemy();
			_turnToSpawn = 4;
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
			StartCoroutine(EnemyTurn());
		}
		
		private void PlayerMove(Point p)
		{
			_player.Move(CalWorldPos(p));
			MapManager.Instance.UpdateTileState(_player.pos, PieceType.Empty);
			_player.pos = p;
			MapManager.Instance.UpdateTileState(_player.pos, PieceType.Player);
			EnemyPiece collidedEnemy = null;
			foreach (var enemy in _enemyPieces.Where(enemy => _player.pos == enemy.pos))
			{
				collidedEnemy = enemy;
			}
			if (collidedEnemy != null)
			{
				_enemyPieces.Remove(collidedEnemy);
				var enemyTransform = collidedEnemy.transform;
				enemyTransform.DOPause();
				Destroy(enemyTransform.gameObject);
			}
		}

		private void InitEnemy()
		{
			MapManager.Instance.ResetTileState();
			for (int i = 0; i < 10; i++)
			{
				CreateEnemy();
			}
			// CreateEnemy(new Point(0, 0));
			// CreateEnemy(new Point(0, MapManager.MapSize - 1));
			// CreateEnemy(new Point(MapManager.MapSize - 1, 0));
			// CreateEnemy(new Point(MapManager.MapSize - 1, MapManager.MapSize - 1));
		}

		private void CreateEnemy()
		{
			var p = enemySpawnPoint.SpawnPoints[Random.Range(0, enemySpawnPoint.SpawnPoints.Capacity - 1)];
			while (MapManager.Instance.IsMapEmpty(p) == false)
				p = enemySpawnPoint.SpawnPoints[Random.Range(0, enemySpawnPoint.SpawnPoints.Capacity - 1)];
			CreateEnemy(p);
		}

		private void CreateEnemy(Point p)
		{
			if (MapManager.Instance.IsMapEmpty(p) == false)
				return;
			var enemy = Instantiate(enemyPrefab).GetComponent<EnemyPiece>();
			enemy.Init(p);
			_enemyPieces.Add(enemy);
		}

		IEnumerator EnemyTurn()
		{
			yield return new WaitForSeconds(0.5f);
			foreach (var enemy in _enemyPieces)
			{
				enemy.Action();
				yield return new WaitForSeconds(0.5f);
			}
			
			EnemyPiece collidedEnemy = null;
			foreach (var enemy in _enemyPieces.Where(enemy => enemy.pos == _player.pos))
				collidedEnemy = enemy;
			if (collidedEnemy != null)
			{
				_enemyPieces.Remove(collidedEnemy);
				collidedEnemy.Destroy();
			}
			
			EnemySpawn();
		}

		private void EnemySpawn()
		{
			_turnToSpawn--;
			if (_turnToSpawn <= 0)
			{
				CreateEnemy();
				_turnToSpawn = 4;
			}
			GameObject.Find("MapUIManager").GetComponent<MapUIManager>().SetTurnToSpawn(_turnToSpawn);
		}

		public void TileSelect(Point p)
		{
			MapManager.Instance.ResetTileColor();
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