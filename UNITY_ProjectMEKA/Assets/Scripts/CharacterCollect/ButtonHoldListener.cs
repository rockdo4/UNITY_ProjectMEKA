using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class ButtonHoldListener : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {

    public float holdDuration = 1f;
    public UnityEvent onClickButton;
    public UnityEvent holdButton;

    private bool isPointerDown = false;
    private bool isLongPressed = false;
    private float pressTime;

    private Button button;
    private WaitForSeconds delay;

    private void Awake() {
        onClickButton = new UnityEvent();
        holdButton = new UnityEvent();

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
        isLongPressed = false;
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
                break;
            }
            yield return delay;
		}
	}

    public void onClickAddListener(UnityAction action)
    {
        Debug.Log("123");
        onClickButton.AddListener(action);
    }
}
