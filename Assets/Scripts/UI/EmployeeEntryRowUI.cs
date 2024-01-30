using System.Collections;
using UnityEngine;
using TMPro;

//Row for employee list

namespace UI
{
    public class EmployeeEntryRowUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI seniorityTm, nameTm;

        public void Initialize(Employee employee)
        {
            seniorityTm.text = employee.seniority.ToString();
            nameTm.text = employee.name;
        }
    }
}