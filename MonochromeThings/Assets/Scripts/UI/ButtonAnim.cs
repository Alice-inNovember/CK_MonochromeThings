using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
	public class ButtonAnim : MonoBehaviour
	{
		[SerializeField] private Transform middle;
		[SerializeField] private float duration = 2;
		[SerializeField] private float distance = 1;
		[SerializeField] private float accentScale = 0.3f;
		private EventTrigger _eventTrigger;
		private Vector3 _originScale;

		private void Start()
		{
			_originScale = transform.localScale;

			// ReSharper disable once Unity.InefficientPropertyAccess
			var pos = transform.position;
			var dir = (middle.position - new Vector3(pos.x, pos.y, 0)).normalized;
			transform.DOMove(pos - dir * distance, duration).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);

			_eventTrigger = GetComponent<EventTrigger>();
			RegisterOnMouseEnter();
			RegisterOnMouseExit();
		}

		private void RegisterOnMouseEnter()
		{
			if (_eventTrigger == null) return;
			var enterUIEntry = new EventTrigger.Entry
			{
				eventID = EventTriggerType.PointerEnter
			};
			enterUIEntry.callback.AddListener(OnUIEnter);
			_eventTrigger.triggers.Add(enterUIEntry);
		}

		private void RegisterOnMouseExit()
		{
			if (_eventTrigger == null) return;
			var enterUIEntry = new EventTrigger.Entry
			{
				eventID = EventTriggerType.PointerExit
			};
			enterUIEntry.callback.AddListener(OnUIExit);
			_eventTrigger.triggers.Add(enterUIEntry);
		}

		public void OnUIEnter(BaseEventData eventData)
		{
			transform.DOScale(_originScale + new Vector3(accentScale, accentScale, accentScale), accentScale)
				.SetEase(Ease.InOutSine);
		}

		public void OnUIExit(BaseEventData eventData)
		{
			transform.DOScale(_originScale, accentScale).SetEase(Ease.InOutSine);
		}
	}
}