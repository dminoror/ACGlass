using ACGlass.Classes;
using ACGlass.Classes.Patterns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACGlass.Utility.Patterns.Score.Full
{
    public class EtudeNo3Score : ScoreUtility
    {
        public EtudeNo3Score()
        {
            Valence = 0.375;
            Arousal = 0.525;
            Name = "EtudeNo3";
        }
        public override Pattern generatePattern(double V, double A)
        {
            int tune = BasicUtility.rander.Next(12);
            int[] registers = new int[2];
            registers[0] = BasicUtility.rander.Next(4, 6) * 12;
            registers[1] = registers[0] - 12;
            byte[] loudness = new byte[] { 127, 127 };

            int BPM = ScoreUtility.BPMFromDuring(V, A,  minimumDuring);

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
            for (int i = 0; i < 8; i++)
            {
                hand1.Add(new Note(12, new byte[] { 
                    (byte)(ACCore.pitchFromMode(mode, pattern.tune, chord.notes[1]) + pattern.registers[0]),
                    (byte)(ACCore.pitchFromMode(mode, pattern.tune, chord.notes[2]) + pattern.registers[0]),
                    (byte)(ACCore.pitchFromMode(mode, pattern.tune, chord.notes[2] + 2) + pattern.registers[0]) }, pattern.loudness[0]));
                hand1.Add(new Note(12, (byte)(ACCore.pitchFromMode(mode, pattern.tune, chord.notes[0] + 7) + pattern.registers[0]), pattern.loudness[0]));
                hand1.Add(new RestNote(12));
            }
            hand1.RemoveAt(hand1.Count - 1);
            hand1[hand1.Count - 1].during = 24;
            List<BaseNote> hand2 = new List<BaseNote>();
            for (int i = 0; i < 11; i++)
            {
                hand2.Add(new Note(12, new byte[] { 
                    (byte)(ACCore.pitchFromMode(mode, pattern.tune, chord.notes[0]) + pattern.registers[1]),
                    (byte)(ACCore.pitchFromMode(mode, pattern.tune, chord.notes[1]) + pattern.registers[1]),
                    (byte)(ACCore.pitchFromMode(mode, pattern.tune, chord.notes[2]) + pattern.registers[1]) }, pattern.loudness[1]));
                hand2.Add(new Note(12, (byte)(ACCore.pitchFromMode(mode, pattern.tune, chord.notes[0] + 7) + pattern.registers[1]), pattern.loudness[0]));
            }
            hand2.Add(new Note(24, (byte)(ACCore.pitchFromMode(mode, pattern.tune, chord.notes[0]) + pattern.registers[1]), pattern.loudness[0]));

            if (BasicUtility.throwCoin)
                mainMode1(pattern, chord, mode, hand1, hand2);
            else
                mainMode2(pattern, chord, mode, hand1, hand2);

            List<BaseNote>[] score = new List<BaseNote>[] { hand1, hand2 };
            if (index != patterns.Length - 1 && pattern.BPM != patterns[index + 1].BPM)
            {
                double distance = (pattern.BPM - patterns[index + 1].BPM) / 7.0;
                hand2.Insert(hand2.Count - 6, new TempoChanger((int)(pattern.BPM - distance)));
                hand2.Insert(hand2.Count - 5, new TempoChanger((int)(pattern.BPM - distance * 2)));
                hand2.Insert(hand2.Count - 4, new TempoChanger((int)(pattern.BPM - distance * 3)));
                hand2.Insert(hand2.Count - 3, new TempoChanger((int)(pattern.BPM - distance * 4)));
                hand2.Insert(hand2.Count - 2, new TempoChanger((int)(pattern.BPM - distance * 5)));
                hand2.Insert(hand2.Count - 1, new TempoChanger((int)(pattern.BPM - distance * 6)));
            }
            hand2.Insert(0, new TempoChanger(pattern.BPM));
            return score;
        }
        public override int minimumDuring
        {
            get { return 12; }
        }

        public void mainMode1(Pattern pattern, Chord chord, int mode, List<BaseNote> hand1, List<BaseNote> hand2)
        {
            int index = hand1.Count;
            for (int i = 0; i < 3; i++)
            {
                hand1.Add(new Note(12, (byte)(ACCore.pitchFromMode(mode, pattern.tune, chord.notes[2]) + pattern.registers[0]), pattern.loudness[0]));
                hand1.Add(new Note(12, (byte)(ACCore.pitchFromMode(mode, pattern.tune, chord.notes[0]) + pattern.registers[0] + 12), pattern.loudness[0]));
            }
            for (int i = 0; i < 3; i++)
            {
                hand1.Add(new Note(12, (byte)(ACCore.pitchFromMode(mode, pattern.tune, chord.notes[2]) + pattern.registers[0] + 1), pattern.loudness[0]));
                hand1.Add(new Note(12, (byte)(ACCore.pitchFromMode(mode, pattern.tune, chord.notes[0]) + pattern.registers[0] + 12), pattern.loudness[0]));
            }
            for (int i = 0; i < 3; i++)
            {
                hand1.Add(new Note(12, (byte)(ACCore.pitchFromMode(mode, pattern.tune, chord.notes[2]) + pattern.registers[0] + 3), pattern.loudness[0]));
                hand1.Add(new Note(12, (byte)(ACCore.pitchFromMode(mode, pattern.tune, chord.notes[0]) + pattern.registers[0] + 12), pattern.loudness[0]));
            }
            hand1.Add(new Note(12, (byte)(ACCore.pitchFromMode(mode, pattern.tune, chord.notes[2]) + pattern.registers[0] + 1), pattern.loudness[0]));
            hand1.Add(new Note(12, (byte)(ACCore.pitchFromMode(mode, pattern.tune, chord.notes[0]) + pattern.registers[0] + 12), pattern.loudness[0]));
            hand1.Add(new Note(12, (byte)(ACCore.pitchFromMode(mode, pattern.tune, chord.notes[2]) + pattern.registers[0]), pattern.loudness[0]));
            hand1.Add(new Note(12, (byte)(ACCore.pitchFromMode(mode, pattern.tune, chord.notes[0]) + pattern.registers[0] + 12), pattern.loudness[0]));
            hand1.Add(new Note(24, (byte)(ACCore.pitchFromMode(mode, pattern.tune, chord.notes[2] - 1) + pattern.registers[0]), pattern.loudness[0]));

            for (int i = index; i < hand1.Count; i++)
            {
                Note note = (Note)hand1[i];
                hand2.Add(new Note(note.during, (byte)(note.pitch[0] - 24), pattern.loudness[1]));
            }
        }
        public void mainMode2(Pattern pattern, Chord chord, int mode, List<BaseNote> hand1, List<BaseNote> hand2)
        {
            int index = hand1.Count;
            for (int i = 0; i < 2; i++)
            {
                hand1.Add(new Note(12, (byte)(ACCore.pitchFromMode(mode, pattern.tune, chord.notes[2]) + pattern.registers[0]), pattern.loudness[0]));
                hand1.Add(new Note(12, (byte)(ACCore.pitchFromMode(mode, pattern.tune, chord.notes[0]) + pattern.registers[0] + 12), pattern.loudness[0]));
                hand1.Add(new Note(12, (byte)(ACCore.pitchFromMode(mode, pattern.tune, chord.notes[1]) + pattern.registers[0] + 11), pattern.loudness[0]));
            }
            for (int i = 0; i < 2; i++)
            {
                hand1.Add(new Note(12, (byte)(ACCore.pitchFromMode(mode, pattern.tune, chord.notes[2]) + pattern.registers[0] + 1), pattern.loudness[0]));
                hand1.Add(new Note(12, (byte)(ACCore.pitchFromMode(mode, pattern.tune, chord.notes[0]) + pattern.registers[0] + 12), pattern.loudness[0]));
                hand1.Add(new Note(12, (byte)(ACCore.pitchFromMode(mode, pattern.tune, chord.notes[1]) + pattern.registers[0] + 11), pattern.loudness[0]));
            }
            for (int i = 0; i < 2; i++)
            {
                hand1.Add(new Note(12, (byte)(ACCore.pitchFromMode(mode, pattern.tune, chord.notes[2]) + pattern.registers[0] + 3), pattern.loudness[0]));
                hand1.Add(new Note(12, (byte)(ACCore.pitchFromMode(mode, pattern.tune, chord.notes[0]) + pattern.registers[0] + 12), pattern.loudness[0]));
                hand1.Add(new Note(12, (byte)(ACCore.pitchFromMode(mode, pattern.tune, chord.notes[1]) + pattern.registers[0] + 11), pattern.loudness[0]));
            }
            hand1.Add(new Note(12, (byte)(ACCore.pitchFromMode(mode, pattern.tune, chord.notes[2]) + pattern.registers[0] + 1), pattern.loudness[0]));
            hand1.Add(new Note(12, (byte)(ACCore.pitchFromMode(mode, pattern.tune, chord.notes[0]) + pattern.registers[0] + 12), pattern.loudness[0]));
            hand1.Add(new Note(12, (byte)(ACCore.pitchFromMode(mode, pattern.tune, chord.notes[2]) + pattern.registers[0]), pattern.loudness[0]));
            hand1.Add(new Note(12, (byte)(ACCore.pitchFromMode(mode, pattern.tune, chord.notes[0]) + pattern.registers[0] + 12), pattern.loudness[0]));
            hand1.Add(new Note(12, (byte)(ACCore.pitchFromMode(mode, pattern.tune, chord.notes[2] - 1) + pattern.registers[0]), pattern.loudness[0]));
            hand1.Add(new Note(12, (byte)(ACCore.pitchFromMode(mode, pattern.tune, chord.notes[0]) + pattern.registers[0] + 12), pattern.loudness[0]));

            for (int i = index; i < hand1.Count; i++)
            {
                Note note = (Note)hand1[i];
                hand2.Add(new Note(note.during, (byte)(note.pitch[0] - 24), pattern.loudness[1]));
            }
        }
    }
}
