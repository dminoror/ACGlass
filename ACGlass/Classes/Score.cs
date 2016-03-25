using Sanford.Multimedia.Midi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACGlass.Classes
{
    public class Score
    {
        public Sequence sequence;
        public int tick;
        public Score(Sequence setSequence)
        {
            sequence = setSequence;
            tick = 0;
            sequence.Clear();
            for (int i = 0; i < 3; i++)
                sequence.Add(new Track());
        }
        public void AddTempo(int BPM)
        {
            TempoChangeBuilder tempo = new TempoChangeBuilder();
            tempo.Tempo = 60000000 / BPM;
            tempo.Build();
            sequence[0].Insert(tick, tempo.Result);
        }
        public void Clear()
        {
            sequence[0].Clear();
            sequence[1].Clear();
            sequence[2].Clear();
            tick = 0;
        }
    }
}
