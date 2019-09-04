using UnityEngine;


[CreateAssetMenu(fileName = "MoveConfig", menuName = "MoveConfig", order = 51)]
public class Config : ScriptableObject
{
    [Header("General")]
    [SerializeField] Trajectories trajectory = default;
    [SerializeField] float time = default;

    [Header("Spikes")]
    [SerializeField] int spikesCount = default;
    [SerializeField] float spikesHeight = default;

    [Header("Spiral")]
    [SerializeField] int spiralTurns = default;


    public Trajectories Trajectory => trajectory;
    public float Time => time;
    public int SpikesCount => spikesCount;
    public float SpikesHeight => spikesHeight;
    public int SpiralTurns => spiralTurns;


    private void OnEnable()
    {
        Ui.OnTrajectoryChange += Ui_OnTrajectoryChange;
        Ui.OnTimeChanged += Ui_OnTimeChanged;
        Ui.OnSpralTurnsChanged += Ui_OnSpralTurnsChanged;
        Ui.OnSpikesCountChanged += Ui_OnSpikesCountChanged;
        Ui.OnSpikesHeightChanged += Ui_OnSpikesHeightChanged;
    }


    private void OnDisable()
    {
        Ui.OnTrajectoryChange -= Ui_OnTrajectoryChange;
        Ui.OnTimeChanged -= Ui_OnTimeChanged;
        Ui.OnSpralTurnsChanged -= Ui_OnSpralTurnsChanged;
        Ui.OnSpikesCountChanged -= Ui_OnSpikesCountChanged;
        Ui.OnSpikesHeightChanged -= Ui_OnSpikesHeightChanged;
    }


    void Ui_OnTrajectoryChange(int newTrajectory) => trajectory = (Trajectories)newTrajectory;
    void Ui_OnTimeChanged(float newTime) => time = newTime;
    void Ui_OnSpralTurnsChanged(int newTurnsCount) => spikesCount = newTurnsCount;
    void Ui_OnSpikesCountChanged(int newSpikesCount) => spikesCount = newSpikesCount;
    void Ui_OnSpikesHeightChanged(float newSpikesHeight) => spikesHeight = newSpikesHeight;
}
