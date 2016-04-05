using ACGlass.Classes;
using ACGlass.Classes.Patterns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACGlass.Utility.Patterns.Score.Full
{
    public class EtudeNo6_1Score : ScoreUtility
    {
        public EtudeNo6_1Score()
        {
            Valence = 0.375;
            Arousal = 0.525;
            Name = "EtudeNo6-1";
        }
        public override Pattern generatePattern(double V, double A)
        {
            int tune = BasicUtility.rander.Next(12);
            int[] registers = new int[2];
            registers[0] = BasicUtility.rander.Next(5, 6) * 12;
            registers[1] = registers[0] - 24;
            byte[] loudness = new byte[] { 127, 127 };

            int BPM = ScoreUtility.BPMFromDuring(V, A, minimumDuring);

            Pattern pattern = new Pattern()
            {
                Valence = V,
                Arousal = A,
                patterns = new ScoreUtility[] { this },
                registers = registers,
                loudness = loudness,
                tune = tune,
                BPM = BPM
            };
            return pattern;
        }
        public override List<BaseNote>[] generateScore(Pattern[] patterns, int index)
        {
            Pattern pattern = patterns[index];

            int mode = findMode(pattern.Valence, pattern.Arousal);
            int degree = 0;// BasicUtility.rander.Next(7);
            Chord chord = new Chord(degree, 0, pattern.tune);

            List<BaseNote> hand1 = new List<BaseNote>();
            List<BaseNote> hand2 = new List<BaseNote>();
            mainMode1(pattern, chord, mode, hand1, BasicUtility.throwCoin);
            pairMode1(pattern, chord, mode, hand2);

            List<BaseNote>[] score = new List<BaseNote>[] { hand1, hand2 };
            if (index != patterns.Length - 1 && pattern.BPM != patterns[index + 1].BPM)
            {
                double distance = (pattern.BPM - patterns[index + 1].BPM) / 5.0;
                hand2.Insert(hand2.Count - 8, new TempoChanger((int)(pattern.BPM - distance)));
                hand2.Insert(hand2.Count - 6, new TempoChanger((int)(pattern.BPM - distance * 2)));
                hand2.Insert(hand2.Count - 4, new TempoChanger((int)(pattern.BPM - distance * 3)));
                hand2.Insert(hand2.Count - 2, new TempoChanger((int)(pattern.BPM - distance * 4)));
            }
            hand2.Insert(0, new TempoChanger(pattern.BPM));
            if (patterns.Length - 1 == index)
            {
                for (int i = 0; i < 16; i++)
                    score[0].Add(new Note(6, (byte)(ACCore.pitchFromMode(0, chord.tune, chord.notes[0]) + pattern.registers[0]), pattern.loudness[0]));
                for (int i = 0; i < 8; i++)
                {
                    score[1].Add(new Note(6, new byte[] { 
                    (byte)(ACCore.pitchFromMode(mode, chord.tune, chord.notes[0]) + pattern.registers[1]), 
                    (byte)(ACCore.pitchFromMode(mode, chord.tune, chord.notes[1]) + pattern.registers[1]) }, pattern.loudness[1]));
                    score[1].Add(new Note(6, (byte)(ACCore.pitchFromMode(mode, chord.tune, chord.notes[2]) + pattern.registers[1]), pattern.loudness[1]));
                }
            }
            return score;
        }
        void mainMode1(Pattern pattern, Chord chord, int mode, List<BaseNote> notes, bool step)
        {
            if (!step)
            {
                for (int i = 0; i < 36; i++)
                    notes.Add(new Note(6, (byte)(ACCore.pitchFromMode(mode, chord.tune, chord.notes[0]) + pattern.registers[0]), pattern.loudness[0]));
                for (int i = 0; i < 6; i++)
                    notes.Add(new Note(6, (byte)(ACCore.pitchFromMode(mode, chord.tune, chord.notes[0] + 1) + pattern.registers[0]), pattern.loudness[0]));
                for (int i = 0; i < 4; i++)
                    notes.Add(new Note(6, (byte)(ACCore.pitchFromMode(mode, chord.tune, chord.notes[0] + 2) + pattern.registers[0]), pattern.loudness[0]));
                for (int i = 0; i < 2; i++)
                    notes.Add(new Note(6, (byte)(ACCore.pitchFromMode(mode, chord.tune, chord.notes[0] + 1) + pattern.registers[0]), pattern.loudness[0]));
            }
            else
            {
                for (int i = 0; i < 4; i++)
                    notes.Add(new Note(6, (byte)(ACCore.pitchFromMode(mode, chord.tune, chord.notes[0]) + pattern.registers[0]), pattern.loudness[0]));
                for (int i = 0; i < 2; i++)
                    notes.Add(new Note(6, (byte)(ACCore.pitchFromMode(mode, chord.tune, chord.notes[0] + 1) + pattern.registers[0]), pattern.loudness[0]));
                for (int i = 0; i < 2; i++)
                    notes.Add(new Note(6, (byte)(ACCore.pitchFromMode(mode, chord.tune, chord.notes[0] + 2) + pattern.registers[0]), pattern.loudness[0]));
                for (int i = 0; i < 2; i++)
                    notes.Add(new Note(6, (byte)(ACCore.pitchFromMode(mode, chord.tune, chord.notes[0] + 3) + pattern.registers[0]), pattern.loudness[0]));
                for (int i = 0; i < 14; i++)
                    notes.Add(new Note(6, (byte)(ACCore.pitchFromMode(mode, chord.tune, chord.notes[0] + 4) + pattern.registers[0]), pattern.loudness[0]));
                for (int i = 0; i < 12; i++)
                    notes.Add(new Note(6, (byte)(ACCore.pitchFromMode(mode, chord.tune, chord.notes[0] + 5) + pattern.registers[0]), pattern.loudness[0]));
                for (int i = 0; i < 6; i++)
                    notes.Add(new Note(6, (byte)(ACCore.pitchFromMode(mode, chord.tune, chord.notes[0] + 4) + pattern.registers[0]), pattern.loudness[0]));
                for (int i = 0; i < 4; i++)
                    notes.Add(new Note(6, (byte)(ACCore.pitchFromMode(mode, chord.tune, chord.notes[0] + 3) + pattern.registers[0]), pattern.loudness[0]));
                for (int i = 0; i < 2; i++)
                    notes.Add(new Note(6, (byte)(ACCore.pitchFromMode(mode, chord.tune, chord.notes[0] + 4) + pattern.registers[0]), pattern.loudness[0]));
            }
        }
        void pairMode1(Pattern pattern, Chord chord, int mode, List<BaseNote> notes)
        {
            Chord section3 = new Chord(chord.degree - 3, 0, chord.tune);
            Chord section4 = new Chord(section3.degree + 2, 0, chord.tune);
            for (int i = 0; i < 12; i++)
            {
                notes.Add(new Note(6, new byte[] { 
                    (byte)(ACCore.pitchFromMode(mode, chord.tune, chord.notes[0]) + pattern.registers[1]), 
                    (byte)(ACCore.pitchFromMode(mode, chord.tune, chord.notes[1]) + pattern.registers[1]) }, pattern.loudness[1]));
                notes.Add(new Note(6, (byte)(ACCore.pitchFromMode(mode, chord.tune, chord.notes[2]) + pattern.registers[1]), pattern.loudness[1]));
            }
            for (int i = 0; i < 6; i++)
            {
                notes.Add(new Note(6, new byte[] { 
                    (byte)(ACCore.pitchFromMode(mode, section3.tune, section3.notes[0]) + pattern.registers[1]), 
                    (byte)(ACCore.pitchFromMode(mode, section3.tune, section3.notes[1]) + pattern.registers[1]) }, pattern.loudness[1]));
                notes.Add(new Note(6, (byte)(ACCore.pitchFromMode(mode, section3.tune, section3.notes[2]) + pattern.registers[1]), pattern.loudness[1]));
            }
            for (int i = 0; i < 6; i++)
            {
                notes.Add(new Note(6, new byte[] { 
                    (byte)(ACCore.pitchFromMode(mode, section4.tune, section4.notes[0]) + pattern.registers[1]), 
                    (byte)(ACCore.pitchFromMode(mode, section4.tune, section4.notes[1]) + pattern.registers[1]) }, pattern.loudness[1]));
                notes.Add(new Note(6, (byte)(ACCore.pitchFromMode(mode, section4.tune, section4.notes[2]) + pattern.registers[1]), pattern.loudness[1]));
            }
        }
    }
}
