using System;
using System.Collections;
using UnityEngine;

public interface IClickable
{
    void OnMouseOverBegin();
    void OnMouseOverEnd();
    void OnLeftClick();
    //public Action<T> OnRightClick;
    void OnLeftDoubleClick();
}

public class MouseSelect : MonoBehaviour
{
    public LayerMask layer;
    public float maxDist = Mathf.Infinity;
    public float doubleClickTime = 0.2f;
    public new Camera camera;

    private IClickable current;
    private Coroutine clickCo;

    void Update()
    {
        if(camera == null)
        {
            return;
        }

        Physics.Raycast(camera.ScreenPointToRay(Input.mousePosition), out RaycastHit hitInfo, maxDist);

        IClickable t = hitInfo.transform?.GetComponent<IClickable>();

        //MouseOver
        if(t != current)
        {
            current?.OnMouseOverEnd();

            t?.OnMouseOverBegin();
        }

        current = t;

        //Clicks
        if (t != null && Input.GetMouseButtonDown(0))
        {
            if (clickCo == null)
            {
                clickCo = StartCoroutine(Click(0, current));
            }
        }
    }

    private IEnumerator Click(int mouseIndex, IClickable curr)
    {
        yield return null;

        float doubleClickTimer = 0f;

        while (doubleClickTimer < doubleClickTime)
        {
            if (Input.GetMouseButtonDown(0) && curr == current)
            {
                curr.OnLeftDoubleClick();
                clickCo = null;
                yield break;
            }

            doubleClickTimer += Time.deltaTime;

            yield return null;
        }

        curr.OnLeftClick();
        clickCo = null;
    }
}
