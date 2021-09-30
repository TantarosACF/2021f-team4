using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerMinigame : MonoBehaviour
{
    public ChangeShape swordShape;
    public float hammerRadius;          // Radius of hammer hit around mouse click where points are moved
    public float hitDistanceMultiplier; // Proportion of hammer radius that is the maximum distance a point can move when hit

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        // Calculate position of mouse click in local space
        Vector3 mousePosScreen = Input.mousePosition;                           // Location of mouse on screen at time of click
        Vector3 mousePosWorld = Camera.main.ScreenToWorldPoint(mousePosScreen); // Location of mouse click in world space
        Vector3 mousePosSword = swordShape.transform.InverseTransformPoint(mousePosWorld); // Location of mouse click in local space of sword GameObject

        Debug.Log(mousePosSword);

        // Move all points near click towards their target
        for (int i = 0; i < swordShape.numPoints(); i++)
        {
            float distFromClick = Vector2.Distance(swordShape.getPoint(i), mousePosSword); // Distance between point on sword and mouse click

            // If point is within hammer hit radius, move it towards its target
            if (distFromClick <= hammerRadius)
            {
                swordShape.movePoint(i, (hammerRadius - distFromClick) * hitDistanceMultiplier);
            }
        }
    }
}
