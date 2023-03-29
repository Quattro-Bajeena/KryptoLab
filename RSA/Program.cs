using MathUtils;
using RSA;

(PublicKey pubKey, PrivateKey privKey) = RSA.RSA.GenerateKeys(1423, 8641, 32);
Console.WriteLine(pubKey.e + " " + pubKey.n);
Console.WriteLine(privKey.d + " " + privKey.n);
