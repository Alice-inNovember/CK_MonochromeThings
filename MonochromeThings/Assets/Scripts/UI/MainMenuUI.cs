using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class MainMenuUI : MonoBehaviour
    {
        [SerializeField] private List<ButtonAnim> buttons;
        
        public void Show()
        {
            foreach (var button in buttons)
            {
                button.Show();
            }
        }

        public void Hide()
        {
            foreach (var button in buttons)
            {
                button.Hide();
            }
        }
    }
}
