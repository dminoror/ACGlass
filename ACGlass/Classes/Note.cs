using ACGlass.Utility;
using Sanford.Multimedia.Midi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACGlass.Classes
{
    public abstract class BaseNote
    {
        public int during;

        public BaseNote(int setDuring)
        {
            during = setDuring;
        }
        public abstract void AddNote(Track track, Score score);
    }

    public class Note : BaseNote
    {
        public byte loudness;
        public byte[] pitch;

        public Note(int setDuring, byte setPitch, byte setLoudness)
            : base(setDuring)
        {
            loudness = setLoudness;
            pitch = new byte[] { setPitch };
        }
        public Note(int setDuring, byte[] setPitch, byte setLoudness)
            : base(setDuring)
        {
            loudness = setLoudness;
            pitch = setPitch;
        }
        public Note(NoteType setType, byte[] setPitch, byte setLoudness)
            : this(NoteUtility.DuringFromNoteType(setType), setPitch, setLoudness)
        {
        }
        public Note(NoteType setType, byte setPitch, byte setLoudness)
            : this(NoteUtility.DuringFromNoteType(setType), setPitch, setLoudness)
        {
        }
        public override void AddNote(Track track, Score score)
        {
            ChannelMessageBuilder builder = new ChannelMessageBuilder();
            for (int i = 0; i < pitch.Length; i++)
            {
                builder.Command = ChannelCommand.NoteOn;
                builder.MidiChannel = 0;
                builder.Data1 = pitch[i];
                builder.Data2 = loudness;
                builder.Build();
                track.Insert(score.tick, builder.Result);
            }
            score.tick += during;
            for (int i = 0; i < pitch.Length; i++)
            {
                builder.Command = ChannelCommand.NoteOff;
                builder.MidiChannel = 0;
                builder.Data1 = pitch[i];
                builder.Data2 = loudness;
                builder.Build();
                track.Insert(score.tick, builder.Result);
            }
        }
    }
    public class RestNote : BaseNote
    {
        public RestNote(int setDuring)
            : base(setDuring)
        {
        }
        public RestNote(NoteType setType)
            : base(NoteUtility.DuringFromNoteType(setType))
        {
        }

        public override void AddNote(Track track, Score score)
        {
            score.tick += during;
        }
    }
    public class TempoChanger : BaseNote
    {
        public TempoChanger(int setBPM)
            : base(setBPM)
        {
        }
        public override void AddNote(Track track, Score score)
        {
            score.AddTempo(during);
        }
    }
}
