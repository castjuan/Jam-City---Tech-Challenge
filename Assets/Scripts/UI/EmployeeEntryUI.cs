using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

//Header row for employee list

namespace UI
{
    public class EmployeeEntryUI : MonoBehaviour
    {
        [SerializeField] RectTransform containerTransform;
        [SerializeField] Transform dropdownButtonTransform;
        [SerializeField] Transform entriesGrid;
        [Space]
        [SerializeField] TextMeshProUGUI tittle;
        [SerializeField] List<EmployeeEntryRowUI> childRows = new List<EmployeeEntryRowUI>();

        GameObject rowPrefab;

        RectTransform rectTransform;
        bool dropdownOpened;

        public void Initialize(IEmployeePositionData EmployeePositionData)
        {
            rowPrefab = Loader.Singleton.GetPrefab("EmployeeEntryRow");
            rectTransform = GetComponent<RectTransform>();

            dropdownButtonTransform.gameObject.GetComponent<Button>().onClick.AddListener(() => { ClickDropdownButton(); });

            tittle.text = EmployeePositionData.tittle;

            foreach (Employee e in Loader.Singleton.GetEmployees(EmployeePositionData.tittle, Employee.Seniority.Senior))
                AddRow(e);

            foreach (Employee e in Loader.Singleton.GetEmployees(EmployeePositionData.tittle, Employee.Seniority.SemiSenior))
                AddRow(e);

            foreach (Employee e in Loader.Singleton.GetEmployees(EmployeePositionData.tittle, Employee.Seniority.Junior))
                AddRow(e);
        }

        public void AddRow(Employee employee) 
        {
            EmployeeEntryRowUI row = Instantiate(rowPrefab, entriesGrid).GetComponent<EmployeeEntryRowUI>();
            row.Initialize(employee);
            childRows.Add(row);
        }

        public void ClickDropdownButton()
        {
            if (dropdownOpened)
                DropdownClose();
            else
                DropdownOpen();
        }

        void DropdownOpen()
        {
            dropdownOpened = true;

            Loader.Singleton.InstantiateSound("Click_1");

            dropdownButtonTransform.gameObject.LeanCancel();
            dropdownButtonTransform.LeanRotateZ(-90, .3f).setEaseOutSine();

            gameObject.LeanCancel();
            gameObject.LeanValue(20, 20 + (childRows.Count * 22), .3f).setOnUpdate(
                (float val) =>
                {
                    rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, val);
                    containerTransform.sizeDelta = new Vector2(containerTransform.sizeDelta.x, val);
                }).setEaseOutSine();
        }

        void DropdownClose()
        {
            dropdownOpened = false;

            Loader.Singleton.InstantiateSound("Click_2");

            dropdownButtonTransform.gameObject.LeanCancel();
            dropdownButtonTransform.LeanRotateZ(0, .3f).setEaseOutSine();

            gameObject.LeanCancel();
            gameObject.LeanValue(containerTransform.sizeDelta.y, 20, .3f).setOnUpdate(
                (float val) =>
                {
                    rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, val);
                    containerTransform.sizeDelta = new Vector2(containerTransform.sizeDelta.x, val);
                }).setEaseOutSine();
        }
    }
}