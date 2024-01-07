#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace UI
{
	[CustomEditor(typeof(UILookAtMe))]
	public class UILookAtMeEditor : Editor
	{
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			//UIAlign.cs 의 객체를 받아옵니다 => 이래야 버튼시 명령을 내릴수 잇습니다
			var uiLookAtMe = (UILookAtMe)target;

			EditorGUILayout.BeginHorizontal();  //BeginHorizontal() 이후 부터는 GUI 들이 가로로 생성됩니다.
			GUILayout.FlexibleSpace(); // 고정된 여백을 넣습니다. ( 버튼이 가운데 오기 위함)

			//버튼을 만듭니다 . GUILayout.Button("버튼이름" , 가로크기, 세로크기)
			if (GUILayout.Button("Look At Me", GUILayout.Width(200), GUILayout.Height(50)))
				uiLookAtMe.SetUI();

			GUILayout.FlexibleSpace();  // 고정된 여백을 넣습니다.
			EditorGUILayout.EndHorizontal();  // 가로 생성 끝
		}
	}
	public class UILookAtMe  : MonoBehaviour
	{
		[SerializeField] private List<GameObject> uiList;

		public void SetUI()
		{
			var targetPos = transform.position;

			foreach (var obj in uiList)
			{
				var position = obj.transform.position;
				var rotation = Mathf.Atan2(position.y - targetPos.y, position.x - targetPos.x) * Mathf.Rad2Deg;
				obj.transform.rotation = Quaternion.Euler(0, 0, rotation);
			}
		}
	}
}
#endif
