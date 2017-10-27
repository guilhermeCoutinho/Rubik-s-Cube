using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubiksBehaviour : MonoBehaviour {
    public int speed = 30; // THE SPEED MUST BE A FORMAL 90 DIVISOR;

    Transform[] cubes;

    int[] clockWiseRotationMapping =     { 2, 5, 8, 1, 4, 7, 0, 3, 6 };
    int[] antiClockWiseRotationMapping = { 6, 3, 0, 7, 4, 1, 8, 5, 2 };
    
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
    
    public IEnumerator RotateAroundX(int index, float targetRotation)
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

    public void remapXRotationCubes(int index, bool isClockwise)
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

    public IEnumerator RotateAroundY(int index, float targetRotation)
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

    public void remapYRotationCubes(int index, bool isClockwise)
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

    public IEnumerator RotateAroundZ(int index, float targetRotation)
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

    public void remapZRotationCubes(int index, bool isClockwise)
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
