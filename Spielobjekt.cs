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

        public double vx;
        public double vy;

        public Spielobjekt(double x, double y, double vx, double vy)
        {
            this.x = x;
            this.y = y;
            this.vx = vx;
            this.vy = vy;
        }

        public abstract void Zeichnen(Canvas Zeichenfläche);

        public bool Bewegen(TimeSpan interval, Canvas Zeichenfläche)
        {
            bool rausgeflogen=false;
            x += vx * interval.TotalSeconds;
            y += vy * interval.TotalSeconds;

            if (x > Zeichenfläche.ActualWidth)
                {
                    rausgeflogen=true;
                    x = 0;
                }
            if (x < 0) 
                {
                    rausgeflogen = true;
                    x = Zeichenfläche.ActualWidth;
                }

            if (y > Zeichenfläche.ActualHeight)
                {
                    rausgeflogen=true;
                    y = 0;
                }
            if (y < 0)
                {
                    rausgeflogen=true;
                    y = Zeichenfläche.ActualHeight;
                }

            return rausgeflogen;
        }

    }

    class Asteroid : Spielobjekt
    {
        static Random rnd = new Random();
        Polygon umriss = new Polygon();

        public Asteroid(Canvas Zeichenfläche)
            : base( rnd.NextDouble() * Zeichenfläche.ActualWidth,
                    rnd.NextDouble() * Zeichenfläche.ActualHeight,
                    (rnd.NextDouble() - 0.5) * 200,
                    (rnd.NextDouble() - 0.5) * 200)
        {
            for (int i = 0; i < 15; i++)
            {
                double radius = rnd.NextDouble()*8+8;
                double winkel = 2 * Math.PI / 15 * i;
                umriss.Points.Add(new Point(radius*Math.Cos(winkel),radius*Math.Sin(winkel)));
            }
            umriss.Fill = Brushes.Gray;
        }

        public override void Zeichnen(Canvas Zeichenfläche)
        {
            Zeichenfläche.Children.Add(umriss);
            Canvas.SetTop(umriss, y);
            Canvas.SetLeft(umriss, x);
        }

        public bool EnthältPunkt(double x, double y)
        {
            return umriss.RenderedGeometry.FillContains(new Point(x-this.x,y-this.y));
        }

    }

    
    class Raumschiff : Spielobjekt
    {
        Polygon umriss = new Polygon();

        public Raumschiff(Canvas Zeichefläche)
            :base(0.5*Zeichefläche.ActualWidth,
                  0.5*Zeichefläche.ActualHeight,
                  0.1,
                  0.1)
        {
            umriss.Points.Add(new Point(0,-10));
            umriss.Points.Add(new Point(5,7));
            umriss.Points.Add(new Point(-5,7));
            umriss.Fill = Brushes.Blue;
        }

        public override void Zeichnen(Canvas Zeichenfläche)
        {
            double WinkelInGrad = Math.Atan2(vy,vx) * 180.0 / Math.PI + 90;
            umriss.RenderTransform = new RotateTransform(WinkelInGrad);
            Zeichenfläche.Children.Add(umriss);
            Canvas.SetTop(umriss, y);
            Canvas.SetLeft(umriss, x);
        }

        public void Beschleunige(bool schneller)
        {
            //if(schneller)
            //{
            //    vx*=1.1;
            //    vy*=1.1;
            //}
            //else
            //{
            //    vx*=0.9;
            //    vy*=0.9;
            //}
            double faktor = schneller ? 1.1 : 0.9;
            vx*=faktor;
            vy*=faktor;
        }

        public void Lenke(bool nachLinks)
        {
            double winkel = (nachLinks ? -5 : 5) / 180.0 * Math.PI;
            double sin = Math.Sin(winkel);
            double cos = Math.Cos(winkel);
            double vxn, vyn;
            vxn = vx * cos - vy*sin;
            vyn = vx * sin + vy*cos;
            vx=vxn;
            vy=vyn;
        }
    }
    
    class Torpedo : Spielobjekt
    {
        public Torpedo(Raumschiff schiff)
            :base(schiff.x, schiff.y, 2*schiff.vx, 2*schiff.vy)
        {

        }

        public override void Zeichnen(Canvas Zeichenfläche)
        {
            Ellipse e = new Ellipse();
            e.Width = 5;
            e.Height = 5;
            e.Fill = Brushes.Red;
            Zeichenfläche.Children.Add(e);
            Canvas.SetTop(e, y-e.Height/2);
            Canvas.SetLeft(e, x-e.Width/2);
        }
    }
}
