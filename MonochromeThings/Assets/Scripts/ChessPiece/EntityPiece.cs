using System;
using System.Linq;
using Data;
using DG.Tweening;
using Manager;
using ScriptableObject;
using TMPro;
using UnityEngine;

namespace ChessPiece
{
	public class EntityPiece : MonoBehaviour
	{
		[SerializeField] protected EntityPieceData pieceData;
		[SerializeField] protected TextMeshProUGUI turnToMoveText;
		protected Point OriginPos;

		public int turnToMove;
		public bool hasMoved;
		public Point pos;

		public void Init(Point p)
		{
			OriginPos = p;
			hasMoved = false;
			pos = p;
			transform.position = new Vector3(100, 100, 100);
			Move(ChessGameManager.CalWorldPos(pos), 0.25f);
		}

		public virtual void HighlightAvailableTile()
		{
			foreach (var point in pieceData.AvailablePoints)
			{
				MapManager.Instance.ChangeTileColor(pos + point, new Color(1f, 0.2f, 0.6f));
			}
		}
		
		protected virtual Point CalculateActionsPoint()
		{
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

		public virtual void Action()
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

		public void OnPointerClick()
		{
			ChessGameManager.Instance.OnEntityClick(this);
		}
		
		protected void Move(Point p, float duration = 0.5f)
		{
			transform.DOPause();
			transform.DOMove(new Vector3(p.x, p.y, 0), duration).SetEase(Ease.InOutSine);
		}

		public string GetEntityName()
		{
			return pieceData.Name;
		}

		public virtual void Destroy()
		{
			transform.DOPause();
			Destroy(gameObject);
		}
	}
}