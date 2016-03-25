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
        public static TwoLineScore ptTowLine = new TwoLineScore();
        public static ThreeLineScore ptThreeLine = new ThreeLineScore();
        public static FourLineScore ptFourLine = new FourLineScore();
        public static SixPeakScore ptSixPeak = new SixPeakScore();
        public static HeadTwoScore ptHeadTwo = new HeadTwoScore();

        public static GlassworksScore ptGlassworks = new GlassworksScore();
        public static Metamor1Score ptMetamor1 = new Metamor1Score();
        public static Metamor2Score ptMetamor2 = new Metamor2Score();
        public static EtudeNo2Score ptEtudeNo2 = new EtudeNo2Score();

        public static ScoreUtility[] mainPatterns = new ScoreUtility[] { 
            ptTowLine, 
            ptThreeLine, 
            ptFourLine, 
            ptSixPeak, 
            ptGlassworks, 
            ptMetamor1,
            ptMetamor2, 
            ptEtudeNo2 };
        public static ScoreUtility[] pairPatterns = new ScoreUtility[] {
            ptTowLine,
            ptThreeLine,
            ptFourLine,
            ptSixPeak,
            ptHeadTwo };
        

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
            Pattern pattern = ptMain.generatePattern(V, A, distance);
            return pattern;
        }
        public static Pattern selectByPU(double V, double A, ScoreUtility PU)
        {
            double[] distance = new double[mainPatterns.Length];
            for (int i = 0; i < mainPatterns.Length; i++)
            {
                ScoreUtility p = mainPatterns[i];
                distance[i] = 2 - (Math.Abs(p.Valence - V) + Math.Abs(p.Arousal - A));
            }
            ScoreUtility ptMain = PU;
            Pattern pattern = ptMain.generatePattern(V, A, distance);
            return pattern;
        }
    }
}
