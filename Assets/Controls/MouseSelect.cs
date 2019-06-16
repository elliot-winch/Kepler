using System;
using System.Collections;
using UnityEngine;

public class MouseSelect<T> where T : MonoBehaviour
{
    public LayerMask layer = LayerMask.NameToLayer("Default");
    public float maxDist = Mathf.Infinity;
    public float doubleClickTime = 0.2f;

    public Action<T> OnMouseOverBegin;
    public Action<T> OnMouseOverEnd;
    public Action<T> OnLeftClick;
    //public Action<T> OnRightClick;
    public Action<T> OnLeftDoubleClick;

    private Camera camera;
    private T current;
    private Coroutine clickCo;

    public MouseSelect(Camera c)
    {
        this.camera = c;
    }

    public void Update()
    {
        if(camera == null)
        {
            return;
        }

        Physics.Raycast(camera.ScreenPointToRay(Input.mousePosition), out RaycastHit hitInfo, maxDist);
        
        T t = hitInfo.transform?.GetComponentInParent<T>();

        //MouseOver
        if(t != current)
        {
            if(current != null)
            {
                OnMouseOverEnd?.Invoke(current);
            }

            if(t != null)
            {
                OnMouseOverBegin?.Invoke(t);
            }
        }

        current = t;

        //Clicks
        if (t != null && Input.GetMouseButtonDown(0))
        {
            if (clickCo == null)
            {
                clickCo = CoroutineDispatch.Instance.StartCoroutine(Click(0, current));
            }
        }
    }

    private IEnumerator Click(int mouseIndex, T curr)
    {
        yield return null;

        float doubleClickTimer = 0f;

        while (doubleClickTimer < doubleClickTime)
        {
            if (Input.GetMouseButtonDown(0) && curr == current)
            {
                OnLeftDoubleClick?.Invoke(curr);
                clickCo = null;
                yield break;
            }

            doubleClickTimer += Time.deltaTime;

            yield return null;
        }

        OnLeftClick?.Invoke(curr);
        clickCo = null;
    }
}
