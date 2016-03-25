using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ACGlass.Utility;

namespace ACGlass.Classes
{
    public class FourChord
    {
        public int[] notes;
        public int tune;
        //public int degree;
        //public int high;
        public FourChord(int buttom, int top, int setTune, int octive, int stable)
        {
            tune = setTune;
            int[] pass = null;
            switch (stable)
            {
                case 0:             // do not move
                    {
                        pass = new int[] { buttom + 2, buttom + 4 };
                    } break;
                case 1:             // move one note
                    {
                        pass = new int[] { buttom + 2, buttom + 4 };
                        pass[BasicUtility.rander.Next(2)] += BasicUtility.throwCoin ? 1 : -1;
                    } break;
                case 2:             // move two note
                    {
                        pass = new int[] { buttom + 2, buttom + 4 };
                        pass[0] += BasicUtility.throwCoin ? 1 : -1;
                        pass[1] += BasicUtility.throwCoin ? 1 : -1;
                    } break;
                case 3:             // total random
                    {
                        pass = new int[top - buttom - 1];
                        for (int i = 0; i < pass.Length; i++)
                            pass[i] = buttom + i + 1;
                        pass = BasicUtility.Cfinder(pass, 2);
                        BasicUtility.checkBalance(ref pass[0], ref pass[1]);
                    } break;
            }
            octive *= 7;
            notes = new int[4] { buttom + octive, pass[0] + octive, pass[1] + octive, top + octive };
        }
    }
}
