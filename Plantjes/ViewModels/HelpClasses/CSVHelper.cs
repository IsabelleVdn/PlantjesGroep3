﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using Plantjes.Models.Db;
using Plantjes.Models.Extensions;

namespace Plantjes.ViewModels.HelpClasses
{
    internal class CSVHelper
    {
        /// <summary>
        /// Converts the read .csv into a list of Gebruikers.
        /// </summary>
        /// <param name="csvLocation">Location of the .csv file.</param>
        /// <returns>A list of Gebruikers</returns>
        public static IList<Gebruiker> CSVToMemberList(string csvLocation)
        {
            // Written by Ian Dumalin on 27/04
            List<Gebruiker> valueList = new List<Gebruiker>();
            using (StreamReader reader = new StreamReader(csvLocation))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    string[] readValues = line.Split(';');
                    if (readValues[0] != "Studentennummer")
                    {
                        valueList.Add(new Gebruiker() { Vivesnr = readValues[0], Voornaam = readValues[1], Achternaam = readValues[2], Emailadres = readValues[5], HashPaswoord = Helper.HashString(readValues[0]) });
                    }
                }
            }
            return valueList;
        }

        public static void ExportPlantsToCSV(IEnumerable<Plant> plantList)
        {
            if (plantList == null || plantList.Count() == 0)
                return;
            IEnumerable<string> lines = plantList.Select(p => p.PlantToString());
            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "CSV file(*.csv)|*.csv";
            string output = $"Type;Familie;Geslacht;Soort;Variant;Nederlandse naam\r\n";
            if (save.ShowDialog() ?? false)
            {
                foreach (string s in lines)
                {
                    output += $"{s}\r\n";
                }
                File.WriteAllText(save.FileName, output);
            }
        }

        public static void RegisterMemberToCSV(string csvLocation, string email, string rNumber, string password)
        {
            using (FileStream fileStream = new FileStream(csvLocation, FileMode.Append, FileAccess.ReadWrite))
            {
                using (StreamWriter writer = new StreamWriter(fileStream))
                {
                    writer.WriteLine($"{email};{rNumber};{password}");
                }
            }
        }
    }
}
