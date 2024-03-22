using System;
using Data;
using Manager;
using UnityEngine;

namespace ChessPiece
{
    public class EntityPieceBargain : EntityPiece
    {
        protected override Point CalculateActionsPoint()
        {
            //이동한 엔티티에 따라서 이동 함수 구현
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
