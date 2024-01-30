using UnityEngine;

public interface IEmployeePositionData
{
    public string tittle { get; }
    SeniorityInfo juniorsInfo { get; set; }
    SeniorityInfo semiSeniorsInfo { get; set; }
    SeniorityInfo seniorsInfo { get; set; }

    #region Getters
    public SeniorityInfo GetSeniorityInfo(Employee.Seniority seniority)
    {
        switch (seniority)
        {
            case Employee.Seniority.Junior:
                return juniorsInfo;
            case Employee.Seniority.SemiSenior:
                return semiSeniorsInfo;
            case Employee.Seniority.Senior:
                return seniorsInfo;
        }

        Debug.LogError("Error, Employee position seniority \"" + seniority + "\" not found.");
        return null;
    }

    public int GetEmployeesCount() => Loader.Singleton.GetEmployeePositionCount(tittle);

    public int GetEmployeesCount(Employee.Seniority seniority) => Loader.Singleton.GetEmployeePositionCount(tittle, seniority);

    public float GetBaseSalary(Employee.Seniority seniority) => GetSeniorityInfo(seniority).baseSalary;

    public float GetBaseSalariesSum() => GetBaseSalary(Employee.Seniority.Junior) * GetEmployeesCount(Employee.Seniority.Junior) +
                                         GetBaseSalary(Employee.Seniority.SemiSenior) * GetEmployeesCount(Employee.Seniority.SemiSenior) +
                                         GetBaseSalary(Employee.Seniority.Senior) * GetEmployeesCount(Employee.Seniority.Senior);

    public float GetBaseSalariesSum(Employee.Seniority seniority) => GetBaseSalary(seniority) * GetEmployeesCount(seniority);

    public float GetSalaryIncrementPct(Employee.Seniority seniority) => GetSeniorityInfo(seniority).salaryIncrementPct;

    public float GetBaseSalaryAfterIncrement(Employee.Seniority seniority) 
    {
        SeniorityInfo info = GetSeniorityInfo(seniority);

        return info.baseSalary * (1 + (info.salaryIncrementPct / 100f));
    }

    public float GetBaseSalariesSumAfterIncrement() => GetBaseSalaryAfterIncrement(Employee.Seniority.Junior) * GetEmployeesCount(Employee.Seniority.Junior) +
                                                       GetBaseSalaryAfterIncrement(Employee.Seniority.SemiSenior) * GetEmployeesCount(Employee.Seniority.SemiSenior) +
                                                       GetBaseSalaryAfterIncrement(Employee.Seniority.Senior) * GetEmployeesCount(Employee.Seniority.Senior);

    public float GetBaseSalariesSumAfterIncrement(Employee.Seniority seniority) => GetBaseSalaryAfterIncrement(seniority) * GetEmployeesCount(seniority);

    public bool HasSeniority(Employee.Seniority seniority) 
    {
        SeniorityInfo info = GetSeniorityInfo(seniority);

        return (info.baseSalary != 0 || info.salaryIncrementPct != 0 || GetEmployeesCount(seniority) != 0);
    }

    #endregion
}