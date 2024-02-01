using System.Collections;
using System.Collections.Generic;
using ClassTemp;
using Data;
using DG.Tweening;
using UnityEngine;
using Manager;
using Unity.Mathematics;


public class PlayerPiece : MonoBehaviour
{
	private bool _isMoving;

	private void Start()
	{
		_isMoving = false;
	}

	public void Move(Point p)
	{
		_isMoving = true;
		transform.DOMove(new Vector3(p.x, p.y, 0), 0.5f).SetEase(Ease.InOutSine).OnComplete(() =>
		{
			_isMoving = false;
		});
	}

	public void OnPointerClick()
	{
		if (_isMoving == true)
			return;
		ChessGameManager.Instance.OnPlayerClick();
	}
}