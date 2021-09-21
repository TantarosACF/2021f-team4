using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class ChangeShape : MonoBehaviour
{

    public SpriteShapeController blockSpriteShape;  // temporary clone of sword spriteshape used to set block position of corresponding points
    public float hammerRadius;                      // Radius of hammer hit around mouse click where points are moved
    public float hitDistanceMultiplier;             // Proportion of hammer radius that is the maximum distance a point can move when hit

    private Spline _swordSpline;        // Spline used for creating sword shape
    private Spline _blockSpline;        // Spline used to set _swordSpline into block position
    private Vector3[] _pointTargets;    // Target/destination of each point in _swordSpline

    // Start is called before the first frame update
    void Start()
    {
        // Get spline components from sword and block SpriteShapeController objects
        _swordSpline = GetComponent<SpriteShapeController>().spline;
        _blockSpline = blockSpriteShape.spline;

        // Set target of each point to its starting position and then move them to block position
        _pointTargets = new Vector3[_swordSpline.GetPointCount()];
        for (int i = 0; i < _swordSpline.GetPointCount(); i++)
        {
            _pointTargets[i] = _swordSpline.GetPosition(i);
            _swordSpline.SetPosition(i, _blockSpline.GetPosition(i));
        }

        // Destroy dummy block shape object
        Destroy(blockSpriteShape.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        /*
        for (int i = 0; i < _swordSpline.GetPointCount(); i++)
        {
            // Move each point towards its destination
            _swordSpline.SetPosition(i, Vector3.MoveTowards(_swordSpline.GetPosition(i), _pointTargets[i], 0.001f));
        }
            */
    }

    private void OnMouseDown()
    {
        // Calculate position of mouse click in local space
        Vector3 mousePosScreen = Input.mousePosition;                           // Location of mouse on screen at time of click
        Vector3 mousePosWorld = Camera.main.ScreenToWorldPoint(mousePosScreen); // Location of mouse click in world space
        Vector3 mousePosSword = transform.InverseTransformPoint(mousePosWorld); // Location of mouse click in local space of sword GameObject

        // Move all points near click towards their target
        for (int i = 0; i < _pointTargets.Length; i++)
        {
            float distFromClick = Vector2.Distance(_swordSpline.GetPosition(i), mousePosSword); // Distance between point on sword and mouse click

            // If point is within hammer hit radius, move it towards its target
            if (distFromClick <= hammerRadius)
            {
                _swordSpline.SetPosition(i, Vector3.MoveTowards(_swordSpline.GetPosition(i), _pointTargets[i], (hammerRadius - distFromClick) * hitDistanceMultiplier));
            }
        }
    }
}
