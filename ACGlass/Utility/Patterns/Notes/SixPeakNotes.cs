using ACGlass.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACGlass.Classes.Patterns.Notes
{
    public static class SixPeakNotes
    {
        public static List<BaseNote> generate(int loopCount, int order, Chord[] chords, bool? isReduce, int register, byte loudness)
        {
            List<BaseNote> score = new List<BaseNote>(32);
            int noteDuring = 96 / loopCount / 6;

            for (int section = 0; section < loopCount; section++)
            {
                Chord chord = chords[section];
                SixPeak peak = new SixPeak(chord, order, isReduce);
                for (int loop = 0; loop < loopCount; loop++)
                {
                    //score.Add(new Note(noteDuring, (byte)(PatternGenerater.pitchFromMajorDegree(chord.tune, chord.Notes[index1]) + register), loudness));
                    //score.Add(new Note(noteDuring, (byte)(PatternGenerater.pitchFromMajorDegree(chord.tune, chord.Notes[index2]) + register), loudness));
                    for (int note = 0; note < peak.notes.Length; note++)
                        score.Add(new Note(noteDuring, (byte)(ACCore.pitchFromMajorDegree(chord.tune, peak.notes[note]) + register), loudness)); ;
                }
            }
            return score;
        }
        public static int findOrder(double V, double A, bool? order)
        {
            double[][] em = null;
            if (order == null)
            {
                em = new double[6][] { 
                    new double[] { 0.75, 0.75 }, 
                    new double[] { 0.7, 0.8 }, 
                    new double[] { 0.65, 0.825 }, 
                    new double[] { 0.6, 0.775 }, 
                    new double[] { 0.575, 0.875 }, 
                    new double[] { 0.6, 0.825 } };
            }
            else if (order == true)
                em = new double[3][] { new double[] { 0.75, 0.75 }, new double[] { 0.7, 0.8 }, new double[] { 0.65, 0.825 } };
            else
                em = new double[3][] { new double[] { 0.6, 0.775 }, new double[] { 0.575, 0.875 }, new double[] { 0.6, 0.825 } };
            int index = BasicUtility.powerSelect(em, new double[] { V, A });
            return index;
        }
        public static bool? findIsReduce(double V, double A)
        {
            double[][] em = new double[3][] { new double[] { 0.2, 0.5 }, new double[] { 0.5, 0.5 }, new double[] { 0.8, 0.5 } };
            int index = BasicUtility.powerSelect(em, new double[] { V, A });
            if (index == 0)
                return false;
            else if (index == 1)
                return null;
            else
                return true;
        }
    }
}
