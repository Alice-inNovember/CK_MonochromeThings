using System;
using Data;
using DG.Tweening;
using Manager;
using UnityEngine;

namespace ChessPiece
{
	public class EnemyPiece : MonoBehaviour
	{
		public Point pos;
		public int typeID = 0;

		public void Init(Point p)
		{
			pos = p;
			transform.position = new Vector3(100, 100, 100);
			Move(ChessGameManager.CalWorldPos(pos));
		}
		private Point FindNearestActionsPoint()
		{
			var availablePoints = EnemyDataManager.Instance.GetEnemyData(typeID).AvailablePoints;
			var distances = new float[availablePoints.Capacity];
			for (var i = 0; i < availablePoints.Capacity; i++)
				distances[i] = Point.Dist(ChessGameManager.Instance.GetPlayerPos(), pos + availablePoints[i]);
			var minIndex = Array.IndexOf(distances, Mathf.Min(distances));

			return pos + availablePoints[minIndex];
		}

		public void Action()
		{
			var p = FindNearestActionsPoint();
			if (MapManager.Instance.GetPieceType(p) == PieceType.Enemy)
				return;
			Move(ChessGameManager.CalWorldPos(p));
			MapManager.Instance.UpdateTileState(pos, PieceType.Empty);
			pos = p;
			MapManager.Instance.UpdateTileState(pos, PieceType.Enemy);
		}

		private void Move(Point p)
		{
			transform.DOPause();
			transform.DOMove(new Vector3(p.x, p.y, 0), 0.5f).SetEase(Ease.InOutSine);
		}

		public void Destroy()
		{
			transform.DOPause();
			Destroy(gameObject);
		}
	}
}