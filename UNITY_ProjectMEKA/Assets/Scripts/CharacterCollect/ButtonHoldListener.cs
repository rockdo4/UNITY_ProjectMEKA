using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class ButtonHoldListener : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {

    public float holdDuration = 0.5f;
    public UnityEvent onClickButton = new UnityEvent();
    public UnityEvent holdButton;

    private bool isPointerDown = false;
    private bool isLongPressed = false;
    private float pressTime;

    private Button button;
    private WaitForSeconds delay;

    private void Awake() {
        button = GetComponent<Button>();
        delay = new WaitForSeconds(0.1f);
    }

    public void OnPointerDown(PointerEventData eventData) {
        isPointerDown = true;
        pressTime = Time.time;
        StartCoroutine(Timer());
    }

    public void OnPointerUp(PointerEventData eventData) {
        isPointerDown = false;
        if(!isLongPressed)
        {
			onClickButton.Invoke();
		}
    }

    private IEnumerator Timer() 
    {
        while (isPointerDown && !isLongPressed) 
        {
            float elapsedSeconds = Time.time - pressTime;

            if (elapsedSeconds >= holdDuration) 
            {
                isLongPressed = true;
                if (button.interactable)
                {
                    holdButton.Invoke();
                }
                yield break;
            }
            yield return delay;
		}
		isLongPressed = false;
	}

    public void onClickAddListener(UnityAction action)
    {
        Debug.Log("123");
        onClickButton.AddListener(action);
    }
}
