using System;
using System.Linq;
using Data;
using DG.Tweening;
using Manager;
using ScriptableObject;
using UnityEngine;

namespace ChessPiece
{
	public class EnemyPiece : MonoBehaviour
	{
		private EnemyPieceData _pieceData;
		public Point pos;

		public void Init(int enemyID, Point p)
		{
			pos = p;
			_pieceData = EnemyDataManager.Instance.GetEnemyData(enemyID);
			transform.position = new Vector3(100, 100, 100);
			MapManager.Instance.SetTileAvailability(pos, false);
			Move(ChessGameManager.CalWorldPos(pos), 0.25f);
		}
		private Point FindNearestActionsPoint()
		{
			var distances = new float[_pieceData.AvailablePoints.Capacity];
			for (var i = 0; i < _pieceData.AvailablePoints.Capacity; i++)
				distances[i] = Point.Dist(ChessGameManager.Instance.GetPlayerPos(), pos + _pieceData.AvailablePoints[i]);
			var minIndex = Array.IndexOf(distances, Mathf.Min(distances));

			if (MapManager.Instance.IsMapAvailable(pos + _pieceData.AvailablePoints[minIndex]))
				return pos + _pieceData.AvailablePoints[minIndex];
			
			//상점 위치로 이동 시도
			
			for (var i = 0; i < _pieceData.AvailablePoints.Capacity - 1; i++)
			{
				if (MapManager.Instance.IsMapAvailable(pos + _pieceData.AvailablePoints[i]))
					return pos + _pieceData.AvailablePoints[minIndex];
			}
			return pos;
		}
		
		public void Action()
		{
			var p = FindNearestActionsPoint();
			if (MapManager.Instance.IsMapAvailable(p) == false)
			{
				Debug.Log("Cant Move");
				return;
			}
			Move(ChessGameManager.CalWorldPos(p));
			MapManager.Instance.SetTileAvailability(pos, true);
			pos = p;
			MapManager.Instance.SetTileAvailability(pos, false);
		}

		public void HighlightAvailableTile()
		{
			foreach (var point in _pieceData.AvailablePoints.Where(point => MapManager.Instance.IsMapAvailable(pos + point)))
			{
				MapManager.Instance.ChangeTileColor(pos + point, new Color(1f, 0.2f, 0.6f));
			}
		}

		private void Move(Point p, float duration = 0.5f)
		{
			transform.DOPause();
			transform.DOMove(new Vector3(p.x, p.y, 0), duration).SetEase(Ease.InOutSine);
		}

		public void Destroy()
		{
			MapManager.Instance.SetTileAvailability(pos, true);
			transform.DOPause();
			Destroy(gameObject);
		}
	}
}