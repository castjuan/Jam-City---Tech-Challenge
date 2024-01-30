using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "newEmployeePositionData", menuName = "ScriptableObjects/EmployeePositionData", order = 1)]
public partial class EmployeePositionDataPreset : ScriptableObject, IEmployeePositionData
{
    public string tittle => this.name;

    public SeniorityInfo juniorsInfo { get => _juniorsInfo; set => _juniorsInfo = value; }
    public SeniorityInfo semiSeniorsInfo { get => _semiSeniorsInfo; set => _semiSeniorsInfo = value; }
    public SeniorityInfo seniorsInfo { get => _seniorsInfo; set => _seniorsInfo = value; }

    [SerializeField] SeniorityInfo _juniorsInfo;
    [SerializeField] SeniorityInfo _semiSeniorsInfo;
    [SerializeField] SeniorityInfo _seniorsInfo;
}