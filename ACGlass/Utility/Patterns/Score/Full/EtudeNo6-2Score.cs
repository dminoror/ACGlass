using ACGlass.Classes;
using ACGlass.Classes.Patterns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACGlass.Utility.Patterns.Score.Full
{
    public class EtudeNo6_2Score : ScoreUtility
    {
        public EtudeNo6_2Score()
        {
            Valence = 0.375;
            Arousal = 0.525;
            Name = "EtudeNo6-2";
            selfId = 13;
        }
        public override Pattern generatePattern(double V, double A, double[] distance)
        {
            int tune = 0;
            int[] registers = new int[1];
            registers[0] = BasicUtility.rander.Next(5, 6) * 12;
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

            int degree = BasicUtility.rander.Next(7);
            TSDChordPattern tsd1 = new TSDChordPattern(pattern.Valence, pattern.Arousal);
            TSDChordPattern tsd2 = new TSDChordPattern(pattern.Valence, pattern.Arousal);
            Chord[] chords = TSDChord.generate(pattern.tune, tsd1);
            Chord[] heads = TSDChord.generate(pattern.tune, tsd2);
            List<BaseNote> hand1 = new List<BaseNote>();
            List<BaseNote> hand2 = new List<BaseNote>();
            for (int i = 0; i < 4; i++)
            {
                Chord chord = chords[i];
                Chord head = heads[i];
                while (head.notes[0] < chord.notes[2])
                    head.turnUp();
                hand1.Add(new Note(12, new byte[] { 
                    (byte)(ACCore.pitchFromMajorDegree(pattern.tune, head.notes[0]) + pattern.registers[0]),
                    (byte)(ACCore.pitchFromMajorDegree(pattern.tune, head.notes[0] + 7) + pattern.registers[0]) }, pattern.loudness[0]));
                for (int j = 0; j < 2; j++)
                {
                    for (int m = 0; m < 2; m++)
                    {
                        hand1.Add(new Note(6, new byte[] { 
                            (byte)(ACCore.pitchFromMajorDegree(pattern.tune, chord.notes[0]) + pattern.registers[0]),
                            (byte)(ACCore.pitchFromMajorDegree(pattern.tune, chord.notes[1]) + pattern.registers[0]),
                            (byte)(ACCore.pitchFromMajorDegree(pattern.tune, chord.notes[2]) + pattern.registers[0]) }, pattern.loudness[0]));
                    }
                    for (int m = 0; m < 2; m++)
                    {
                        hand1.Add(new Note(6, new byte[] { 
                            (byte)(ACCore.pitchFromMajorDegree(pattern.tune, chord.notes[0] + 1) + pattern.registers[0]),
                            (byte)(ACCore.pitchFromMajorDegree(pattern.tune, chord.notes[1]) + pattern.registers[0]),
                            (byte)(ACCore.pitchFromMajorDegree(pattern.tune, chord.notes[2]) + pattern.registers[0]) }, pattern.loudness[0]));
                    }
                }
                hand1.Add(new Note(12, new byte[] { 
                    (byte)(ACCore.pitchFromMajorDegree(pattern.tune, chord.notes[0]) + pattern.registers[0]),
                    (byte)(ACCore.pitchFromMajorDegree(pattern.tune, chord.notes[1]) + pattern.registers[0]),
                    (byte)(ACCore.pitchFromMajorDegree(pattern.tune, chord.notes[2]) + pattern.registers[0]) }, pattern.loudness[0]));
            }
            for (int i = 0; i < 4; i++)
            {
                int begin = i * 10;
                hand2.Add(new Note(12, new byte[] { 
                    (byte)(((Note)hand1[begin]).pitch[0] - 36), 
                    (byte)(((Note)hand1[begin]).pitch[1] - 36) }, pattern.loudness[1]));
                begin++;
                for (int j = 0; j < 9; j++)
                {
                    hand2.Add(new Note(hand1[begin + j].during, new byte[] { 
                        (byte)(((Note)hand1[begin + j]).pitch[0] - 12), 
                        (byte)(((Note)hand1[begin + j]).pitch[1] - 12), 
                        (byte)(((Note)hand1[begin + j]).pitch[2] - 12) }, pattern.loudness[1]));
                }
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
            return score;
        }
        
    }
}
