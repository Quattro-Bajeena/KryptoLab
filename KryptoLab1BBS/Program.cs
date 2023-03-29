using KryptoLab1BBS;

var generator = new BBS();
var sequence = generator.GenerateBits(100);
Console.WriteLine(String.Join("", sequence));