using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubiksMover : MonoBehaviour {

    RubiksBehaviour rubiksBehaviour;

    void Awake ()
    {
        rubiksBehaviour = GetComponent<RubiksBehaviour>();
    }
    string s = "abcdefghijklmnopqr";
    int index = 0;
    int frames =0;

    void Update ()
    {
        if (frames++ % 2 == 0)
        {
            move(s[index]);
            index++;
            index %= s.Length;
        }
    }

    void performRotations (string rotations)
    {
        for (int i = 0; i < rotations.Length; i++)
        {
            move(rotations[i]);
        }
    }

    void  move (char c) {
        //abc def
        if (c <= 'f') {
            if (c <= 'c')
            {
                int index = c - 'a';
                StartCoroutine(rubiksBehaviour.RotateAroundX( index, 90 ));
            }else
            {
                int index = c - 'd';
                StartCoroutine(rubiksBehaviour.RotateAroundX(index, -90));
            }
        }
        // ghi jkl
        else if (c <= 'l')
        {
            if (c <= 'i')
            {
                int index = c - 'g';
                StartCoroutine(rubiksBehaviour.RotateAroundY(index, 90));
            }
            else
            {
                int index = c - 'j';
                StartCoroutine(rubiksBehaviour.RotateAroundY(index, -90));
            }
        }
        // mno pqr
        else
        {
            if (c <= 'o')
            {
                int index = c - 'm';
                StartCoroutine(rubiksBehaviour.RotateAroundZ(index, 90));
            }
            else
            {
                int index = c - 'p';
                StartCoroutine(rubiksBehaviour.RotateAroundZ(index, -90));
            }
        }
    }
}
