using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class EmployeeCountTest
{
    [UnityTest]
    public IEnumerator EmployeeCountTestWithEnumeratorPasses()
    {
        GameObject loaderGameObject = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Loader"));

        while (Loader.Singleton == null)
            yield return null;

        while (!Loader.Singleton.loaded)
            yield return null;

        Debug.Log("In the company there are:");
        AssertPositionCount(Loader.Singleton.GetPosition("HR"), 5, 2, 13);
        AssertPositionCount(Loader.Singleton.GetPosition("Engineering"), 50, 68, 32);
        AssertPositionCount(Loader.Singleton.GetPosition("Artist"), 5, 20, 0);
        AssertPositionCount(Loader.Singleton.GetPosition("Design"), 10, 0, 15);
        AssertPositionCount(Loader.Singleton.GetPosition("PMs"), 10, 20, 0);
        AssertPositionCount(Loader.Singleton.GetPosition("Ceo"), 1, 0, 0);
    }

    void AssertPositionCount(IEmployeePositionData position, int seniorsCountExp, int semiSeniorsCountExp, int juniorsCountExp) 
    {
        int seniorsCountVal = position.GetEmployeesCount(Employee.Seniority.Senior);
        int semiSeniorsCountVal = position.GetEmployeesCount(Employee.Seniority.SemiSenior);
        int juniorsCountVal = position.GetEmployeesCount(Employee.Seniority.Junior);

        int total = seniorsCountVal + semiSeniorsCountVal + juniorsCountVal;

        Assert.AreEqual(seniorsCountVal, seniorsCountExp, "There are " + seniorsCountVal + " seniors in " + position.tittle + ", expected " + seniorsCountExp); ;
        Assert.AreEqual(semiSeniorsCountVal, semiSeniorsCountExp, "There are " + semiSeniorsCountVal + " semiSeniors in " + position.tittle + ", expected " + semiSeniorsCountExp);
        Assert.AreEqual(juniorsCountVal, juniorsCountExp, "There are " + juniorsCountVal + " juniors in " + position.tittle + ", expected " + juniorsCountExp);

        Debug.Log(total + " " + position.tittle + " → ("
                        + (seniorsCountVal != 0 ? seniorsCountVal + " Seniors" : "")
                        + (juniorsCountVal == 0 ? (semiSeniorsCountVal != 0 ? " and " : "") : semiSeniorsCountVal != 0 ? ", " : "")
                        + (semiSeniorsCountVal != 0 ? "" + semiSeniorsCountVal + " Semi Seniors" : "")
                        + (juniorsCountVal != 0 ? " and " + juniorsCountVal + " Juniors" : "") + ")");
    }
}
