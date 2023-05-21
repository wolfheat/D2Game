using UnityEngine;
using UnityEngine.InputSystem;

public class InputRestriction : MonoBehaviour
{
    private Rect inputRegion;
    public static bool HasFocus { get; private set; } = true;

    public static InputRestriction Instance { get; private set; }

    private void Start()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }else Instance = this;

        // Get the boundaries of the parent object in screen coordinates
        Rect parentRect = GetComponent<RectTransform>().rect;
        Vector3 position = GetComponent<RectTransform>().position;
        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, position);
        inputRegion = new Rect(0,0,1920 ,1080);
    }

    public bool CheckFocus()
    {
        Debug.Log("Checking focus at pos"+ Mouse.current.position.ReadValue().x + " region: ("+inputRegion.xMin+","+ inputRegion.xMax+")");
        InGameConsol.Instance.AddInfo("Focus X pos:" + Mouse.current.position.ReadValue().x);// + " region: (" + inputRegion.xMin + "," + inputRegion.xMax + ")");
        // Check for mouse input within the input region
        if (Mouse.current.position.ReadValue().x >= inputRegion.xMin &&
            Mouse.current.position.ReadValue().x <= inputRegion.xMax &&
            Mouse.current.position.ReadValue().y >= inputRegion.yMin &&
            Mouse.current.position.ReadValue().y <= inputRegion.yMax)
        {
            if (HasFocus != true)
            {
                HasFocus = true;
                InGameConsol.Instance.AddInfo("Mouse in Focus");
            }
            return HasFocus;
        }
        else
        {
            if (HasFocus != false)
            {
                HasFocus = false;
                InGameConsol.Instance.AddInfo("Mouse lost Focus");
            }
            return HasFocus;
        }


    }
}
