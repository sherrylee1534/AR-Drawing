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
    private int undoRedoStackIndex;
    private int totalNumberOfLinesInScene;

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

        int tapCount = Input.touchCount > 1 && lineSettings.allowMultiTouch ? Input.touchCount : 1;

        for (int i = 0; i < tapCount; i++)
        {
            Touch touch = Input.GetTouch(i);
            Vector3 touchPosition = arCamera.ScreenToWorldPoint(new Vector3(Input.GetTouch(i).position.x, Input.GetTouch(i).position.y, lineSettings.distanceFromCamera));
            
            ARDebugManager.Instance.LogInfo($"{touch.fingerId}");

            if ((Screen.height * 0.15f) < touch.position.y && touch.position.y < (Screen.height * 0.85f))
            {
                if (touch.phase == TouchPhase.Began)
                {
                    // Block off borders at the top and bottom of the screen
                    
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
                    undoRedoStackIndex++;
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
    }

    void DrawOnMouse()
    {
        if (!canDraw) return;

        Vector3 mousePosition = arCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, lineSettings.distanceFromCamera));

        if (Input.GetMouseButton(0))
        {
            // Block off borders at the top and bottom of the screen
            if ((Screen.height * 0.15f) < Input.mousePosition.y && Input.mousePosition.y < (Screen.height * 0.85f))
            {
                OnDraw?.Invoke();

                if (Lines.Keys.Count == 0)
                {
                    ARLine line = new ARLine(lineSettings);
                    Lines.Add(0, line);
                    line.AddNewLineRenderer(transform, null, mousePosition);
                    undoRedoStackIndex++;
                    Debug.Log("undoRedoStackIndex: " + undoRedoStackIndex);
                    // totalNumberOfLinesInScene++;
                    // Debug.Log("totalNumberOfLinesInScene: " + totalNumberOfLinesInScene);
                }

                else
                {
                    Lines[0].AddPoint(mousePosition);
                }
            }
        }

        else if (Input.GetMouseButtonUp(0))
        {
            Lines.Remove(0);
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

        GameObject[] lines = GetAllLinesInScene();
        
        if (lines.Length > 0)
        {

            // int lastIndex = lines.Length - 1;
            // GameObject lastObject = lines[lastIndex];
            // undoRedoStack.Push(lastObject);
            // Debug.Log("undoRedoStack: " + undoRedoStack.Count);
            // undoRedo.UndoPress(lastObject);

            undoRedoStack.Push(lines[undoRedoStackIndex - 1]);
            Debug.Log("undoRedoStack: " + undoRedoStack.Count);
            undoRedo.UndoPress(lines[undoRedoStackIndex - 1]);
            undoRedoStackIndex--;
        }

        else
        {
            Debug.Log("No lines to undo");
        }
        
        StartCoroutine(AllowDrawAgain()); // Allow drawing again
    }

    public void RedoLine()
    {
        AllowDraw(false); // Stop drawing

        GameObject[] lines = GetAllLinesInScene();

        if (undoRedoStack.Count > 0)
        {
            Debug.Log("undoRedoStack before pop: " + undoRedoStack.Count);
            GameObject gameObjectToRedo = undoRedoStack.Pop();
            Debug.Log("undoRedoStack after pop: " + undoRedoStack.Count);
            undoRedo.RedoPress(gameObjectToRedo);
            undoRedoStackIndex++;
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

    IEnumerator AllowDrawAgain()
    {
        yield return new WaitForSeconds(0.1f);
        AllowDraw(true); // Allow drawing again
    }
}