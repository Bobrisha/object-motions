using UnityEngine;
using System;
using TMPro;
using System.Linq;


public class Ui : MonoBehaviour
{
    #region Fields

    public static event Action<int> OnTrajectoryChange;
    public static event Action<float> OnTimeChanged;
    public static event Action<int> OnSpikesCountChanged;
    public static event Action<float> OnSpikesHeightChanged;
    public static event Action<int> OnSpralTurnsChanged;


    [SerializeField] TMP_Dropdown trajectoriesDropdown = default;
    [SerializeField] TMP_InputField timeInput = default;
    [SerializeField] TMP_InputField spikesCountInput = default;
    [SerializeField] TMP_InputField spikesHeightInput = default;
    [SerializeField] TMP_InputField spralTurnsInput = default;

    #endregion



    #region Public methods

    public void Init(Config defaulConfig)
    {
        trajectoriesDropdown.ClearOptions();
        trajectoriesDropdown.AddOptions(Enum.GetNames(typeof(Trajectories)).ToList());
        trajectoriesDropdown.onValueChanged.AddListener(OnTrajectorySet);

        timeInput.text = defaulConfig.Time.ToString();
        timeInput.onValueChanged.AddListener(OnTimeSet);

        spikesCountInput.text = defaulConfig.SpikesCount.ToString();
        spikesCountInput.onValueChanged.AddListener(OnSpikesCountSet);

        spikesHeightInput.text = defaulConfig.SpikesHeight.ToString();
        spikesHeightInput.onValueChanged.AddListener(OnSpikesHeightSet);

        spralTurnsInput.text = defaulConfig.SpralTurns.ToString();
        spralTurnsInput.onValueChanged.AddListener(OnSpralTurnsSet);
    }

    #endregion



    #region Events handlers

    void OnTrajectorySet(int selectedTrajectory) => OnTrajectoryChange?.Invoke(selectedTrajectory);


    void OnTimeSet(string timeString)
    {
        if (float.TryParse(timeString, out float time))
        {
            OnTimeChanged?.Invoke(time);
        }
    }


    void OnSpikesCountSet(string spikesCountString)
    {
        if (int.TryParse(spikesCountString, out int spikesCount))
        {
            OnSpikesCountChanged?.Invoke(spikesCount);
        }
    }


    void OnSpikesHeightSet(string spicesHeightString)
    {
        if (float.TryParse(spicesHeightString, out float spicesHeight))
        {
            OnSpikesHeightChanged?.Invoke(spicesHeight);
        }
    }


    void OnSpralTurnsSet(string spralTurnsString)
    {
        if (int.TryParse(spralTurnsString, out int spralTurns))
        {
            OnSpralTurnsChanged?.Invoke(spralTurns);
        }
    }

    #endregion
}
