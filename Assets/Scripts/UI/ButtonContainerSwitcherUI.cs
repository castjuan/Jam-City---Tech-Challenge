using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonContainerSwitcherUI : MonoBehaviour
{
    [SerializeField] GameObject containerToActivate;
    [SerializeField] GameObject containerToDeactivate;

    void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            Click();
        });
    }

    void Click() 
    {
        Loader.Singleton.InstantiateSound("Click_1");

        if (containerToActivate) containerToActivate.SetActive(true);
        if (containerToDeactivate) containerToDeactivate.SetActive(false);
    }
}
