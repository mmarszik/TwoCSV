using System;
using CsvHelper;
using CsvHelper.Configuration;
using System.Collections.Generic;

namespace TwoCSV
{
	class WorkDay {
		public string worker;
		public string date;
		public string begin;
		public string end;
		public string toString() {
			return worker + " " + date + " " + begin + " " + end;
		}
	}

	class MainClass {

		private static int findWorkDay( string worker, string date, List<WorkDay> workDays) {
			for( int i=0; i<workDays.Count; i++ ) {
				if( workDays[i].worker == worker && workDays[i].date == date ) {
					return i;
				}
			}
			Console.WriteLine("Error: " + worker + " " + date);
			return -1;
		}

		private static void readCsvFile( string path , ref List<WorkDay> workDays ) {
			var reader = new System.IO.StreamReader(path);
			var config = new CsvConfiguration(System.Globalization.CultureInfo.CurrentCulture) { Delimiter = ";" };
			var csv = new CsvReader(reader, config );
			while (csv.Read()) {
				WorkDay workDay = new WorkDay();
				workDay.worker = csv.GetField(0);
				workDay.date = csv.GetField(1);
				if (csv.GetField(3).ToLower() == "we" || csv.GetField(3).ToLower() == "wy" ) {
					if(csv.GetField(3).ToLower() == "we") {
						workDay.begin = csv.GetField(2);
						workDays.Add(workDay);
					} else if(csv.GetField(3).ToLower() == "wy") {
						int idx = findWorkDay(workDay.worker, workDay.date, workDays);
						workDays[idx].end = csv.GetField(2);
					} else {
						Console.WriteLine("error " + csv.GetField(3));
					}
				} else {
					workDay.begin = csv.GetField(2);
					workDay.end   = csv.GetField(3);
					workDays.Add(workDay);
				}
				workDay = null;
			}
		}

		private static string[] getCsvFiles( string dirPaht) {
            string ext = ".csv";
            List<string> csvFiles = new List<string>();
            string[] allFiles = System.IO.Directory.GetFiles(dirPaht);
            for (int i = 0; i < allFiles.Length; i++) {
                int fileLen = allFiles[i].Length;
                if (fileLen < ext.Length ) {
                    continue;
                }
                if( allFiles[i].Substring(fileLen - ext.Length).ToLower() != ext.ToLower()) {
                    continue;
                }
                csvFiles.Add(allFiles[i]);
            }
            return csvFiles.ToArray();
        }

        private static void printFilesList(string[] files) {
            for (int i = 0; i < files.Length; i++) {
				Console.WriteLine(files[i]);
			}
		}

		public static void Main(string[] args) {
            Console.WriteLine("Hello");
            string dirWithCsv;
            if( args.Length > 0) {
                dirWithCsv = args[0];
            } else {
				//dirWithCsv = ".";
				dirWithCsv = "/tmp/workcsv";
			}
			string[] csvFiles = getCsvFiles(dirWithCsv);
			printFilesList(csvFiles);
			List<WorkDay> workDays = new List<WorkDay>();
			for (int i = 0; i < csvFiles.Length; i++) {
				readCsvFile(csvFiles[i], ref workDays);
			}
			for( int i=0; i<workDays.Count; i++ ) {
				Console.WriteLine( workDays[i].toString() );
			}
		}
    }
}
