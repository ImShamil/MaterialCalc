using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaterialCalc
{
    internal class Program
    {
        const int stringSize = 30;
        public static string TableHead(string cellName, string cellMaterial, string cellParam, string cellQuantiti, string cellTotal)
        {
            string tableHead = ($"|{cellName,stringSize}|{cellMaterial,stringSize}|{cellParam,stringSize}|{cellQuantiti,stringSize}|{cellTotal,stringSize}|");
            return tableHead;
        }
        public static void TableContent(string cellName, string cellMaterial, string cellParam, string cellQuantiti, string cellTotal)
        {
            string tableContent = ($"|{cellName,-stringSize}|{cellMaterial,stringSize}|{cellParam,stringSize}|{cellQuantiti,stringSize}|{cellTotal,stringSize}|");
            Console.WriteLine(tableContent);
        }


        public struct Assemble
        {
            string name;
            int count;
            int totalCount;
            List<Assemble> assemblesList;
            List<Part> partList;

            public Assemble(string name, int count, int assembleCount = 1)
            {
                if (name == "")
                {
                    name = "Сборка";
                }

                if (count == 0)
                {
                    count = 1;
                }

                this.name = name;
                this.count = count;
                this.totalCount = count * assembleCount;
                assemblesList = new List<Assemble>(); //здесь лежат подсборки
                partList = new List<Part>();//здесь лежат детали
            }

            public string GetName()
            {
                return this.name;
            }

            public int GetCount()
            {
                return this.count;
            }

            public int GetTotalCount()
            {
                return this.totalCount;
            }

            public void AddAssemble(Assemble assemble)
            {
                assemblesList.Add(assemble);
            }

            public Assemble GetAssembleItem(int index)
            {
                return assemblesList[index];
            }

            public void AddPart(Part part)
            {
                partList.Add(part);
            }
            public List<Assemble> GetAssembleList()
            {
                return assemblesList;
            }
            public int GetAssebleListCount()
            {
                return assemblesList.Count;
            }
            public List<Part> GetPartList()
            {
                return partList;
            }
            public int GetPartListCount()
            {
                return partList.Count;
            }

        }

        public struct Part
        {
            string name;
            string material_name;
            double param;
            int quantity;
            int totalQuantity;
            double total;
            double totalForOne;

            public Part(string name, string material, string param, string quantity, int assembleQuantity)
            {
                if (name == "")
                {
                    name = "Деталь";
                }

                if (material == "")
                {
                    material = "Ст3";
                }
                if (param == "")
                {
                    param = "1";
                }
                if (quantity == "")
                {
                    quantity = "1";
                }

                this.name = name;
                this.material_name = material.ToLower();
                this.param = Convert.ToDouble(param);
                this.quantity = Convert.ToInt32(quantity);
                this.totalQuantity = this.quantity * assembleQuantity;
                this.total = this.param * Convert.ToDouble(totalQuantity);
                this.totalForOne = this.param * this.quantity;
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
            public int GetQuantiti()
            {
                return this.quantity;
            }
            public int GetTotalQuantiti()
            {
                return this.totalQuantity;
            }

            public double GetTotal()
            {
                return this.total;
            }

            public double GetTotalForOne()
            {
                return this.totalForOne;
            }
        };


        public static void showAssembles(Assemble assemble, int repeat = 0, string step = "")
        {
            string stepAssambleName = step + assemble.GetName();
            string quantityStringAssemble = $"{Convert.ToString(assemble.GetTotalCount())}" + $"({assemble.GetCount()})";
            TableContent(stepAssambleName, "", "", quantityStringAssemble, "");
            repeat++;
            string Newstep = new String(' ', repeat);
            foreach (Assemble assembleItem in assemble.GetAssembleList())
            {
                showAssembles(assembleItem, repeat, Newstep);
            }
            foreach (Part partItem in assemble.GetPartList())
            {
                string stepPartName = Newstep + partItem.GetName();
                string quantityString = $"{Convert.ToString(partItem.GetTotalQuantiti())}" + $"({Convert.ToString(partItem.GetQuantiti())})";
                string totalForOneString = $"{Convert.ToString(partItem.GetTotal())}" + $"({Convert.ToString(partItem.GetTotalForOne())})";
                TableContent(stepPartName, partItem.GetMaterial(), Convert.ToString(partItem.GetParam()), quantityString, totalForOneString);
            }
        }

        public static void ShowAllTable(Assemble assemble, Dictionary<string, double> finalList,int totalCount)
        {
            Console.Clear();
            string tableHead = TableHead("Название", "Материал", "Параметр", "Количество", "Всего");
            Console.WriteLine(tableHead);
            for (int i = 0; i < tableHead.Length; i++)
            {
                Console.Write('_');
            }
            Console.Write('\n');
            showAssembles(assemble);
            for (int i = 0; i < tableHead.Length; i++)
            {
                Console.Write('_');
            }
            Console.Write('\n');
            ShowFinalList(finalList,totalCount);
            Console.Write('\n');
        }


        public static void AssembleFill(Assemble assemble, Assemble assembleItem, Dictionary<string, double> finalList, int assembleTotalCount)
        { ShowAllTable(assemble,finalList,assembleTotalCount);
          
            Console.WriteLine("Находимся в узле {0}", assembleItem.GetName());
            Console.Write("Кол-во подсборок в узле:");
            int assembleCount = Convert.ToInt32(Console.ReadLine());

            for (int i = 1; i <= assembleCount; i++)
            {
                Console.Write($"Название подсборки №{i}:");
                string assembleName = Console.ReadLine();

                Console.Write("Кол-во:");
                int totalCount = Convert.ToInt32(Console.ReadLine());

                Assemble subAssemble = new Assemble(assembleName, totalCount, assembleItem.GetTotalCount());

                assembleItem.AddAssemble(subAssemble);
               
                ShowAllTable(assemble, finalList,assembleTotalCount);
            }
            Console.Write("Кол-во деталей в узле:");
            int partCount = Convert.ToInt32(Console.ReadLine());

            for (int i = 1; i <= partCount; i++)
            {
                Console.Write($"Введите название детали №{i}:");
                string name = Console.ReadLine();
                Console.Write("Введите кол-во:");
                string quantity = Console.ReadLine();
                Console.Write("Введите название материала:");
                string material = Console.ReadLine();
                Console.Write("Введите контролируемый параметр:");
                string param = Console.ReadLine();

                if (param.Contains('.'))
                {
                    param = param.Replace('.', ',');
                }
                Part part = new Part(name, material, param, quantity, Convert.ToInt32(assembleItem.GetTotalCount()));
                assembleItem.AddPart(part);

                if (finalList.Count == 0)
                {
                    finalList.Add(part.GetMaterial(), part.GetTotal());
                    
                }
                else
                {
                    if (finalList.ContainsKey(part.GetMaterial()))
                    {
                        double newValue = finalList[part.GetMaterial()] + part.GetTotal();
                        finalList[part.GetMaterial()] = newValue;
                    }
                    else
                    {
                        finalList.Add(part.GetMaterial(), part.GetTotal());
                    }
                }
               
                ShowAllTable(assemble, finalList,assembleTotalCount);
            }

            for (int i = 1; i <= assembleCount; i++)
            {
                AssembleFill(assemble, assembleItem.GetAssembleItem(i - 1), finalList,assembleTotalCount);
            }
            return;
        }
        public static void ShowFinalList(Dictionary<string, double> finalList,int totalCount)
        {
            foreach(var item in finalList)
            {
                Console.WriteLine($"{item.Key}............{item.Value}({item.Value/Convert.ToDouble(totalCount)})");
            }
        }
    static void Main(string[] args)
        {   
            //List <Part> partList=new List<Part>();
            Console.Write("Название изделия:");
            string assembleName=Console.ReadLine();
            
            Console.Write("Кол-во изделий:");
            int totalCount=Convert.ToInt32(Console.ReadLine());
           
            Assemble assemble=new Assemble(assembleName,totalCount);
            var finalList = new Dictionary<string, double>();

            AssembleFill(assemble,assemble, finalList, totalCount );
            
        }
    }
}
