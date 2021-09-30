using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class ChangeShape : MonoBehaviour
{
    // Attach this script to the SpriteShapeController GameObject in the final/target shape

    public SpriteShapeController startSpriteShape;  // Temporary clone/dummy of this spriteShape used to set start position of corresponding points

    private Spline _startSpline;        // Spline used to set _shapeSpline into start position
    private Spline _shapeSpline;        // Spline of this spriteShapeConrtoller that is actually shaped
    private Vector3[] _pointTargets;    // Target/destination of each point in _shapeSpline

    // Start is called before the first frame update
    void Start()
    {
        // Get spline components from this SpriteShapeController and start SpriteShapeController
        _shapeSpline = GetComponent<SpriteShapeController>().spline;
        _startSpline = startSpriteShape.spline;

        // Set target of each point to its current/shaped position and then move them to start position
        _pointTargets = new Vector3[_shapeSpline.GetPointCount()];
        for (int i = 0; i < _shapeSpline.GetPointCount(); i++)
        {
            _pointTargets[i] = _shapeSpline.GetPosition(i);
            _shapeSpline.SetPosition(i, _startSpline.GetPosition(i));
        }

        // Destroy dummy start shape object
        Destroy(startSpriteShape.gameObject);
    }

    // Get number of points making up ChangeShape
    public int numPoints()
    {
        return _shapeSpline.GetPointCount();
    }

    // Get 2D position of a point
    public Vector2 getPoint(int i)
    {
        return _shapeSpline.GetPosition(i);
    }

    // Move a point towards its destination by a specified distance
    public void movePoint(int i, float dist)
    {
        _shapeSpline.SetPosition(i, Vector2.MoveTowards(_shapeSpline.GetPosition(i), _pointTargets[i], dist));
    }
}
