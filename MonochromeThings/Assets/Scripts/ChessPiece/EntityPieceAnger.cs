using System;
using System.Linq;
using Data;
using DG.Tweening;
using Manager;
using ScriptableObject;
using UnityEngine;

namespace ChessPiece
{
	public class EntityPieceAnger : EntityPiece
	{
		protected override Point CalculateActionsPoint()
		{
			//플레이어와 Accept(킹) 사이의 거리가 2.8초과면 리턴
			var distances = new float[pieceData.AvailablePoints.Capacity];
			for (var i = 0; i < pieceData.AvailablePoints.Capacity; i++)
				distances[i] = Point.Dist(ChessGameManager.Instance.GetPlayerPos(), pos + pieceData.AvailablePoints[i]);
			var minIndex = Array.IndexOf(distances, Mathf.Min(distances));

			//이동하려는 위치가 가능하다면 리턴
			if (MapManager.Instance.IsMapAvailable(pos + pieceData.AvailablePoints[minIndex]))
				return pos + pieceData.AvailablePoints[minIndex];

			//이동 반경 내 가능한 아무 위치로 이동
			for (var i = 0; i < pieceData.AvailablePoints.Capacity - 1; i++)
			{
				if (MapManager.Instance.IsMapAvailable(pos + pieceData.AvailablePoints[i]))
					return pos + pieceData.AvailablePoints[minIndex];
			}
			return pos;
		}

		public override void Action()
		{
			var p = CalculateActionsPoint();
			if (MapManager.Instance.IsMapAvailable(p) == false)
				Debug.Log("Cant Move");
			else
			{
				Move(ChessGameManager.CalWorldPos(p));
				pos = p;
			}
		}
	}
}