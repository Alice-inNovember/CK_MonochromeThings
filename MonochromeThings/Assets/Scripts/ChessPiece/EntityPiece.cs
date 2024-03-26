﻿using ChessPiece.AI;
using Data;
using DG.Tweening;
using Manager;
using ScriptableObject;
using UnityEngine;

namespace ChessPiece
{
	public enum EntityType
	{
		Meme,
		Denial,
		Anger,
		Bargain,
		Depress,
		Accept,
		Store,
		ShelterSafe,
		ShelterCaptured
	}

	public class EntityPiece : MonoBehaviour
	{
		private EntityData _data;
		private EntityAi _ai;
		public EntityType Type { get; private set; }
		public Point Pos{ get; private set; }
		public int UniqueNbr{ get; private set; }
		public bool HasMoved{ get; set; }
		public int TurnToMove{ get; private set; }

		public void Init(EntityType type, Point p)
		{
			_data = ChessDataManager.Instance.GetEntityData(type);
			_ai = ChessDataManager.Instance.GetEntityAi(type);
			Type = type;
			Pos = p;
			UniqueNbr = _data.UniqueNbr;
			HasMoved = false;
			TurnToMove = Random.Range(_data.MinTurnToMove, _data.MaxTurnToMove + 1);
			Move(ChessGameManager.CalWorldPos(Pos), 0.25f);
		}
		public void Init(EntityType type, int uniqueNbr, EntityPiece subject)
		{
			_data = ChessDataManager.Instance.GetEntityData(type);
			_ai = ChessDataManager.Instance.GetEntityAi(type);
			Type = type;
			Pos = subject.Pos;
			UniqueNbr = subject.UniqueNbr;
			HasMoved = subject.HasMoved;
			TurnToMove = subject.TurnToMove;
		}

		public void HighlightAvailableTile()
		{
			foreach (var point in _data.AvailablePoints)
			{
				MapManager.Instance.ChangeTileColor(Pos + point, new Color(1f, 0.2f, 0.6f));
			}
		}

		public EntityPiece Action()
		{
			EntityPiece encounter = null;
			HasMoved = true;
			if (TurnToMove <= 0)
			{
				var p = _ai.CalculateActionsPoint(_data, Pos);
				if (MapManager.Instance.IsMapAvailable(p) == false)
					Debug.Log("Cant Move");
				else
				{
					Move(ChessGameManager.CalWorldPos(p));
					if(p != ChessGameManager.Instance.GetPlayerPos())
						encounter = ChessGameManager.Instance.GetEntity(p);
					Pos = p;
				}
				TurnToMove = Random.Range(_data.MinTurnToMove, _data.MaxTurnToMove + 1);
			}
			else
				TurnToMove--;
			return encounter;
		}

		public virtual void Destroy()
		{
			transform.DOPause();
			Destroy(gameObject);
		}

		public void OnPointerClick()
		{
			ChessGameManager.Instance.OnEntityClick(this);
		}

		private void Move(Point p, float duration = 0.5f)
		{
			transform.DOPause();
			transform.DOMove(new Vector3(p.x, p.y, 0), duration).SetEase(Ease.InOutSine);
		}

	}
}