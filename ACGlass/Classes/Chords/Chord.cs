using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACGlass.Classes
{
    public class Chord
    {
        public int[] notes;
        public int turn;
        public int degree;
        public int tune;
        public Chord() { }
        public Chord(int buttom, int setTurn, int setTune)
        {
            degree = buttom;
            turn = setTurn;
            tune = setTune;
            notes = Chord.initNotes(buttom, turn);
        }
        public Chord Clone()
        {
            Chord clone = new Chord();
            clone.turn = turn;
            clone.degree = degree;
            clone.tune = tune;
            clone.notes = new int[notes.Length];
            notes.CopyTo(clone.notes, 0);
            return clone;
        }
        public void turnUp()
        {
            turn++;
            int temp = notes[0] + 7;
            notes[0] = notes[1];
            notes[1] = notes[2];
            notes[2] = temp;
        }
        public void turnDown()
        {
            turn--;
            int temp = notes[2] - 7;
            notes[2] = notes[1];
            notes[1] = notes[0];
            notes[0] = temp;
        }
        public void turnOctave(int count)
        {
            turn += 3 * count;
            notes[0] += 7 * count;
            notes[1] += 7 * count;
            notes[2] += 7 * count;
        }
        public static int[] initNotes(int buttom, int turn)
        {
            int[] notes = new int[3] { buttom, buttom + 2, buttom + 4 };
            int reg = (turn / 3) * 7;
            switch (turn % 3)
            {
                case 0:
                    {
                        notes[0] += reg;
                        notes[1] += reg;
                        notes[2] += reg;
                    } break;
                case 1:
                    {
                        int temp = notes[0] + 7 + reg;
                        notes[0] = notes[1] + reg;
                        notes[1] = notes[2] + reg;
                        notes[2] = temp;
                    } break;
                case 2:
                    {
                        int temp = notes[0] + 7 + reg;
                        notes[0] = notes[2] + reg;
                        notes[2] = notes[1] + 7 + reg;
                        notes[1] = temp;
                    } break;
                case -1:
                    {
                        int temp = notes[0];
                        notes[0] = notes[2] - 7 + reg;
                        notes[2] = notes[1] + reg;
                        notes[1] = temp + reg;
                    } break;
                case -2:
                    {
                        int temp = notes[2];
                        notes[2] = notes[0] + reg;
                        notes[0] = notes[1] - 7 + reg;
                        notes[1] = temp - 7 + reg;
                    } break;
            }
            return notes;
        }
        public static int[] initNotes(int buttom, int turn, int length)
        {
            int[] notes = Chord.initNotes(buttom, turn);
            int[] result = new int[length];
            Array.Copy(notes, result, length);
            return result;
        }
    }

    /*
     * ####### C#  1
     * ######  F#  6
     * #####   B   11
     * ####    E   4
     * ###     A   9
     * ##      D   2
     * #       G   7
     *         C   0
     * b       F   5
     * bb      Bb  10
     * bbb     Eb  3
     * bbbb    Ab  8
     * bbbbb   Db  1
     * bbbbbb  Gb  6
     * bbbbbbb Cb  11
     */
}
