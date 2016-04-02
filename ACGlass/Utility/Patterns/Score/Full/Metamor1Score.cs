using ACGlass.Classes;
using ACGlass.Classes.Patterns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACGlass.Utility.Patterns.Score.Full
{
    public class Metamor1Score : ScoreUtility
    {
        public Metamor1Score()
        {
            Valence = 0.3;
            Arousal = 0.275;
            Name = "Metamor1";
        }
        public override Pattern generatePattern(double V, double A)
        {
            int tune = BasicUtility.rander.Next(12);
            int[] registers = new int[2];
            registers[0] = BasicUtility.rander.Next(4, 7) * 12;
            registers[1] = registers[0] - 12;
            byte loudness = 127;

            int BPM = ScoreUtility.BPMFromDuring(V, A, minimumDuring);

            Pattern pattern = new Pattern()
            {
                Valence = V,
                Arousal = A,
                patterns = new ScoreUtility[] { this },
                registers = registers,
                loudness = new byte[] { loudness },
                tune = tune,
                BPM = BPM
            };
            return pattern;
        }
        public override List<BaseNote>[] generateScore(Pattern[] patterns, int index)
        {
            Pattern pattern = patterns[index];
            Chord[] chords = new Chord[4];
            int mode = findMode(pattern.Valence, pattern.Arousal);
            int degree = BasicUtility.rander.Next(7);
            chords[0] = new Chord(degree, 0, pattern.tune);
            chords[1] = new Chord(chords[0].degree, 0, pattern.tune);
            chords[2] = new Chord(chords[0].degree, 0, pattern.tune);
            chords[3] = new Chord(chords[0].degree, 0, pattern.tune);
            chords[1].notes[0] -= 1;
            chords[2].notes[0] -= 2;
            chords[3].notes[0] -= 2;
            chords[3].notes[1] -= 2;
            List<BaseNote> hand1 = new List<BaseNote>();
            List<BaseNote> hand2 = new List<BaseNote>();
            for (int section = 0; section < 3; section++)
            {
                Chord chord = chords[section];
                hand1.Add(new Note(24, new byte[] { 
                    (byte)(ACCore.pitchFromMode(mode, chord.tune, chord.notes[0]) + pattern.registers[0]),
                    (byte)(ACCore.pitchFromMode(mode, chord.tune, chord.notes[1]) + pattern.registers[0]),
                    (byte)(ACCore.pitchFromMode(mode, chord.tune, chord.notes[2]) + pattern.registers[0]) }, pattern.loudness[0]));
                hand1.Add(new Note(72, new byte[] { 
                    (byte)(ACCore.pitchFromMode(mode, chord.tune, chord.notes[0]) + pattern.registers[0]),
                    (byte)(ACCore.pitchFromMode(mode, chord.tune, chord.notes[1]) + pattern.registers[0]),
                    (byte)(ACCore.pitchFromMode(mode, chord.tune, chord.notes[2]) + pattern.registers[0]) }, pattern.loudness[0]));
                hand2.Add(new Note(24, new byte[] { 
                    (byte)(ACCore.pitchFromMode(mode, chord.tune, chord.notes[0]) + pattern.registers[1]),
                    (byte)(ACCore.pitchFromMode(mode, chord.tune, chord.notes[1]) + pattern.registers[1]),
                    (byte)(ACCore.pitchFromMode(mode, chord.tune, chord.notes[2]) + pattern.registers[1]) }, pattern.loudness[0]));
                hand2.Add(new Note(72, new byte[] { 
                    (byte)(ACCore.pitchFromMode(mode, chord.tune, chord.notes[0]) + pattern.registers[1]),
                    (byte)(ACCore.pitchFromMode(mode, chord.tune, chord.notes[1]) + pattern.registers[1]),
                    (byte)(ACCore.pitchFromMode(mode, chord.tune, chord.notes[2]) + pattern.registers[1]) }, pattern.loudness[0]));
            }
            hand1.Add(new Note(24, new byte[] { 
                    (byte)(ACCore.pitchFromMode(mode, pattern.tune, chords[3].notes[0]) + pattern.registers[0]),
                    (byte)(ACCore.pitchFromMode(mode, pattern.tune, chords[3].notes[1]) + pattern.registers[0]),
                    (byte)(ACCore.pitchFromMode(mode, pattern.tune, chords[3].notes[2]) + pattern.registers[0] - 1) }, pattern.loudness[0]));
            hand1.Add(new Note(72, new byte[] { 
                    (byte)(ACCore.pitchFromMode(mode, pattern.tune, chords[3].notes[0]) + pattern.registers[0]),
                    (byte)(ACCore.pitchFromMode(mode, pattern.tune, chords[3].notes[1]) + pattern.registers[0]),
                    (byte)(ACCore.pitchFromMode(mode, pattern.tune, chords[3].notes[2]) + pattern.registers[0] - 1) }, pattern.loudness[0]));
            hand2.Add(new Note(24, new byte[] { 
                    (byte)(ACCore.pitchFromMode(mode, pattern.tune, chords[3].notes[0]) + pattern.registers[1]),
                    (byte)(ACCore.pitchFromMode(mode, pattern.tune, chords[3].notes[1]) + pattern.registers[1]),
                    (byte)(ACCore.pitchFromMode(mode, pattern.tune, chords[3].notes[2]) + pattern.registers[1] - 1) }, pattern.loudness[0]));
            hand2.Add(new Note(72, new byte[] { 
                    (byte)(ACCore.pitchFromMode(mode, pattern.tune, chords[3].notes[0]) + pattern.registers[1]),
                    (byte)(ACCore.pitchFromMode(mode, pattern.tune, chords[3].notes[1]) + pattern.registers[1]),
                    (byte)(ACCore.pitchFromMode(mode, pattern.tune, chords[3].notes[2]) + pattern.registers[1] - 1) }, pattern.loudness[0]));
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
            get { return 14; }
        }
    }
}
