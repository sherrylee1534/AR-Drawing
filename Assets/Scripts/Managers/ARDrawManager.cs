using System.Collections.Generic;
using System.Collections;
using Cores.Singletons;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(ARAnchorManager))]
public class ARDrawManager : Singleton<ARDrawManager>
{
    [SerializeField]
    private LineSettings lineSettings = null;

    [SerializeField]
    private UnityEvent OnDraw = null;

    [SerializeField]
    private ARAnchorManager anchorManager = null;

    [SerializeField] 
    private Camera arCamera = null;
    
    private List<ARAnchor> anchors = new List<ARAnchor>();
    private Dictionary<int, ARLine> Lines = new Dictionary<int, ARLine>();
    private bool canDraw { get; set; }
    private Stack<GameObject> undoRedoStack = new Stack<GameObject>();
    private UndoRedo undoRedo;
    private bool isUndoPressed = false;
    private bool isDrawingAfterUndo = false;

    void Start()
    {
        undoRedo = GetComponent<UndoRedo>();
    }

    void Update ()
    {
        #if (!UNITY_EDITOR)
        DrawOnTouch();
        #else
        DrawOnMouse();
        #endif
	}

    public void AllowDraw(bool isAllow)
    {
        canDraw = isAllow;
    }

    void DrawOnTouch()
    {
        if (!canDraw) return;

        if (isUndoPressed)
        {
            isDrawingAfterUndo = true;
        }

        int tapCount = Input.touchCount > 1 && lineSettings.allowMultiTouch ? Input.touchCount : 1;

        for (int i = 0; i < tapCount; i++)
        {
            Touch touch = Input.GetTouch(i);
            Vector3 touchPosition = arCamera.ScreenToWorldPoint(new Vector3(Input.GetTouch(i).position.x, Input.GetTouch(i).position.y, lineSettings.distanceFromCamera));
            
            ARDebugManager.Instance.LogInfo($"{touch.fingerId}");

            if (touch.phase == TouchPhase.Began)
            {
                OnDraw?.Invoke();
                
                ARAnchor anchor = anchorManager.AddAnchor(new Pose(touchPosition, Quaternion.identity));

                if (anchor == null)
                {
                    Debug.LogError("Error creating reference point");
                }

                else 
                {
                    anchors.Add(anchor);
                    ARDebugManager.Instance.LogInfo($"Anchor created & total of {anchors.Count} anchor(s)");
                }

                ARLine line = new ARLine(lineSettings);
                Lines.Add(touch.fingerId, line);
                line.AddNewLineRenderer(transform, anchor, touchPosition);
            }

            else if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
            {
                Lines[touch.fingerId].AddPoint(touchPosition);
            }

            else if (touch.phase == TouchPhase.Ended)
            {
                Lines.Remove(touch.fingerId);
            }
        }
    }

    void DrawOnMouse()
    {
        if (!canDraw) return;

        if (isUndoPressed)
        {
            isDrawingAfterUndo = true;
        }

        Vector3 mousePosition = arCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, lineSettings.distanceFromCamera));

        if (Input.GetMouseButton(0))
        {
            OnDraw?.Invoke();

            if (Lines.Keys.Count == 0)
            {
                ARLine line = new ARLine(lineSettings);
                Lines.Add(0, line);
                line.AddNewLineRenderer(transform, null, mousePosition);
                //Debug.Log("Lines if: " + Lines.Count);
            }

            else
            {
                Lines[0].AddPoint(mousePosition);
                //Debug.Log("Lines else: " + Lines.Count);
            }
        }

        else if (Input.GetMouseButtonUp(0))
        {
            Lines.Remove(0);
            //Debug.Log("Lines else if: " + Lines.Count);
        }

        if (Input.GetKeyDown("k"))
        {
            GameObject[] lines = GetAllLinesInScene();
            Debug.Log("total no. of lines: " + lines.Length);
        }
    }

    public GameObject[] GetAllLinesInScene()
    {
        return GameObject.FindGameObjectsWithTag("Line");
    }

    public void UndoLine()
    {
        AllowDraw(false); // Stop drawing

        isUndoPressed = true;

        GameObject[] lines = GetAllLinesInScene();

        if (isDrawingAfterUndo)
        {
            undoRedoStack.Clear();
            isUndoPressed = false;
        }
        
        if (isDrawingAfterUndo)
        {
            if (lines.Length > 0)
            {
                int lastIndex = lines.Length - 1;
                GameObject lastObject = lines[lastIndex];
                undoRedoStack.Push(lastObject);
                Debug.Log("undoRedoStack: " + undoRedoStack.Count);
                undoRedo.UndoPress(lastObject);
            }

            else
            {
                Debug.Log("No lines to undo");
            }
        }
        
        StartCoroutine(AllowDrawAgain()); // Allow drawing again
    }

    public void RedoLine()
    {
        AllowDraw(false); // Stop drawing

        if (undoRedoStack.Count > 0)
        {
            GameObject gameObjectToRedo = undoRedoStack.Pop();
            Debug.Log("undoRedoStack after pop: " + undoRedoStack.Count);
            undoRedo.RedoPress(gameObjectToRedo);
        }

        else
        {
            Debug.Log("No lines to redo");
        }

        StartCoroutine(AllowDrawAgain()); // Allow drawing again
    }

    public void ClearLines()
    {
        GameObject[] lines = GetAllLinesInScene();
        foreach (GameObject currentLine in lines)
        {
            LineRenderer line = currentLine.GetComponent<LineRenderer>();
            Destroy(currentLine);
        }
    }

    // public void UndoLine()
    // {
    //     AllowDraw(false); // Stop drawing when you press undo

    //     // Remove last object
    //     GameObject[] lines = GetAllLinesInScene();

    //     if (lines.Length >= 1)
    //     {
    //         int lastObject = lines.Length - 1;
    //         Destroy(lines[lastObject]);
    //     }

    //     else
    //     {
    //         Debug.Log("No more lines to undo");
    //     }

    //     Debug.Log("no. of lines drawn: " + lines.Length);
    //     Debug.Log("no. of lines left: " + lines.Length);

    //     #if (!UNITY_EDITOR)
    //     {
    //         // Remove last anchor (only applicable for DrawOnTouch())
    //         int lastAnchor = anchors.Count - 1;
    //         anchors.RemoveAt(lastAnchor);
    //     }
    //     #endif

    //     StartCoroutine(AllowDrawAgain());
    // }

    IEnumerator AllowDrawAgain()
    {
        yield return new WaitForSeconds(0.1f);
        AllowDraw(true); // Allow drawing again
    }
}