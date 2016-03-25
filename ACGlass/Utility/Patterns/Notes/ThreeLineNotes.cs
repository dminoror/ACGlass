using ACGlass.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACGlass.Classes.Patterns.Notes
{
    public static class ThreeLineNotes
    {
        public static List<BaseNote> generate(int loopCount, int order, Chord[] chords, int register, byte loudness)
        {
            List<BaseNote> score = new List<BaseNote>(32);
            int noteDuring = 96 / loopCount / 3;
            int[] index = null;
            if (order == 0)
                index = new int[] { 0, 1, 2 };
            else
                index = new int[] { 2, 1, 0 };
            for (int section = 0; section < 4; section++)
            {
                Chord chord = chords[section];
                for (int loop = 0; loop < loopCount; loop++)
                {
                    for (int i = 0; i < index.Length; i++)
                        score.Add(new Note(8, (byte)(ACCore.pitchFromMajorDegree(chord.tune, chord.notes[index[i]]) + register), loudness));
                }
            }
            return score;
        }
    }
}
