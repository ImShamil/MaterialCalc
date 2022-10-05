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

        struct Assemble
        {
            string name;
            int count;
            List <Assemble> assemblesList;
            List <Part> partList;
           
            public Assemble (string name, int count)
            {
                if (name=="")
                {
                    name = "Сборка";
                }

                if (count==0)
                {
                    count =1;
                }

                this.name = name;
                this.count = count;
                assemblesList=new List<Assemble>(); //здесь лежат подсборки
                partList=new List<Part>();//здесь лежат детали
            }

            public string GetName()
            {
                return this.name;
            }

            public void AddAssemble (Assemble assemble)
            {
                assemblesList.Add(assemble);
            }

            public void ShowAssebleList()
            {
                foreach (Assemble one in assemblesList)
                    {
                        Console.WriteLine(one.GetName());
                    }
            }

             public void AddPart (Part part)
            {
                partList.Add(part);
            }

            public void ShowPartList()
            {
                foreach (Part one in partList)
                    {
                       string tableRow = TableRow(one.GetName(), one.GetMaterial(), Convert.ToString(one.GetParam()));
                       Console.WriteLine(tableRow);
                    }
            }
           
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
            //List <Part> partList=new List<Part>();
            Console.Write("Название изделия:");
            string assembleName=Console.ReadLine();
            
            Console.Write("Кол-во изделий:");
            int totalCount=Convert.ToInt32(Console.ReadLine());
           
            Assemble assemble=new Assemble(assembleName,totalCount);

            Console.Write("Кол-во подсборок в узле:");
            int subAssembleCount=Convert.ToInt32(Console.ReadLine());
            for (int i=1; i <= subAssembleCount; i++)
            {
                Assemble subAssemble = new Assemble($"Подсборка_{i}",i);
                assemble.AddAssemble(subAssemble);
            }
      
            Console.Write("Кол-во деталей в основной сборке:");
            int partCount=Convert.ToInt32(Console.ReadLine());
            for (int i=1; i <= subAssembleCount; i++)
            {   
                Console.Write("Введите название детали:");
                string name = Console.ReadLine();
                Console.Write("Введите название материала:");
                string material = Console.ReadLine();
                Console.Write("Введите контролируемый параметр:");
                string param = Console.ReadLine();

                if (param.Contains('.'))
                {
                   param = param.Replace('.', ',');
                }
                Part part = new Part(name, material,param);
                assemble.AddPart(part);
            }

            while (true)
            {
                Console.Clear();
                string tableRow=TableRow("Название", "Материал", "Параметр");
                Console.WriteLine(tableRow);
                    for (int i=0; i < tableRow.Length; i++)
                    {
                        Console.Write('_');
                    }
                    Console.Write('\n');

                   //assemble.ShowAssebleList();
                   assemble.ShowPartList();
            }
            
            
        }
    }
}
