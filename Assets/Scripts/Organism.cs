using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Organism : MonoBehaviour
{
    public GameObject Visual
    {
        get { return _visual; }
    }

    private Cell _cell;
    private GameObject _visual;

    public void Initialize(Cell cell)
    {
        _cell = cell;
        _visual = gameObject;
    }

    void Update()
    {

    }
}
