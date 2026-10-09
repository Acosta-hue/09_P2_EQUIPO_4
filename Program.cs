using System.Text;

Console.OutputEncoding = Encoding.UTF8;

Console.WriteLine("=== Comparación de bombas eléctricas ===");

// Objetos: dos bombas creadas a partir de la clase Bomba
Bomba bomba1 = Bomba.Capturar(1);
Bomba bomba2 = Bomba.Capturar(2);

bomba1.Mostrar();
bomba2.Mostrar();

// Comparación de indicadores (con tolerancia por redondeo de decimales)
Console.WriteLine("\n--- Resultado ---");
double dif = bomba1.Indicador - bomba2.Indicador;
if (Math.Abs(dif) < 1e-9)
    Console.WriteLine("Existe EMPATE: ambas bombas entregan el mismo caudal por watt.");
else if (dif > 0)
    Console.WriteLine($"La bomba {bomba1.Id} entrega más caudal por watt.");
else
    Console.WriteLine($"La bomba {bomba2.Id} entrega más caudal por watt.");

// ===== Clase: Bomba =====
class Bomba
{
    // Propiedades
    public string Id { get; }          // string: identificador
    public double Voltaje { get; }     // double: V
    public double Corriente { get; }   // double: A
    public double Caudal { get; }      // double: L/min
    public double Potencia => Voltaje * Corriente;                       // double: W (P = V*I)
    public double Indicador => Potencia > 0 ? Caudal / Potencia : 0.0;   // double: (L/min)/W, sin dividir entre cero

    public Bomba(string id, double voltaje, double corriente, double caudal)
    {
        Id = id; Voltaje = voltaje; Corriente = corriente; Caudal = caudal;
    }

    // Método estático: captura los datos de una bomba (numero: 1 o 2)
    public static Bomba Capturar(int numero)
    {
        Console.WriteLine($"\nDatos de la bomba {numero}:");
        string id = Entrada.LeerTexto("  Identificador: ");
        double v = Entrada.LeerDouble("  Voltaje (V): ", 0, true);
        double i = Entrada.LeerDouble("  Corriente (A): ", 0, true);
        double q = Entrada.LeerDouble("  Caudal (L/min): ", 0, false);
        return new Bomba(id, v, i, q);
    }

    // Método: muestra potencia e indicador
    public void Mostrar()
    {
        Console.WriteLine($"\nBomba {Id}: P = {Potencia:F2} W | Indicador = {Indicador:F4} (L/min)/W");
    }
}

// ===== Clase auxiliar: lectura segura de datos =====
static class Entrada
{
    // Lee un número decimal; repite mientras el dato no sea válido o no cumpla el mínimo
    public static double LeerDouble(string mensaje, double minimo, bool estricto)
    {
        while (true)
        {
            Console.Write(mensaje);
            string? texto = Console.ReadLine();
            if (double.TryParse((texto ?? "").Trim().Replace(',', '.'),
                System.Globalization.NumberStyles.Float,
                System.Globalization.CultureInfo.InvariantCulture, out double v)
                && (estricto ? v > minimo : v >= minimo))
                return v;
            Console.WriteLine(estricto ? $"  Valor inválido: debe ser un número mayor que {minimo}."
                                       : $"  Valor inválido: debe ser un número mayor o igual que {minimo}.");
        }
    }

    // Lee un entero dentro de un rango; repite mientras sea inválido
    public static int LeerEntero(string mensaje, int minimo, int maximo)
    {
        while (true)
        {
            Console.Write(mensaje);
            if (int.TryParse(Console.ReadLine()?.Trim(), out int v) && v >= minimo && v <= maximo)
                return v;
            Console.WriteLine($"  Valor inválido: debe ser un entero entre {minimo} y {maximo}.");
        }
    }

    // Lee texto no vacío
    public static string LeerTexto(string mensaje)
    {
        while (true)
        {
            Console.Write(mensaje);
            string? t = Console.ReadLine()?.Trim();
            if (!string.IsNullOrEmpty(t)) return t;
            Console.WriteLine("  El texto no puede estar vacío.");
        }
    }
}
