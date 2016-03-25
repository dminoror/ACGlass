using ACGlass.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACGlass.Classes.Patterns.Notes
{
    public static class HeadTwoNotes
    {
        public static List<BaseNote> generate(Chord[] chords, int register, byte loudness)
        {
            List<BaseNote> score = new List<BaseNote>();
            for (int section = 0; section < 4; section++)
            {
                Chord chord = chords[section];
                int bass = BasicUtility.throwCoin ? chord.notes[0] : chord.notes[2] - 7;
                score.Add(new Note(12,
                    new byte[] { (byte)(ACCore.pitchFromMajorDegree(chord.tune, bass) + register), 
                                 (byte)(ACCore.pitchFromMajorDegree(chord.tune, chord.notes[1]) + register) }, loudness));
                score.Add(new Note(12, (byte)(ACCore.pitchFromMajorDegree(chord.tune, chord.notes[2]) + register), loudness));
                for (int i = 0; i < 3; i++)
                {
                    score.Add(new Note(12, (byte)(ACCore.pitchFromMajorDegree(chord.tune, chord.notes[1]) + register), loudness));
                    score.Add(new Note(12, (byte)(ACCore.pitchFromMajorDegree(chord.tune, chord.notes[2]) + register), loudness));
                }
            }
            return score;
        }
    }
}
