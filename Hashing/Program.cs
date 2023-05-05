using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using System;

using SHA3.Net;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using static System.Reflection.Metadata.BlobBuilder;

namespace Hashing
{
	internal class Program
	{
		static void Main(string[] args)
		{
			if(args.Length > 0)
			{
				PrintHashes(args[0]);
			}
			else
			{
				//MeasureTimes();
				//ShortWordHash();
				//FindCollisions();
				MeasureSAC();
				
			}
			
		}

		const string filesDir = "test_files";
		
		static void PrintHashes(string word)
		{
			var word_bytes = Encoding.ASCII.GetBytes(word);
			using var md5 = MD5.Create();
			using var sha1 = SHA1.Create();
			using var sha3 = Sha3.Sha3384();

			var md5_hash = md5.ComputeHash(word_bytes);
			var sha1_hash = sha1.ComputeHash(word_bytes);
			var sha3_hash = sha3.ComputeHash(word_bytes);

			Console.WriteLine("Hashed value: " + word);
			Console.WriteLine("MD5:   " + Convert.ToHexString(md5_hash));
			Console.WriteLine("SHA-1: " + Convert.ToHexString(sha1_hash));
			Console.WriteLine("SHA-3: " + Convert.ToHexString(sha3_hash));

		}


		static void MeasureTimes()
		{
			var test_files = new DirectoryInfo(filesDir).GetFiles().OrderBy(f => f.Length);
			var sw = new Stopwatch();



			var times = new Dictionary<string, List<(string, long)>>() {
				{ "md5",  new () }, 
				{ "sha1", new () },
				{ "sha3", new () }
			};

			foreach (var file in test_files)
			{
				
				using var md5 = MD5.Create();
				using var sha1 = SHA1.Create();
				using var sha3 = Sha3.Sha3384();

				var file_description = "File: " + file.Name + " Size: " + file.Length;
				Console.WriteLine(file_description);

				using var fs1 = file.OpenRead();
				sw.Start();
				var md5_hash = md5.ComputeHash(fs1);
				times["md5"].Add( (file.Name, sw.ElapsedMilliseconds) );

				using var fs2 = file.OpenRead();
				sw.Restart();
				var sha1_hash = sha1.ComputeHash(fs2);
				times["sha1"].Add((file.Name, sw.ElapsedMilliseconds));

				using var fs3 = file.OpenRead();
				sw.Restart();
				var sha3_hash = sha3.ComputeHash(fs3);
				times["sha3"].Add((file.Name, sw.ElapsedMilliseconds));
			}

			foreach(var algo_time in times)
			{
				Console.WriteLine(algo_time.Key);
				foreach(var time in algo_time.Value)
				{
					Console.WriteLine(time.Item1 + ": " + time.Item2 + "ms");
				}
			}


		}

		static void ShortWordHash()
		{
			var word = "ares";
			using var md5 = MD5.Create();

			var hash = md5.ComputeHash(Encoding.ASCII.GetBytes(word));
			Console.WriteLine(Convert.ToHexString(hash));
		}

		static void FindCollisions()
		{
			using var md5 = MD5.Create();
			var no_hashes = 1_000;

			var randomStrings = Enumerable.Range(0, no_hashes).Select(i => Guid.NewGuid().ToString()).ToArray();
			var hashedStrings = randomStrings
				.Select(
					str => new BitArray(md5.ComputeHash(Encoding.UTF8.GetBytes(str)))
				)
				.ToArray();

			

			for (int i = 1; i < md5.HashSize; i++)
			{
				var hashedPrefixes = hashedStrings.Select(bits => GetFirstNBits(bits, i)).ToArray();

				var collisionsFound = hashedPrefixes.Length - hashedPrefixes.Distinct(new BitArrayEqualityComparer()).ToArray().Length;

				Console.WriteLine("Collisions on first " + i + " bits: " + collisionsFound);
				
			}
		}

		class BitArrayEqualityComparer : IEqualityComparer<BitArray>
		{
			public bool Equals(BitArray? x, BitArray? y)
			{
				if (x == null && y == null) return true;
				if (x == null || y == null) return false; 
				if (x.Length != y.Length) return false;	
				return x.Cast<bool>().SequenceEqual(y.Cast<bool>());
			}

			public int GetHashCode([DisallowNull] BitArray obj)
			{
				var bools = obj.Cast<bool>().ToArray();
				int hash = 17;
				for (int index = 0; index < bools.Length; index++)
				{
					hash = hash * 23 + bools[index].GetHashCode();
				}
				return hash;
			}
		}


		static BitArray GetFirstNBits(BitArray bits, int n)
		{
			var first_n = new bool[n];
			for(int i = 0; i < n; i++)
			{
				first_n[i] = bits.Get(i);
			}

			var output = new BitArray(first_n);
			return output;
		}

		static float SAC()
		{
			using var md5 = MD5.Create();
			var value = Guid.NewGuid().ToString();
			var value_bytes = Encoding.ASCII.GetBytes(value);
			var value_bits = new BitArray(value_bytes);

			var first_hash = md5.ComputeHash(value_bytes);
			var first_hash_bits = new BitArray(first_hash);


			var value_modified_bits = (BitArray) value_bits.Clone();
			value_modified_bits.Set(0, !value_modified_bits[0]);

			var value_modified_bytes = new byte[value_bytes.Length];
			value_modified_bits.CopyTo(value_modified_bytes, 0);

			var second_hash = md5.ComputeHash(value_modified_bytes);
			var second_hash_bits = new BitArray(second_hash);

			var hashes_xor = first_hash_bits.Xor(second_hash_bits);
			var changed_bits = 0;
			foreach(bool bit in hashes_xor)
			{
				changed_bits += bit ? 1 : 0;
			}

			var changed_bit_ratio = (float)changed_bits / first_hash_bits.Count;
			//Console.WriteLine(changed_bit_ratio);
			return changed_bit_ratio;

		}

		static void MeasureSAC()
		{
			var ratios = new List<float>();
			for (int i = 0; i < 1000000; i++)
			{
				ratios.Add(SAC());
			}
			Console.WriteLine("Min: " + ratios.Min());
			Console.WriteLine("Max: " + ratios.Max());
			Console.WriteLine("Average: " + ratios.Average());
		}
	}
}