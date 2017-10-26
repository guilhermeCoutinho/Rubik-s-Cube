using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubiksBehaviour : MonoBehaviour {
    public int moves = 20;
    public int speed = 30; // THE SPEED MUST BE A FORMAL 90 DIVISOR;

    Transform[] cubes;

    int[] clockWiseRotationMapping =     { 2, 5, 8, 1, 4, 7, 0, 3, 6 };
    int[] antiClockWiseRotationMapping = { 6, 3, 0, 7, 4, 1, 8, 5, 2 };

    Stack<Vector4> rotationHistory = new Stack<Vector4>();
    bool isScrambling = false;
    bool isSolving = false;


    void Awake ()
    {
        if (90 % speed != 0)
        {
            Debug.LogWarning("Speed must be a divisor of 90");
            speed = 3;
        }
        cubes = new Transform[transform.childCount];
        for (int i=0;i < transform.childCount; i++){
            cubes[i] = transform.GetChild(i);
        }
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
                yield return StartCoroutine(RotateAroundX(rowIndex, rotation));
            }
            if (randomAxis == 1)
            {
                rotationHistory.Push(new Vector4(0, rotation, 0, rowIndex));
                yield return StartCoroutine(RotateAroundY(rowIndex, rotation));
            }
            if (randomAxis == 2)
            {
                rotationHistory.Push(new Vector4(0, 0, rotation, rowIndex));
                yield return StartCoroutine(RotateAroundZ(rowIndex, rotation));
            }

        }
        isScrambling = false;
    }

    public void Solve()
    {
        StartCoroutine(PopStack());
    }

    IEnumerator PopStack()
    {
        if (isSolving || isScrambling)
            yield break;
        isSolving = true;
        while (rotationHistory.Count > 0)
        {
            Vector4 rotation = rotationHistory.Pop();
            if (rotation.x != 0)
                yield return StartCoroutine(RotateAroundX((int)rotation.w, -(int)rotation.x));
            else if (rotation.y != 0)
                yield return StartCoroutine(RotateAroundY((int)rotation.w, -(int)rotation.y));
            else if (rotation.z != 0)
                yield return StartCoroutine(RotateAroundZ((int)rotation.w, -(int)rotation.z));

        }
        isSolving = false;
    }

    
    IEnumerator RotateAroundX(int index, float targetRotation)
    {
        index *= 9;
        int rotateAmount = 0;
        float actualSpeed = targetRotation > 0 ? speed : -speed;

        Transform center = cubes[index + 4];
        while (rotateAmount < Mathf.Abs(targetRotation))
        {
            for (int i = 0; i < 9; i++)
                cubes[index + i].RotateAround(center.position, Vector3.right, actualSpeed);
            rotateAmount += speed;
            yield return null;
        }
        remapXRotationCubes(index,targetRotation >0);
    }

    void remapXRotationCubes(int index, bool isClockwise)
    {
        Transform[] subArray = new Transform[9];
        for (int i = 0; i < 9; i++)
        {
            if (isClockwise)
                subArray[i] = cubes[index + clockWiseRotationMapping[i]];
            else
                subArray[i] = cubes[index + antiClockWiseRotationMapping[i]];
        }
        for (int i = 0; i < 9; i++)
        {
            cubes[index + i] = subArray[i];
        }
    }

    IEnumerator RotateAroundY(int index, float targetRotation)
    {
        int rotateAmount = 0;
        float actualSpeed = targetRotation > 0 ? speed : -speed;
        Transform center = cubes[4 / 3 * 9 + 4 % 3 + index * 3];
        while (rotateAmount < Mathf.Abs(targetRotation))
        {
            for (int i = 0; i < 9; i++)
            {
                int j = i / 3 * 9 + i % 3 + index * 3;
                cubes[j].RotateAround(center.position, Vector3.down, actualSpeed);
            }
            rotateAmount += speed;
            yield return null;
        }
        remapYRotationCubes(index, targetRotation > 0);
    }

    void remapYRotationCubes(int index, bool isClockwise)
    {
        Transform[] subArray = new Transform[9];
        for (int i = 0; i < 9; i++)
        {
            int k = isClockwise ?  clockWiseRotationMapping[i] : antiClockWiseRotationMapping[i];
            int j = k / 3 * 9 + k % 3 + index * 3;
            if (isClockwise)
                subArray[i] = cubes[j];
            else
                subArray[i] = cubes[j];
        }
        for (int i = 0; i < 9; i++)
        {
            int j = i / 3 * 9 + i % 3 + index * 3;
            cubes[j] = subArray[i];
        }
    }

    IEnumerator RotateAroundZ(int index, float targetRotation)
    {
        int rotateAmount = 0;
        float actualSpeed = targetRotation > 0 ? speed : -speed;

        Transform center = cubes[index + 3*4];
        while (rotateAmount < Mathf.Abs(targetRotation))
        {
            for (int i = 0; i < 9; i++)
            {
                cubes[index + i * 3].RotateAround(center.position, Vector3.forward, actualSpeed);
            }
            rotateAmount += speed;
            yield return null;
        }
        remapZRotationCubes(index, targetRotation > 0);
    }

    void remapZRotationCubes(int index, bool isClockwise)
    {
        Transform[] subArray = new Transform[9];
        for (int i = 0; i < 9; i++)
        {
            if (isClockwise)
                subArray[i] = cubes[index + 3*clockWiseRotationMapping[i]];
            else
                subArray[i] = cubes[index + 3*antiClockWiseRotationMapping[i]];
        }
        for (int i = 0; i < 9; i++)
        {
            cubes[index + i*3] = subArray[i];
        }
    }

}
