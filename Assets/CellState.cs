using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellState : MonoBehaviour
{
    int counter = 0;
    public bool isAlive = false;
    Color color;
    void Start()
    {
        color = new Color(Random.Range(0, 255), Random.Range(0, 255), Random.Range(0, 255), 1);
        GetComponent<MeshRenderer>().material.color = color;
    }
    public int CheckNeighborCells()
    {
        for (int i = (int)transform.position.x - 1; i < 3; i++)
        {
            for (int j = (int)transform.position.y - 1; i < 3; j++)
            {
                for (int z = (int)transform.position.z - 1; z < 3; z++)
                {
                    if (this.isAlive)
                    {
                        counter++;
                    }
                }
            }
        }
        return counter;
    }
    public void UpdateLifeState()
    {
        if (!isAlive)
        {
            gameObject.GetComponent<MeshRenderer>().enabled = false;
        }
        else
        {
            gameObject.GetComponent<MeshRenderer>().enabled = true;
        }
    }

}
