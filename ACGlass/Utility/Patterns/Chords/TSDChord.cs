using ACGlass.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACGlass.Classes
{
    public class TSDChordPattern
    {
        public int[] chords = new int[4];
        public bool direct;
        public byte range;

        public TSDChordPattern(double V, double A)
        {
            Random rander = BasicUtility.rander;
            chords = new int[4];
            int chordType = rander.Next(3);
            if (chordType == 2)
            {
                chords[0] = TSDChord.chordT[rander.Next(TSDChord.chordT.Length)];
                chords[1] = TSDChord.chordS[rander.Next(TSDChord.chordS.Length)];
                chords[2] = TSDChord.chordD[rander.Next(TSDChord.chordD.Length)];
                chords[3] = TSDChord.chordT[0];
            }
            else
            {
                int chord1 = rander.Next(TSDChord.chordT.Length);
                chords[0] = TSDChord.chordT[chord1];
                if (chord1 == 0)
                    chords[1] = TSDChord.chordT[rander.Next(1, 3)];
                else
                {
                    if (chord1 == 1)
                        chords[1] = TSDChord.chordT[2];
                    else
                        chords[1] = TSDChord.chordT[1];
                }
                chords[2] = chordType == 1 ? TSDChord.chordD[rander.Next(TSDChord.chordD.Length)] : TSDChord.chordS[rander.Next(TSDChord.chordS.Length)];
                chords[3] = TSDChord.chordT[0];
            }

            //direct = rander.Next(2) == 0;
            //range = (byte)rander.Next(3);
            findDirection(V, A);
            findRange(V, A);
        }
        public void findDirection(double V, double A)
        {
            double[][] em = new double[2][] { new double[] { 0.6, 0.5 }, new double[] { 0.4, 0.5 } };
            int index = BasicUtility.powerSelect(em, new double[] { V, A });
            direct = index == 1;
        }
        public void findRange(double V, double A)
        {
            double[][] em = new double[3][] { new double[] { 0, 0 }, new double[] { 0.5, 0.5 }, new double[] { 1, 1 } };
            int index = BasicUtility.powerSelect(em, new double[] { V, A });
            range = (byte)index;
        }
    }

    public static class TSDChord
    {
        public static byte[] chordT = new byte[3] { 0, 2, 5 };
        public static byte[] chordS = new byte[2] { 1, 3 };
        public static byte[] chordD = new byte[2] { 4, 6 };
        public static byte[][] chordTSD = new byte[3][] { chordT, chordS, chordD };

        public static Chord[] generate(int tune, TSDChordPattern pattern)
        {
            int turn = BasicUtility.rander.Next(3) * (pattern.direct ? 1 : -1);
            Chord[] scores = new Chord[4];
            scores[0] = new Chord(pattern.chords[0], turn, tune);
            scores[1] = ACCore.findChordByClose(scores[0], pattern.chords[1], pattern.direct, pattern.range);
            scores[2] = ACCore.findChordByClose(scores[1], pattern.chords[2], pattern.direct, pattern.range);
            scores[3] = ACCore.findChordByClose(scores[0], pattern.chords[3], pattern.direct, 0);
            return scores;
        }
    }
}
