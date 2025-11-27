using System;
using System.Windows.Forms;
using System.Drawing;
using System.Diagnostics.Eventing.Reader;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

Form scherm = new Form();
scherm.ClientSize = new Size(600, 300);
scherm.Text = "mandelbrot";
int schermHoogte = 300;
int schermBreedte = 500;

// Knop voor tekenen
Button knop = new Button();
knop.Size = new Size(50, 50);
knop.Location = new Point(510, 20);
scherm.Controls.Add(knop);
knop.Text = "Teken!";

Bitmap plaatje = new Bitmap(schermBreedte, schermHoogte);

// globale minimale en maximale waardes
double min_x = -2, max_x = 2;
double min_y = -2, max_y = 2;


int schermMidden_Hoogte = schermHoogte / 2;
int schermMidden_Breedte = schermBreedte / 2;

// berekening mandelbrotgetal
int mandelbrotgetal(double x, double y)
{
    double afstand = 0;
    int teller = 0;
    int MaxTeller = 100;

    double a = 0;
    double b = 0;

    while (afstand < 2 && teller < MaxTeller)
    {
        double xa = a * a - b * b + x;
        double yb = 2 * a * b + y;

        double pythagoras = Math.Pow(xa - 0, 2) + Math.Pow(yb - 0, 2);
        afstand = Math.Sqrt(pythagoras);

        a = xa;
        b = yb;

        teller = teller + 1;
    }

    return teller;

}
//berekening mandelbrot en pixelkleuren
void mandelbrotplaatje(object o, EventArgs e)
{
    for (int pixel_x = 0; pixel_x < schermBreedte; pixel_x++) 
    {
        for (int pixel_y = 0; pixel_y < schermHoogte; pixel_y++)
        {
           

            double x = min_x + (pixel_x / (double)schermBreedte) * (max_x - min_x);
            double y = min_y + (pixel_y / (double)schermHoogte) * (max_y - min_y);
            
            int uitkomst = mandelbrotgetal(x, y);

            if (uitkomst % 2 == 0)
                plaatje.SetPixel(pixel_x, pixel_y, Color.White);
            else
                plaatje.SetPixel(pixel_x, pixel_y, Color.Black);
        }
   
    
    }
    scherm.Invalidate();

};
// daadwerkelijke tekening op bitmap
void tekening(object o, PaintEventArgs pea)
{
    pea.Graphics.DrawImage(plaatje, 0, 0);
}

void zoom(object o, MouseEventArgs ea)
{ if (ea.Button == MouseButtons.Left)
    {
        double zoom_x = min_x + (ea.X / (double)schermBreedte) * (max_x - min_x);
        double zoom_y = min_y + (ea.Y / (double)schermHoogte) * (max_y - min_y);

        double zoomfactor = 0.5;

        double nieuw_x = (max_x - min_x) * zoomfactor;
        double nieuw_y = (max_y - min_y) * zoomfactor;

        // nieuw bereik aanmaken voor zoom

        min_x = zoom_x - nieuw_x / 2;
        max_x = zoom_x + nieuw_x / 2;
        min_y = zoom_y - nieuw_y / 2;
        max_y = zoom_y + nieuw_y / 2;
    } mandelbrotplaatje(o, ea);
}





scherm.MouseClick += zoom;
scherm.Paint += tekening;
knop.Click += mandelbrotplaatje;
Application.Run(scherm);
