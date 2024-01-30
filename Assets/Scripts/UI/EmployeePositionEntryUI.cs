using UnityEngine;
using TMPro;

//Row for position information list

namespace UI
{
    public class EmployeePositionEntryUI : MonoBehaviour
    {
        [SerializeField] RectTransform containerTransform;
        [SerializeField] Transform dropdownButtonTransform;
        [Space]
        [SerializeField] TextMeshProUGUI tittle;
        [SerializeField] Row total, seniors, semiSeniors, juniors;

        RectTransform rectTransform;
        bool dropdownOpened;

        int rowCount;

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
        }
        public void Initialize(IEmployeePositionData EmployeePositionData)
        {
            tittle.text = EmployeePositionData.tittle;

            rowCount = (EmployeePositionData.HasSeniority(Employee.Seniority.Senior) ? 1 : 0) +
                       (EmployeePositionData.HasSeniority(Employee.Seniority.SemiSenior) ? 1 : 0) +
                       (EmployeePositionData.HasSeniority(Employee.Seniority.Junior) ? 1 : 0);

            int totalEmployees = EmployeePositionData.GetEmployeesCount();

            float salarySum = EmployeePositionData.GetBaseSalariesSum();

            float salarySumAfterIncrement = EmployeePositionData.GetBaseSalariesSumAfterIncrement();

            total.SetValues(totalEmployees, salarySum, salarySumAfterIncrement);

            seniors.SetValues(EmployeePositionData, Employee.Seniority.Senior);
            semiSeniors.SetValues(EmployeePositionData, Employee.Seniority.SemiSenior);
            juniors.SetValues(EmployeePositionData, Employee.Seniority.Junior);
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
            dropdownButtonTransform.LeanRotateZ(-180, .3f).setEaseOutSine();

            gameObject.LeanCancel();
            gameObject.LeanValue(20, 20 + (rowCount * 22), .3f).setOnUpdate(
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
            dropdownButtonTransform.LeanRotateZ(-90, .3f).setEaseOutSine();

            gameObject.LeanCancel();
            gameObject.LeanValue(containerTransform.sizeDelta.y, 20, .3f).setOnUpdate(
                (float val) =>
                {
                    rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, val);
                    containerTransform.sizeDelta = new Vector2(containerTransform.sizeDelta.x, val);
                }).setEaseOutSine();
        }

        [System.Serializable]
        class Row 
        {
            [SerializeField] GameObject gameObject;
            [SerializeField] TextMeshProUGUI employees, baseSalary, salariesSum, increment, salaryAfterIncrement;

            public void SetValues(IEmployeePositionData positionData, Employee.Seniority seniority) 
            {
                if (!positionData.HasSeniority(seniority))
                {
                    gameObject.SetActive(false);
                    return;
                }

                employees.text = positionData.GetEmployeesCount(seniority).ToString();
                baseSalary.text = "$" + positionData.GetBaseSalary(seniority).ToString();
                salariesSum.text = "$" + positionData.GetBaseSalariesSum(seniority).ToString();
                increment.text = "%" + positionData.GetSalaryIncrementPct(seniority).ToString();
                salaryAfterIncrement.text = "$" + positionData.GetBaseSalariesSumAfterIncrement(seniority).ToString();
            }

            public void SetValues(int employeeCount, float salarySum, float salarySumAfterIncrement)
            {
                employees.text = employeeCount.ToString();
                salariesSum.text = "$" + salarySum;
                salaryAfterIncrement.text = "$" + salarySumAfterIncrement;
            }
        }
    }
}