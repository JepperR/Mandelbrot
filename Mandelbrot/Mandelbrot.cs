using System;
using System.Windows.Forms;
using System.Drawing;
using System.Diagnostics.Eventing.Reader;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using System.Drawing.Text;
using System.Globalization;



Form scherm = new Form();
scherm.ClientSize = new Size(1200, 815);
scherm.Text = "mandelbrot";

//label aanmaken voor foutmelding

Label foutmelding = new Label();
foutmelding.Location = new Point(260, 160);
foutmelding.Text = "";
foutmelding.ForeColor = System.Drawing.Color.FromArgb(220, 20, 60);
scherm.Controls.Add(foutmelding);

//aanmaken tekst-invoervelden en bijbehorende labels
Label midden_x = new Label();
TextBox invoerMidden_x = new TextBox();

Label midden_y = new Label();
TextBox invoerMidden_y = new TextBox();

Label schaal = new Label();
TextBox invoerSchaal = new TextBox();

Label maxAantal = new Label();
TextBox invoerMaxAantal = new TextBox();

Label zoomFactor = new Label();
TextBox invoerZoomFactor = new TextBox();

scherm.Controls.Add(midden_x);
scherm.Controls.Add(invoerMidden_x);

scherm.Controls.Add(midden_y);
scherm.Controls.Add(invoerMidden_y);

scherm.Controls.Add(schaal);
scherm.Controls.Add(invoerSchaal);

scherm.Controls.Add(maxAantal);
scherm.Controls.Add(invoerMaxAantal);

scherm.Controls.Add(zoomFactor);
scherm.Controls.Add(invoerZoomFactor);

// toekennen van eigenschappen aan tekst-invoervelden en bijbehorende labels
midden_x.Location = new Point(20, 20);
invoerMidden_x.Location = new Point(120, 18);
midden_x.Text = "midden x:";

midden_y.Location = new Point(20, 55);
invoerMidden_y.Location = new Point(120, 53);
midden_y.Text = "midden y:";

schaal.Location = new Point(20, 90);
invoerSchaal.Location = new Point(120, 88);
schaal.Text = "schaal:";

maxAantal.Location = new Point(20, 125);
invoerMaxAantal.Location = new Point(120, 123);
maxAantal.Text = "max aantal:";

zoomFactor.Location = new Point(20, 160);
invoerZoomFactor.Location = new Point(120, 158);
zoomFactor.Text = "zoomfactor:";

//aanmaken schuifbar voor rood, groen en blauw
TrackBar invoerschuifrood = new TrackBar();
TrackBar invoerschuifgroen = new TrackBar();
TrackBar invoerschuifblauw = new TrackBar();

scherm.Controls.Add(invoerschuifrood);
scherm.Controls.Add(invoerschuifgroen);
scherm.Controls.Add(invoerschuifblauw);

Label schuifrood = new Label();
scherm.Controls.Add(schuifrood);
schuifrood.Location = new Point(20, 200);

Label schuifgroen = new Label();
scherm.Controls.Add(schuifgroen);
schuifgroen.Location = new Point(20, 280);

Label schuifblauw = new Label();
scherm.Controls.Add(schuifblauw);
schuifblauw.Location = new Point(20, 360);

invoerschuifrood.Location = new Point(20, 220);
schuifrood.Text = "roodwaarde:";
invoerschuifrood.Size = new Size(200, 20);
invoerschuifrood.Minimum = 0;
invoerschuifrood.Maximum = 255;
invoerschuifrood.Orientation = Orientation.Horizontal;

invoerschuifgroen.Location = new Point(20, 300);
schuifgroen.Text = "groenwaarde:";
invoerschuifgroen.Size = new Size(200, 20);
invoerschuifgroen.Minimum = 0;
invoerschuifgroen.Maximum = 255;
invoerschuifgroen.Orientation = Orientation.Horizontal;

invoerschuifblauw.Location = new Point(20, 380);
schuifblauw.Text = "blauwwaarde:";
invoerschuifblauw.Size = new Size(200, 20);
invoerschuifblauw.Minimum = 0;
invoerschuifblauw.Maximum = 255;
invoerschuifblauw.Orientation = Orientation.Horizontal;

// knop voor tekenen
Button tekenknop = new Button();
tekenknop.Location = new Point(300, 380);
tekenknop.Size = new Size(75, 40);
scherm.Controls.Add(tekenknop);
tekenknop.Text = "Teken!";

//knoppen voor voorbeelden
Button voorbeeld_1 = new Button();
voorbeeld_1.Location = new Point(20, 450);
voorbeeld_1.Size = new Size(100, 100);
scherm.Controls.Add(voorbeeld_1);
voorbeeld_1.Text = "Voorbeeld 1:";

Button voorbeeld_2 = new Button();
voorbeeld_2.Location = new Point(140, 450);
voorbeeld_2.Size = new Size(100, 100);
scherm.Controls.Add(voorbeeld_2);
voorbeeld_2.Text = "Voorbeeld 2:";

Button voorbeeld_3 = new Button();
voorbeeld_3.Location = new Point(20, 570);
voorbeeld_3.Size = new Size(100, 100);
scherm.Controls.Add(voorbeeld_3);
voorbeeld_3.Text = "Voorbeeld 3:";

Button voorbeeld_4 = new Button();
voorbeeld_4.Location = new Point(140, 570);
voorbeeld_4.Size = new Size(100, 100);
scherm.Controls.Add(voorbeeld_4);
voorbeeld_4.Text = "Voorbeeld 4:";

// aanmaken bitmap en grootte bepalen
Bitmap plaatje = new Bitmap(720, 720);
int bitmapHoogte = 720;
int bitmapBreedte = 720;
Label mandel = new Label();
mandel.Size = new Size(720, 720);
mandel.Location = new Point(440, 20);
scherm.Controls.Add(mandel);
mandel.Image = plaatje;

//globale waardes bij opstart
invoerMidden_x.Text = "-0,7";
invoerMidden_y.Text = "0";
invoerSchaal.Text = "0,7";
invoerMaxAantal.Text = "50";
invoerZoomFactor.Text = "2";

invoerschuifrood.Value = 127;
invoerschuifgroen.Value = 127;
invoerschuifblauw.Value = 127;

// globale minimale en maximale waardes
double min_x = -2;
double max_x = 2;
double min_y = -2;
double max_y = 2;


// berekening mandelbrotgetal
int mandelbrotgetal(double x, double y)
{
    double afstand = 0;
    int teller = 0;
    double maxAantalHerhalingen = double.Parse(invoerMaxAantal.Text);

    double a = 0;
    double b = 0;

    while (afstand < 2 && teller < maxAantalHerhalingen)
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


//kleurtjes!
Color[] kleuren = new Color[]
{
    Color.Blue,
    Color.Green,
    Color.Yellow,
    Color.Orange,
    Color.Red,
    Color.Purple
};
Color InterpoleerKleur(int iteratie, int maxIteraties)
{
    double t = (double)iteratie / double.Parse(invoerMaxAantal.Text);
    int index1 = (int)(t * (kleuren.Length - 1));
    int index2 = Math.Min(index1 + 1, kleuren.Length - 1);

    double fractie = t * (kleuren.Length - 1) - index1;

    int rood = invoerschuifrood.Value;
    int groen = invoerschuifgroen.Value;
    int blauw = invoerschuifblauw.Value;

    int r = Math.Min(255, kleuren[index1].R + (int)(fractie * (kleuren[index2].R - kleuren[index1].R)) + invoerschuifrood.Value);
    int g = Math.Min(255, kleuren[index1].G + (int)(fractie * (kleuren[index2].G - kleuren[index1].G)) + invoerschuifgroen.Value);
    int b = Math.Min(255, kleuren[index1].B + (int)(fractie * (kleuren[index2].B - kleuren[index1].B)) + invoerschuifblauw.Value);


    return Color.FromArgb(r, g, b);
}


void boxVeranderd(object sender, EventArgs ea)
{
    try
    {
        double.Parse(invoerMidden_x.Text); invoerMidden_x.BackColor = Color.White;
        double.Parse(invoerMidden_y.Text); invoerMidden_y.BackColor = Color.White;
        double.Parse(invoerMaxAantal.Text); invoerMaxAantal.BackColor = Color.White;
        double.Parse(invoerZoomFactor.Text); invoerZoomFactor.BackColor = Color.White;
        double.Parse(invoerSchaal.Text); invoerSchaal.BackColor = Color.White;
        foutmelding.Text = "";
        mandel.Invalidate();
    }
    catch (Exception e)
    {
        ((TextBox)sender).BackColor = Color.FromArgb(220, 20, 60);
        foutmelding.Text = "Verkeerde input";
    }
    ;
}


//berekening mandelbrot en pixelkleuren
void mandelbrotplaatje(object o, EventArgs e)
{
    int maxAantalHerhalingen = int.Parse(invoerMaxAantal.Text);

    double midden_x = Double.Parse(invoerMidden_x.Text);
    double midden_y = Double.Parse(invoerMidden_y.Text);
    double schaal = Double.Parse(invoerSchaal.Text);

    min_x = midden_x - (2 * schaal);
    max_x = midden_x + (2 * schaal);
    min_y = midden_y - (2 * schaal);
    max_y = midden_y + (2 * schaal);

    for (int pixel_x = 0; pixel_x < bitmapBreedte; pixel_x++)
    {

        for (int pixel_y = 0; pixel_y < bitmapHoogte; pixel_y++)
        {

            double x = min_x + (pixel_x / (double)bitmapBreedte) * (max_x - min_x);
            double y = min_y + (pixel_y / (double)bitmapHoogte) * (max_y - min_y);

            int uitkomst = mandelbrotgetal(x, y);

            Color kleur = InterpoleerKleur(uitkomst, maxAantalHerhalingen);
            plaatje.SetPixel(pixel_x, pixel_y, kleur);

        }

        void veranderschuif(object o, EventArgs ea)
        {
            mandelbrotplaatje(o, ea);
            invoerschuifrood.Scroll += veranderschuif;
            invoerschuifgroen.Scroll += veranderschuif;
            invoerschuifblauw.Scroll += veranderschuif;
        }

    }
    mandel.Invalidate();

}
;

// daadwerkelijke tekening op bitmap
void tekening(object o, PaintEventArgs pea)
{
    pea.Graphics.DrawImage(plaatje, 440, 20);
}

//voorbeeldplaatjes waardes
void voorbeeld_1_tekenen(object o, EventArgs ea)
{
    invoerMidden_x.Text = "-0,7";
    invoerMidden_y.Text = "0";
    invoerSchaal.Text = "0,7";
    invoerMaxAantal.Text = "50";
    invoerZoomFactor.Text = "2";

    invoerschuifrood.Value = 127;
    invoerschuifgroen.Value = 127;
    invoerschuifblauw.Value = 127;

    mandelbrotplaatje(o, ea);
}

void voorbeeld_2_tekenen(object o, EventArgs ea)
{
    invoerMidden_x.Text = "-0,737458";
    invoerMidden_y.Text = "0,208322";
    invoerSchaal.Text = "0,000833";
    invoerMaxAantal.Text = "750";
    invoerZoomFactor.Text = "2";

    invoerschuifrood.Value = 0;
    invoerschuifgroen.Value = 95;
    invoerschuifblauw.Value = 50;

    mandelbrotplaatje(o, ea);
}

void voorbeeld_3_tekenen(object o, EventArgs ea)
{
    invoerMidden_x.Text = "-0,561798";
    invoerMidden_y.Text = "0,643213";
    invoerSchaal.Text = "0,007";
    invoerMaxAantal.Text = "200";
    invoerZoomFactor.Text = "2";

    invoerschuifrood.Value = 0;
    invoerschuifgroen.Value = 0;
    invoerschuifblauw.Value = 255;

    mandelbrotplaatje(o, ea);
}

void voorbeeld_4_tekenen(object o, EventArgs ea)
{
    invoerMidden_x.Text = "-1,374780";
    invoerMidden_y.Text = "0,014885";
    invoerSchaal.Text = "0,000875";
    invoerMaxAantal.Text = "300";
    invoerZoomFactor.Text = "2";

    invoerschuifrood.Value = 230;
    invoerschuifgroen.Value = 0;
    invoerschuifblauw.Value = 0;

    mandelbrotplaatje(o, ea);
}
//inzoomen
void zoom(object o, MouseEventArgs ea)
{

    if (ea.Button == MouseButtons.Left)
    {
        // fixen van offset veroorzaakt doordat je de bitmap tekent op (440,20)
        int offsetx = 0;
        int offsety = 0;

        double zoom_x = min_x + ((ea.X - offsetx) / (double)bitmapBreedte) * (max_x - min_x);
        double zoom_y = min_y + ((ea.Y - offsety) / (double)bitmapHoogte) * (max_y - min_y);

        double zoomFactorInzoomen = 1 / double.Parse(invoerZoomFactor.Text);

        double nieuw_x = (max_x - min_x) * zoomFactorInzoomen;
        double nieuw_y = (max_y - min_y) * zoomFactorInzoomen;

        // nieuw bereik aanmaken voor zoom
        min_x = zoom_x - nieuw_x / 2;
        max_x = zoom_x + nieuw_x / 2;
        min_y = zoom_y - nieuw_y / 2;
        max_y = zoom_y + nieuw_y / 2;

        //meeveranderen van waarden van midden x en y met 6 decimalen
        double nieuwmidden_x = (min_x + max_x) / 2;
        double nieuwmidden_y = (min_y + max_y) / 2;

        invoerMidden_x.Text = nieuwmidden_x.ToString("F6");
        invoerMidden_y.Text = nieuwmidden_y.ToString("F6");

        // nieuw invoer maken zodat click meerdere keer werkt
        double nieuweSchaal = (max_x - min_x) / 4;
        invoerSchaal.Text = nieuweSchaal.ToString("F6");

    }
    mandelbrotplaatje(o, ea);
}

//uitzoomen
void zoomOut(object o, MouseEventArgs ea)
{

    if (ea.Button == MouseButtons.Right)
    {
        // fixen van offset veroorzaakt doordat je de bitmap tekent op (440,20)
        int offsetx = 0;
        int offsety = 0;

        double zoom_x = min_x + ((ea.X - offsetx) / (double)bitmapBreedte) * (max_x - min_x);
        double zoom_y = min_y + ((ea.Y - offsety) / (double)bitmapHoogte) * (max_y - min_y);

        double zoomFactorUitzoomen = 1 / Double.Parse(invoerZoomFactor.Text);

        double nieuw_x = (max_x - min_x) / zoomFactorUitzoomen;
        double nieuw_y = (max_y - min_y) / zoomFactorUitzoomen;

        // nieuw bereik aanmaken voor zoom
        min_x = zoom_x - nieuw_x / 2;
        max_x = zoom_x + nieuw_x / 2;
        min_y = zoom_y - nieuw_y / 2;
        max_y = zoom_y + nieuw_y / 2;

        //meeveranderen van waarden van midden x en y met 6 decimalen
        double nieuwmidden_x = (min_x + max_x) / 2;
        double nieuwmidden_y = (min_y + max_y) / 2;

        invoerMidden_x.Text = nieuwmidden_x.ToString("F6");
        invoerMidden_y.Text = nieuwmidden_y.ToString("F6");

        double nieuweSchaal = (max_x - min_x) / 4;
        invoerSchaal.Text = nieuweSchaal.ToString("F6");

    }
    mandelbrotplaatje(o, ea);
}



// exception catch voor verandering kleur
invoerMidden_x.TextChanged += boxVeranderd;
invoerMidden_y.TextChanged += boxVeranderd;
invoerMaxAantal.TextChanged += boxVeranderd;
invoerZoomFactor.TextChanged += boxVeranderd;
invoerSchaal.TextChanged += boxVeranderd;

// exception catch geen waarde

//aanroep functies
mandel.MouseClick += zoom;
mandel.MouseClick += zoomOut;
scherm.Paint += tekening;
tekenknop.Click += mandelbrotplaatje;

voorbeeld_1.Click += voorbeeld_1_tekenen;
voorbeeld_2.Click += voorbeeld_2_tekenen;
voorbeeld_3.Click += voorbeeld_3_tekenen;
voorbeeld_4.Click += voorbeeld_4_tekenen;

mandelbrotplaatje(null, EventArgs.Empty);

Application.Run(scherm);
