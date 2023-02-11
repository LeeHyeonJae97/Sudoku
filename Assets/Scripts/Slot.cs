using System.Collections;
using System.Collections.Generic;

public class Slot
{
    public int row;
    public int column;
    public int number;
    public List<int> candidates;

    public Slot(int row, int column)
    {
        this.row = row;
        this.column = column;
        number = 0;
        candidates = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
    }

    public Slot(Slot slot)
    {
        row = slot.row;
        column = slot.column;
        number = slot.number;
        candidates = new List<int>(slot.candidates);
    }
}
