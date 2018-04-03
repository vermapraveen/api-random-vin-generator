using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Globalization;

namespace RadomVinGenerator
{
    [Route("api/[controller]")]
    public class VinGeneratorController : Controller
    {
        private readonly Dictionary<char, int> _vinDigitValues = new Dictionary<char, int>();
        readonly int[] _vinDigitPositionMultiplier = { 8, 7, 6, 5, 4, 3, 2, 10, 0, 9, 8, 7, 6, 5, 4, 3, 2 };
        readonly Random _random = new Random();

        public VinGeneratorController()
        {
            _vinDigitValues.Add('A', 1);
            _vinDigitValues.Add('B', 2);
            _vinDigitValues.Add('C', 3);
            _vinDigitValues.Add('D', 4);
            _vinDigitValues.Add('E', 5);
            _vinDigitValues.Add('F', 6);
            _vinDigitValues.Add('G', 7);
            _vinDigitValues.Add('H', 8);
            _vinDigitValues.Add('J', 1);
            _vinDigitValues.Add('K', 2);
            _vinDigitValues.Add('L', 3);
            _vinDigitValues.Add('M', 4);
            _vinDigitValues.Add('N', 5);
            _vinDigitValues.Add('P', 7);
            _vinDigitValues.Add('Q', 8);
            _vinDigitValues.Add('R', 9);
            _vinDigitValues.Add('S', 2);
            _vinDigitValues.Add('T', 3);
            _vinDigitValues.Add('U', 4);
            _vinDigitValues.Add('V', 5);
            _vinDigitValues.Add('W', 6);
            _vinDigitValues.Add('X', 7);
            _vinDigitValues.Add('Y', 8);
            _vinDigitValues.Add('Z', 9);
            _vinDigitValues.Add('1', 1);
            _vinDigitValues.Add('2', 2);
            _vinDigitValues.Add('3', 3);
            _vinDigitValues.Add('4', 4);
            _vinDigitValues.Add('5', 5);
            _vinDigitValues.Add('6', 6);
            _vinDigitValues.Add('7', 7);
            _vinDigitValues.Add('8', 8);
            _vinDigitValues.Add('9', 9);
            _vinDigitValues.Add('0', 0);
        }

        [HttpGet]
        public IActionResult Get()
        {
            var vinYear = GetRandomVinStart();
            var vinChar = GetRandomVinChar();

            var sampleVin = vinYear.Item1 + vinChar + vinYear.Item2;

            for (var i = 0; i < 7; i++)
            {
                sampleVin += GetRandomVinChar();
            }

            var checkChar = GetCheckSumChar(sampleVin);
            return Ok(sampleVin.Substring(0, 8) + checkChar + sampleVin.Substring(9, 8));
        }

        private string GetCheckSumChar(string sampleVin)
        {
            if (sampleVin.Length < 17)
                return "";

            var checkSumTotal = sampleVin.Select((t, i) => _vinDigitValues[t] * _vinDigitPositionMultiplier[i]).Sum();
            var remain = checkSumTotal % 11;

            var checkSumchar = remain.ToString(CultureInfo.InvariantCulture);
            if (remain == 10)
                checkSumchar = "X";

            return checkSumchar;
        }

        private Tuple<string, string> GetRandomVinStart()
        {
            var lineToRead = _random.Next(1, 62177);
            var prefixFilePath = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), @"data/VinPrefixes.txt");

            var line = System.IO.File.ReadLines(prefixFilePath).Skip(lineToRead).Take(1).First();
            var splits = line.Split(new[] { "   " }, StringSplitOptions.None);

            return new Tuple<string, string>(splits[0].Trim(' '), splits[1].Trim(' '));
        }

        private char GetRandomVinChar()
        {
            var lineToRead = _random.Next(0, _vinDigitValues.Count() - 1);
            return _vinDigitValues.Keys.ToArray()[lineToRead];
        }
    }
}