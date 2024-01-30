using System.Collections.Generic;
using UnityEngine;

namespace UI
{
	public class MainMenuUI : MonoBehaviour
	{
		[SerializeField] private List<GameObject> buttons;

		public void Show()
		{
			foreach (var button in buttons)
			{
				button.GetComponent<ButtonAnim>().Show();
			}
		}

		public void Hide()
		{
			foreach (var button in buttons)
			{
				button.GetComponent<ButtonAnim>().Hide();
			}
		}
	}
}