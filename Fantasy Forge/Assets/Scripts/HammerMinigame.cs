using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerMinigame : MonoBehaviour
{
    public float hammerRadius;          // Radius of hammer hit around mouse click where points are moved
    public float hitDistanceMultiplier; // Proportion of hammer radius that is the maximum distance a point can move when hit
    public float hitCooldownTime;       // Minimum time allowed between hits in seconds

    private ChangeShape _swordShape;    // ChangeShape of this HameObject/sword being formed
    private bool _hitReady;             // Indicates whether or not sufficient cooldown time has passed since last hit

    // Start is called before the first frame update
    void Start()
    {
        _swordShape = GetComponent<ChangeShape>();
        _hitReady = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        if (_hitReady)
        {
            // Calculate position of mouse click in local space
            Vector3 mousePosScreen = Input.mousePosition;                           // Location of mouse on screen at time of click
            Vector3 mousePosWorld = Camera.main.ScreenToWorldPoint(mousePosScreen); // Location of mouse click in world space
            Vector3 mousePosSword = _swordShape.transform.InverseTransformPoint(mousePosWorld); // Location of mouse click in local space of sword GameObject

            // Move all points near click towards their target
            for (int i = 0; i < _swordShape.numPoints(); i++)
            {
                float distFromClick = Vector2.Distance(_swordShape.getPoint(i), mousePosSword); // Distance between point on sword and mouse click

                // If point is within hammer hit radius, move it towards its target
                if (distFromClick <= hammerRadius)
                {
                    _swordShape.movePoint(i, (hammerRadius - distFromClick) * hitDistanceMultiplier);
                }
            }

            StartCoroutine("HitCooldown");
        }
    }

    IEnumerator HitCooldown()
    {
        _hitReady = false;
        yield return new WaitForSeconds(hitCooldownTime);
        _hitReady = true;
    }
}
