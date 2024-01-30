using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class EmployeeSalaryTest
{
    [UnityTest]
    public IEnumerator EmployeeSalaryTestWithEnumeratorPasses()
    {
        GameObject loaderGameObject = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Loader"));

        while (Loader.Singleton == null)
            yield return null;

        while (!Loader.Singleton.loaded)
            yield return null;

        Debug.Log("The base salary is:");
        AssertPositionSalary(Loader.Singleton.GetPosition("HR"), 1500, 1000, 500);
        AssertPositionSalary(Loader.Singleton.GetPosition("Engineering"), 5000, 3000, 1500);
        AssertPositionSalary(Loader.Singleton.GetPosition("Artist"), 2000, 1200, 0);
        AssertPositionSalary(Loader.Singleton.GetPosition("Design"), 2000, 0, 800);
        AssertPositionSalary(Loader.Singleton.GetPosition("PMs"), 4000, 2400, 0);
        AssertPositionSalary(Loader.Singleton.GetPosition("Ceo"), 20000, 0, 0);

        Debug.Log("The resulting salary after increment should be:");
        AssertPositionSalaryAfterInc(Loader.Singleton.GetPosition("HR"), 1500, 1000, 500, 5, 2, .5f);
        AssertPositionSalaryAfterInc(Loader.Singleton.GetPosition("Engineering"), 5000, 3000, 1500, 10, 7, 5);
        AssertPositionSalaryAfterInc(Loader.Singleton.GetPosition("Artist"), 2000, 1200, 0, 5, 2.5f, 0);
        AssertPositionSalaryAfterInc(Loader.Singleton.GetPosition("Design"), 2000, 0, 800, 7, 0, 4);
        AssertPositionSalaryAfterInc(Loader.Singleton.GetPosition("PMs"), 4000, 2400, 0, 10, 5, 0);
        AssertPositionSalaryAfterInc(Loader.Singleton.GetPosition("Ceo"), 20000, 0, 0, 100, 0, 0);

    }

    void AssertPositionSalary(IEmployeePositionData position, float seniorsSalExp, float semiSeniorsSalExp, float juniorsSalExp) 
    {
        float seniorsSalVal = position.GetBaseSalary(Employee.Seniority.Senior);
        float semiSeniorsSalVal = position.GetBaseSalary(Employee.Seniority.SemiSenior);
        float juniorsSalVal = position.GetBaseSalary(Employee.Seniority.Junior);

        Assert.AreEqual(seniorsSalVal, seniorsSalExp, "Base salary is " + seniorsSalVal + " for seniors in " + position.tittle + ", expected " + seniorsSalExp);
        Assert.AreEqual(semiSeniorsSalVal, semiSeniorsSalExp, "Base salary is" + semiSeniorsSalVal + " for semiSeniors in " + position.tittle + ", expected " + semiSeniorsSalExp);
        Assert.AreEqual(juniorsSalVal, juniorsSalExp, "Base salary is" + juniorsSalVal + " for juniors in " + position.tittle + ", expected " + juniorsSalExp);

        Debug.Log(position.tittle + " → ($"
                        + (seniorsSalVal != 0 ? seniorsSalVal + " Seniors" : "")
                        + (juniorsSalVal == 0 ? (semiSeniorsSalVal != 0 ? " and " : "") : semiSeniorsSalVal != 0 ? ", " : "")
                        + (semiSeniorsSalVal != 0 ? "$" + semiSeniorsSalVal + " Semi Seniors" : "")                        
                        + (juniorsSalVal != 0 ? " and $" + juniorsSalVal + " Juniors" : "") + ")");
    }

    void AssertPositionSalaryAfterInc(IEmployeePositionData position, float seniorsSalExp, float semiSeniorsSalExp, float juniorsSalExp, float seniorsIncExp, float semiSeniorsIncExp, float juniorsIncExp)
    {
        float seniorsSalAftrIncVal = seniorsSalExp * (1 + seniorsIncExp / 100f);
        float semiSeniorsSalAftrIncVal = semiSeniorsSalExp * (1 + semiSeniorsIncExp / 100f);
        float juniorsSalAftrIncVal = juniorsSalExp * (1 + juniorsIncExp / 100f);

        float seniorsSalVal = position.GetBaseSalaryAfterIncrement(Employee.Seniority.Senior);
        float semiSeniorsSalVal = position.GetBaseSalaryAfterIncrement(Employee.Seniority.SemiSenior);
        float juniorsSalVal = position.GetBaseSalaryAfterIncrement(Employee.Seniority.Junior);

        Assert.AreEqual(seniorsSalVal, seniorsSalAftrIncVal, "Salary after increment is " + seniorsSalVal + " for seniors in " + position.tittle + ", expected " + seniorsSalAftrIncVal);
        Assert.AreEqual(semiSeniorsSalVal, semiSeniorsSalAftrIncVal, "Salary after increment is " + semiSeniorsSalVal + " for semiSeniors in " + position.tittle + ", expected " + semiSeniorsSalAftrIncVal);
        Assert.AreEqual(juniorsSalVal, juniorsSalAftrIncVal, "Salary after increment is " + juniorsSalVal + " for juniors in " + position.tittle + ", expected " + juniorsSalAftrIncVal);

        Debug.Log(position.tittle + " → ($"
                        + (seniorsSalVal != 0 ? seniorsSalVal + " Seniors" : "")
                        + (juniorsSalVal == 0 ? (semiSeniorsSalVal != 0 ? " and " : "") : semiSeniorsSalVal != 0 ? ", " : "")
                        + (semiSeniorsSalVal != 0 ? "$" + semiSeniorsSalVal + " Semi Seniors" : "")
                        + (juniorsSalVal != 0 ? " and $" + juniorsSalVal + " Juniors" : "") + ")");
    }
}
