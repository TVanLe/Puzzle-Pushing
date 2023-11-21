using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxManager : MonoBehaviour
{
    public static BoxManager Instance;
    public static Dictionary<Vector3, Transform> allBoxLocation;
    [SerializeField] protected float speed;

    [SerializeField] protected int haveNumber;

    [SerializeField] protected int targetNumber;

    private GameManager _gameManager;
    private void Awake()
    {
        Instance = this;
        allBoxLocation = new Dictionary<Vector3, Transform>();
        _gameManager = GameObject.FindObjectOfType<GameManager>();
    }

    private void Start()
    {
        targetNumber = 0;
        LoadPositionAllBox();
        // print();
        // StartCoroutine(Test());
    }

    IEnumerator Test()
    {
        yield return new WaitForSeconds(2f);
        TranslateBox(new Vector3(-5.5f, 0.5f , 0)  , new Vector3(-5.5f, -0.5f , 0));
    }

    protected virtual void LoadPositionAllBox()
    {
        foreach (Transform box in transform)
        {
            targetNumber++;
            allBoxLocation[box.position] = box;
        }
    }

    public void TranslateBox(Vector3 oldLocation, Vector3 newLocation)
    {
        Transform boxTransform = allBoxLocation[oldLocation];
        StartCoroutine(TEST(boxTransform, oldLocation, newLocation));
        allBoxLocation[newLocation] = boxTransform;
        allBoxLocation.Remove(oldLocation);
        CalculateNumberBoxSatisfy();
    }

    public void TranslateBoxFast(Vector3 oldLocation, Vector3 newLocation)
    {
        Transform boxTransform = allBoxLocation[oldLocation];
        allBoxLocation[newLocation] = boxTransform;
        allBoxLocation.Remove(oldLocation);
        boxTransform.GetComponent<Box>().POPStack();
        boxTransform.GetComponent<Box>().AddToStack(newLocation);
        CaclulateRestBox(boxTransform);
        CalculateNumberBoxSatisfy();
        boxTransform.position = newLocation;
    }
    
    IEnumerator TEST(Transform objTransform, Vector3 oldLocation, Vector3 newLocation)
    {
        while (objTransform.position != newLocation)
        {
            objTransform.position = Vector3.MoveTowards(objTransform.position, newLocation, speed * Time.deltaTime);
            yield return null; // Chờ mỗi frame
        }
        
        // caculate every box stack
        objTransform.GetComponent<Box>().AddToStack(newLocation);
        CaclulateRestBox(objTransform);
    }

    protected virtual void CalculateNumberBoxSatisfy()
    {
        haveNumber = 0;
        foreach (var box in allBoxLocation)
        {
            if (TilemapManager.target.ContainsKey(box.Key))
            {
                haveNumber++;
            }
        }

        if (haveNumber == targetNumber)
        {
            Debug.Log("Win");
            _gameManager.NextLevel();
        }
    }

    public void CalculateStackForEveryBoxes()
    {
        foreach (var box in allBoxLocation)
        {
            box.Value.GetComponent<Box>().AddToStack(box.Key);
        }
    }

    protected virtual void CaclulateRestBox(Transform _box)
    {
        foreach (var box in allBoxLocation)
        {
            if (box.Value != _box)
            {
                box.Value.GetComponent<Box>().AddToStack(box.Key);
            }
        }
    }
    public void CalculateUndoForEveryBoxes()
    {
        foreach (Transform box in transform)
        {
            box.GetComponent<Box>().Undo();            
        }
    }
    void print()
    {
        foreach (var box in allBoxLocation)
        {
            Debug.Log(box.Key);
        }
    }
}
