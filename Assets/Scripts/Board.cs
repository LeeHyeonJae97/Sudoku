using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    private const int SIZE = 9;

    [SerializeField] private int _tryCount;
    [SerializeField] private int _blank;
    [SerializeField] private SlotBehaviour _slotBehaviourPrefab;

    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        Slot[,] slots = new Slot[SIZE, SIZE];

        int count = 0;

        while (!Initialize() && count++ < _tryCount) { Debug.Log("Fail!"); }

        Erase();

        Instantiate();

        bool Initialize()
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
            List<Slot> blanks = new List<Slot>();

            int[][] weights = new int[SIZE][];

            for (int i = 0; i < weights.Length; i++)
            {
                weights[i] = new int[SIZE];

                for (int j = 0; j < weights.Length; j++)
                {
                    weights[i][j] = SIZE - 1;
                }
            }

            for (int i = 0; i < _blank; i++)
            {
                int row = Random(weights.Select((w) => w.Sum()).ToArray());
                int column = Random(weights[row]);

                weights[row][column] = 0;

                blanks.Add(slots[row, column]);
            }

            for (int i = 0; i < blanks.Count; i++)
            {
                var blank1 = blanks[i];

                for (int j = 0; j < blanks.Count; j++)
                {
                    var blank2 = blanks[j];

                    if (blank1.row == blank2.row || blank1.column == blank2.column || (blank1.row / 3 == blank2.row / 3 && blank1.column / 3 == blank2.column / 3))
                    {
                        blank2.candidates.Add(blank1.number);
                    }
                }

                blank1.number = 0;
            }

            int Random(int[] weights)
            {
                int sum = weights.Sum();
                int num = UnityEngine.Random.Range(0, sum);

                for (int j = 0; j < weights.Length; j++)
                {
                    if (weights[j] > 0 && num < weights[j])
                    {
                        return j;
                    }

                    num -= weights[j];
                }

                return -1;
            }
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
