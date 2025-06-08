using UnityEngine;
using CodeMonkey.Utils;

public class Testing : MonoBehaviour
{

    [SerializeField] private HeatMapVisual heatMapVisual;
    [SerializeField] private HeatMapBoolVisual heatMapBoolVisual;
    [SerializeField] private HeatMapGenericVisual heatMapGenericVisual;

    private Grid<HeatMapGridObject> grid;

    private void Start()
    {
        grid = new Grid<HeatMapGridObject>(20, 10, 10f, new Vector3(0, 0),
                    (Grid<HeatMapGridObject> g, int x, int y) => new HeatMapGridObject(g, x, y));

        // heatMapVisual.SetGrid(grid);
        // heatMapBoolVisual.SetGrid(grid);
        heatMapGenericVisual.SetGrid(grid);
    }

    private void Update()
    {
        HandleClickToModifyGrid();

    }

    private void HandleClickToModifyGrid()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 position = UtilsClass.GetMouseWorldPosition();
            HeatMapGridObject heatMapGridObject = grid.GetGridObject(position);
            if (heatMapGridObject != null)
            {
                heatMapGridObject.AddValue(5);
            }
        }
    }

}


public class HeatMapGridObject
{
    public const int MAX = 100;
    public const int MIN = 0;

    private Grid<HeatMapGridObject> grid;
    int x;
    int y;
    public int value;

    public HeatMapGridObject(Grid<HeatMapGridObject> grid, int x, int y)
    {
        this.grid = grid;
        this.x = x;
        this.y = y;
    }

    public void AddValue(int addValue)
    {
        value += addValue;
        value = Mathf.Clamp(value, MIN, MAX);
        grid.TriggerGridObjectChanged(x, y);
    }

    public float GetValueNormalized()
    {
        return (float)value / MAX;
    }

    public override string ToString()
    {
        return value.ToString();
    }
}