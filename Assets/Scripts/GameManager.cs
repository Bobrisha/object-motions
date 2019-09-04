using UnityEngine;


public class GameManager : MonoBehaviour
{
    [SerializeField] Config config = default;
    [SerializeField] Mover mover = default;
    [SerializeField] Transform objectTomove = default;
    [SerializeField] Ui ui = default;
   

    void Awake()
    {
        mover.Init(config, objectTomove);
        ui.Init(config);
    }
}
