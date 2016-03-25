using ACGlass.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACGlass.Classes
{
    public class SixPeak
    {
        public int[] notes;
        public SixPeak(Chord chord, int order, bool? isReduce)
        {
            if (order < 3)
            {
                notes = new int[6] { 
                        chord.notes[0], chord.notes[1], 
                        chord.notes[2], chord.notes[0] + 7, 
                        chord.notes[2], chord.notes[1] };
            }
            else
            {
                notes = new int[6] { 
                        chord.notes[0] + 7, chord.notes[2], 
                        chord.notes[1], chord.notes[0], 
                        chord.notes[1], chord.notes[2] };
            }
            if (order == 0 || order == 3) return;

            double move = BasicUtility.rander.NextDouble();
            if (order == 1 || order == 4)
                if (move <= 0.5)
                    moveFree(1, isReduce);
                else if (move <= 0.9)
                    moveFarFirst(2, isReduce);
                else
                    moveReduceOnly(3);
            else if (order == 2 || order == 5)
                if (move <= 0.5)
                    moveFree(5, isReduce);
                else if (move <= 0.9)
                    moveFarFirst(4, isReduce);
                else
                    moveReduceOnly(3);
        }
        // order note:
        // 0. low-high-low  classic
        // 1. low-high-low  move first low
        // 2. low-high-low  move second low
        // 3. high-low-high classic
        // 4. high-low-high move first high
        // 5. high-low-high move second high
        void moveFree(int index, bool? isReduce)
        {
            if (isReduce == null)
            {
                if (BasicUtility.throwCoin)
                    notes[index] += 1;
                else
                    notes[index] -= 1;
            }
            else if (isReduce == true)
                notes[index] -= 1;
            else
                notes[index] += 1;
            
        }
        void moveFarFirst(int index, bool? isReduce)
        {
            if (Math.Abs(notes[index - 1] - notes[index]) > 2)
                notes[index] -= 1;
            else if (Math.Abs(notes[index + 1] - notes[index]) > 2)
                notes[index] += 1;
            else
            {
                if (isReduce == true)
                    notes[index] -= 1;
                else if (isReduce == false)
                    notes[index] += 1;
                else
                    if (BasicUtility.throwCoin)
                        notes[index] += 1;
                    else
                        notes[index] -= 1;
            }
        }
        void moveReduceOnly(int index)
        {
            notes[index] -= 1;
        }
    }
}
