using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Button startButton;
    public Button optionButton;
    public Button shopButton;

    private UnityAction action;

    private void Start()
    {
        action = () => OnButtonClick();

        startButton.onClick.AddListener(action);
        optionButton.onClick.AddListener(delegate { OnButtonClick(); });
        shopButton.onClick.AddListener(() => OnButtonClick());
    }

    public void OnButtonClick()
    {
        Debug.Log("Button Clicked");
    }
}
