using System;
using System.IO;
using Anamorphing.Math;
namespace Anamorphing
{
    public class Program
    {
        static private void applySigmoida(ref double[] Pokazateli)
        {
            double min = Pokazateli[0];
            double max = Pokazateli[0];
            for (int i = 0; i < Pokazateli.Length; i++)
            {
                if (Pokazateli[i] > max)
                    max = Pokazateli[i];
                if (Pokazateli[i] < min)
                    min = Pokazateli[i];
            }
            for (int i = 0; i < Pokazateli.Length; i++)
            {
                Console.WriteLine("Sigmoid -" + i + " " + sigmoidFunction(Pokazateli[i], max, min));
                Pokazateli[i] = sigmoidFunction(Pokazateli[i], max, min);
            }
        }

        static private double sigmoidFunction(double x, double max, double min)
        {

            return x / (max - min);
            return 1 / (1 + System.Math.Pow(2.71, (-1) * x + 1306241));
        }
        struct Point
        {
            public int x;
            public int y;
        }

        static public void createGeoFile()
        {
            int row = 120;
            int column = 50;
            string result = "{\n \"features\": [\n";
            Point ZeroPoint;
            ZeroPoint.x = 100;
            ZeroPoint.y = 100;
            for (int i = 0; i < column; i++)
            {
                for (int j = 0; j < row; j++)
                {
                    result += "{\"geometry\": {\n \"geometries\": [{\n \"coordinates\": [\n [";
                    result += "[" + ZeroPoint.x + "," + ZeroPoint.y + "],\n";
                    result += "[" + ZeroPoint.x + "," + (ZeroPoint.y + 15) + "],\n";
                    result += "[" + (ZeroPoint.x + 15) + "," + (ZeroPoint.y + 15) + "],\n";
                    result += "[" + (ZeroPoint.x + 15) + "," + ZeroPoint.y + "],\n";
                    result += "[" + ZeroPoint.x + "," + ZeroPoint.y + "]\n";
                    result += "]\n],\n \"type\": \"Polygon\"\n 	}],\n \"type\": \"GeometryCollection\"	\n	},\n\"type\": \"Feature\",\"properties\": {}\n},";
                    ZeroPoint.x += 15;

                }
                ZeroPoint.y += 15;
                ZeroPoint.x = 0;
            }
            result += "], \n\"type\": \"FeatureCollection\",\n \"properties\": {} \n}";
            File.WriteAllText(Guid.NewGuid().ToString() + ".json", result);
        }

        public static void Main(string[] args)
        {
            // Stream str = File.Open(@"Data/simplerussiaedited.geojson",FileMode.Open);

            Console.WriteLine("Hello World!");
            double[] MugNaselenieRUS = {
               439870,389031,335521,225537,348986,511046,206930,
230696,406854,462816,345477,438769,317193,222098,2011010,256276,369355,324062,
501918,416305,1058458,463929,222593,273171,389833,431473,697118,365705,361835,355215,305260,813999,999998,442259,
809895,423885,1101489,792223,86823,1306241,126790,658738,251431,200028,123244,305113,307581,657679,864437,1384062,
1091902,1182857,480435,800567,900254,827430,637604,334153,1080921,58332,943050,801674,371064,305252,305252,168089,
714674,493436,301921,139588,73206,194426,23226,304265,62708,309760,
439870,389031,335521,225537,348986,511046,206930,194426,23226,304265,62708};

            double[] NaselenieAFG ={
                819500,884700,522100,1191600,411200,581800,1092000,1762157,859600,
680200,417300,512100,320100,3528000,1070200,399500,417300,953800,
441000,409900,1520100,154900,132000,438900,550400,144400,629500,
345100,514500,930000,342400,488100,948200,545800
            };
            // applySigmoida(ref MugNaselenieRUS);
            // applySigmoida(ref NaselenieAFG);


            string AFG = File.ReadAllText(@"Data/double.txt");
            var AFGsplit = AFG.Split(',');
            double[] Prohod = new double[AFGsplit.Length];
            for (int i = 0; i < AFGsplit.Length; i++)
            {
                Prohod[i] = Convert.ToDouble(AFGsplit[i]);
            }
            //createGeoFile();
            //            applySigmoida(ref Prohod);
               File.WriteAllText(Guid.NewGuid().ToString()+".json",Anamorph.MorphVBA(File.ReadAllText(@"squares2.json"), Prohod, 0.1));
        }
    }
}
