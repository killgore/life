using UnityEngine;

public class Cell
{
    public int Id
    {
        get { return _id; }
    }

    public Vector3 Location
    {
        get { return _location; }
    }

    public bool Occupied
    {
        get { return _occupied; }
    }

    public GameObject Visual
    {
        get { return _visual;}
    }

    private int _id;
    private GameObject _visual;
    private Vector3 _location;
    private bool _occupied;
    private Organism _organism;

    public Cell(int id, Vector3 location)
    {
        _id = id;
        _location = location;
        _occupied = false;
        SetVisual(GameObject.CreatePrimitive(PrimitiveType.Cube).transform);
    }

    public void SetOrganism(Organism organism)
    {
        if(organism == null)
        {
            Debug.Log("Null organism sent to cell!");
            SetEmpty();
            return;
        }

        _organism = organism;
        _occupied = true;
        SetVisual(_organism.gameObject.transform);
    }

    public void SetVisual(Transform newVisual)
    {
        if(newVisual == null)
        {
            return;
        }

        newVisual.transform.position = _location;

        if (_visual != null)
        {
            newVisual.transform.SetParent(_visual.transform.parent);
            GameObject.Destroy(_visual);
        }

        _visual = newVisual.gameObject;
    }

    public void SetEmpty()
    {
        if (_occupied)
        {
            _occupied = false;
            var newVisual = GameObject.CreatePrimitive(PrimitiveType.Cube);

            SetVisual(newVisual.transform);

            _organism = null;
        }
    }

    public void Delete()
    {
        if(_organism != null)
        {
            _organism = null;
        }

        _occupied = false;

        GameObject.Destroy(_visual);
    }
}
