using ACGlass.Classes;
using ACGlass.Classes.Patterns.Notes;
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
            List<BaseNote> notes = HeadTwoNotes.generate(chords, register, loudness);
            return notes;
        }
        public override int minimumDuring
        {
            get { return 12; }
        }
    }
}
