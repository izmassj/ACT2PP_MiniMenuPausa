using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class BallData
{
    public string nameBall;
    public int numBounces;

    private static BallData instance;

    public static BallData GetInstance()
    {
        if (instance == null)
        {
            instance = new BallData();
        }
        return instance;
    }

    public static void SetInstance(BallData json)
    {
        instance = json;
    }

    BallData() 
    {
        nameBall = string.Empty;
        numBounces = 0;
    }

    public void SetNameBall(string name)
    {
        nameBall = name;
    }

    public string GetNameBall()
    {
        return nameBall;
    }

    public void SetNumBounces(int bounces)
    {
        numBounces = bounces;
    }

    public int GetNumBounces()
    {
        return numBounces;
    }
}
