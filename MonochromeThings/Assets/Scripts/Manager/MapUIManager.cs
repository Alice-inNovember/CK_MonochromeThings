using TMPro;
using UnityEngine;

namespace Manager
{
    public class MapUIManager : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI turnToSpawn;

        public void SetTurnToSpawn(int turn)
        {
            turnToSpawn.text = turn.ToString();
        }
    }
}
