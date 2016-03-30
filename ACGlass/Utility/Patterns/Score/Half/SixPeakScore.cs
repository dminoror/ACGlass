using ACGlass.Classes;
using ACGlass.Classes.Patterns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACGlass.Utility.Patterns.Score
{
    public class SixPeakScore : ScoreUtility// 3
    {
        public SixPeakScore()
        {
            Valence = 0.6458333;
            Arousal = 0.8083333;
            Name = "SixPeak";
            pair = new int[] { 0, 1, 3, 4 };
            selfId = 3;
        }
        public override Pattern generatePattern(double V, double A, double[] distance)
        {
            double picker = 0;
            List<int> cand = new List<int>();
            for (int i = 0; i < pair.Length; i++)
                if (distance[pair[i]] > picker)
                    picker = distance[pair[i]];
            picker = BasicUtility.rander.NextDouble() * picker;
            for (int i = 0; i < pair.Length; i++)
                if (distance[pair[i]] >= picker)
                    cand.Add(pair[i]);
            int indexPT = BasicUtility.rander.Next(cand.Count);
            ScoreUtility ptPair = PatternSelector.pairPatterns[cand[indexPT]];

            int tune = 0;
            TSDChordPattern tsd = new TSDChordPattern(V, A);
            Chord[] chords1 = TSDChord.generate(tune, tsd);
            Chord[] chords2 = TSDChord.generate(tune, tsd);
            int[] registers = BasicUtility.Cfinder(new int[] { 4, 5, 6 }, 2);
            registers[0] *= 12;
            registers[1] *= 12;
            byte loudness1 = 127;
            byte loudness2 = (byte)(loudness1 * 0.8);
            bool? order1 = null, order2 = null;
            if (cand[indexPT] == selfId)
            {
                order1 = BasicUtility.throwCoin;
                order2 = order1 == false;
            }

            //BaseNote[] hand1 = generateNotes(V, A, chords1, registers[0], loudness1, order1);
            //BaseNote[] hand2 = ptPair.generateNotes(V, A, chords2, registers[1], loudness2, order2);
            int BPM = ScoreUtility.BPMFromDurings(V, A, minimumDuring, ptPair.minimumDuring);

            Pattern pattern = new Pattern()
            {
                Valence = V,
                Arousal = A,
                patterns = new ScoreUtility[] { this, ptPair },
                registers = registers,
                loudness = new byte[] { loudness1, loudness2 },
                orders = new bool?[] { order1, order2 },
                tune = tune,
                BPM = BPM
            };
            return pattern;
        }
        public override List<BaseNote> generateNotes(double V, double A, Chord[] chords, int register, byte loudness, bool? setOrder)
        {
            List<BaseNote> notes = new List<BaseNote>();
            int order = findOrder(V, A, setOrder);
            bool? isReduce = findIsReduce(V, A);
            for (int section = 0; section < 4; section++)
            {
                Chord chord = chords[section];
                SixPeak peak = new SixPeak(chord, order, isReduce);
                for (int loop = 0; loop < 4; loop++)
                {
                    for (int note = 0; note < peak.notes.Length; note++)
                        notes.Add(new Note(4, (byte)(ACCore.pitchFromMajor(chord.tune, peak.notes[note]) + register), loudness)); ;
                }
            }
            return notes;
        }

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
                    for (int note = 0; note < peak.notes.Length; note++)
                        score.Add(new Note(noteDuring, (byte)(ACCore.pitchFromMajor(chord.tune, peak.notes[note]) + register), loudness)); ;
                }
            }
            return score;
        }
        public int findOrder(double V, double A, bool? order)
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
        public bool? findIsReduce(double V, double A)
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

        public override int minimumDuring
        {
            get { return 4; }
        }
    }
}
