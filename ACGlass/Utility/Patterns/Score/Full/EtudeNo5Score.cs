using ACGlass.Classes;
using ACGlass.Classes.Patterns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACGlass.Utility.Patterns.Score.Full
{
    public class EtudeNo5Score : ScoreUtility
    {
        public EtudeNo5Score()
        {
            Valence = 0.375;
            Arousal = 0.525;
            Name = "EtudeNo5";
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

            int degree = BasicUtility.rander.Next(7);
            int mode = findMode(pattern.Valence, pattern.Arousal);;
            Chord chord = new Chord(degree, 0, pattern.tune);
            int turnCount = BasicUtility.rander.Next(3);
            for (int i = 0; i < turnCount; i++)
                chord.turnUp();
            List<BaseNote> hand1 = new List<BaseNote>();
            mainMode1(pattern, chord, mode, hand1, BasicUtility.throwCoin);

            List<BaseNote> hand2 = new List<BaseNote>();
            pairMode(pattern, chord, mode, hand2);

            List<BaseNote>[] score = new List<BaseNote>[] { hand1, hand2 };
            if (index != patterns.Length - 1 && pattern.BPM != patterns[index + 1].BPM)
            {
                double distance = (pattern.BPM - patterns[index + 1].BPM) / 5.0;
                hand2.Insert(hand2.Count - 4, new TempoChanger((int)(pattern.BPM - distance)));
                hand2.Insert(hand2.Count - 3, new TempoChanger((int)(pattern.BPM - distance * 2)));
                hand2.Insert(hand2.Count - 2, new TempoChanger((int)(pattern.BPM - distance * 3)));
                hand2.Insert(hand2.Count - 1, new TempoChanger((int)(pattern.BPM - distance * 4)));
            }
            hand2.Insert(0, new TempoChanger(pattern.BPM));
            if (patterns.Length - 1 == index)
            {
                score[0].Add(new Note(96, new byte[] { 
                    (byte)(ACCore.pitchFromMode(0, pattern.tune, chord.notes[0]) + pattern.registers[0]),
                    (byte)(ACCore.pitchFromMode(0, pattern.tune, chord.notes[1]) + pattern.registers[0]),
                    (byte)(ACCore.pitchFromMode(0, pattern.tune, chord.notes[2]) + pattern.registers[0]) }, pattern.loudness[0]));
                score[1].Add(new Note(96, (byte)(ACCore.pitchFromMode(0, pattern.tune, chord.notes[0]) + pattern.registers[1]), pattern.loudness[1]));
            }
            return score;
        }
        void mainMode1(Pattern pattern, Chord chord, int mode, List<BaseNote> notes, bool step)
        {
            notes.Add(new RestNote(36));
            notes.Add(new Note(60, new byte[] { 
                (byte)(ACCore.pitchFromMode(mode, pattern.tune, chord.notes[1]) + pattern.registers[0]),
                (byte)(ACCore.pitchFromMode(mode, pattern.tune, chord.notes[1]) + pattern.registers[0] + 12) }, pattern.loudness[0]));
            notes.Add(new RestNote(36));
            if (!step)
            {
                notes.Add(new Note(60, new byte[] { 
                    (byte)(ACCore.pitchFromMode(mode, pattern.tune, chord.notes[1] + 2) + pattern.registers[0]),
                    (byte)(ACCore.pitchFromMode(mode, pattern.tune, chord.notes[1] + 2) + pattern.registers[0] + 12) }, pattern.loudness[0]));
                notes.Add(new RestNote(36));
                notes.Add(new Note(60, new byte[] { 
                    (byte)(ACCore.pitchFromMode(mode, pattern.tune, chord.notes[1] + 3) + pattern.registers[0]),
                    (byte)(ACCore.pitchFromMode(mode, pattern.tune, chord.notes[1] + 3) + pattern.registers[0] + 12) }, pattern.loudness[0]));
                notes.Add(new RestNote(36));
                notes.Add(new Note(36, new byte[] { 
                    (byte)(ACCore.pitchFromMode(mode, pattern.tune, chord.notes[1] + 4) + pattern.registers[0]),
                    (byte)(ACCore.pitchFromMode(mode, pattern.tune, chord.notes[1] + 4) + pattern.registers[0] + 12) }, pattern.loudness[0]));
                notes.Add(new Note(24, new byte[] { 
                    (byte)(ACCore.pitchFromMode(mode, pattern.tune, chord.notes[1] + 5) + pattern.registers[0]),
                    (byte)(ACCore.pitchFromMode(mode, pattern.tune, chord.notes[1] + 5) + pattern.registers[0] + 12) }, pattern.loudness[0]));
            }
            else
            {
                notes.Add(new Note(36, new byte[] { 
                    (byte)(ACCore.pitchFromMode(mode, pattern.tune, chord.notes[1] + 2) + pattern.registers[0]),
                    (byte)(ACCore.pitchFromMode(mode, pattern.tune, chord.notes[1] + 2) + pattern.registers[0] + 12) }, pattern.loudness[0]));
                notes.Add(new Note(24, new byte[] { 
                    (byte)(ACCore.pitchFromMode(mode, pattern.tune, chord.notes[1] + 3) + pattern.registers[0]),
                    (byte)(ACCore.pitchFromMode(mode, pattern.tune, chord.notes[1] + 3) + pattern.registers[0] + 12) }, pattern.loudness[0]));
                notes.Add(new RestNote(36));
                notes.Add(new Note(60, new byte[] { 
                    (byte)(ACCore.pitchFromMode(mode, pattern.tune, chord.notes[1] + 4) + pattern.registers[0]),
                    (byte)(ACCore.pitchFromMode(mode, pattern.tune, chord.notes[1] + 4) + pattern.registers[0] + 12) }, pattern.loudness[0]));
                notes.Add(new RestNote(36));
                notes.Add(new Note(36, new byte[] { 
                    (byte)(ACCore.pitchFromMode(mode, pattern.tune, chord.notes[1] + 6) + pattern.registers[0]),
                    (byte)(ACCore.pitchFromMode(mode, pattern.tune, chord.notes[1] + 6) + pattern.registers[0] + 12) }, pattern.loudness[0]));
                notes.Add(new Note(24, new byte[] { 
                    (byte)(ACCore.pitchFromMode(mode, pattern.tune, chord.notes[1] + 5) + pattern.registers[0]),
                    (byte)(ACCore.pitchFromMode(mode, pattern.tune, chord.notes[1] + 5) + pattern.registers[0] + 12) }, pattern.loudness[0]));
            }
            
        }
        void mainMode2(Pattern pattern, Chord chord, int mode, List<BaseNote> notes, bool step)
        {
            if (step)
            {
                Chord section2 = new Chord(chord.degree - 3, chord.turn + 1, chord.tune);
                notes.Add(new Note(96, new byte[] { 
                    (byte)(ACCore.pitchFromMode(mode, pattern.tune, chord.notes[0]) + pattern.registers[0]),
                    (byte)(ACCore.pitchFromMode(mode, pattern.tune, chord.notes[2]) + pattern.registers[0])}, pattern.loudness[0]));
                notes.Add(new Note(72, new byte[] { 
                    (byte)(ACCore.pitchFromMode(mode, pattern.tune, section2.notes[1]) + pattern.registers[0]),
                    (byte)(ACCore.pitchFromMode(mode, pattern.tune, section2.notes[2]) + pattern.registers[0])}, pattern.loudness[0]));
                notes.Add(new Note(24, new byte[] { 
                    (byte)(ACCore.pitchFromMode(mode, pattern.tune, section2.notes[0]) + pattern.registers[0]),
                    (byte)(ACCore.pitchFromMode(mode, pattern.tune, section2.notes[1]) + pattern.registers[0])}, pattern.loudness[0]));
                notes.Add(new Note(96, new byte[] { 
                    (byte)(ACCore.pitchFromMode(mode, pattern.tune, chord.notes[0]) + pattern.registers[0]),
                    (byte)(ACCore.pitchFromMode(mode, pattern.tune, chord.notes[0] + 3) + pattern.registers[0])}, pattern.loudness[0]));
                notes.Add(new Note(96, new byte[] { 
                    (byte)(ACCore.pitchFromMode(mode, pattern.tune, chord.notes[0]) + pattern.registers[0]),
                    (byte)(ACCore.pitchFromMode(mode, pattern.tune, chord.notes[0] + 3) + pattern.registers[0])}, pattern.loudness[0]));
            }
            else
            {
                Chord section2 = new Chord(chord.degree - 1, chord.turn + 1, chord.tune);
                notes.Add(new Note(96, new byte[] { 
                    (byte)(ACCore.pitchFromMode(mode, pattern.tune, chord.notes[0]) + pattern.registers[0]),
                    (byte)(ACCore.pitchFromMode(mode, pattern.tune, chord.notes[2]) + pattern.registers[0]) }, pattern.loudness[0]));
                notes.Add(new Note(36, new byte[] { 
                    (byte)(ACCore.pitchFromMode(mode, pattern.tune, section2.notes[0]) + pattern.registers[0]),
                    (byte)(ACCore.pitchFromMode(mode, pattern.tune, section2.notes[1]) + pattern.registers[0]) }, pattern.loudness[0]));
                notes.Add(new Note(36, new byte[] { 
                    (byte)(ACCore.pitchFromMode(mode, pattern.tune, section2.notes[0]) + pattern.registers[0]),
                    (byte)(ACCore.pitchFromMode(mode, pattern.tune, section2.notes[1]) + pattern.registers[0]),
                    (byte)(ACCore.pitchFromMode(mode, pattern.tune, section2.notes[2]) + pattern.registers[0]) }, pattern.loudness[0]));
                notes.Add(new Note(24, new byte[] { 
                    (byte)(ACCore.pitchFromMode(mode, pattern.tune, section2.notes[2] - 7) + pattern.registers[0]),
                    (byte)(ACCore.pitchFromMode(mode, pattern.tune, section2.notes[0]) + pattern.registers[0])}, pattern.loudness[0]));
                notes.Add(new Note(96, new byte[] { 
                    (byte)(ACCore.pitchFromMode(mode, pattern.tune, chord.notes[1]) + pattern.registers[0]),
                    (byte)(ACCore.pitchFromMode(mode, pattern.tune, chord.notes[2]) + pattern.registers[0]) }, pattern.loudness[0]));
                notes.Add(new Note(96, (byte)(ACCore.pitchFromMode(mode, pattern.tune, chord.notes[0]) + pattern.registers[0]), pattern.loudness[0]));

            }
        }

        void pairMode(Pattern pattern, Chord chord, int mode, List<BaseNote> notes)
        {
            byte[][] headNote = new byte[4][];
            headNote[0] = new byte[] { 
                (byte)(ACCore.pitchFromMode(mode, pattern.tune, chord.notes[0]) + pattern.registers[1]), 
                (byte)(ACCore.pitchFromMode(mode, pattern.tune, chord.notes[1]) + pattern.registers[1]) };
            if (Math.Abs(chord.notes[0] - chord.notes[1]) > 2)
                headNote[1] = new byte[] { 
                    (byte)(ACCore.pitchFromMode(mode, pattern.tune, chord.notes[0] + 1) + pattern.registers[1]), 
                    headNote[0][1] };
            else
                headNote[1] = new byte[] { 
                    (byte)(ACCore.pitchFromMode(mode, pattern.tune, chord.notes[0] - 1) + pattern.registers[1]), 
                    headNote[0][1] };
            headNote[2] = new byte[] { headNote[0][0], headNote[0][1] };
            headNote[3] = new byte[] { (byte)(headNote[0][0] - 1), (byte)(headNote[0][1] - 1) };

            for (int i = 0; i < 4; i++)
            {
                notes.Add(new Note(12, headNote[i], pattern.loudness[1]));
                notes.Add(new Note(12, (byte)(ACCore.pitchFromMode(mode, pattern.tune, chord.notes[2]) + pattern.registers[1]), pattern.loudness[1]));
                notes.Add(new Note(12, headNote[i][1], pattern.loudness[1]));
                notes.Add(new Note(60, (byte)(ACCore.pitchFromMode(mode, pattern.tune, chord.notes[2]) + pattern.registers[1]), pattern.loudness[1]));
            }
        }

        public override int minimumDuring
        {
            get { return 12; }
        }
    }
}
