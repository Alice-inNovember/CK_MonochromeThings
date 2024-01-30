using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Manager;


public class EnemyPiece : MonoBehaviour
{
	public int X;
	public int Y;

	public void Move(int x, int y)
	{
		transform.DOPause();
		transform.DOMove(new Vector3(x, y, 0), 0.5f).SetEase(Ease.InOutSine);
	}
}