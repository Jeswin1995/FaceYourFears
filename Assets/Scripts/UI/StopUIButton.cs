using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using  Oculus.Interaction;
using UnityEngine.Events;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.UI;

public class StopUIButton : MonoBehaviour
{
    [SerializeField] TMPro.TextMeshProUGUI text;
    [SerializeField] private float timeLimit;
    [SerializeField] private Image firstCircle, secondCircle;
    [SerializeField] private Color32 activeColor;
    [SerializeField, Interface(typeof(IInteractableView))]

    private UnityEngine.Object _interactableView;
    private int step = 0;
    private bool isPressEnabled;
    private float remainingTime;
    private bool isCountingDown;
    
    private IInteractableView InteractableView { get; set; }
    
    [Space]
    public UnityEvent OnTwoTimesTapped;
    public UnityEvent OnTimeOut;
    
    private void OnEnable()
    {
        if(_interactableView)
            InteractableView.WhenStateChanged += UpdateVisualState;
    }

    private void Start()
    {
        StartCountdown();
    }
    
    private void StartCountdown()
    {
        remainingTime = timeLimit;
        isCountingDown = true;
        StartCoroutine(CountdownRoutine());
    }

    private void StopCountdown()
    {
        isCountingDown = false;
        StopCoroutine(CountdownRoutine());
    }

    IEnumerator CountdownRoutine()
    {
        while (remainingTime > 0 && isCountingDown)
        {
            remainingTime -= Time.deltaTime;

            // Update the text in the format "MM:SS"
            TimeSpan timeSpan = TimeSpan.FromSeconds(remainingTime);
            text.text = $"{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}";

            yield return null;
        }

        if (remainingTime <= 0)
        {
            text.text = "00:00";
            text.color = Color.red;
            OnTimeOut.Invoke(); // Trigger timeout event
        }
    }


    public void PressButton()
    {
        if (isPressEnabled)
        {
            step++;
            switch (step)
            {
                case 1:
                    firstCircle.color = activeColor;
                    firstCircle.rectTransform.localPosition =
                        new Vector3(firstCircle.rectTransform.localPosition.x, 0, -0.01f);
                    break;
                case 2:
                    secondCircle.color = activeColor;
                    secondCircle.rectTransform.localPosition =
                        new Vector3(secondCircle.rectTransform.localPosition.x, 0, -0.01f);
                    OnTwoTimesTapped.Invoke();
                    break;
                default:
                    break;
            }
        }
    }

    private void UpdateVisualState(InteractableStateChangeArgs args)
    {
        PressButton();
    }
    
    protected virtual void Awake()
    {
        InteractableView = _interactableView as IInteractableView;
        StartCoroutine(enablePress());
    }

    IEnumerator enablePress()
    {
        yield return new WaitForSeconds(3);
        isPressEnabled = true;
    }

    private void OnDisable()
    {
        if(_interactableView)
            InteractableView.WhenStateChanged -= UpdateVisualState;
        StopCountdown();
    }
}
