using ACGlass.Classes;
using ACGlass.Classes.Patterns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACGlass.Utility.Patterns.Score.Full
{
    public class GlassworksScore : ScoreUtility// 5
    {
        public GlassworksScore()
        {
            Valence = 0.525;
            Arousal = 0.3;
            Name = "Glassworks";
            selfId = 5;
        }
        public override Pattern generatePattern(double V, double A, double[] distance)
        {
            int tune = 0;
            int register1 = BasicUtility.Cfinder(new int[] { 6, 7 }, 1)[0] * 12;
            int register2 = register1 - 12;
            byte loudness = 127;
            int BPM = ScoreUtility.BPMFromDuring(V, A, minimumDuring);

            Pattern pattern = new Pattern()
            {
                Valence = V,
                Arousal = A,
                patterns = new ScoreUtility[] { this },
                registers = new int[] { register1, register2 },
                loudness = new byte[] { loudness },
                tune = tune,
                BPM = BPM
            };
            return pattern;
        }
        public override List<BaseNote>[] generateScore(Pattern[] patterns, int index)
        {
            Pattern pattern = patterns[index];
            bool isEnd = true;
            Chord[] chords = generateChords(pattern.tune, isEnd);
            int stable = BasicUtility.rander.Next(4);
            List<BaseNote> hand1 = generateNotes(chords, pattern.registers[0], pattern.loudness[0], stable);
            int turnCount = BasicUtility.rander.Next(3);
            foreach (Chord chord in chords)
                for (int i = 0; i < turnCount; i++)
                    chord.turnDown();
            List<BaseNote> hand2 = PatternSelector.ptHeadTwo.generateNotes(pattern.Valence, pattern.Arousal, chords, pattern.registers[1], pattern.loudness[0], null);
            List<BaseNote>[] score = new List<BaseNote>[] { hand1, hand2 };
            if (index != patterns.Length - 1 && pattern.BPM != patterns[index + 1].BPM)
            {
                double distance = (pattern.BPM - patterns[index + 1].BPM) / 5.0;
                int beatLen = hand2.Count / 16;
                for (int i = 4; i > 0; i--)
                    hand2.Insert(hand2.Count - beatLen * i, new TempoChanger((int)(pattern.BPM - distance * (5 - i))));
            }
            hand2.Insert(0, new TempoChanger(pattern.BPM));
            return score;
        }

        public List<BaseNote> generateNotes(Chord[] chords, int register, byte loudness, int stable)
        {
            int[] noteIndex1 = null, noteIndex2 = null;
            int index1 = BasicUtility.rander.Next(2);
            int index2 = 1 - index1;
            index1++; index2++;
            switch (stable)
            {
                case 0:
                    {
                        noteIndex1 = new int[4] { index1, index1, index1, index1 };
                        noteIndex2 = new int[4] { index2, index2, index2, index2 };
                    } break;
                case 1:
                    {
                        noteIndex1 = new int[4] { index1, index2, index1, index1 };
                        noteIndex2 = new int[4] { index2, index1, index2, index2 };
                    } break;
                case 2:
                    {
                        noteIndex1 = new int[4] { index1, index2, index2, index1 };
                        noteIndex2 = new int[4] { index2, index1, index1, index2 };
                    } break;
                case 3:
                    {
                        noteIndex1 = new int[4] { BasicUtility.rander.Next(1, 3), BasicUtility.rander.Next(1, 3), BasicUtility.rander.Next(1, 3), BasicUtility.rander.Next(1, 3) };
                        noteIndex2 = new int[4] { 3 - noteIndex1[0], 3 - noteIndex1[1], 3 - noteIndex1[2], 3 - noteIndex1[3] };
                    } break;
            }
            List<BaseNote> score = new List<BaseNote>();
            for (int section = 0; section < 4; section++)
            {
                Chord chord = chords[section];
                for (int i = 0; i < 6; i++)
                {
                    score.Add(new Note(8, (byte)(ACCore.pitchFromMajorDegree(chord.tune, chord.notes[noteIndex1[section]]) + register), loudness));
                    score.Add(new Note(8, (byte)(ACCore.pitchFromMajorDegree(chord.tune, chord.notes[noteIndex2[section]]) + register), loudness));
                }
            }
            return score;
        }

        public override int minimumDuring
        {
            get { return 12; }
        }

        public Chord[] generateChords(int tune, bool isEnd)
        {
            int[] degrees = new int[4];
            Chord[] chordSelected = null;
            if (!isEnd)
            {
                degrees[0] = 0;
                degrees[1] = degreeFromGlassworks(degrees[0]);
                Chord[] pattern1 = chordsFromGlassworks(tune, degrees[0], degrees[1]);
                List<int[]> listDegrees = degreesUseableFromGlassworks(pattern1, 1);
                int same = listDegrees[0][0] + listDegrees[0][1] * 7;
                int selectIndex = BasicUtility.rander.Next(3);
                int[] selected3 = listDegrees[selectIndex]; listDegrees.RemoveAt(selectIndex);
                int[] selected4 = listDegrees[BasicUtility.rander.Next(listDegrees.Count)];
                if (BasicUtility.throwCoin)
                    BasicUtility.Swap(ref selected3, ref selected4);
                degrees[2] = selected3[0];
                degrees[3] = selected4[0];
                Chord[] pattern2 = new Chord[2] { new Chord(degrees[2], 0, tune), new Chord(degrees[3], 0, tune) };
                pattern2[0].turnOctave(selected3[1]);
                pattern2[1].turnOctave(selected4[1]);
                while (pattern2[0].notes[2] != same)
                    pattern2[0].turnDown();
                while (pattern2[1].notes[2] != same)
                    pattern2[1].turnDown();
                chordSelected = new Chord[4] { pattern1[0], pattern1[1], pattern2[0], pattern2[1] };
            }
            else
            {
                degrees[3] = 0;
                degrees[2] = degreeFromGlassworks(degrees[3]);
                Chord[] pattern2 = chordsFromGlassworks(tune, degrees[2], degrees[3]);
                List<int[]> listDegrees = degreesUseableFromGlassworks(pattern2, 0);
                int same = listDegrees[0][0] + listDegrees[0][1] * 7;
                int selectIndex = BasicUtility.rander.Next(3);
                int[] selected1 = listDegrees[selectIndex]; listDegrees.RemoveAt(selectIndex);
                int[] selected2 = listDegrees[BasicUtility.rander.Next(listDegrees.Count)];
                if (BasicUtility.throwCoin)
                    BasicUtility.Swap(ref selected1, ref selected2);
                degrees[0] = selected1[0];
                degrees[1] = selected2[0];
                Chord[] pattern1 = new Chord[] { new Chord(degrees[0], 0, tune), new Chord(degrees[1], 0, tune) };
                pattern1[0].turnOctave(selected1[1]);
                pattern1[1].turnOctave(selected2[1]);
                while (pattern1[0].notes[2] != same)
                    pattern1[0].turnDown();
                while (pattern1[1].notes[2] != same)
                    pattern1[1].turnDown();
                chordSelected = new Chord[4] { pattern1[0], pattern1[1], pattern2[0], pattern2[1] };
            }
            return chordSelected;
        }
        public int degreeFromGlassworks(int prevDegree)
        {
            int[] useableDegree = new int[4];
            int index = 0;
            if (prevDegree == 0)
            {
                for (int i = 2; i < 6; i++, index++)
                    useableDegree[index] = i;
            }
            else if (prevDegree == 6)
            {
                for (int i = 1; i < 5; i++, index++)
                    useableDegree[index] = i;
            }
            else
            {
                for (int i = 0; i < prevDegree - 1; i++, index++)
                    useableDegree[index] = i;
                for (int i = prevDegree + 2; i < 7; i++, index++)
                    useableDegree[index] = i;
            }
            return useableDegree[BasicUtility.rander.Next(4)];
        }
        public Chord[] chordsFromGlassworks(int tune, int degree1, int degree2)
        {
            int[] noteChord1 = new int[3] { degree1, (degree1 + 2) % 7, (degree1 + 4) % 7 };
            int[] noteChord2 = new int[3] { degree2, (degree2 + 2) % 7, (degree2 + 4) % 7 };

            List<int> listSame = new List<int>(2);
            for (int i = 0; i < noteChord1.Length; i++)
            {
                for (int j = 0; j < noteChord2.Length; j++)
                {
                    if (noteChord1[i] == noteChord2[j])
                    {
                        listSame.Add(noteChord1[i]);
                    }
                }
            }
            Chord[] chordSelected = null;
            foreach (int note in listSame)
            {
                Chord chord1 = new Chord(degree1, 0, tune);
                Chord chord2 = new Chord(degree2, 0, tune);
                if (chord1.notes[chord1.notes.Length - 1] > note)
                {
                    chord1.turnDown();
                    while (chord1.notes[chord1.notes.Length - 1] > note)
                        chord1.turnDown();
                }
                else if (chord1.notes[chord1.notes.Length - 1] < note)
                {
                    chord1.turnUp();
                    while (chord1.notes[chord1.notes.Length - 1] < note)
                        chord1.turnUp();
                }
                if (chord2.notes[chord2.notes.Length - 1] > note)
                {
                    chord2.turnDown();
                    while (chord2.notes[chord2.notes.Length - 1] > note)
                        chord2.turnDown();
                }
                else if (chord2.notes[chord2.notes.Length - 1] < note)
                {
                    chord2.turnUp();
                    while (chord2.notes[chord2.notes.Length - 1] < note)
                        chord2.turnUp();
                }
                if (chord1.notes[1] != chord2.notes[1])
                    chordSelected = new Chord[] { chord1, chord2 };
            }
            if (chordSelected == null)
            {
                throw new Exception("select chord fail");
            }
            return chordSelected;
        }
        public List<int[]> degreesUseableFromGlassworks(Chord[] chords, int sameIndex)
        {
            int same = 0;
            if (BasicUtility.throwCoin)
            {
                same = chords[sameIndex].notes[1];
            }
            else
            {
                same = chords[sameIndex].notes[2];
                if (BasicUtility.throwCoin)
                    same++;
                else
                    same--;
            }
            List<int[]> listDegrees =
                new List<int[]>(new int[][] { ACCore.pitchFromDegree(same), ACCore.pitchFromDegree(same - 2), ACCore.pitchFromDegree(same - 4) });
            return listDegrees;
        }
    }
}
