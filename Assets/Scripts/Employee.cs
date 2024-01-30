using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Employee
{
    public string name { get; private set; }
    public IEmployeePositionData employeePositionData { get; private set; }

    public enum Seniority { Junior, SemiSenior, Senior }
    public Seniority seniority { get; private set; }

    public Employee(string name, IEmployeePositionData employeePositionData, Seniority seniority) 
    {
        this.name = name;
        this.employeePositionData = employeePositionData;
        this.seniority = seniority;
    }
}