using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Threading.Tasks;

public class Loader : MonoBehaviour
{
    #region Singleton

    public static Loader Singleton;
    void Awake()
    {
        if (Singleton == null)
        {
            DontDestroyOnLoad(gameObject);
            Singleton = this;
        }
        else if (Singleton != this)
        {
            Destroy(gameObject);
            return;
        }

        SingletonAwake();
    }


    #endregion
    //Addressables tag list in order to access groups with string tag
    [SerializeField] AssetLabelReference[] labelReferencesRaw;
    Dictionary<string, AssetLabelReference> labelReferences = new Dictionary<string, AssetLabelReference>();

    Dictionary<string, GameObject> prefabs = new Dictionary<string, GameObject>();
    Dictionary<string, AudioClip> audios = new Dictionary<string, AudioClip>();

    //Premade positions through scriptable objects
    Dictionary<string, EmployeePositionDataPreset> positionPresets = new Dictionary<string, EmployeePositionDataPreset>();
    //Generated employees from PositionsCount.json
    Dictionary<(IEmployeePositionData, Employee.Seniority), List<Employee>> employees = new Dictionary<(IEmployeePositionData, Employee.Seniority), List<Employee>>();

    public bool loaded { get; private set; }

    void SingletonAwake() 
    {
        LoadAndContinue();
    }

    #region Load

    //Load assets and go to menu scene
    async void LoadAndContinue() 
    {
        await LoadResources();
        await LoadDependentResources();
        loaded = true;
        SceneManager.LoadScene(1);
    }

    async Task LoadResources() 
    {
        foreach (AssetLabelReference label in labelReferencesRaw)
            labelReferences.Add(label.labelString, label);

        AsyncOperationHandle<IList<GameObject>> prefabsLoad = Addressables.LoadAssetsAsync<GameObject>(labelReferences["Prefabs"],
                    go =>
                    {
                        prefabs.Add(go.name.ToLower(), go);
                    }, false);

        Debug.Log("Loading prefabs...");
        while (!prefabsLoad.IsDone) await Task.Yield();

        AsyncOperationHandle<IList<AudioClip>> audioLoad = Addressables.LoadAssetsAsync<AudioClip>(labelReferences["Audio"],
                        aud =>
                        {
                            audios.Add(aud.name.ToLower(), aud);
                        }, false);
        Debug.Log("Loading audio...");
        while (!audioLoad.IsDone) await Task.Yield();

        AsyncOperationHandle<IList<EmployeePositionDataPreset>> presetLoad = Addressables.LoadAssetsAsync<EmployeePositionDataPreset>(labelReferences["PositionPresets"],
                        i =>
                        {
                            positionPresets.Add(i.name, i);
                        }, false);
        Debug.Log("Loading presets...");
        while (!presetLoad.IsDone) await Task.Yield();
    }

    async Task LoadDependentResources()
    {
        await ReadEmployees();
    }

    async Task ReadEmployees() 
    {
        AsyncOperationHandle<TextAsset> JSONLoad = Addressables.LoadAssetAsync<TextAsset>(labelReferences["JSONs"]);
        while (!JSONLoad.IsDone) await Task.Yield();
        TextAsset JSON = JSONLoad.Result;

        PositionsCountList positionsCount = JsonUtility.FromJson<PositionsCountList>(JSON.text);

        for (int i = 0; i < positionsCount.PositionsCount.Length; i++)
        {
            GenerateEmployees(positionsCount.PositionsCount[i].Position, Employee.Seniority.Junior, positionsCount.PositionsCount[i].Junior);
            GenerateEmployees(positionsCount.PositionsCount[i].Position, Employee.Seniority.SemiSenior, positionsCount.PositionsCount[i].SemiSenior);
            GenerateEmployees(positionsCount.PositionsCount[i].Position, Employee.Seniority.Senior, positionsCount.PositionsCount[i].Senior);
        }
    }

    void GenerateEmployees(string positionTittle, Employee.Seniority seniority, int count) 
    {
        IEmployeePositionData positionData = positionPresets[positionTittle];

        employees.Add((positionData, seniority), new List<Employee>());

        for (int i = 0; i < count; i++)
        {
            Employee e = new Employee("Emp" + positionTittle + seniority + i, positionData, seniority);

            employees[(positionData, seniority)].Add(e);
        }
    }

    #endregion

    #region Getters

    public GameObject GetPrefab(string name)
    {
        if (name == null) return null;

        name = name.ToLower();

        if (!prefabs.ContainsKey(name))
        {
            Debug.LogError("No se encontro el prefab '" + name + "'");
            return null;
        }

        return prefabs[name];
    }

    public List<EmployeePositionDataPreset> GetPositionPresets() => new List<EmployeePositionDataPreset>(positionPresets.Values);

    public IEmployeePositionData GetPosition(string name) => positionPresets[name];

    public int GetEmployeePositionCount(string positionTittle) => employees[(positionPresets[positionTittle], Employee.Seniority.Junior)].Count +
                                                                  employees[(positionPresets[positionTittle], Employee.Seniority.SemiSenior)].Count +
                                                                  employees[(positionPresets[positionTittle], Employee.Seniority.Senior)].Count;
    public int GetEmployeePositionCount(string positionTittle, Employee.Seniority seniority)
    {
        if (!ValidateEmployeePosition(positionTittle, seniority)) return 0;

        return employees[(positionPresets[positionTittle], seniority)].Count;
    }

    public List<Employee> GetEmployees(string positionTittle, Employee.Seniority seniority)
    {
        if (!ValidateEmployeePosition(positionTittle, seniority)) return null;

        return employees[(positionPresets[positionTittle], seniority)];
    }

    bool ValidateEmployeePosition(string positionTittle, Employee.Seniority seniority) 
    {
        if (!positionPresets.ContainsKey(positionTittle))
        {
            Debug.LogError("Position " + positionTittle + " could not be found.");
            return false;
        }

        if (!employees.ContainsKey((positionPresets[positionTittle], seniority)))
        {
            Debug.LogError("Employee seniority " + seniority + " for Position " + positionTittle + " could not be found.");
            return false;
        }

        return true;
    }
    #endregion

    #region Audio
    public GameObject InstantiateSound(string name)
    {
        if (name == null) return null;

        name = name.ToLower();

        if (!audios.ContainsKey(name))
        {
            Debug.LogError("Audio '" + name + "' no encontrado.");
            return null;
        }

        GameObject currentAudio = new GameObject(name);
        AudioSource source = currentAudio.AddComponent<AudioSource>();
        source.clip = audios[name];
        source.Play();

        currentAudio.AddComponent<AudioInstance>(); //Component to destroy sound after playing

        return currentAudio;
    }

    #endregion
}

#region UtilityClasses

//Used to convert employee count json

[System.Serializable]
class PositionsCountList
{
    public PositionsCount[] PositionsCount;
}

[System.Serializable]
class PositionsCount 
{
    public string Position;
    public int Junior;
    public int SemiSenior;
    public int Senior;
}

#endregion