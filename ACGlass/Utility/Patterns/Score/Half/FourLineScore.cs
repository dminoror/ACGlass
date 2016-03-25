using ACGlass.Classes;
using ACGlass.Classes.Patterns;
using ACGlass.Classes.Patterns.Chords;
using ACGlass.Classes.Patterns.Notes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACGlass.Utility.Patterns.Score
{
    public class FourLineScore : ScoreUtility// 2
    {
        public FourLineScore()
        {
            Valence = 0.5;
            Arousal = 0.575;
            Name = "FourLine";
            pair = new int[] { 2 };
            selfId = 2;
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
                chords = new Chord[][] { chords1, chords2 },
                BPM = BPM
            };
            return pattern;
        }
        public override List<BaseNote> generateNotes(double V, double A, Chord[] chords, int register, byte loudness, bool? setOrder)
        {
            int order = 0;
            if (setOrder == null)
                order = BasicUtility.rander.Next(2);
            else
                order = setOrder == true ? 1 : 0;
            int stable = 0;
            FourChord[] fourChords = FourChords.generate(chords[0].tune, chords, stable, FourChords.findIsSeven(V, A));
            List<BaseNote> notes = FourLineNotes.generate(4, order, fourChords, register, loudness);
            return notes;
        }
        public override int minimumDuring
        {
            get { return 6; }
        }
    }
}
