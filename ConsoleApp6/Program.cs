using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaterialCalc
{
    internal class Program
    {
        public static string TableRow(string cellName, string cellMaterial, string cellParam)
        {
            string tableRow = string.Format("|{0,20}|{1,20}|{2,20}|", cellName, cellMaterial, cellParam);
            return tableRow;
        }

        struct Part
        {
            string name;
            string material_name;
            double param;

 
            public Part(string name, string material,string param)
            {
                if (name=="")
                {
                    name = "Деталь";
                }

                if (material == "")
                {
                    material = "Ст3";
                }
                if (param == "")
                {
                    param = "0";
                }

                this.name = name.ToLower();
                this.material_name = material.ToLower();
                this.param =Convert.ToDouble( param);
            }

            public void Print()
            {
                Console.WriteLine("|{0,20}|{1,20}|{2,20}|", this.name, this.material_name, this.param);

            }

            public string GetName()
            {
                return this.name;
            }

            public string GetMaterial()
            {
                return this.material_name;
            }

            public double GetParam()
            {
                return this.param;
            }
        };

        static void Main(string[] args)
        {  
            List<Part> partList = new List<Part>();
            while (true)
            {

                Console.Clear();
                if (partList.Count > 0)
                {
                    string tableRow=TableRow("Название", "Материал", "Параметр");
                    Console.WriteLine(tableRow);
                    for (int i=0; i < tableRow.Length; i++)
                    {
                        Console.Write('_');
                    }
                    Console.Write('\n');
                    foreach (Part one in partList)
                    {
                        tableRow = TableRow(one.GetName(), one.GetMaterial(), Convert.ToString(one.GetParam()));
                        Console.WriteLine(tableRow);
                    }
                    Console.Write('\n');
                }

                Console.Write("Введите название детали:");
                string name = Console.ReadLine();
                Console.Write('\n');
                Console.Write("Введите название материала:");
                string material = Console.ReadLine();
                Console.Write('\n');
                Console.Write("Введите контролируемый параметр:");
                string param = Console.ReadLine();

                if (param.Contains('.'))
                {
                   param = param.Replace('.', ',');
                }

                Part part = new Part(name, material,param);
                partList.Add(part);
               
            }
           

        }
    }
}
