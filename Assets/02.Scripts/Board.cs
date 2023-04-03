using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    private const int SIZE = 9;

    [SerializeField] private SlotBehaviour _slotBehaviourPrefab;

    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        Slot[,] slots = new Slot[SIZE, SIZE];

        if (!Generate())
        {
            Debug.LogError("Fail to generate sudoku puzzle");
            return;
        }
        Erase();
        Instantiate();

        bool Generate()
        {
            for (int row = 0; row < SIZE; row++)
            {
                for (int column = 0; column < SIZE; column++)
                {
                    slots[row, column] = new Slot(row, column);
                }
            }

            for (int i = 0; i < SIZE * SIZE; i++)
            {
                int count = SIZE + 1;
                Slot min = null;

                foreach (var slot in slots)
                {
                    if (slot.number == 0 && slot.candidates.Count < count)
                    {
                        count = slot.candidates.Count;
                        min = slot;
                    }
                }

                if (min.candidates.Count == 0) return false;

                var number = min.candidates[Random.Range(0, min.candidates.Count)];

                min.number = number;
                min.candidates.Remove(number);

                Remove(min.row, min.column, number);
            }

            return true;

            void Remove(int row, int column, int number)
            {
                for (int r = 0; r < SIZE; r++)
                {
                    if (r == row) continue;

                    slots[r, column].candidates.Remove(number);
                }
                for (int c = 0; c < SIZE; c++)
                {
                    if (c == column) continue;

                    slots[row, c].candidates.Remove(number);
                }
                for (int r = row / 3 * 3; r < (row / 3 + 1) * 3; r++)
                {
                    for (int c = column / 3 * 3; c < (column / 3 + 1) * 3; c++)
                    {
                        if (r == row || c == column) continue;

                        slots[r, c].candidates.Remove(number);
                    }
                }
            }
        }

        void Erase()
        {

        }

        void Instantiate()
        {
            foreach (var slot in slots)
            {
                if (slot.number == 0) continue;

                var prefab = _slotBehaviourPrefab;
                var position = new Vector2(slot.column, slot.row);
                var parent = transform;
                var rotation = Quaternion.identity;

                GameObject.Instantiate(prefab, position, rotation, parent).Number = slot.number;
            }
        }
    }
}
