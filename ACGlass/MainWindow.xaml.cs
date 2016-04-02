using ACGlass.Classes;
using ACGlass.Classes.Patterns;
using ACGlass.Utility;
using Sanford.Multimedia.Midi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ACGlass
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        Sequence sequence;
        Sequencer seqer;
        OutputDevice outDevice;

        Score score;

        private void mainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            #region MIDI Plugin
            sequence = new Sequence();
            sequence.LoadProgressChanged += sequence_LoadProgressChanged;
            sequence.LoadCompleted += sequence_LoadCompleted;

            seqer = new Sequencer();
            seqer.Sequence = sequence;
            seqer.ChannelMessagePlayed += seqer_ChannelMessagePlayed;

            outDevice = new OutputDevice(0);
            #endregion
            
            score = new Score(sequence);
        }


        private void btnClearPath_Click(object sender, RoutedEventArgs e)
        {
            stackPaths.Children.Clear();
        }

        private void btnPlay_Click(object sender, RoutedEventArgs e)
        {
            if (stackPaths.Children.Count == 0) return;

            score.Clear();

            //PatternGenerater.ptTwo2Three(0, 0, score);
            //PatternGenerater.ptGlassworks(0, 0, BasicUtility.throwCoin, score);
            //PatternGenerater.ptMetamorphosis(0, 0, score);
            //PatternGenerater.ptEtude2(0, 0, score, 2);
            //PatternGenerater.ptFourChord(0, 0, score);
            Pattern[] patterns = new Pattern[stackPaths.Children.Count];
            for (int i = 0; i < stackPaths.Children.Count; i++)
            {
                UIElement[] parameters = (UIElement[])(((Grid)stackPaths.Children[i]).Tag);
                double valence, arousal;
                double.TryParse(((TextBox)parameters[0]).Text, out valence);
                double.TryParse(((TextBox)parameters[1]).Text, out arousal);
                valence /= 100;
                arousal /= 100;
                bool follow = ((CheckBox)parameters[2]).IsChecked == true;

                //patterns[i] = ScoreUtility.generateScore(valence, arousal);
                if (follow && i > 0)
                {
                    ScoreUtility prevPT = patterns[i - 1].patterns[0];
                    patterns[i] = ACCore.generateScore(valence, arousal, prevPT);
                    //patterns[i].BPM = patterns[i - 1].BPM;
                    patterns[i].tune = patterns[i - 1].tune;
                }
                else
                {
                    if (comboPattern.SelectedIndex != 0)
                    {
                        patterns[i] = ACCore.generateScore(valence, arousal, PatternSelector.mainPatterns[comboPattern.SelectedIndex - 1]);
                    }
                    else
                        patterns[i] = ACCore.generateScore(valence, arousal, null);
                }
            }
            ACCore.executeScore(patterns, score);
            

            //seqer.Start();
            sequence.Save("testt.mid");

        }

        private void mapMouse_Move(object sender, MouseEventArgs e)
        {
            Grid grid = (Grid)sender;
            Point point = e.GetPosition(grid);
            txtX.Text = Math.Round((point.X / grid.ActualWidth * 100), 0).ToString();
            txtY.Text = Math.Round(((1 - point.Y / grid.ActualHeight) * 100), 0).ToString();
        }

        #region MINI Plugin
        void sequence_LoadProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
        }
        void sequence_LoadCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            List<MidiEvent>[] tracks = new List<MidiEvent>[sequence.Count];
            for (int i = 0; i < sequence.Count; i++)
            {
                List<MidiEvent> list = new List<MidiEvent>(sequence[i].Count);
                for (int j = 0; j < sequence[i].Count; j++)
                {
                    MidiEvent midi = sequence[i].GetMidiEvent(j);
                    list.Add(midi);
                }
                tracks[i] = list;
            }

        }
        void seqer_ChannelMessagePlayed(object sender, ChannelMessageEventArgs e)
        {
            outDevice.Send(e.Message);
        }
        #endregion

        private void mapMouse_Click(object sender, MouseButtonEventArgs e)
        {
            createEmotionPath(txtX.Text, txtY.Text);
        }

        void createEmotionPath(string V, string A)
        {
            Grid grid = new Grid();
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(3, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(3, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            TextBox boxV = new TextBox() { Text = V, TextAlignment = TextAlignment.Center };
            TextBox boxA = new TextBox() { Text = A, TextAlignment = TextAlignment.Center };
            Grid.SetColumn(boxA, 1);
            Viewbox view = new Viewbox();
            Grid.SetColumn(view, 2);
            CheckBox check = new CheckBox();
            view.Child = check;
            grid.Children.Add(boxV);
            grid.Children.Add(boxA);
            grid.Children.Add(view);
            grid.Tag = new UIElement[] { boxV, boxA, check };
            stackPaths.Children.Add(grid);
        }

    }
}
