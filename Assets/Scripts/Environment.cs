using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

using CellGrid = System.Collections.Generic.List<System.Collections.Generic.List<Cell>>;

public class Environment : MonoBehaviour
{
    public GameObject Organism;

    public int Width = 50;
    public int Height = 50;
    public int ChanceOfLife = 30;
    public int UpdateMS = 1000;

    private CellGrid _grid;
    private Stopwatch _stopwatch = new Stopwatch();
    private bool _paused;

    public CellGrid GenerateEmptyGrid(int width, int height)
    {
        int id = 0;

        var grid = new CellGrid(width);
        for (int w = 0; w < width; w++)
        {
            grid.Add(new List<Cell>(height));
            var column = new GameObject("column" + w);
            column.transform.SetParent(transform);
            for (int h = 0; h < height; h++)
            {
                var location = new Vector3(w, h, 0);
                var cell = new Cell(id, location);
                cell.Visual.transform.SetParent(column.transform);
                grid[w].Add(cell);
                id++;
            }
        }

        return grid;
    }

    private void CreateEnvironment(CellGrid grid)
    {
        var rand = new System.Random();
        for(int w = 0; w < Width; w++)
        {
            for(int h = 0; h < Height; h++)
            {
                var chance = Mathf.Clamp(ChanceOfLife, 0, 100);
                var result = rand.Next(0, 100);
                //Debug.Log("Life roll: " + result);
                if(result <= chance)
                {
                    CreateOrganismInCell(grid[w][h]);
                }
            }
        }
    }

	// Use this for initialization
	void Start ()
    {
        _grid = GenerateEmptyGrid(Width, Height);
        CreateEnvironment(_grid);
        _stopwatch.Start();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (_paused)
        {
            return;
        }

        if (_stopwatch.ElapsedMilliseconds > UpdateMS)
        {
            _stopwatch.Reset();
            UpdateEnvironment();
            _stopwatch.Start();
        }
	}

    public void TogglePause()
    {
        _paused = !_paused;
    }

    public void StepSim()
    {
        if(_paused)
        {
            UpdateEnvironment();
        }
    }

    private void CreateOrganismInCell(Cell cell)
    {
        var go = GameObject.Instantiate(Organism);
        var organism = go.GetComponent<Organism>();
        organism.Initialize(cell);
        cell.SetOrganism(organism);
    }

    private void UpdateEnvironment()
    {
        var newGrid = GenerateEmptyGrid(Width, Height);
        for (int w = 0; w < Width; w++)
        {
            for(int h = 0; h < Height; h++)
            {
                var numNeighbors = CountNeighbors(_grid, w, h);
                EvaluateEvolution(_grid[w][h], newGrid[w][h], numNeighbors);
            }
        }

        var oldGrid = _grid;
        _grid = newGrid;
        DeleteGrid(oldGrid);

    }

    private void EvaluateEvolution(Cell oldCell, Cell newCell, int neighborCount)
    {
        if(oldCell.Occupied)
        {
            if(neighborCount == 2 || neighborCount == 3)
            {
                CreateOrganismInCell(newCell);
            }
        }
        else
        {
            if(neighborCount == 3)
            {
                CreateOrganismInCell(newCell);
            }
        }
    }

    private int CountNeighbors(CellGrid grid, int w, int h)
    {
        int count = 0;

        if(w-1 >= 0 && grid[w-1][h].Occupied)
        {
            count++;   
        }

        if(w+1 < Width && grid[w+1][h].Occupied)
        {
            count++;
        }

        if(h-1 >=0 && grid[w][h-1].Occupied)
        {
            count++;
        }

        if(h+1 < Height && grid[w][h+1].Occupied)
        {
            count++;
        }

        if(w-1 >=0 && h+1 < Height && grid[w-1][h+1].Occupied)
        {
            count++;
        }

        if(w-1 >= 0 && h-1 >= 0 && grid[w-1][h-1].Occupied)
        {
            count++;
        }

        if(w+1 < Width && h+1 < Height && grid[w+1][h+1].Occupied)
        {
            count++;
        }

        if(w+1 < Width && h-1 >=0 && grid[w+1][h-1].Occupied)
        {
            count++;
        }

        return count;
    }

    private void DeleteGrid(CellGrid grid)
    {
        for(int w = 0; w < grid.Count; w++)
        {
            for(int h = 0; h < grid[w].Count; h++)
            {
                grid[w][h].Delete();
            }

            grid[w].Clear();
        }
    }
}
