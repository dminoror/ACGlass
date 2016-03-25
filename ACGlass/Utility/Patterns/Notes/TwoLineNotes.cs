using ACGlass.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACGlass.Classes.Patterns.Notes
{
    public static class TwoLineNotes
    {
        public static List<BaseNote> generate(int loopCount, int order, Chord[] chords, int register, byte loudness)
        {
            List<BaseNote> score = new List<BaseNote>(32);
            int noteDuring = 96 / loopCount / 2;
            int index1 = order, index2 = 1 - order;
            for (int section = 0; section < loopCount; section++)
            {
                Chord chord = chords[section];
                for (int loop = 0; loop < loopCount; loop++)
                {
                    score.Add(new Note(noteDuring, (byte)(ACCore.pitchFromMajorDegree(chord.tune, chord.notes[index1]) + register), loudness));
                    score.Add(new Note(noteDuring, (byte)(ACCore.pitchFromMajorDegree(chord.tune, chord.notes[index2]) + register), loudness));
                }
            }
            return score;
        }
    }
}
