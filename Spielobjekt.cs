using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Asteroids
{
    abstract class Spielobjekt
    {
        public double x { get; private set; }
        public double y { get; private set; }

        protected double vx;
        protected double vy;

        public Spielobjekt(double x, double y, double vx, double vy)
        {
            this.x = x;
            this.y = y;
            this.vx = vx;
            this.vy = vy;
        }

        public abstract void Zeichnen(Canvas Zeichenfläche);

        public void Bewegen(TimeSpan interval, Canvas Zeichenfläche)
        {
            x += vx * interval.TotalSeconds;
            y += vy * interval.TotalSeconds;

            if (x > Zeichenfläche.ActualWidth) x = 0;
            if (x < 0) x = Zeichenfläche.ActualWidth;

            if (y > Zeichenfläche.ActualHeight) y = 0;
            if (y < 0) y = Zeichenfläche.ActualHeight;
        }

    }

    class Asteroid : Spielobjekt
    {
        static Random rnd = new Random();

        public Asteroid(Canvas Zeichenfläche)
            : base( rnd.NextDouble() * Zeichenfläche.ActualWidth,
                    rnd.NextDouble() * Zeichenfläche.ActualHeight,
                    (rnd.NextDouble() - 0.5) * 200,
                    (rnd.NextDouble() - 0.5) * 200)
        {

        }

        public override void Zeichnen(Canvas Zeichenfläche)
        {
            Polygon umriss = new Polygon();
            for (int i = 0; i < 7; i++)
            {
                double winkel = 2 * Math.PI / 7 * i;
                umriss.Points.Add(new Point(10*Math.Cos(winkel),10*Math.Sin(winkel)));
            }

            umriss.Fill = Brushes.Gray;
            Zeichenfläche.Children.Add(umriss);
            Canvas.SetTop(umriss, y);
            Canvas.SetLeft(umriss, x);
        }
    }

    //to do:
    //class Raumschiff : Spielobjekt
    //{

    //}

    //class Torpedo : Spielobjekt
    //{

    //}
}
