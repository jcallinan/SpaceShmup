using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Keeps a GameObject on the screen
/// NOTE: this only works for an orthographic Main Camera
/// </summary>
public class BoundsCheck : MonoBehaviour
{
    [System.Flags]
    public enum eScreenLocs
    {
        onScreen = 0, // 0000 in binary
        offRight = 1, // 0001
         offLeft = 2, // 0010
          offUp = 4,  // 0100
          offDown = 8 // 1000 in binary
    }
    public enum eType { center, inset, outset};
    [Header("Inscribed")]
    public eType boundsType = eType.center;
    public float radius = 1f;
    public bool keepOnScreen = true;

    [Header("Dynamic")]
    public eScreenLocs screenLocs = eScreenLocs.onScreen;
    public float camWidth;
    public float camHeight;

    private void Awake()
    {
        camHeight = Camera.main.orthographicSize;
        camWidth = camHeight * Camera.main.aspect;

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // LateUpdate is called after Update() has been called on all GameObjects.
    void LateUpdate()
    {
        float checkRadius = 0;
        if (boundsType == eType.inset) checkRadius = -radius;
        if (boundsType == eType.outset) checkRadius = radius;

        Vector3 pos = transform.position;
        screenLocs = eScreenLocs.onScreen;
        //restrict the X pos to camWidth
        if (pos.x > camWidth + checkRadius)
        {
            pos.x = camWidth + checkRadius;
            screenLocs = eScreenLocs.offRight;
        }
        if (pos.x < -camWidth - checkRadius)
        {
            pos.x = -camWidth - checkRadius;
            screenLocs = eScreenLocs.offLeft;
        }
        //restrict the Y pos to camHeight
        if (pos.y > camHeight + checkRadius)
        {
            pos.y = camHeight + checkRadius;
            screenLocs = eScreenLocs.offUp;
        }
        if (pos.y < -camHeight - checkRadius)
        {
            pos.y = -camHeight - checkRadius;
            screenLocs = eScreenLocs.offDown;
        }
        if (keepOnScreen && !isOnScreen)
        {
            transform.position = pos;
            screenLocs = eScreenLocs.onScreen;
        }
         
    }
    public bool isOnScreen
    {
        get { return (screenLocs == eScreenLocs.onScreen);  }
    }
    public bool LocIs( eScreenLocs checkLoc)
    {
        if (checkLoc == eScreenLocs.onScreen) return isOnScreen;
        return ((screenLocs & checkLoc) == checkLoc);
    }
}
