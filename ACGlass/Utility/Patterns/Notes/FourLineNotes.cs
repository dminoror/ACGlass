using ACGlass.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACGlass.Classes.Patterns.Notes
{
    public static class FourLineNotes
    {
        public static List<BaseNote> generate(int loopCount, int order, FourChord[] chords, int register, byte loudness)
        {
            List<BaseNote> score = new List<BaseNote>();
            int noteDuring = 96 / loopCount / 4;
            if (order == 0)
            {
                for (int section = 0; section < 4; section++)
                {
                    FourChord chord = chords[section];
                    for (int loop = 0; loop < loopCount; loop++)
                    {
                        for (int n = 0; n < 4; n++)
                            score.Add(new Note(noteDuring, (byte)(ACCore.pitchFromMajorDegree(chord.tune, chord.notes[n]) + register), loudness));
                    }
                }
            }
            else
            {
                for (int section = 0; section < 4; section++)
                {
                    FourChord chord = chords[section];
                    for (int loop = 0; loop < loopCount; loop++)
                    {
                        for (int n = 3; n >= 0; n--)
                            score.Add(new Note(noteDuring, (byte)(ACCore.pitchFromMajorDegree(chord.tune, chord.notes[n]) + register), loudness));
                    }
                }
            }
            return score;
        }
    }
}
