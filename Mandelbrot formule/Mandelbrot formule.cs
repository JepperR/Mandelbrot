using System;
using System.ComponentModel;
using System.Globalization;

Console.WriteLine("Wat is de waarde van x?");
string x1 = Console.ReadLine();
double x = double.Parse(x1, CultureInfo.InvariantCulture);

Console.WriteLine("Wat is de waarde van y?");
string y1 = Console.ReadLine();
double y = double.Parse(y1, CultureInfo.InvariantCulture);

double mandelbrotgetal(double x, double y)
{
    double afstand = 0;
    double teller = 0;
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

Console.WriteLine($"Het mandelbrot getal is {mandelbrotgetal(x, y)}");

