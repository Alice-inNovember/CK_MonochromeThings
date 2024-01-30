using TMPro;
using UnityEngine;

namespace Manager
{
    public class ChessMapUIManager : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI turnToSpawn;

        public void SetTurnToSpawn(int turn)
        {
            turnToSpawn.text = turn.ToString();
        }
    }
}
