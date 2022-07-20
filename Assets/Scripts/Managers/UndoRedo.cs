using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UndoRedo : MonoBehaviour
{
    public class Settings
    {
        public GameObject gameObject;
        public Vector3 position;
        public Quaternion rotation;
        public bool isActive;

        public void DeactivateOrReactivate()
        {
            gameObject.transform.position = position;
            gameObject.transform.rotation = rotation;
            gameObject.SetActive(isActive);
        }

        public Settings(GameObject go)
        {
            gameObject = go;
            position = go.transform.position;
            rotation = go.transform.rotation;
            isActive = go.activeSelf;
        }
    }

    // public List<Settings> undoList;
    // public List<Settings> redoList;
    // public void AddToUndoList(GameObject line)
    // {
    //     Settings undoLine = new Settings(line);
    //     undoList.Add(undoLine);
    // }

    // public void Undo()
    // {
    //     if (undoList.Count > 0)
    //     {
    //         int lastIndex = undoList.Count - 1;
    //         undoList[lastIndex].Redo();
    //         undoList.RemoveAt(lastIndex);
    //     }
    // }

    // public void Redo()
    // {

    // }

    // void Start()
    // {
    //     undoList = new List<Settings>();
    // }

    public Stack<Settings> undoStack;
    public Stack<Settings> redoStack;

    public void UndoPress(GameObject line)
    {
        Settings undoLine = new Settings(line);
        undoStack.Push(undoLine);
        Debug.Log("undoStack: " + undoStack.Count);
        undoLine.isActive = false;
        Undo();
    }

    public void Undo()
    {
        if (undoStack.Count > 0)
        {
            redoStack.Push(undoStack.Peek());
            undoStack.Pop().DeactivateOrReactivate();
        }
    }

    public void RedoPress(GameObject line)
    {
        Settings undoLine = new Settings(line);
        redoStack.Push(undoLine);
        Debug.Log("redoStack: " + undoStack.Count);
        undoLine.isActive = true;
        Redo();
    }

    public void Redo()
    {
        if (redoStack.Count > 0)
        {
            undoStack.Push(redoStack.Peek());
            redoStack.Pop().DeactivateOrReactivate();
        }
    }

    void Start()
    {
        undoStack = new Stack<Settings>();
        redoStack = new Stack<Settings>();
    }
}
