using ACGlass.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACGlass.Classes.Patterns.Notes
{
    public static class GlassworksNotes
    {
        public static List<BaseNote> generate(Chord[] chords, int register, byte loudness, int stable)
        {
            int[] noteIndex1 = null, noteIndex2 = null;
            int index1 = BasicUtility.rander.Next(2);
            int index2 = 1 - index1;
            index1++; index2++;
            switch (stable)
            {
                case 0:
                    {
                        noteIndex1 = new int[4] { index1, index1, index1, index1 };
                        noteIndex2 = new int[4] { index2, index2, index2, index2 };
                    } break;
                case 1:
                    {
                        noteIndex1 = new int[4] { index1, index2, index1, index1 };
                        noteIndex2 = new int[4] { index2, index1, index2, index2 };
                    } break;
                case 2:
                    {
                        noteIndex1 = new int[4] { index1, index2, index2, index1 };
                        noteIndex2 = new int[4] { index2, index1, index1, index2 };
                    } break;
                case 3:
                    {
                        noteIndex1 = new int[4] { BasicUtility.rander.Next(1, 3), BasicUtility.rander.Next(1, 3), BasicUtility.rander.Next(1, 3), BasicUtility.rander.Next(1, 3) };
                        noteIndex2 = new int[4] { 3 - noteIndex1[0], 3 - noteIndex1[1], 3 - noteIndex1[2], 3 - noteIndex1[3] };
                    } break;
            }
            List<BaseNote> score = new List<BaseNote>();
            for (int section = 0; section < 4; section++)
            {
                Chord chord = chords[section];
                for (int i = 0; i < 6; i++)
                {
                    score.Add(new Note(8, (byte)(ACCore.pitchFromMajorDegree(chord.tune, chord.notes[noteIndex1[section]]) + register), loudness));
                    score.Add(new Note(8, (byte)(ACCore.pitchFromMajorDegree(chord.tune, chord.notes[noteIndex2[section]]) + register), loudness));
                }
            }
            return score;
        }
    }
}
