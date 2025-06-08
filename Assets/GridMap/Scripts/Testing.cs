using UnityEngine;
using CodeMonkey.Utils;

public class Testing : MonoBehaviour
{

    [SerializeField] private HeatMapVisual heatMapVisual;
    private Grid grid;
    private float mouseMoveTimer;
    private float mouseMoveTimerMax = .01f;

    private void Start()
    {
        grid = new Grid(100, 100, 4f, new Vector3(-200, -200));

        heatMapVisual.SetGrid(grid);

    }

    private void Update()
    {
        HandleClickToModifyGrid();

        // HandleHeatMapMouseMove();

        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log(grid.GetValue(UtilsClass.GetMouseWorldPosition()));
        }
    }

    private void HandleClickToModifyGrid()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 position = UtilsClass.GetMouseWorldPosition();
            grid.AddValue(position, 100, 5, 40);
        }
    }

    private void HandleHeatMapMouseMove()
    {
        mouseMoveTimer -= Time.deltaTime;
        if (mouseMoveTimer < 0f)
        {
            mouseMoveTimer += mouseMoveTimerMax;
            int gridValue = grid.GetValue(UtilsClass.GetMouseWorldPosition());
            grid.SetValue(UtilsClass.GetMouseWorldPosition(), gridValue + 1);
        }
    }
}
