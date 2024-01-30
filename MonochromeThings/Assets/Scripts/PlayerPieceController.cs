using System.Collections;
using System.Collections.Generic;
using ClassTemp;
using DG.Tweening;
using UnityEngine;
using Manager;
using Unity.Mathematics;


public class PlayerPieceController : MonoBehaviour
{
	private bool _isMoving;

	private void Start()
	{
		_isMoving = false;
	}

	public void Move(int x, int y)
	{
		_isMoving = true;
		transform.DOMove(new Vector3(x, y, 0), 0.5f).SetEase(Ease.InOutSine).OnComplete(() =>
		{
			_isMoving = false;
		});
	}

	public void OnPointerClick()
	{
		if (_isMoving == true)
			return;
		ChessMapManager.Instance.OnPlayerClick();
	}
}