Console.WriteLine("RSA Example");


var p = GeneratePrimaryNumber();
var q = GeneratePrimaryNumber();


Console.WriteLine($"p={p} \t q={q}");

var n = p * q;

var phi = CalculatePhi(p, q);
var e = GenerateRelativePrimeNumberOf(phi);

Console.WriteLine($"n={n} \t e={e} \t phi={phi}");

ExtendedGCD(phi, e, out int x, out int y);

Console.WriteLine($"x={x} \t y={y}");

var d = y + phi;

Console.WriteLine("d=" + d);

Console.WriteLine("enter number to encript");
string? inputStr = Console.ReadLine();
if (string.IsNullOrWhiteSpace(inputStr))
{
    Console.WriteLine("Invalid input. Exiting.");
    return;
}
int input = int.Parse(inputStr);

int encript = (int)FastModularExponentiation(input, e, n);

Console.WriteLine("Encripted");
Console.WriteLine(encript);

int decripted = (int)FastModularExponentiation(encript, d, n);

Console.WriteLine("Decript");
Console.WriteLine(decripted);
Console.ReadLine();

/// <summary>
/// Computes the Greatest Common Divisor (GCD) of two integers using the Extended Euclidean Algorithm.
/// Also calculates coefficients x and y such that: a*x + b*y = gcd(a, b) (Bézout's identity).
/// </summary>
/// <param name="a">First integer</param>
/// <param name="b">Second integer</param>
/// <param name="x">Coefficient for a in the linear combination</param>
/// <param name="y">Coefficient for b in the linear combination</param>
/// <returns>The GCD of a and b</returns>
int ExtendedGCD(int a, int b, out int x, out int y)
{
    if (a == 0)
    {
        x = 0;
        y = 1;
        return b;
    }

    int x1, y1;
    int gcd = ExtendedGCD(b % a, a, out x1, out y1);

    x = y1 - (b / a) * x1;
    y = x1;

    return gcd;
}

/// <summary>
/// Computes modular exponentiation using the fast binary exponentiation algorithm (exponentiation by squaring).
/// Calculates (baseNum^exponent) % mod efficiently without computing the full power first.
/// </summary>
/// <param name="baseNum">The base number to be raised to a power</param>
/// <param name="exponent">The exponent (power)</param>
/// <param name="mod">The modulus for the operation</param>
/// <returns>The result of (baseNum^exponent) mod mod</returns>
static long FastModularExponentiation(long baseNum, long exponent, long mod)
{
    long result = 1;
    baseNum %= mod; // Reduce base modulo mod

    while (exponent > 0)
    {
        if ((exponent & 1) == 1)  // If exponent is odd, multiply result by base
            result = (result * baseNum) % mod;

        baseNum = (baseNum * baseNum) % mod; // Square base
        exponent >>= 1; // Divide exponent by 2
    }

    return result;
}



int GeneratePrimaryNumber()
{

    int[] primes = [2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 37, 41, 43, 47, 53, 59, 61, 67, 71];

    var rnd = new Random();
    var inx = rnd.Next(3, 10);

    return primes[inx];

}

/// <summary>
/// Generates a number that is relatively prime (coprime) to the given number.
/// A number is relatively prime to another if their greatest common divisor (GCD) is 1.
/// This is used to find the public exponent 'e' in RSA encryption.
/// </summary>
/// <param name="num">The number to find a coprime for (typically Euler's totient phi)</param>
/// <returns>A positive integer that is relatively prime to num</returns>
int GenerateRelativePrimeNumberOf(int num)
{
    var rnd = new Random();
    var candidate = rnd.Next(10);
    do
    {
        candidate++;
        var gdc = FindGCD(num, candidate);
        if (gdc == 1) break;
    } while (true);

    return candidate;

}

int FindGCD(int a, int b)
{
    while (b != 0)
    {
        int temp = b;
        b = a % b;
        a = temp;
    }
    return a;
}


int CalculatePhi(int p, int q)
{
    return (p - 1) * (q - 1);
}

