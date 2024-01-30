//Data class for Seniority Information
[System.Serializable]
public class SeniorityInfo
{
    public float baseSalary;
    public float salaryIncrementPct;

    public SeniorityInfo()
    {
        this.baseSalary = 0;
        this.salaryIncrementPct = 0;
    }

    public SeniorityInfo(float baseSalary, float salaryIncrementPct) 
    {
        this.baseSalary = baseSalary;
        this.salaryIncrementPct = salaryIncrementPct;
    }
}