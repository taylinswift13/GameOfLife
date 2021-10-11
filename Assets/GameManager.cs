using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class GameManager : MonoBehaviour
{
    int size_x = 20;
    int size_y = 20;
    int size_z = 20;
    public bool[,,,] room = new bool[2, 20, 20, 20];
    public int indexBuffer = 1;
    public GameObject Cell;
    public List<GameObject> cellsSpawned = new List<GameObject>();
    public Transform cellsParent;
    bool updating = true;
    public Text liveText;
    public Text generationText;
    int generation = 0;

    private void Start()
    {
        //DesignedGenerator();
        generation = 1;
        RandomGenerator();
        generationText.text = "Generation: " + generation.ToString();
    }
    float elapsed = 0f;
    private void Update()
    {
        if (updating)
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
                    liveText.text = "Lives: " + cellsSpawned.Count.ToString() + "/8000";
                    generation++;
                    generationText.text = "Generation: " + generation.ToString();
                });
                DrawCells(indexBuffer, sequence);
            }
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
    void RandomGenerator()
    {
        for (int I = 0; I < 100; I++)
        {
            Vector3 position = new Vector3(Random.Range(8, 12), Random.Range(8, 12), Random.Range(8, 12));
            GameObject cell = Instantiate(this.Cell, position, new Quaternion(0, 0, 0, 0), cellsParent);
            room[0, (int)position.x, (int)position.y, (int)position.z] = true;
            cellsSpawned.Add(cell);
        }
        updating = true;
    }
    void DesignedGenerator()
    {
        bool ifReturn = false;
        for (int i = 8; i <= 12; i++)
        {
            for (int j = 8; j <= 12; j++)
            {
                for (int q = 8; q <= 12; q++)
                {
                    if (ifReturn)
                    {
                        GameObject cell = Instantiate(this.Cell, new Vector3(i, j, q), new Quaternion(0, 0, 0, 0), cellsParent);
                        room[0, i, j, q] = true;
                        cellsSpawned.Add(cell);
                        ifReturn = !ifReturn;
                    }
                    else
                    {
                        ifReturn = !ifReturn;
                        continue;
                    }
                }
            }
        }
    }
    public void Reset()
    {
        updating = false;
        for (int x = 1; x <= size_x - 2; x++)
        {
            for (int y = 1; y <= size_y - 2; y++)
            {
                for (int z = 1; z <= size_z - 2; z++)
                {
                    room[0, x, y, z] = false;
                }
            }
        }
        ClearAll();
        cellsSpawned.Clear();
        //DesignedGenerator();
        RandomGenerator();
        generation = 1;
        generationText.text = "Generation: " + generation.ToString();
    }
}