using System;
class Program {
	static void Main() {
		double a = double.Parse(Console.ReadLine());
		double b = double.Parse(Console.ReadLine());
		double c = double.Parse(Console.ReadLine());

		double raizPositiva = (-b+(Math.Sqrt(Math.Pow(b,2)-4*a*c)))/(2*a);
		double raizNegativa = (-b-(Math.Sqrt(Math.Pow(b,2)-4*a*c)))/(2*a);

		if(raizNegativa.ToString() == "NaN" || raizPositiva.ToString() == "NaN") {
			Console.WriteLine("no real roots");
		}else if(raizPositiva != raizNegativa) {
			Console.WriteLine("{0:0.00}",raizNegativa);
			Console.WriteLine("{0:0.00}",raizPositiva);
		}else {
			Console.WriteLine("{0:0.00}",raizPositiva);
		}
	}
}
