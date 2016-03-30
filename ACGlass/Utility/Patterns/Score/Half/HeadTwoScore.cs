using ACGlass.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACGlass.Utility.Patterns.Score
{
    public class HeadTwoScore : ScoreUtility// 4
    {
        public HeadTwoScore()
        {
            Valence = 0.525;
            Arousal = 0.3;
            Name = "HeadTwo";
            selfId = 4;
        }
        public override List<BaseNote> generateNotes(double V, double A, Chord[] chords, int register, byte loudness, bool? setOrder)
        {
            List<BaseNote> notes = new List<BaseNote>();
            for (int section = 0; section < 4; section++)
            {
                Chord chord = chords[section];
                int bass = BasicUtility.throwCoin ? chord.notes[0] : chord.notes[2] - 7;
                notes.Add(new Note(12,
                    new byte[] { (byte)(ACCore.pitchFromMajor(chord.tune, bass) + register), 
                                 (byte)(ACCore.pitchFromMajor(chord.tune, chord.notes[1]) + register) }, loudness));
                notes.Add(new Note(12, (byte)(ACCore.pitchFromMajor(chord.tune, chord.notes[2]) + register), loudness));
                for (int i = 0; i < 3; i++)
                {
                    notes.Add(new Note(12, (byte)(ACCore.pitchFromMajor(chord.tune, chord.notes[1]) + register), loudness));
                    notes.Add(new Note(12, (byte)(ACCore.pitchFromMajor(chord.tune, chord.notes[2]) + register), loudness));
                }
            }
            return notes;
        }
        public override int minimumDuring
        {
            get { return 12; }
        }
    }
}
