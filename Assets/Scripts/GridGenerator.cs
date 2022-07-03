using System;

using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GridGenerator : MonoBehaviour
{
    [Header("Resize the grid before running if needed")]
    [SerializeField] float widthGrid = 40f;
    [SerializeField] float heightGrid = 40f;
    [SerializeField] private bool isBorderShowed;
    [Space]
    [SerializeField] private int maxLettersCountOnSide = 12;
    [Space]
    [SerializeField] private LetterController letterControllerPrefab;
    [SerializeField] private InputField widthInput;
    [SerializeField] private InputField heightInput;
    [SerializeField] private Border border;
    private Vector3 gridCenterPosition;
    
    private int width = 4;
    private int height = 4;
    
    private LetterController[][] letterControllers;
    string allLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

    private void Start()
    {
        gridCenterPosition = transform.position;
        SetBorder();
    }
    
    public void GenerateGrid()
    {
        DeleteAllLetters();
        
        if (!TrySetGridSize())
        {
            Debug.Log("Not correct input");
            return;
        }
        letterControllers = new LetterController[width][];
        for (int y = 0; y < width; y++)
        {
            letterControllers[y] = new LetterController[height];
            for (int x = 0; x < height; x++)
            {
                var letterPosition = GetPositionByCoordinates(y, x);
                letterControllers[y][x] = Instantiate(letterControllerPrefab, letterPosition, Quaternion.identity);
                letterControllers[y][x].SetLetter(GetRandomLetter());
                
                Vector2 letterDeltaSize = new Vector2(widthGrid / width, heightGrid / height);
                letterControllers[y][x].SetSize(letterDeltaSize);
            }
        }
    }
    
    public void Shuffle()
    {
        if (letterControllers == null)
        {
            Debug.Log("Grid not exist");
            return;
        }
        ShuffleAllLettersInArray();
        SetNewPositionToAllLetters();
    }
    
    private void SetBorder()
    {
        border.SetBorder(widthGrid, heightGrid);
        border.SetEnable(isBorderShowed);
    }

    Vector3 GetPositionByCoordinates(int x, int y)
    {
        var weightOfLetterCenter = widthGrid / width;
        var heightOfLetterCenter = heightGrid / height;
        
        var zPos = -heightOfLetterCenter * height / 2 + heightOfLetterCenter / 2 + heightOfLetterCenter * y;
        var xPos = -weightOfLetterCenter * width / 2 + weightOfLetterCenter / 2 + weightOfLetterCenter * x;

        return new Vector3(xPos + gridCenterPosition.x, 0, zPos + gridCenterPosition.z);
    }

    char GetRandomLetter()
    {
        return allLetters[Random.Range(0, allLetters.Length)];
    }

    bool TrySetGridSize()
    {
        int widthResult;
        int heightResult;
        
        if (int.TryParse(widthInput.text, out widthResult))
        {
            width = widthResult;
        }
        if (int.TryParse(heightInput.text, out heightResult))
        {
            height = heightResult;
        }
        
        if (heightResult < 1 || widthResult < 1) return false;
        if(heightResult > maxLettersCountOnSide || widthResult > maxLettersCountOnSide) return false;
        return true;
    }

    void DeleteAllLetters()
    {
        if(letterControllers == null) return;
        
        foreach (var letter in letterControllers)
        {
            foreach (var let in letter)
            {
                if(let == null) return;
                Destroy(let.gameObject);
            }
        }

        letterControllers = null;
    }

    void SetNewPositionToAllLetters()
    {
        for (int y = 0; y < letterControllers.Length; y++)
        {
            for (int x = 0; x < letterControllers[y].Length; x++)
            {
                var letterPosition = GetPositionByCoordinates(y, x);
                letterControllers[y][x].MoveToNewPosition(letterPosition);
            }
        }
    }

    void ShuffleAllLettersInArray()
    {
        letterControllers.OrderBy(e => Guid.NewGuid()).ToArray();
        var array = letterControllers.SelectMany(item => item).Distinct().ToArray(); 
        
        int n = array.Length;
        for (int i = 0; i < (n - 1); i++)
        {
            int r = i + Random.Range(0, n - i);
            var t = array[r];
            array[r] = array[i];
            array[i] = t;
        }

        int numOfLetter = 0;
        for (int y = 0; y < width; y++)
        {
            for (int x = 0; x < height; x++)
            {
                letterControllers[y][x] = array[numOfLetter];
                numOfLetter++;
            }
        }
    }
}
