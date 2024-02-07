using System;
using System.Collections.Generic;
using Data;
using DG.Tweening;
using Manager;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ChessPiece
{
	public class EnemyPiece : MonoBehaviour
	{
		private List<Point> _availablePoints;
		public Point pos;
		public int typeID = 1;

		public void Init(Point p)
		{
			pos = p;
			_availablePoints = EnemyDataManager.Instance.GetEnemyData(typeID).AvailablePoints;
			transform.position = new Vector3(100, 100, 100);
			Move(ChessGameManager.CalWorldPos(pos), 0.25f);
		}
		private Point FindNearestActionsPoint()
		{
			var distances = new float[_availablePoints.Capacity];
			for (var i = 0; i < _availablePoints.Capacity; i++)
				distances[i] = Point.Dist(ChessGameManager.Instance.GetPlayerPos(), pos + _availablePoints[i]);
			var minIndex = Array.IndexOf(distances, Mathf.Min(distances));

			if (MapManager.Instance.IsMapAvailable(pos + _availablePoints[minIndex]))
				return pos + _availablePoints[minIndex];
			
			//상점 위치로 이동 시도
			
			for (var i = 0; i < _availablePoints.Capacity - 1; i++)
			{
				if (MapManager.Instance.IsMapAvailable(pos + _availablePoints[i]))
					return pos + _availablePoints[minIndex];
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
			foreach (var point in _availablePoints)
			{
				if (MapManager.Instance.IsMapAvailable(pos + point)) 
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