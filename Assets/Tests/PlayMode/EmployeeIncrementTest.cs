using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class EmployeeIncrementTest
{
    [UnityTest]
    public IEnumerator EmployeeIncrementTestWithEnumeratorPasses()
    {
        GameObject loaderGameObject = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Loader"));

        while (Loader.Singleton == null)
            yield return null;

        while (!Loader.Singleton.loaded)
            yield return null;

        Debug.Log("The salary increment percentage is:");
        AssertPositionIncrement(Loader.Singleton.GetPosition("HR"), 5, 2, .5f);
        AssertPositionIncrement(Loader.Singleton.GetPosition("Engineering"), 10, 7, 5);
        AssertPositionIncrement(Loader.Singleton.GetPosition("Artist"), 5, 2.5f, 0);
        AssertPositionIncrement(Loader.Singleton.GetPosition("Design"), 7, 0, 4);
        AssertPositionIncrement(Loader.Singleton.GetPosition("PMs"), 10, 5, 0);
        AssertPositionIncrement(Loader.Singleton.GetPosition("Ceo"), 100, 0, 0);
    }

    void AssertPositionIncrement(IEmployeePositionData position, float seniorsIncExp, float semiSeniorsIncExp, float juniorsIncExp) 
    {
        float seniorsIncVal = position.GetSalaryIncrementPct(Employee.Seniority.Senior);
        float semiSeniorsIncVal = position.GetSalaryIncrementPct(Employee.Seniority.SemiSenior);
        float juniorsIncVal = position.GetSalaryIncrementPct(Employee.Seniority.Junior);

        Assert.AreEqual(seniorsIncVal, seniorsIncExp, "Salary increment percentage is " + seniorsIncVal + " for seniors in " + position.tittle + ", expected " + seniorsIncExp);
        Assert.AreEqual(semiSeniorsIncVal, semiSeniorsIncExp, "Salary increment percentage is " + semiSeniorsIncVal + " for semiSeniors in " + position.tittle + ", expected " + semiSeniorsIncExp);
        Assert.AreEqual(juniorsIncVal, juniorsIncExp, "Salary increment percentage is " + juniorsIncVal + " for juniors in " + position.tittle + ", expected " + juniorsIncExp);

        Debug.Log(position.tittle + " → ("
                        + (seniorsIncVal != 0 ? seniorsIncVal + "% Seniors" : "")
                        + (juniorsIncVal == 0 ? (semiSeniorsIncVal != 0 ? " and " : "") : semiSeniorsIncVal != 0 ? ", " : "")
                        + (semiSeniorsIncVal != 0 ? "" + semiSeniorsIncVal + "% Semi Seniors" : "")                        
                        + (juniorsIncVal != 0 ? " and " + juniorsIncVal + "% Juniors" : "") + ")");
    }
}
