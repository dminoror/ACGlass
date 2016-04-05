using ACGlass.Classes.Patterns;
using ACGlass.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACGlass.Classes
{
    public abstract class ScoreUtility
    {
        public double Valence;
        public double Arousal;
        public string Name;
        protected ScoreUtility[] pair;
        //protected int selfId;
        public virtual void getPairs() { }
        public virtual Pattern generatePattern(double V, double A) { return null; }
        public virtual List<BaseNote>[] generateScore(Pattern[] patterns, int index)
        {
            Pattern pattern = patterns[index];
            TSDChordPattern tsd = new TSDChordPattern(patterns[index].Valence, patterns[index].Arousal);
            Chord[] chords1 = TSDChord.generate(patterns[index].tune, tsd);
            Chord[] chords2 = TSDChord.generate(patterns[index].tune, tsd);
            List<BaseNote>[] score = new List<BaseNote>[] { 
                pattern.patterns[0].generateNotes(pattern, chords1, pattern.registers[0], pattern.loudness[0], pattern.orders[0]), 
                pattern.patterns[1].generateNotes(pattern, chords2, pattern.registers[1], pattern.loudness[1], pattern.orders[1]) };
            List<BaseNote> hand1 = score[0];
            if (index != patterns.Length - 1 && pattern.BPM != patterns[index + 1].BPM)
            {
                double distance = (pattern.BPM - patterns[index + 1].BPM) / 5.0;
                int beatLen = hand1.Count / 16;
                for (int i = 4; i > 0; i--)
                    hand1.Insert(hand1.Count - beatLen * i, new TempoChanger((int)(pattern.BPM - distance * (5 - i))));
            }
            hand1.Insert(0, new TempoChanger(pattern.BPM));
            if (patterns.Length - 1 == index)
            {
                score[0].Add(new Note(96, (byte)(ACCore.pitchFromMode(pattern.mode, pattern.tune, chords1[0].notes[0]) + pattern.registers[0]), pattern.loudness[0]));
                score[1].Add(new Note(96, (byte)(ACCore.pitchFromMode(pattern.mode, pattern.tune, chords2[0].notes[0]) + pattern.registers[1]), pattern.loudness[1]));
            }
            return score;
        }
        public virtual List<BaseNote> generateNotes(Pattern pattern, Chord[] chords, int register, byte loudness, bool? setOrder) 
        {
            return null;
        }
        public virtual void handleFollow(Pattern[] patterns, ref int index) 
        {
            if (index > 0)
            {
                Pattern prevPT = patterns[index - 1];
                Pattern currPT = patterns[index];
                int prevReg = 0, currReg = 0;
                foreach (int reg in prevPT.registers)
                    prevReg += reg;
                prevReg /= prevPT.registers.Length;
                foreach (int reg in currPT.registers)
                    currReg += reg;
                currReg /= currPT.registers.Length;
                if (prevReg > currReg + 12)
                    currReg += 12;
                else if (currReg > prevReg + 12)
                    currReg -= 12;
            }
            index++;
        }
        public virtual int minimumDuring { get { return 8; } }
        public static int BPMFromDurings(double V, double A, int during1, int during2)
        {
            return BPMFromDuring(V, A, (during1 > during2 ? during1 : during2));
        }
        public static int BPMFromDuring(double V, double A, int during)
        {
            int speed = (int)((15 * (A + (BasicUtility.rander.NextDouble() / 10 - 0.05))) + 5);
            int BPM = speed * during;
            return BPM;
        }
        public int findMode(double V, double A)
        {
            int selectV = BasicUtility.powerSelect(new double[][] { new double[] { 1, 0.5 }, new double[] { 0, 0.5 } }, new double[] { V, 0.5 });
            int selectA = BasicUtility.powerSelect(new double[][] { new double[] { 0.5, 1 }, new double[] { 0.5, 0 } }, new double[] { 0.5, A });
            if (selectV == 1 && selectA == 1)
                return 1;
            else
                return 0;
        }
    }
    
    
    
    
    
    
    
    
}
