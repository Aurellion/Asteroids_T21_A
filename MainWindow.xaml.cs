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
using System.Windows.Threading;

namespace Asteroids
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer timer = new DispatcherTimer();
        List<Asteroid> Asteroiden = new List<Asteroid>();
        List<Torpedo> Torpedos = new List<Torpedo>();
        Raumschiff Enterprise;

        public MainWindow()
        {
            InitializeComponent();
            timer.Interval = TimeSpan.FromMilliseconds(17);
            
            timer.Tick += Animiere;

           
        }

        private void Animiere(object sender, EventArgs e)
        {
            Zeichenfläche.Children.Clear();

            List<Torpedo> zuLöschendeTorpedos = new List<Torpedo>();
            List<Asteroid> zuLöschendeAsteroiden = new List<Asteroid>();

            foreach (Asteroid item in Asteroiden)
            {
                item.Zeichnen(Zeichenfläche);
                item.Bewegen(timer.Interval, Zeichenfläche);
            }
            Enterprise.Zeichnen(Zeichenfläche);
            Enterprise.Bewegen(timer.Interval, Zeichenfläche);
            foreach (Torpedo item in Torpedos)
	        {                 
                item.Zeichnen(Zeichenfläche);
                bool rausgeflogen = item.Bewegen(timer.Interval, Zeichenfläche);
                if(rausgeflogen)
                {
                    zuLöschendeTorpedos.Add(item);
                }
	        }
            //Torpedos.RemoveAll(x => zuLöschendeTorpedos.Contains(x));
            foreach (Torpedo item in zuLöschendeTorpedos)
            {
                Torpedos.Remove(item);
            }

            bool verloren=false;
            foreach (Asteroid A in Asteroiden)
            {
                foreach (Torpedo T in Torpedos)
                {
                    if(A.EnthältPunkt(T.x, T.y))
                    {
                        zuLöschendeTorpedos.Add(T);
                        zuLöschendeAsteroiden.Add(A);
                    }
                }
                if(A.EnthältPunkt(Enterprise.x, Enterprise.y))
                {
                    verloren=true;
                }
            }
            Torpedos.RemoveAll(t => zuLöschendeTorpedos.Contains(t));
            Asteroiden.RemoveAll(a => zuLöschendeAsteroiden.Contains(a));
	
            if(verloren)
            {
                MessageBoxResult ergebnis;
                ergebnis = MessageBox.Show("Noch eine Runde?","Game over!", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if(ergebnis == MessageBoxResult.Yes)
                {
                    SpielStarten();
                }
                else
                {
                    Application.Current.Shutdown();
                }
            }
        }

        private void BTN_Start_Click(object sender, RoutedEventArgs e)
        {
            timer.Start();
            SpielStarten();
            BTN_Start.IsEnabled = false;
        }

        private void SpielStarten()
        {
            Asteroiden.Clear();
            Torpedos.Clear();
            for (int i = 0; i < 20; i++)
            {
                Asteroiden.Add(new Asteroid(Zeichenfläche));
            }
            Enterprise = new Raumschiff(Zeichenfläche);
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if(timer.IsEnabled)
            {
                switch(e.Key)
                {
                    case Key.Left:
                    case Key.A:
                        Enterprise.Lenke(true);
                        break;
                    case Key.Right:
                    case Key.D:
                        Enterprise.Lenke(false);
                        break;
                    case Key.Up:
                    case Key.W:
                        Enterprise.Beschleunige(true);
                        break;
                    case Key.Down:
                    case Key.S:
                        Enterprise.Beschleunige(false);
                        break;
                    case Key.Space:                        
                        Torpedos.Add(new Torpedo(Enterprise));
                        break;
                }
            }
        }
    }
}
