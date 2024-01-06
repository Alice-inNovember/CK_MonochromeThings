using UnityEngine;

namespace ClassTemp
{
	public class Singleton<T> : MonoBehaviour where T : Singleton<T>
	{
		private static T _instance;
		protected bool DoNotDestroyOnLoad = false;
		public static T Instance => _instance == null ? null : _instance;

		protected virtual void Awake()
		{
			if (_instance == null)
			{
				_instance = (T)this;

				if (DoNotDestroyOnLoad) DontDestroyOnLoad(gameObject);
			}
			else
			{
				Destroy(gameObject);
			}
		}
	}
}