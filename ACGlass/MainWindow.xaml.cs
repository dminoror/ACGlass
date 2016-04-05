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
            for (int i = 0; i < stackPaths.Children.Count; i++)
            {
                Grid parent = (Grid)stackPaths.Children[i];
                TextBox boxV = (TextBox)parent.Children[0];
                TextBox boxA = (TextBox)parent.Children[1];
                CheckBox check = (CheckBox)parent.Children[2];
                boxV.TextChanged -= box_TextChanged;
                boxA.TextChanged -= box_TextChanged;
            }
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
            createEmotionPath(e.GetPosition((Grid)sender));
        }

        const double Round_Diameter = 16;
        const double Round_Radius = Round_Diameter / 2;
        const double Line_Thinkness = Round_Diameter / 4;

        void createEmotionPath(Point point)
        {
            Grid grid = new Grid();
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(3, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(3, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            TextBox boxV = new TextBox() { Text = txtX.Text, TextAlignment = TextAlignment.Center };
            TextBox boxA = new TextBox() { Text = txtY.Text, TextAlignment = TextAlignment.Center };
            boxV.TextChanged += box_TextChanged;
            boxA.TextChanged += box_TextChanged;
            Grid.SetColumn(boxA, 1);
            Viewbox view = new Viewbox();
            Grid.SetColumn(view, 2);
            CheckBox check = new CheckBox();
            check.Checked += check_Checked;
            check.Unchecked += check_Checked;
            view.Child = check;
            grid.Children.Add(boxV);
            grid.Children.Add(boxA);
            grid.Children.Add(view);
            stackPaths.Children.Add(grid);

            Ellipse ellipse = new Ellipse()
            {
                Width = Round_Diameter,
                Height = Round_Diameter,
                Fill = Brushes.Black,
                Margin = new Thickness(point.X - Round_Radius, point.Y - Round_Radius, 0, 0)
            };
            grid.Tag = new UIElement[] { boxV, boxA, check, ellipse };
            if (stackPaths.Children.Count > 1)
            {
                Grid prevGrid = (Grid)stackPaths.Children[stackPaths.Children.Count - 2];
                Ellipse prevEll = (Ellipse)((UIElement[])prevGrid.Tag)[3];
                Line line = new Line() 
                {
                    X1 = prevEll.Margin.Left + Round_Radius,
                    Y1 = prevEll.Margin.Top + Round_Radius,
                    X2 = ellipse.Margin.Left + Round_Radius,
                    Y2 = ellipse.Margin.Top + Round_Radius,
                    Stroke = ellipse.Fill,
                    StrokeThickness = Line_Thinkness,
                    Tag = prevEll
                };
                ellipse.Tag = line;
                canvasPaths.Children.Add(line);
            }
            canvasPaths.Children.Add(ellipse);
        }

        void check_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox check = (CheckBox)sender;
            bool isChecked = check.IsChecked == true;
            Grid parent = (Grid)((Viewbox)check.Parent).Parent;
            Ellipse ellipse = (Ellipse)((UIElement[])parent.Tag)[3];
            ellipse.Fill = isChecked ? Brushes.DarkGray : Brushes.Black;
        }

        void box_TextChanged(object sender, TextChangedEventArgs e)
        {
            Grid parent = (Grid)((TextBox)sender).Parent;
            double V, A;
            double.TryParse(((TextBox)parent.Children[0]).Text, out V);
            double.TryParse(((TextBox)parent.Children[1]).Text, out A);

            Ellipse ellipse = (Ellipse)parent.Tag;
            ellipse.Margin = new Thickness((V / 100 * canvasPaths.ActualWidth) - Round_Radius, ((1 - A / 100) * canvasPaths.ActualHeight) - Round_Radius, 0, 0);
            if (ellipse.Tag != null)
            {
                Line line = (Line)ellipse.Tag;
                line.X2 = ellipse.Margin.Left + Round_Radius;
                line.Y2 = ellipse.Margin.Top + Round_Radius;
            }
            int index = 0;
            for (; index < stackPaths.Children.Count - 1; index++)
                if (stackPaths.Children[index].Equals(parent))
                    break;
            index += 1;
            if (index != stackPaths.Children.Count)
            {
                Line linker = (Line)((Ellipse)((UIElement[])((Grid)stackPaths.Children[index]).Tag)[3]).Tag;
                linker.X1 = ellipse.Margin.Left + Round_Radius;
                linker.Y1 = ellipse.Margin.Top + Round_Radius;
            }
        }

    }
}
