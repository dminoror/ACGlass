using ACGlass.Classes;
using ACGlass.Classes.Patterns;
using ACGlass.Utility.Patterns.Score;
using ACGlass.Utility.Patterns.Score.Full;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACGlass.Utility
{
    public class PatternSelector
    {
        public static TwoLineScore ptTwoLine = new TwoLineScore();
        public static ThreeLineScore ptThreeLine = new ThreeLineScore();
        public static FourLineScore ptFourLine = new FourLineScore();
        public static SixPeakScore ptSixPeak = new SixPeakScore();
        public static HeadTwoScore ptHeadTwo = new HeadTwoScore();

        public static GlassworksScore ptGlassworks = new GlassworksScore();
        public static Metamor1Score ptMetamor1 = new Metamor1Score();
        public static Metamor2Score ptMetamor2 = new Metamor2Score();
        public static EtudeNo2Score ptEtudeNo2 = new EtudeNo2Score();
        public static EtudeNo3Score ptEtudeNo3 = new EtudeNo3Score();
        public static EtudeNo4Score ptEtudeNo4 = new EtudeNo4Score();
        public static EtudeNo5Score ptEtudeNo5 = new EtudeNo5Score();
        public static EtudeNo6_1Score ptEtudeNo6_1 = new EtudeNo6_1Score();
        public static EtudeNo6_2Score ptEtudeNo6_2 = new EtudeNo6_2Score();
        public static EtudeNo8Score ptEtudeNo8 = new EtudeNo8Score();

        public static ScoreUtility[] mainPatterns = new ScoreUtility[] { 
            ptTwoLine, 
            ptThreeLine, 
            ptFourLine, 
            ptSixPeak, 
            ptGlassworks, 
            ptMetamor1,
            ptMetamor2, 
            ptEtudeNo2,
            ptEtudeNo3,
            ptEtudeNo4,
            ptEtudeNo5,
            ptEtudeNo6_1,
            ptEtudeNo6_2,
            ptEtudeNo8
        };

        static PatternSelector()
        {
            foreach (ScoreUtility p in mainPatterns)
                p.getPairs();
        }

        public static Pattern select(double V, double A)
        {
            double picker = 0;// = BasicUtility.rander.NextDouble() * 2;
            List<int> cand = new List<int>();
            double[] distance = new double[mainPatterns.Length];
            for (int i = 0; i < mainPatterns.Length; i++)
            {
                ScoreUtility p = mainPatterns[i];
                distance[i] = 2 - (Math.Abs(p.Valence - V) + Math.Abs(p.Arousal - A));
                if (distance[i] > picker)
                    picker = distance[i];
            }
            picker = BasicUtility.rander.NextDouble() * picker;
            for (int i = 0; i < distance.Length; i++)
                if (distance[i] >= picker)
                    cand.Add(i);
            int indexPT = BasicUtility.rander.Next(cand.Count);
            ScoreUtility ptMain = mainPatterns[indexPT];
            Pattern pattern = ptMain.generatePattern(V, A);
            return pattern;
        }
        public static Pattern selectByPU(double V, double A, ScoreUtility PU)
        {
            ScoreUtility ptMain = PU;
            Pattern pattern = ptMain.generatePattern(V, A);
            return pattern;
        }
    }
}
