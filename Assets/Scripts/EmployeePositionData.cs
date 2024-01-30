using UnityEngine;

public class EmployeePositionData : IEmployeePositionData
{
    public string tittle { get; set; }
    public SeniorityInfo juniorsInfo { get; set; }
    public SeniorityInfo semiSeniorsInfo { get; set; }
    public SeniorityInfo seniorsInfo { get; set; }

    public EmployeePositionData(string tittle, SeniorityInfo juniorsInfo, SeniorityInfo semiSeniorsInfo, SeniorityInfo seniorsInfo) 
    {
        this.tittle = tittle;
        this.juniorsInfo = juniorsInfo;
        this.semiSeniorsInfo = semiSeniorsInfo;
        this.seniorsInfo = seniorsInfo;
    }
}
