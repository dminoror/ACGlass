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
        int Tune;
        public int mode;
        public int BPM;
        public object tag;
        public Pattern()
        {
            mode = -1;
        }
        public int tune
        {
            set
            {
                if (Tune > 11)
                {
                    throw new Exception("why over 11");
                }
                Tune = value;
            }
            get
            {
                return Tune;
            }
        }
    }
}
