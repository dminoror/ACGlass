using ACGlass.Classes;
using ACGlass.Classes.Patterns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACGlass.Utility.Patterns.Score.Full
{
    public class EtudeNo8Score : ScoreUtility
    {
        public EtudeNo8Score()
        {
            Valence = 0.375;
            Arousal = 0.525;
            Name = "EtudeNo8";
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
            int[] candDegree = new int[] { 1, 3, 4, 5, 6 };
            int degreeIndex = BasicUtility.rander.Next(candDegree.Length);
            int degree = candDegree[degreeIndex];
            List<BaseNote> hand1 = new List<BaseNote>();
            hand1.Add(new Note(72, new byte[] { 
                (byte)(ACCore.pitchFromMode(0, pattern.tune, degree) + pattern.registers[0]),
                (byte)(ACCore.pitchFromMode(0, pattern.tune, degree + 2) + pattern.registers[0]),
                (byte)(ACCore.pitchFromMode(0, pattern.tune, degree + 4) + pattern.registers[0]) }, pattern.loudness[0]));
            hand1.Add(new Note(24, new byte[] { 
                (byte)(ACCore.pitchFromMode(0, pattern.tune, degree) + pattern.registers[0]),
                (byte)(ACCore.pitchFromMode(0, pattern.tune, degree + 2) + pattern.registers[0]),
                (byte)(ACCore.pitchFromMode(0, pattern.tune, degree + 4) + pattern.registers[0]) }, pattern.loudness[0]));
            hand1.Add(new Note(48, new byte[] { 
                (byte)(ACCore.pitchFromMode(0, pattern.tune, degree) + pattern.registers[0]),
                (byte)(ACCore.pitchFromMode(0, pattern.tune, degree + 2) + pattern.registers[0]),
                (byte)(ACCore.pitchFromMode(0, pattern.tune, degree + 5) + pattern.registers[0]) }, pattern.loudness[0]));
            hand1.Add(new Note(24, new byte[] { 
                (byte)(ACCore.pitchFromMode(0, pattern.tune, degree) + pattern.registers[0]),
                (byte)(ACCore.pitchFromMode(0, pattern.tune, degree + 2) + pattern.registers[0]),
                (byte)(ACCore.pitchFromMode(0, pattern.tune, degree + 6) + pattern.registers[0]) }, pattern.loudness[0]));
            hand1.Add(new Note(24, new byte[] { 
                (byte)(ACCore.pitchFromMode(0, pattern.tune, degree) + pattern.registers[0]),
                (byte)(ACCore.pitchFromMode(0, pattern.tune, degree + 2) + pattern.registers[0]),
                (byte)(ACCore.pitchFromMode(0, pattern.tune, degree + 5) + pattern.registers[0]) }, pattern.loudness[0]));
            Chord chordClosing1 = findClosingDegree1(degree, pattern);
            hand1.Add(new Note(96, new byte[] { 
                (byte)(ACCore.pitchFromMode(0, pattern.tune, chordClosing1.notes[0]) + pattern.registers[0]),
                (byte)(ACCore.pitchFromMode(0, pattern.tune, chordClosing1.notes[1]) + pattern.registers[0]),
                (byte)(ACCore.pitchFromMode(0, pattern.tune, chordClosing1.notes[2]) + pattern.registers[0]) }, pattern.loudness[0]));
            Chord chordClosing3 = findClosingDegree3(chordClosing1, pattern);
            hand1.Add(new Note(96, new byte[] { 
                (byte)(ACCore.pitchFromMode(1, pattern.tune, chordClosing3.notes[0]) + pattern.registers[0]),
                (byte)(ACCore.pitchFromMode(1, pattern.tune, chordClosing3.notes[1]) + pattern.registers[0]),
                (byte)(ACCore.pitchFromMode(1, pattern.tune, chordClosing3.notes[2]) + pattern.registers[0]) }, pattern.loudness[0]));
            List<BaseNote> hand2 = new List<BaseNote>();
            hand2.Add(new Note(12, new byte[] { 
                (byte)(ACCore.pitchFromMode(0, pattern.tune, degree) + pattern.registers[1]),
                (byte)(ACCore.pitchFromMode(0, pattern.tune, degree + 2) + pattern.registers[1]) }, pattern.loudness[1]));
            hand2.Add(new Note(12, (byte)(ACCore.pitchFromMode(0, pattern.tune, degree + 4) + pattern.registers[1]), pattern.loudness[1]));
            for (int i = 0; i < 3; i++)
            {
                hand2.Add(new Note(12, (byte)(ACCore.pitchFromMode(0, pattern.tune, degree + 2) + pattern.registers[1]), pattern.loudness[1]));
                hand2.Add(new Note(12, (byte)(ACCore.pitchFromMode(0, pattern.tune, degree + 4) + pattern.registers[1]), pattern.loudness[1]));
            }
            Chord chordSec2 = new Chord(degree - 2, 0, pattern.tune);
            hand2.Add(new Note(12, new byte[] { 
                (byte)(ACCore.pitchFromMode(0, pattern.tune, chordSec2.notes[0]) + pattern.registers[1]),
                (byte)(ACCore.pitchFromMode(0, pattern.tune, chordSec2.notes[2]) + pattern.registers[1]) }, pattern.loudness[1]));
            hand2.Add(new Note(12, (byte)(ACCore.pitchFromMode(0, pattern.tune, chordSec2.notes[0] + 7) + pattern.registers[1]), pattern.loudness[1]));
            for (int i = 0; i < 3; i++)
            {
                hand2.Add(new Note(12, (byte)(ACCore.pitchFromMode(0, pattern.tune, chordSec2.notes[2]) + pattern.registers[1]), pattern.loudness[1]));
                hand2.Add(new Note(12, (byte)(ACCore.pitchFromMode(0, pattern.tune, chordSec2.notes[0] + 7) + pattern.registers[1]), pattern.loudness[1]));
            }
            hand2.Add(new Note(12, new byte[] { 
                (byte)(ACCore.pitchFromMode(0, pattern.tune, chordClosing1.notes[0]) + pattern.registers[1]),
                (byte)(ACCore.pitchFromMode(0, pattern.tune, chordClosing1.notes[2]) + pattern.registers[1]) }, pattern.loudness[1]));
            hand2.Add(new Note(12, (byte)(ACCore.pitchFromMode(0, pattern.tune, chordClosing1.notes[0] + 7) + pattern.registers[1]), pattern.loudness[1]));
            for (int i = 0; i < 3; i++)
            {
                hand2.Add(new Note(12, (byte)(ACCore.pitchFromMode(0, pattern.tune, chordClosing1.notes[2]) + pattern.registers[1]), pattern.loudness[1]));
                hand2.Add(new Note(12, (byte)(ACCore.pitchFromMode(0, pattern.tune, chordClosing1.notes[0] + 7) + pattern.registers[1]), pattern.loudness[1]));
            }
            hand2.Add(new Note(12, new byte[] { 
                (byte)(ACCore.pitchFromMode(1, pattern.tune, chordClosing3.notes[0]) + pattern.registers[1]),
                (byte)(ACCore.pitchFromMode(1, pattern.tune, chordClosing3.notes[2]) + pattern.registers[1]) }, pattern.loudness[1]));
            hand2.Add(new Note(12, (byte)(ACCore.pitchFromMode(1, pattern.tune, chordClosing3.notes[0] + 7) + pattern.registers[1]), pattern.loudness[1]));
            for (int i = 0; i < 3; i++)
            {
                hand2.Add(new Note(12, (byte)(ACCore.pitchFromMode(1, pattern.tune, chordClosing3.notes[2]) + pattern.registers[1]), pattern.loudness[1]));
                hand2.Add(new Note(12, (byte)(ACCore.pitchFromMode(1, pattern.tune, chordClosing3.notes[0] + 7) + pattern.registers[1]), pattern.loudness[1]));
            }

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
                score[0].Add(new Note(96, new byte[] { 
                    (byte)(ACCore.pitchFromMode(0, pattern.tune, degree) + pattern.registers[0]),
                    (byte)(ACCore.pitchFromMode(0, pattern.tune, degree + 2) + pattern.registers[0]),
                    (byte)(ACCore.pitchFromMode(0, pattern.tune, degree + 4) + pattern.registers[0]) }, pattern.loudness[0]));
                score[1].Add(new Note(96, (byte)(ACCore.pitchFromMode(0, pattern.tune, degree) + pattern.registers[1]), pattern.loudness[1]));
            }
            return score;
        }
        public override int minimumDuring
        {
            get
            {
                return 12;
            }
        }
        Chord findClosingDegree1(int degree, Pattern pattern)
        {
            Chord chord = new Chord(0, 0, pattern.tune);
            int tarValue = (degree + 2) * 3;
            int distance = Math.Abs(tarValue - 6);
            while (true)
            {
                if (chord.notes[0] < degree)
                    chord.turnUp();
                int currValue = chord.notes[0] + chord.notes[1] + chord.notes[2];
                int currDis = Math.Abs(currValue - tarValue);
                if (currDis > distance)
                {
                    chord.turnDown();
                    break;
                }
                if (currDis == distance)
                {
                    break;
                }
                else
                {
                    distance = currDis;
                }
            }
            return chord;
        }
        Chord findClosingDegree3(Chord target, Pattern pattern)
        {
            Chord chord = new Chord(4, 0, pattern.tune);
            int tune = pattern.tune;
            int tarValue = ACCore.pitchFromMode(0, tune, target.notes[0]) + ACCore.pitchFromMode(0, tune, target.notes[1]) + ACCore.pitchFromMode(0, tune, target.notes[2]);
            int distance = Math.Abs(tarValue - (ACCore.pitchFromMode(1, tune, chord.notes[0]) + ACCore.pitchFromMode(1, tune, chord.notes[1]) + ACCore.pitchFromMode(1, tune, chord.notes[2])));
            while (true)
            {
                if (ACCore.pitchFromMode(1, tune, chord.notes[0]) < ACCore.pitchFromMode(0, tune, target.notes[0]))
                {
                    chord.turnUp();
                    int currValue = ACCore.pitchFromMode(1, tune, chord.notes[0]) + ACCore.pitchFromMode(1, tune, chord.notes[1]) + ACCore.pitchFromMode(1, tune, chord.notes[2]);
                    int currDis = Math.Abs(tarValue - currValue);
                    if (currDis > distance)
                    {
                        chord.turnDown();
                        break;
                    }
                    else if (currDis == distance)
                        break;
                    else
                        distance = currDis;
                }
                else
                {
                    chord.turnDown();
                    int currValue = ACCore.pitchFromMode(1, tune, chord.notes[0]) + ACCore.pitchFromMode(1, tune, chord.notes[1]) + ACCore.pitchFromMode(1, tune, chord.notes[2]);
                    int currDis = Math.Abs(tarValue - currValue);
                    if (currDis > distance)
                    {
                        chord.turnUp();
                        break;
                    }
                    if (currDis == distance)
                        break;
                    else
                        distance = currDis;
                }
            }
            return chord;
        }
    }
}
