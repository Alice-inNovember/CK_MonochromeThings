using Data;
using UnityEngine;
using DG.Tweening;
using Manager;
using UnityEngine.Serialization;

public class EnemyPiece : MonoBehaviour
{
	public Point pos;
	public int typeID = 0;
	public void Move(Point p)
	{
		transform.DOPause();
		transform.DOMove(new Vector3(p.x, p.y, 0), 0.5f).SetEase(Ease.InOutSine);
	}
}