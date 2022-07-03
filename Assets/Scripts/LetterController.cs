using TMPro;
using UnityEngine;

public class LetterController : MonoBehaviour
{
    [SerializeField] private TextMeshPro textLetter;
    private bool isMoved = false;
    private Vector3 newPosition;
    private float distanceStep;
    private float timeToMove = 2f;

    void Update()
    {
        if(!isMoved) return;
        transform.position = Vector3.MoveTowards(transform.position, newPosition, distanceStep * Time.deltaTime);
        if ((newPosition - transform.position).magnitude < 0.0001) isMoved = false;

    }
    public void SetLetter(char symbol)
    {
        textLetter.text = symbol.ToString();
    }

    public void SetSize(Vector2 letterDeltaSize)
    {
        if (letterDeltaSize.x > letterDeltaSize.y)
        {
            letterDeltaSize = new Vector2(letterDeltaSize.y, letterDeltaSize.y);
        }
        if (letterDeltaSize.x < letterDeltaSize.y)
        {
            letterDeltaSize = new Vector2(letterDeltaSize.x, letterDeltaSize.x);
        }
        textLetter.rectTransform.sizeDelta = letterDeltaSize;
    }

    public void MoveToNewPosition(Vector3 position)
    {
        newPosition = position;
        distanceStep = (newPosition - transform.position).magnitude / timeToMove;
        isMoved = true;
    }
}
