using UnityEngine;

namespace UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField]
        Transform positionEntriesGrid;
        [SerializeField]
        Transform employeeEntriesGrid;

        GameObject positionDataEntryPrefab;
        GameObject employeeEntryPrefab;

        private void Awake()
        {
            positionDataEntryPrefab = Loader.Singleton.GetPrefab("PositionEntry");
            employeeEntryPrefab = Loader.Singleton.GetPrefab("EmployeeEntry");
        }

        public void AddEmployeePositionEntry(IEmployeePositionData EmployeePositionData)
        {
            GameObject entry = Instantiate(positionDataEntryPrefab, positionEntriesGrid);
            entry.GetComponent<EmployeePositionEntryUI>().Initialize(EmployeePositionData);
        }

        public void AddEmployeeCounyEntry(IEmployeePositionData EmployeePositionData) 
        {
            GameObject entry = Instantiate(employeeEntryPrefab, employeeEntriesGrid);
            entry.GetComponent<EmployeeEntryUI>().Initialize(EmployeePositionData);
        }
    }
}