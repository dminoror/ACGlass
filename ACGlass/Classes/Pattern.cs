using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACGlass.Classes.Patterns
{
    public class Pattern
    {
        public double Valence;
        public double Arousal;
        public ScoreUtility[] patterns;
        public int[] registers;
        public byte[] loudness;
        public bool?[] orders;
        public int tune;
        public int BPM;
        public object tag;
    }
}
