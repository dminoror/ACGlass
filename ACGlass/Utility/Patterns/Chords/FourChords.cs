using ACGlass.Classes;
using ACGlass.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACGlass.Classes.Patterns.Chords
{
    public static class FourChords
    {
        public static FourChord[] generate(int tune, Chord[] sourceChords, int stable, bool isSeven)
        {
            FourChord[] chords = new FourChord[sourceChords.Length];
            if (isSeven)
            {
                for (int i = 0; i < chords.Length; i++)
                {
                    Chord chord = sourceChords[i];
                    chords[i] = new FourChord(chord.notes[0], chord.notes[2] + 3, tune, 0, stable);
                }
            }
            else
            {
                for (int i = 0; i < chords.Length; i++)
                {
                    Chord chord = sourceChords[i];
                    chords[i] = new FourChord(chord.notes[0], chord.notes[2] + 2, tune, 0, stable);
                }
            }
            return chords;
        }
        public static FourChord[] generateEtude2(int tune)
        {
            int offset = BasicUtility.rander.Next(7);
            FourChord[] fourChords = new FourChord[5];
            fourChords[0] = new FourChord(-1 + offset, 4 + offset, tune, 0, 0);
            fourChords[1] = new FourChord(-2 + offset, 4 + offset, tune, 0, 0);
            fourChords[2] = new FourChord(3 + offset, 9 + offset, tune + 3, -1, 0);
            fourChords[3] = new FourChord(-3 + offset, 4 + offset, tune, 0, 0);
            fourChords[4] = new FourChord(-5 + offset, 2 + offset, tune, 0, 0);
            return fourChords;
        }
        public static bool findIsSeven(double V, double A)
        {
            double[][] em = new double[2][] { new double[] { 0.3, 0.5 }, new double[] { 0.7, 0.5 } };
            int index = BasicUtility.powerSelect(em, new double[] { V, A });
            return index == 1;
        }
    }
}
