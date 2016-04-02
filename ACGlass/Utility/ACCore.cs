using ACGlass.Classes;
using ACGlass.Classes.Patterns;
using Sanford.Multimedia.Midi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACGlass.Utility
{
    public static class ACCore
    {
        static int[][] stepMajor = null; //{ 0, 2, 4, 5, 7, 9, 11 };
        //static int[][] stepMinor = null;
        static int[][] stepMinor = null;
        static int[][][] stepMode;

        static ACCore()
        {
            stepMajor = new int[12][];
            int[] step = new int[7] { 0, 2, 4, 5, 7, 9, 11 };
            for (int i = 0; i < 12; i++)
                stepMajor[i] = new int[7] { step[0] + i, step[1] + i, step[2] + i, step[3] + i, step[4] + i, step[5] + i, step[6] + i };
            stepMinor = new int[12][];
            step = new int[7] { -3, -1, 0, 2, 4, 5, 8 };
            for (int i = 0; i < 12; i++)
                stepMinor[i] = new int[7] { step[0] + i, step[1] + i, step[2] + i, step[3] + i, step[4] + i, step[5] + i, step[6] + i };
            stepMode = new int[2][][] { stepMajor, stepMinor };
        }

        public static Pattern generateScore(double V, double A, ScoreUtility PU)
        {
            if (PU == null)
            {
                Pattern pattern = PatternSelector.select(V, A);
                return pattern;
            }
            else
            {
                Pattern pattern = PatternSelector.selectByPU(V, A, PU);
                return pattern;
            }
        }
        public static void executeScore(Pattern[] patterns, Score score)
        {
            for (int index = 0; index < patterns.Length; )
                patterns[index].patterns[0].handleFollow(patterns, ref index);

            List<BaseNote>[] mainScore = new List<BaseNote>[2] { new List<BaseNote>(), new List<BaseNote>() };
            for (int i = 0; i < patterns.Length - 1; i++)
            {
                Pattern pattern = patterns[i];
                List<BaseNote>[] sectionScore = pattern.patterns[0].generateScore(patterns, i);
                mainScore[0].AddRange(sectionScore[0]);
                mainScore[1].AddRange(sectionScore[1]);
            }
            List<BaseNote>[] scores = patterns[patterns.Length - 1].patterns[0].generateScore(patterns, patterns.Length - 1);
            mainScore[0].AddRange(scores[0]);
            mainScore[1].AddRange(scores[1]);

            writeNotesToScore(score, mainScore);
        }


        public static void writeNotesToScore(Score score, IEnumerable<BaseNote>[] noteTracks)
        {
            for (int i = 0; i < noteTracks.Length; i++)
            {
                score.tick = 0;
                Track track = score.sequence[i + 1];
                foreach (BaseNote note in noteTracks[i])
                    note.AddNote(track, score);
            }
            /*
            Track track1 = score.sequence[1];
            Track track2 = score.sequence[2];
            score.AddTempo(BPM);
            int tick = 0;
            foreach (BaseNote note in noteTracks[0])
                note.AddNote(track1, ref tick);
            tick = 0;
            foreach (BaseNote note in noteTracks[1])
                note.AddNote(track2, ref tick);
            score.tick = tick;*/
        }
        
        public static int[] chordTurner(int buttom, int turn)
        {
            switch (turn)
            {
                case 0:
                    return new int[] { 0 + buttom, 2 + buttom, 4 + buttom };
                case 1:
                    return new int[] { 2 + buttom, 4 + buttom, 7 + buttom };
                case 2:
                    return new int[] { 4 + buttom, 7 + buttom, 9 + buttom };
                case 3:
                    return new int[] { 7 + buttom, 9 + buttom, 11 + buttom };
                case -1:
                    return new int[] { -3 + buttom, 0 + buttom, 2 + buttom };
                case -2:
                    return new int[] { -5 + buttom, -3 + buttom, 0 + buttom };
                case -3:
                    return new int[] { -7 + buttom, -5 + buttom, -3 + buttom };
            }
            return null;
        }
        public static Chord findChordByClose(Chord prevChord, int currentChord, bool fUp, int fClose)
        {
            Chord chord = new Chord(currentChord, 0, prevChord.tune);
            if (fClose == 0)
            {
                if (chord.notes[0] > prevChord.notes[0])
                {
                    int min = Math.Abs(chord.notes[0] - prevChord.notes[0]);
                    while (true)
                    {
                        chord.turnDown();
                        int distance = Math.Abs(chord.notes[0] - prevChord.notes[0]);
                        if (distance > min)
                        {
                            chord.turnUp();
                            break;
                        }
                        min = distance;
                    }
                }
                else if (chord.notes[0] < prevChord.notes[0])
                {
                    int min = Math.Abs(chord.notes[0] - prevChord.notes[0]);
                    while (true)
                    {
                        chord.turnUp();
                        int distance = Math.Abs(chord.notes[0] - prevChord.notes[0]);
                        if (distance > min)
                        {
                            chord.turnDown();
                            break;
                        }
                        min = distance;
                    }
                }
            }
            else if (fClose == 1)
            {
                if (chord.notes[0] == prevChord.notes[0])
                {
                    if (fUp)
                        chord.turnUp();
                    else
                        chord.turnDown();
                }
                else if (chord.notes[0] > prevChord.notes[0])
                {
                    if (fUp)
                    {
                        while (chord.notes[0] > prevChord.notes[0])
                            chord.turnDown();
                        chord.turnUp();
                    }
                    else
                        while (chord.notes[0] >= prevChord.notes[0])
                            chord.turnDown();
                }
                else
                {
                    if (!fUp)
                    {
                        while (chord.notes[0] < prevChord.notes[0])
                            chord.turnUp();
                        chord.turnDown();
                    }
                    else
                        while (chord.notes[0] <= prevChord.notes[0])
                            chord.turnUp();
                }
            }
            else
            {
                if (fUp)
                {
                    if (chord.notes[0] == prevChord.notes[2])
                        chord.turnUp();
                    else if (chord.notes[0] < prevChord.notes[2])
                    {
                        while (chord.notes[0] < prevChord.notes[2])
                            chord.turnUp();
                        chord.turnDown();
                    }
                    else
                    {
                        while (chord.notes[0] > prevChord.notes[2])
                            chord.turnDown();
                    }
                }
                else
                {
                    if (chord.notes[0] == prevChord.notes[2])
                        chord.turnDown();
                    else if (chord.notes[2] > prevChord.notes[0])
                    {
                        while (chord.notes[2] > prevChord.notes[0])
                            chord.turnDown();
                        chord.turnUp();
                    }
                    else
                    {
                        while (chord.notes[2] < prevChord.notes[0])
                            chord.turnUp();
                    }
                }
            }
            return chord;
        }

        
        

        public static int[] pitchFromDegree(int degree)
        {
            if (degree < 0)
            {
                int reg = (-degree / 7) + 1;
                int index = 7 + (degree % 7);
                if (index == 7)
                {
                    index = 0;
                    reg -= 1;
                }
                return new int[] { index, -reg };
            }
            else
                return new int[] { degree % 7, degree / 7 };
        }
        public static int pitchFromMode(int mode, int tune, int degree)
        {
            int[] pitchData = pitchFromDegree(degree);
            return stepMode[mode][tune][pitchData[0]] + pitchData[1] * 12;
        }
        /*
        public static int pitchFromMajor(int tune, int degree)
        {
            int[] pitchData = pitchFromDegree(degree);
            return Major[tune][pitchData[0]] + pitchData[1] * 12;
        }
        public static int pitchFromMinorHarmony(int tune, int degree)
        {
            int[] pitchData = pitchFromDegree(degree);
            return Minor[tune][pitchData[0]] + pitchData[1] * 12;
        }*/
    }
}
