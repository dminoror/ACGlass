using ACGlass.Classes.Patterns;
using ACGlass.Classes.Patterns.Chords;
using ACGlass.Classes.Patterns.Notes;
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
        protected int[] pair;
        protected int selfId;
        public virtual Pattern generatePattern(double V, double A, double[] distance) 
        {
            double picker = 0;
            List<int> cand = new List<int>();
            for (int i = 0; i < pair.Length; i++)
            {
                if (distance[pair[i]] > picker)
                {
                    picker = distance[pair[i]];
                }
            }
            picker = BasicUtility.rander.NextDouble() * picker;
            for (int i = 0; i < pair.Length; i++)
                if (distance[pair[i]] >= picker)
                    cand.Add(pair[i]);
            int indexPT = BasicUtility.rander.Next(cand.Count);
            ScoreUtility ptPair = PatternSelector.mainPatterns[cand[indexPT]];

            int tune = 0;
            TSDChordPattern tsd = new TSDChordPattern(V, A);
            Chord[] chords1 = TSDChord.generate(tune, tsd);
            Chord[] chords2 = TSDChord.generate(tune, tsd);
            int[] registers = BasicUtility.Cfinder(new int[] { 4, 5, 6 }, 2);
            registers[0] *= 12;
            registers[1] *= 12;
            byte loudness1 = 127;
            byte loudness2 = (byte)(loudness1 * 0.8);
            return null;
        }
        public virtual List<BaseNote>[] generateScore(Pattern[] patterns, int index)
        {
            Pattern pattern = patterns[index];
            List<BaseNote>[] score = new List<BaseNote>[] { 
                pattern.patterns[0].generateNotes(pattern.Valence, pattern.Arousal, pattern.chords[0], pattern.registers[0], pattern.loudness[0], pattern.orders[0]), 
                pattern.patterns[1].generateNotes(pattern.Valence, pattern.Arousal, pattern.chords[1], pattern.registers[1], pattern.loudness[1], pattern.orders[1]) };
            List<BaseNote> hand1 = score[0];
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
        public virtual List<BaseNote> generateNotes(double V, double A, Chord[] chords, int register, byte loudness, bool? setOrder) 
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
    }
    
    
    
    
    
    
    
    
}
