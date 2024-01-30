using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UI;

public class EmployeePositionManager : MonoBehaviour
{
    #region Singleton

    public static EmployeePositionManager Singleton;
    void Awake()
    {
        if (Singleton == null)
        {
            DontDestroyOnLoad(gameObject);
            Singleton = this;
        }
        else if (Singleton != this)
        {
            Destroy(gameObject);
            return;
        }

        SingletonAwake();
    }


    #endregion

    void SingletonAwake() { }

    [SerializeField] UIManager uIManager;

    private void Start()
    {
        foreach (EmployeePositionDataPreset epp in Loader.Singleton.GetPositionPresets())
        {
            uIManager.AddEmployeePositionEntry(epp);
            uIManager.AddEmployeeCounyEntry(epp);
        }
    }
}
