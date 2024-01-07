using UnityEngine;

namespace UI
{
	public class MainMenuUIManager : MonoBehaviour
	{
		[SerializeField] private GameObject mainMenu;
		[SerializeField] private GameObject settings;

		public void MenuHide()
		{
			mainMenu.GetComponent<MainMenuUI>().Hide();
		}

		public void MenuShow()
		{
			mainMenu.GetComponent<MainMenuUI>().Show();
		}

		public void SettingsShow()
		{
			MenuHide();
		}

		public void SettingsHide()
		{
			MenuShow();
		}
	}
}