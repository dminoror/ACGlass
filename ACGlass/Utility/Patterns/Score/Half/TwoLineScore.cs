using ACGlass.Classes;
using ACGlass.Classes.Patterns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACGlass.Utility.Patterns.Score
{
    public class TwoLineScore : ScoreUtility// 0
    {
        double[][] emOrder;
        public TwoLineScore()
        {
            Valence = 0.5375;
            Arousal = 0.2875;
            Name = "TwoLine";
            emOrder = new double[2][] { new double[] { 5.5, 3 }, new double[] { 5.25, 2.75 } };
        }
        public override void getPairs()
        {
            pair = new ScoreUtility[] { PatternSelector.ptTwoLine, PatternSelector.ptThreeLine, PatternSelector.ptSixPeak, PatternSelector.ptHeadTwo };
        }
        public override Pattern generatePattern(double V, double A)
        {
            double[][] em = new double[pair.Length][];
            for (int i = 0; i < em.Length; i++)
                em[i] = new double[] { pair[i].Valence, pair[i].Arousal };
            int indexPT = BasicUtility.powerSelect(em, new double[] { V, A });
            ScoreUtility ptPair = pair[indexPT];

            int mode = findMode(V, A);
            int tune = BasicUtility.rander.Next(12);
            TSDChordPattern tsd = new TSDChordPattern(V, A);
            Chord[] chords1 = TSDChord.generate(tune, tsd);
            Chord[] chords2 = TSDChord.generate(tune, tsd);
            int[] registers = BasicUtility.Cfinder(new int[] { 4, 5, 6 }, 2);
            registers[0] *= 12;
            registers[1] *= 12;
            byte loudness1 = 127;
            byte loudness2 = (byte)(loudness1 * 0.8);
            bool? order1 = null, order2 = null;
            if (ptPair.Equals(this))
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
                mode = mode,
                BPM = BPM
            };
            return pattern;
        }
        public override List<BaseNote> generateNotes(Pattern pattern, Chord[] chords, int register, byte loudness, bool? setOrder)
        {
            int order = 0;
            if (setOrder == null)
                order = BasicUtility.rander.Next(2);
            else
                order = setOrder == true ? 1 : 0;
            List<BaseNote> notes = new List<BaseNote>();
            int index1 = order, index2 = 1 - order;
            for (int section = 0; section < 4; section++)
            {
                Chord chord = chords[section];
                for (int loop = 0; loop < 4; loop++)
                {
                    notes.Add(new Note(12, (byte)(ACCore.pitchFromMode(pattern.mode, chord.tune, chord.notes[index1]) + register), loudness));
                    notes.Add(new Note(12, (byte)(ACCore.pitchFromMode(pattern.mode, chord.tune, chord.notes[index2]) + register), loudness));
                }
            }
            return notes;
        }
        public override int minimumDuring
        {
            get { return 12; }
        }
    }
}
