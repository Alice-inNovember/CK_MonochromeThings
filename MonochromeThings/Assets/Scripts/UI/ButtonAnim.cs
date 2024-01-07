using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI
{
	public class ButtonAnim : MonoBehaviour
	{
		[SerializeField] private Transform middle;
		[SerializeField] private float duration = 2;
		[SerializeField] private float distance = 1;
		[SerializeField] private float accentVal = 0.3f;
		[SerializeField] private Color highLightedColor;
		[SerializeField] private Color normalColor;

		private EventTrigger _eventTrigger;
		private Vector3 _originScale;
		private Vector3 _originPos;

		private void Start()
		{
			_originScale = transform.localScale;
			// ReSharper disable once Unity.InefficientPropertyAccess
			_originPos = transform.position;
			IdleMotion();
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

		private void OnUIEnter(BaseEventData eventData)
		{
			GetComponent<Image>().DOColor(highLightedColor, accentVal).SetEase(Ease.InOutSine);
			transform.DOScale(_originScale + new Vector3(accentVal, accentVal, accentVal), accentVal).SetEase(Ease.InOutSine);
		}

		private void OnUIExit(BaseEventData eventData)
		{
			GetComponent<Image>().DOColor(normalColor, accentVal).SetEase(Ease.Linear);
			transform.DOScale(_originScale, accentVal).SetEase(Ease.InOutSine);
		}

		public void IdleMotion()
		{
			// ReSharper disable once Unity.InefficientPropertyAccess
			var pos = _originPos;
			var dir = (middle.position - new Vector3(pos.x, pos.y, 0)).normalized;
			transform.DOMove(pos - dir * distance, duration).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
		}
		public void Hide()
		{
			_originScale = transform.localScale;

			// ReSharper disable once Unity.InefficientPropertyAccess
			var pos = _originPos;
			var dir = (middle.position - new Vector3(pos.x, pos.y, 0)).normalized;
			transform.DOMove(pos + dir * distance * 10, duration * 10).SetEase(Ease.InOutSine)
				.OnComplete(()=>{
				gameObject.SetActive(false);
			});
		}

		public void Show()
		{
			gameObject.SetActive(true);
			transform.DOMove(_originPos, duration * 10).SetEase(Ease.InOutSine);
		}
	}
}