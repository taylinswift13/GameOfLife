using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class GameManager : MonoBehaviour
{
    int size_x = 20;
    int size_y = 20;
    int size_z = 20;
    public bool[,,,] room = new bool[2, 20, 20, 20];
    public int indexBuffer = 1;
    //bool[,,,] room_now = new bool[2,10, 10, 10];
    public GameObject Cell;
    public List<GameObject> cellsSpawned = new List<GameObject>();
    public Transform cellsParent;

    private void Start()
    {
        GameObject cell_1 = Instantiate(this.Cell, new Vector3(10, 10, 10), new Quaternion(0, 0, 0, 0), cellsParent);
        room[0, 10, 10, 10] = true;
        cellsSpawned.Add(cell_1);
        GameObject cell_2 = Instantiate(this.Cell, new Vector3(11, 10, 10), new Quaternion(0, 0, 0, 0), cellsParent);
        room[0, 11, 10, 10] = true;
        cellsSpawned.Add(cell_2);
        GameObject cell_3 = Instantiate(this.Cell, new Vector3(9, 10, 10), new Quaternion(0, 0, 0, 0), cellsParent);
        room[0, 9, 10, 10] = true;
        cellsSpawned.Add(cell_3);
        GameObject cell_4 = Instantiate(this.Cell, new Vector3(10, 11, 10), new Quaternion(0, 0, 0, 0), cellsParent);
        room[0, 10, 11, 10] = true;
        cellsSpawned.Add(cell_4);
        GameObject cell_5 = Instantiate(this.Cell, new Vector3(10, 9, 10), new Quaternion(0, 0, 0, 0), cellsParent);
        room[0, 10, 9, 10] = true;
        cellsSpawned.Add(cell_5);
        GameObject cell_6 = Instantiate(this.Cell, new Vector3(10, 10, 11), new Quaternion(0, 0, 0, 0), cellsParent);
        room[0, 10, 10, 11] = true;
        cellsSpawned.Add(cell_6);

        /*for (int I = 0; I < 10; I++)
        {
            Vector3 position = new Vector3(Random.Range(8, 13), Random.Range(8, 13), Random.Range(8, 13));
            GameObject cell = Instantiate(this.Cell, position, new Quaternion(0, 0, 0, 0), cellsParent);
            room[0, (int)position.x, (int)position.y, (int)position.z] = true;
            cellsSpawned.Add(cell);
        }*/

    }
    float elapsed = 0f;
    private void Update()
    {
        Sequence sequence = DOTween.Sequence();

        elapsed += Time.deltaTime;
        if (elapsed >= 1f)
        {
            elapsed = elapsed % 1f;
            UpdateCellsState(sequence);
            sequence.AppendCallback(() =>
            {
                indexBuffer = 1 - indexBuffer;
            });
            DrawCells(indexBuffer, sequence);
        }

    }
    void UpdateCellsState(Sequence sequence)//travesal every single cell and update their life state
    {
        sequence.AppendCallback(() =>
        {
            for (int x = 1; x <= size_x - 2; x++)
            {
                for (int y = 1; y <= size_y - 2; y++)
                {
                    for (int z = 1; z <= size_z - 2; z++)
                    {
                        int counter = 0;
                        for (int i = x - 1; i <= x + 1; i++)
                        {
                            for (int j = y - 1; j <= y + 1; j++)
                            {
                                for (int q = z - 1; q <= z + 1; q++)
                                {
                                    if (i == x && j == y && q == z)
                                    {
                                        continue;
                                    }
                                    if (room[1 - indexBuffer, i, j, q] == true)
                                    {
                                        counter++;
                                    }
                                }
                            }
                        }
                        if (counter == 5)
                        {
                            room[indexBuffer, x, y, z] = true;
                        }
                        else if (counter == 4)
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
    void DrawCells(int indexNow, Sequence sequence)
    {
        sequence.AppendCallback(() =>
        {
            ClearAll();

        });
        sequence.AppendCallback(() =>
        {
            cellsSpawned.Clear();
        });
        sequence.AppendCallback(() =>
        {
            Color color = new Color(Random.value, Random.value, Random.value, 1);
            for (int x = 0; x < size_x; x++)
            {
                for (int y = 0; y < size_y; y++)
                {
                    for (int z = 0; z < size_z; z++)
                    {
                        if (room[indexNow, x, y, z] == true)
                        {
                            GameObject cellPrefab = Instantiate(Cell, new Vector3(x, y, z), new Quaternion(0, 0, 0, 0), cellsParent);
                            cellPrefab.GetComponent<MeshRenderer>().material.color = color;
                            cellsSpawned.Add(cellPrefab);
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