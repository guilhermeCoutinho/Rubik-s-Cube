using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubiksScrambler : MonoBehaviour {
    RubiksBehaviour rubiksBehaviour;
    public int moves = 20;
    Stack<Vector4> rotationHistory = new Stack<Vector4>();
    bool isScrambling = false;
    bool isSolving = false;

    void Awake()
    {
        rubiksBehaviour = GetComponent<RubiksBehaviour>();
    }

    void Start ()
    {
        StartCoroutine(showCase());
    }

    IEnumerator showCase()
    {
        while (true)
        {
            yield return StartCoroutine(ScrambleAnimation(moves));
            yield return new WaitForSeconds(.3f);
            yield return StartCoroutine(PopStack());
            yield return new WaitForSeconds(.3f);
        }
    }
    public void Scramble(int moveCount)
    {
        StartCoroutine(ScrambleAnimation(moveCount));
    }

    IEnumerator ScrambleAnimation(int moveCount)
    {
        if (isScrambling || isSolving)
            yield break;
        isScrambling = true;
        while (moveCount-- > 0)
        {
            int randomAxis = Random.Range(0, 3);
            int rowIndex = Random.Range(0, 3);
            float rotation = Random.Range(0, 1) > 0.5f ? 90 : -90;

            if (randomAxis == 0)
            {
                rotationHistory.Push(new Vector4(rotation, 0, 0, rowIndex));
                yield return StartCoroutine(rubiksBehaviour.RotateAroundX(rowIndex, rotation));
            }
            if (randomAxis == 1)
            {
                rotationHistory.Push(new Vector4(0, rotation, 0, rowIndex));
                yield return StartCoroutine(rubiksBehaviour.RotateAroundY(rowIndex, rotation));
            }
            if (randomAxis == 2)
            {
                rotationHistory.Push(new Vector4(0, 0, rotation, rowIndex));
                yield return StartCoroutine(rubiksBehaviour.RotateAroundZ(rowIndex, rotation));
            }

        }
        isScrambling = false;
    }

    public void Solve()
    {
        StartCoroutine(PopStack());
    }

    public IEnumerator PopStack()
    {
        if (isSolving || isScrambling)
            yield break;
        isSolving = true;
        while (rotationHistory.Count > 0)
        {
            Vector4 rotation = rotationHistory.Pop();
            if (rotation.x != 0)
                yield return StartCoroutine(rubiksBehaviour.RotateAroundX((int)rotation.w, -(int)rotation.x));
            else if (rotation.y != 0)
                yield return StartCoroutine(rubiksBehaviour.RotateAroundY((int)rotation.w, -(int)rotation.y));
            else if (rotation.z != 0)
                yield return StartCoroutine(rubiksBehaviour.RotateAroundZ((int)rotation.w, -(int)rotation.z));

        }
        isSolving = false;
    }
}
