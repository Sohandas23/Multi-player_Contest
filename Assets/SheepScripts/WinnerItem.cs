using UnityEngine;
using UnityEngine.UI;

namespace SheepScripts
{
    public class WinnerItem : MonoBehaviour
    {
        public Text winnerName;
        
        public void SetWinner(string sheepName) {
            winnerName.text = $"Winner: {sheepName}";
        }
    }
}
