using ACGlass.Classes;
using ACGlass.Classes.Patterns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACGlass.Utility.Patterns.Score.Full
{
    public class Metamor2Score : ScoreUtility// 7
    {
        public Metamor2Score()
        {
            Valence = 0.3;
            Arousal = 0.275;
            Name = "Metamor2";
            selfId = 7;
        }
        public override Pattern generatePattern(double V, double A, double[] distance)
        {
            int tune = 0;
            byte loudness1 = 90;
            byte loudness2 = 127;
            int[] registers = new int[] { BasicUtility.rander.Next(3, 6) * 12 };

            int BPM = ScoreUtility.BPMFromDuring(V, A, minimumDuring);

            Pattern pattern = new Pattern()
            {
                Valence = V,
                Arousal = A,
                patterns = new ScoreUtility[] { this },
                registers = registers,
                loudness = new byte[] { loudness1, loudness2 },
                tune = tune,
                BPM = BPM
            };
            return pattern;
        }
        public override List<BaseNote>[] generateScore(Pattern[] patterns, int index)
        {
            Pattern pattern = patterns[index];
            List<BaseNote> hand1 = new List<BaseNote>();
            List<BaseNote> hand2 = new List<BaseNote>();
            Chord[] chords = new Chord[4];
            int degree = BasicUtility.rander.Next(7);//TSDChord.chordT[BasicUtility.rander.Next(3)];
            chords[0] = new Chord(degree, 0, pattern.tune);
            chords[1] = new Chord(chords[0].degree, 0, pattern.tune);
            chords[2] = new Chord(chords[0].degree, 0, pattern.tune);
            chords[3] = new Chord(chords[0].degree, 0, pattern.tune);
            chords[1].notes[0] -= 1;
            chords[2].notes[0] -= 2;

            hand1.Add(new RestNote(192));
            hand1.Add(new Note(192, new byte[] { 
                (byte)(ACCore.pitchFromMajor(pattern.tune, chords[0].notes[0]) + pattern.registers[0]),
                (byte)(ACCore.pitchFromMajor(pattern.tune, chords[0].notes[0]) + pattern.registers[0] - 12) }, pattern.loudness[0]));
            for (int i = 0; i < 3; i++)
            {
                hand1.Add(new RestNote(48));
                hand1.Add(new Note(24, new byte[] { 
                    (byte)(ACCore.pitchFromMajor(pattern.tune, chords[0].notes[2] + 2) + pattern.registers[0] + 24),
                    (byte)(ACCore.pitchFromMajor(pattern.tune, chords[0].notes[2] + 2) + pattern.registers[0] + 12) }, pattern.loudness[0]));
                hand1.Add(new Note(24, new byte[] { 
                    (byte)(ACCore.pitchFromMajor(pattern.tune, chords[0].notes[2]) + pattern.registers[0] + 24),
                    (byte)(ACCore.pitchFromMajor(pattern.tune, chords[0].notes[2]) + pattern.registers[0] + 12) }, pattern.loudness[0]));
            }
            hand1.Add(new RestNote(48));
            hand1.Add(new Note(24, new byte[] { 
                (byte)(ACCore.pitchFromMajor(pattern.tune, chords[0].notes[2]) + pattern.registers[0] + 23),
                (byte)(ACCore.pitchFromMajor(pattern.tune, chords[0].notes[2]) + pattern.registers[0] + 11) }, pattern.loudness[0]));
            hand1.Add(new Note(24, new byte[] { 
                (byte)(ACCore.pitchFromMajor(pattern.tune, chords[0].notes[1]) + pattern.registers[0] + 24),
                (byte)(ACCore.pitchFromMajor(pattern.tune, chords[0].notes[1]) + pattern.registers[0] + 12) }, pattern.loudness[0]));

            for (int i = 0; i < 2; i++)
            {
                hand2.Add(new Note(12, new byte[] { 
                    (byte)(ACCore.pitchFromMajor(pattern.tune, chords[0].notes[0]) + pattern.registers[0]),
                    (byte)(ACCore.pitchFromMajor(pattern.tune, chords[0].notes[1]) + pattern.registers[0]) }, pattern.loudness[0]));
                hand2.Add(new Note(12, (byte)(ACCore.pitchFromMajor(pattern.tune, chords[0].notes[2]) + pattern.registers[0]), pattern.loudness[0]));
                for (int j = 0; j < 7; j++)
                {
                    hand2.Add(new Note(12, (byte)(ACCore.pitchFromMajor(pattern.tune, chords[0].notes[1]) + pattern.registers[0]), pattern.loudness[0]));
                    hand2.Add(new Note(12, (byte)(ACCore.pitchFromMajor(pattern.tune, chords[0].notes[2]) + pattern.registers[0]), pattern.loudness[0]));
                }
            }
            for (int i = 0; i < 3; i++)
            {
                hand2.Add(new Note(12, new byte[] { 
                    (byte)(ACCore.pitchFromMajor(pattern.tune, chords[i].notes[0]) + pattern.registers[0]),
                    (byte)(ACCore.pitchFromMajor(pattern.tune, chords[i].notes[1]) + pattern.registers[0]) }, pattern.loudness[0]));
                hand2.Add(new Note(12, (byte)(ACCore.pitchFromMajor(pattern.tune, chords[i].notes[2]) + pattern.registers[0]), pattern.loudness[0]));
                for (int j = 0; j < 3; j++)
                {
                    hand2.Add(new Note(12, (byte)(ACCore.pitchFromMajor(pattern.tune, chords[i].notes[1]) + pattern.registers[0]), pattern.loudness[0]));
                    hand2.Add(new Note(12, (byte)(ACCore.pitchFromMajor(pattern.tune, chords[i].notes[2]) + pattern.registers[0]), pattern.loudness[0]));
                }
            }
            hand2.Add(new Note(12, new byte[] { 
                    (byte)(ACCore.pitchFromMajor(pattern.tune, chords[3].notes[0]) + pattern.registers[0] - 1),
                    (byte)(ACCore.pitchFromMajor(pattern.tune, chords[3].notes[1]) + pattern.registers[0]) }, pattern.loudness[0]));
            hand2.Add(new Note(12, (byte)(ACCore.pitchFromMajor(pattern.tune, chords[3].notes[2]) + pattern.registers[0] - 1), pattern.loudness[0]));
            for (int j = 0; j < 3; j++)
            {
                hand2.Add(new Note(12, (byte)(ACCore.pitchFromMajor(pattern.tune, chords[3].notes[1]) + pattern.registers[0]), pattern.loudness[0]));
                hand2.Add(new Note(12, (byte)(ACCore.pitchFromMajor(pattern.tune, chords[3].notes[2]) + pattern.registers[0] - 1), pattern.loudness[0]));
            }

            List<BaseNote>[] score = new List<BaseNote>[] { hand1, hand2 };
            if (index != patterns.Length - 1 && pattern.BPM != patterns[index + 1].BPM)
            {
                double distance = (pattern.BPM - patterns[index + 1].BPM) / 5.0;
                int beatLen = hand1.Count / 16;
                for (int i = 4; i > 0; i--)
                    hand1.Insert(hand1.Count - beatLen * i, new TempoChanger((int)(pattern.BPM - distance * (5 - i))));
            }
            hand1.Insert(0, new TempoChanger(pattern.BPM));
            return score;
        }
        public override int minimumDuring
        {
            get { return 12; }
        }
    }
}
