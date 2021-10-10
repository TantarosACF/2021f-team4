using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grindstone : MonoBehaviour
{
    public GameObject edge;         // Sprite of edge of grinstone, rotated along with grindstone side 
    public RectTransform grindArea; // Region where blade is positioned to be grinded
    public GameObject blade;        // Blade positioned along edge of grindstone using mouse

    public float speedCap;      // Maximum rotation speed of grindstone in degrees/second
    public float scrollPower;   // Degrees/second added to speed each scroll
    public float friction;      // Degrees/second lost every second

    private float _speed;           // Degrees rotated by grindstone every second
    private float _bladeBoundLeft;  // Left boundary of blade X position in grindArea local space
    private float _bladeBoundRight; // Right boundary of "   "

    // Start is called before the first frame update
    void Start()
    {
        _speed = 0;

        // RE-DO THIS USING MEASUREMENT OTHER THAN SCALE (SPRITE BOUNDS, ETC)
        _bladeBoundRight = blade.transform.localScale.x / 2 - grindArea.rect.width / 2;
        _bladeBoundLeft = -_bladeBoundRight;
    }

    // Update is called once per frame
    void Update()
    {
        rotateGrindstone();
        moveBlade();
    }

    // Take mousewheel input, adjust grindstone speed, and rotate accordingly
    void rotateGrindstone()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");  // Direction/magnitude scrolled

        // Slow down grindstone w/ constant
        _speed -= friction * Time.deltaTime;

        // Speed up grindstone if scroll wheel moved in negative/downwards direction
        if (scroll < 0)
        {
            _speed += -1 * scrollPower * scroll;
        }

        // Clamp speed between stopped-speed cap after acceleration/decelleration
        _speed = Mathf.Clamp(_speed, 0, speedCap);

        //Debug.Log(_speed);

        // Rotate grindstone & edge
        transform.Rotate(Vector3.forward * _speed * Time.deltaTime);
        edge.transform.Rotate(Vector3.forward * _speed * Time.deltaTime);
    }

    void moveBlade()
    {
        // Calculate position of mouse in local space of grind area
        Vector3 mousePosScreen = Input.mousePosition;                               // Location of mouse on screen at time of click
        Vector3 mousePosWorld  = Camera.main.ScreenToWorldPoint(mousePosScreen);    // Location of mouse click in world space
        Vector3 mousePosLocal  = grindArea.InverseTransformPoint(mousePosWorld);    // Location of mouse click in local space of grind area

        // Move blade to mouse position within bounds
        blade.transform.position = new Vector3(
            Mathf.Clamp(mousePosLocal.x, _bladeBoundLeft, _bladeBoundRight), 
            blade.transform.position.y, 
            blade.transform.position.z);
    }
}
