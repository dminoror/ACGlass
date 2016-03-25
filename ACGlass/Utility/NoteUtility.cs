using Sanford.Multimedia.Midi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACGlass.Utility
{
    public static class NoteUtility
    {
        public const int SectionDuring = 96;
        public static int DuringFromNoteType(NoteType type)
        {
            switch (type)
            {
                case NoteType.NoteFull:
                    return SectionDuring;
                case NoteType.NoteHalf:
                    return SectionDuring / 2;
                case NoteType.NoteQuarter:
                    return SectionDuring / 4;
                case NoteType.NoteEighth:
                    return SectionDuring / 8;
                case NoteType.NoteSixteenth:
                    return SectionDuring / 16;
                case NoteType.NoteThird:
                    return SectionDuring / 12;
            }
            return 0;
        }

        public static void AddTempo(int BPM, Track track)
        {
            TempoChangeBuilder tempo = new TempoChangeBuilder();
            tempo.Tempo = 60000000 / BPM;
            tempo.Build();
            int AbsoluteTicks = track.GetMidiEvent(track.Count - 1).AbsoluteTicks - 1;
            track.Insert(AbsoluteTicks, tempo.Result);
        }
    }

    public enum NoteType
    {
        NoteFull,
        NoteHalf,
        NoteQuarter,
        NoteEighth,
        NoteSixteenth,
        NoteThird
    }
}
