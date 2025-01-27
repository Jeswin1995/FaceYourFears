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
    
    [SerializeField] private Image firstCircle, secondCircle;
    [SerializeField] private Color32 activeColor;
    [SerializeField, Interface(typeof(IInteractableView))]
    
    private UnityEngine.Object _interactableView;
    private int step = 0;
    private bool isPressEnabled;
    private IInteractableView InteractableView { get; set; }
    public UnityEvent OnTwoTimesTapped;

    private void OnEnable()
    {
        if(_interactableView)
            InteractableView.WhenStateChanged += UpdateVisualState;
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
    }
}
