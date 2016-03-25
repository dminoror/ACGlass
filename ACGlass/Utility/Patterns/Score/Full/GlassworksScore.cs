using ACGlass.Classes;
using ACGlass.Classes.Patterns;
using ACGlass.Classes.Patterns.Chords;
using ACGlass.Classes.Patterns.Notes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACGlass.Utility.Patterns.Score.Full
{
    public class GlassworksScore : ScoreUtility// 5
    {
        public GlassworksScore()
        {
            Valence = 0.525;
            Arousal = 0.3;
            Name = "Glassworks";
            selfId = 5;
        }
        public override Pattern generatePattern(double V, double A, double[] distance)
        {
            int tune = 0;
            int register1 = BasicUtility.Cfinder(new int[] { 6, 7 }, 1)[0] * 12;
            int register2 = register1 - 12;
            byte loudness = 127;
            bool isEnd = true;
            Chord[] chords = GlassworksChord.generate(tune, isEnd);

            int turnCount = BasicUtility.rander.Next(3);
            foreach (Chord chord in chords)
                for (int i = 0; i < turnCount; i++)
                    chord.turnDown();
            int BPM = ScoreUtility.BPMFromDuring(V, A, minimumDuring);

            Pattern pattern = new Pattern()
            {
                Valence = V,
                Arousal = A,
                patterns = new ScoreUtility[] { this },
                registers = new int[] { register1, register2 },
                loudness = new byte[] { loudness },
                tune = tune,
                chords = new Chord[][] { chords },
                BPM = BPM
            };
            return pattern;
        }
        public override List<BaseNote>[] generateScore(Pattern[] patterns, int index)
        {
            Pattern pattern = patterns[index];
            int stable = BasicUtility.rander.Next(4);
            List<BaseNote> hand1 = GlassworksNotes.generate(pattern.chords[0], pattern.registers[0], pattern.loudness[0], stable);
            List<BaseNote> hand2 = HeadTwoNotes.generate(pattern.chords[0], pattern.registers[1], pattern.loudness[0]);
            List<BaseNote>[] score = new List<BaseNote>[] { hand1, hand2 };
            if (index != patterns.Length - 1 && pattern.BPM != patterns[index + 1].BPM)
            {
                double distance = (pattern.BPM - patterns[index + 1].BPM) / 5.0;
                int beatLen = hand2.Count / 16;
                for (int i = 4; i > 0; i--)
                    hand2.Insert(hand2.Count - beatLen * i, new TempoChanger((int)(pattern.BPM - distance * (5 - i))));
            }
            hand2.Insert(0, new TempoChanger(pattern.BPM));
            return score;
        }
        public override int minimumDuring
        {
            get { return 12; }
        }
    }
}
