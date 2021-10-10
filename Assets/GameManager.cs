using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class GameManager : MonoBehaviour
{
    public int size_x = 50;
    public int size_y = 50;
    public int size_z = 50;
    bool[,,,] room = new bool[2, 50, 50, 50];
    int indexBuffer = 1;
    //bool[,,,] room_now = new bool[2,10, 10, 10];
    public GameObject Cell;
    public List<GameObject> cellsSpawned = new List<GameObject>();
    public Transform cellsParent;
    private void Start()
    {
        for (int I = 0; I < 50; I++)
        {
            Vector3 position = new Vector3(Random.Range(0, 10), Random.Range(0, 10), Random.Range(0, 10));
            GameObject cell = Instantiate(this.Cell, position, new Quaternion(0, 0, 0, 0), cellsParent);
            room[indexBuffer, (int)position.x, (int)position.y, (int)position.z] = true;
            cellsSpawned.Add(cell);
        }

    }
    float elapsed = 0f;
    private void Update()
    {

        elapsed += Time.deltaTime;
        if (elapsed >= 0.5f)
        {
            elapsed = elapsed % 0.5f;
            UpdateCellsState();
            indexBuffer = 1 - indexBuffer;
            DrawCells();
        }

    }
    void UpdateCellsState()//travesal every single cell and update their life state
    {
        Sequence sequence = DOTween.Sequence();
        sequence.AppendCallback(() =>
        {
            int counter = 0;
            for (int x = 1; x <= size_x - 1; x++)
            {
                for (int y = 1; y <= size_y - 1; y++)
                {
                    for (int z = 1; z <= size_z - 1; z++)
                    {
                        for (int i = x - 1; i <= x + 1; i++)
                        {
                            for (int j = y - 1; j <= y + 1; j++)
                            {
                                for (int q = z - 1; q <= z + 1; q++)
                                {
                                    if (i == x && j == y && q == z)
                                    {
                                        break;
                                    }
                                    if (room[1 - indexBuffer, i, j, q] == true)
                                    {
                                        counter++;
                                    }
                                }
                            }
                        }
                        if (counter == 3)
                        {
                            room[indexBuffer, x, y, z] = true;
                        }
                        else if (counter == 2)
                        {
                            room[indexBuffer, x, y, z] = room[1 - indexBuffer, x, y, z];
                        }
                        else
                        {
                            room[indexBuffer, x, y, z] = false;
                        }
                    }
                }
            }
        });
    }
    void DrawCells()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.AppendCallback(() =>
        {
            ClearAll();
            cellsSpawned.Clear();
        });
        sequence.AppendCallback(() =>
        {
            Color color = new Color(Random.Range(0, 1), Random.Range(0, 1), Random.Range(0, 1), 1);
            for (int x = 0; x <= size_x; x++)
            {
                for (int y = 0; y <= size_y; y++)
                {
                    for (int z = 0; z <= size_z; z++)
                    {
                        if (room[indexBuffer, x, y, z] == true)
                        {
                            GameObject cellPrefab = Instantiate(Cell, new Vector3(x, y, z), new Quaternion(0, 0, 0, 0), cellsParent);
                            cellsSpawned.Add(cellPrefab);
                            cellPrefab.GetComponent<MeshRenderer>().material.color = color;
                        }
                    }
                }
            }
        });
    }
    void ClearAll()
    {
        for (int i = 0; i < cellsSpawned.Count; i++)
        {
            if (cellsSpawned.Count != 0)
            {
                Destroy(cellsSpawned[i]);
            }
        }
    }

}