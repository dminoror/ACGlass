using ACGlass.Classes;
using ACGlass.Classes.Patterns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACGlass.Utility.Patterns.Score.Full
{
    public class EtudeNo2Score : ScoreUtility// 8
    {
        public EtudeNo2Score()
        {
            Valence = 0.375;
            Arousal = 0.525;
            Name = "EtudeNo2";
            selfId = 8;
        }
        public override Pattern generatePattern(double V, double A, double[] distance)
        {
            int tune = 0;

            int[] registers = new int[] { 3 * 12, 5 * 12 };
            byte loudness1 = 127;
            byte loudness2 = 90;

            int BPM = ScoreUtility.BPMFromDuring(V, A, 12);

            Pattern pattern = new Pattern()
            {
                Valence = V,
                Arousal = A,
                patterns = new ScoreUtility[] { this },
                registers = registers,
                loudness = new byte[] { loudness1, loudness2 },
                tune = tune,
                BPM = BPM
            };
            return pattern;
        }
        public override List<BaseNote>[] generateScore(Pattern[] patterns, int index)
        {
            Pattern pattern = patterns[index];
            int status = 1;
            if (pattern.tag != null)
            {
                status = (int)pattern.tag;
            }
            FourChord[] fourChords = generateChords(pattern.tune);
            List<BaseNote> hand1 = new List<BaseNote>();
            switch (status)
            {
                case 0:
                    {
                    } break;
                case 1:
                    {
                        FourChord chord = fourChords[0];
                        hand1.Add(new RestNote(48));
                        hand1.Add(new Note(180, new byte[] { (byte)(ACCore.pitchFromMajor(chord.tune, chord.notes[3] - 4) + pattern.registers[0]), (byte)(ACCore.pitchFromMajor(chord.tune, chord.notes[3] + 3) + pattern.registers[0]) }, pattern.loudness[0]));
                        hand1.Add(new Note(180, new byte[] { (byte)(ACCore.pitchFromMajor(chord.tune, chord.notes[3] - 4) + pattern.registers[0]), (byte)(ACCore.pitchFromMajor(chord.tune, chord.notes[3] + 3) + pattern.registers[0]) }, pattern.loudness[0]));
                        hand1.Add(new Note(84, new byte[] { (byte)(ACCore.pitchFromMajor(chord.tune, chord.notes[3] - 4) + pattern.registers[0]), (byte)(ACCore.pitchFromMajor(chord.tune, chord.notes[3] + 3) + pattern.registers[0]) }, pattern.loudness[0]));
                        hand1.Add(new Note(24, new byte[] { (byte)(ACCore.pitchFromMajor(chord.tune, chord.notes[3] - 3) + pattern.registers[0]), (byte)(ACCore.pitchFromMajor(chord.tune, chord.notes[3] + 4) + pattern.registers[0]) }, pattern.loudness[0]));
                        chord = fourChords[2];
                        hand1.Add(new Note(24, new byte[] { (byte)(ACCore.pitchFromMajor(chord.tune, chord.notes[0] + 4) + pattern.registers[0]), (byte)(ACCore.pitchFromMajor(chord.tune, chord.notes[0] + 11) + pattern.registers[0]) }, pattern.loudness[0]));
                        chord = fourChords[0];
                        hand1.Add(new Note(180, new byte[] { (byte)(ACCore.pitchFromMajor(chord.tune, chord.notes[3] - 4) + pattern.registers[0]), (byte)(ACCore.pitchFromMajor(chord.tune, chord.notes[3] + 3) + pattern.registers[0]) }, pattern.loudness[0]));
                        hand1.Add(new Note(180, new byte[] { (byte)(ACCore.pitchFromMajor(chord.tune, chord.notes[3]) + pattern.registers[0]), (byte)(ACCore.pitchFromMajor(chord.tune, chord.notes[3] + 7) + pattern.registers[0]) }, pattern.loudness[0]));
                    } break;
                case 2:
                    pattern.registers[0] += 12;
                    goto case 1;
            }
            List<BaseNote> hand2 = new List<BaseNote>();
            for (int section = 0; section < fourChords.Length; section++)
            {
                FourChord chord = fourChords[section];
                hand2.Add(new Note(12, (byte)(ACCore.pitchFromMajor(chord.tune, chord.notes[0]) + pattern.registers[1]), pattern.loudness[1]));
                hand2.Add(new Note(12, (byte)(ACCore.pitchFromMajor(chord.tune, chord.notes[1]) + pattern.registers[1]), pattern.loudness[1]));
                hand2.Add(new Note(12, (byte)(ACCore.pitchFromMajor(chord.tune, chord.notes[2]) + pattern.registers[1]), pattern.loudness[1]));
                hand2.Add(new Note(12, (byte)(ACCore.pitchFromMajor(chord.tune, chord.notes[3]) + pattern.registers[1]), pattern.loudness[1]));
                hand2.Add(new Note(12, (byte)(ACCore.pitchFromMajor(chord.tune, chord.notes[0]) + pattern.registers[1]), pattern.loudness[1]));
                hand2.Add(new Note(12, (byte)(ACCore.pitchFromMajor(chord.tune, chord.notes[1]) + pattern.registers[1]), pattern.loudness[1]));
                hand2.Add(new Note(12, (byte)(ACCore.pitchFromMajor(chord.tune, chord.notes[2]) + pattern.registers[1]), pattern.loudness[1]));
                hand2.Add(new Note(12, (byte)(ACCore.pitchFromMajor(chord.tune, chord.notes[0]) + pattern.registers[1]), pattern.loudness[1]));
                hand2.Add(new Note(12, (byte)(ACCore.pitchFromMajor(chord.tune, chord.notes[1]) + pattern.registers[1]), pattern.loudness[1]));
                hand2.Add(new Note(12, (byte)(ACCore.pitchFromMajor(chord.tune, chord.notes[2]) + pattern.registers[1]), pattern.loudness[1]));
                hand2.Add(new Note(12, (byte)(ACCore.pitchFromMajor(chord.tune, chord.notes[3]) + pattern.registers[1]), pattern.loudness[1]));
                hand2.Add(new Note(12, (byte)(ACCore.pitchFromMajor(chord.tune, chord.notes[0]) + pattern.registers[1]), pattern.loudness[1]));
                hand2.Add(new Note(12, (byte)(ACCore.pitchFromMajor(chord.tune, chord.notes[1]) + pattern.registers[1]), pattern.loudness[1]));
                hand2.Add(new Note(12, (byte)(ACCore.pitchFromMajor(chord.tune, chord.notes[0]) + pattern.registers[1]), pattern.loudness[1]));
                hand2.Add(new Note(12, (byte)(ACCore.pitchFromMajor(chord.tune, chord.notes[1]) + pattern.registers[1]), pattern.loudness[1]));
            }
            List<BaseNote>[] score = new List<BaseNote>[] { hand1, hand2 };
            if (index != patterns.Length - 1 && pattern.BPM != patterns[index + 1].BPM)
            {
                double distance = (pattern.BPM - patterns[index + 1].BPM) / 5.0;
                hand2.Insert(hand2.Count - 15, new TempoChanger((int)(pattern.BPM - distance)));
                hand2.Insert(hand2.Count - 11, new TempoChanger((int)(pattern.BPM - distance * 2)));
                hand2.Insert(hand2.Count - 8, new TempoChanger((int)(pattern.BPM - distance * 3)));
                hand2.Insert(hand2.Count - 4, new TempoChanger((int)(pattern.BPM - distance * 4)));
            }
            hand2.Insert(0, new TempoChanger(pattern.BPM));
            return score;
        }

        public FourChord[] generateChords(int tune)
        {
            int offset = BasicUtility.rander.Next(7);
            FourChord[] fourChords = new FourChord[5];
            fourChords[0] = new FourChord(-1 + offset, 4 + offset, tune, 0, 0);
            fourChords[1] = new FourChord(-2 + offset, 4 + offset, tune, 0, 0);
            fourChords[2] = new FourChord(3 + offset, 9 + offset, tune + 3, -1, 0);
            fourChords[3] = new FourChord(-3 + offset, 4 + offset, tune, 0, 0);
            fourChords[4] = new FourChord(-5 + offset, 2 + offset, tune, 0, 0);
            return fourChords;
        }

        public override int minimumDuring
        {
            get { return 12; }
        }
        public override void handleFollow(Pattern[] patterns, ref int index)
        {
            if (index + 1 < patterns.Length && patterns[index + 1].patterns[0].Equals(this))
            {
                patterns[index].tag = 0;
                bool isIncrease = true;
                int tag = 1;
                for (int i = 1; ; i++)
                {
                    if (patterns[i + index].patterns[0].Equals(this))
                    {
                        patterns[i + index].tag = tag;
                        if (isIncrease)
                        {
                            tag++;
                            if (tag == 2)
                                isIncrease = false;
                        }
                        else
                        {
                            tag--;
                            if (tag == 0)
                                isIncrease = true;
                        }
                    }
                    else
                    {
                        index += i;
                        break;
                    }
                    if (i + index < patterns.Length)
                    {
                        index += i;
                        break;
                    }
                }
            }
            else
                index++;
        }
    }
}
