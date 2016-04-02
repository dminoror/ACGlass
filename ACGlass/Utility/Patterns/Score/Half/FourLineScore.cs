using ACGlass.Classes;
using ACGlass.Classes.Patterns;
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
        }
        public override void getPairs()
        {
            pair = new ScoreUtility[] { PatternSelector.ptFourLine };
        }
        public override Pattern generatePattern(double V, double A)
        {
            ScoreUtility ptPair = this;

            int tune = BasicUtility.rander.Next(12);
            int mode = findMode(V, A);
            TSDChordPattern tsd = new TSDChordPattern(V, A);
            Chord[] chords1 = TSDChord.generate(tune, tsd);
            Chord[] chords2 = TSDChord.generate(tune, tsd);
            int[] registers = BasicUtility.Cfinder(new int[] { 4, 5, 6 }, 2);
            registers[0] *= 12;
            registers[1] *= 12;
            byte loudness1 = 127;
            byte loudness2 = (byte)(loudness1 * 0.8);
            bool? order1 = BasicUtility.throwCoin, order2 = order1 == false;
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
            int stable = 0;
            FourChord[] fourChords = generateChords(chords[0].tune, chords, stable, findIsSeven(pattern.Valence, pattern.Arousal));
            List<BaseNote> notes = new List<BaseNote>();
            if (order == 0)
            {
                for (int section = 0; section < 4; section++)
                {
                    FourChord chord = fourChords[section];
                    for (int loop = 0; loop < 4; loop++)
                    {
                        for (int n = 0; n < 4; n++)
                            notes.Add(new Note(6, (byte)(ACCore.pitchFromMode(pattern.mode, chord.tune, chord.notes[n]) + register), loudness));
                    }
                }
            }
            else
            {
                for (int section = 0; section < 4; section++)
                {
                    FourChord chord = fourChords[section];
                    for (int loop = 0; loop < 4; loop++)
                    {
                        for (int n = 3; n >= 0; n--)
                            notes.Add(new Note(6, (byte)(ACCore.pitchFromMode(pattern.mode, chord.tune, chord.notes[n]) + register), loudness));
                    }
                }
            }
            
            return notes;
        }

        public FourChord[] generateChords(int tune, Chord[] sourceChords, int stable, bool isSeven)
        {
            FourChord[] chords = new FourChord[sourceChords.Length];
            if (isSeven)
            {
                for (int i = 0; i < chords.Length; i++)
                {
                    Chord chord = sourceChords[i];
                    chords[i] = new FourChord(chord.notes[0], chord.notes[2] + 3, tune, 0, stable);
                }
            }
            else
            {
                for (int i = 0; i < chords.Length; i++)
                {
                    Chord chord = sourceChords[i];
                    chords[i] = new FourChord(chord.notes[0], chord.notes[2] + 2, tune, 0, stable);
                }
            }
            return chords;
        }

        public bool findIsSeven(double V, double A)
        {
            double[][] em = new double[2][] { new double[] { 0.3, 0.5 }, new double[] { 0.7, 0.5 } };
            int index = BasicUtility.powerSelect(em, new double[] { V, A });
            return index == 1;
        }

        public override int minimumDuring
        {
            get { return 6; }
        }
    }
}
