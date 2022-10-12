using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Windows.Forms;
using System.Security.Cryptography;


namespace MaterialCalc
{
    internal class Program
    {
        const int stringSize = 30;
        public static string TableHead(string cellType, string cellName, string cellMaterial, string cellParam, string cellQuantiti, string cellTotal)
        {
            string tableHead = ($"|{cellType,3}|{cellName,stringSize}|{cellMaterial,stringSize}|{cellParam,stringSize}|{cellQuantiti,stringSize}|{cellTotal,stringSize}|");
            return tableHead;
        }
        public static string TableContent(string cellType, string cellName, string cellMaterial, string cellParam, string cellQuantiti, string cellTotal)
        {
            string tableContent = ($"|{cellType,3}|{cellName,-stringSize}|{cellMaterial,stringSize}|{cellParam,stringSize}|{cellQuantiti,stringSize}|{cellTotal,stringSize}|");
           return tableContent;
        }

        public static void WriteTableContent(StreamWriter sw, string cellType, string cellName, string cellMaterial, string cellParam, string cellQuantiti, string cellTotal)
        {
            string tableContent = ($"|{cellType,3}|{cellName,-stringSize}|{cellMaterial,stringSize}|{cellParam,stringSize}|{cellQuantiti,stringSize}|{cellTotal,stringSize}|");
            sw.WriteLine(tableContent);
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


        public static void showAssembles(Assemble assemble, List<string> list, int repeat = 0, string step = "" )
        {
            string stepAssambleName = step + assemble.GetName();
            string quantityStringAssemble = $"{Convert.ToString(assemble.GetTotalCount())}" + $"({assemble.GetCount()})";
            string table = TableContent("C", stepAssambleName, "", "", quantityStringAssemble, "");
            Console.WriteLine(table);
            list.Add(table);
            repeat++;
            string newstep = new String(' ', repeat);
            foreach (Assemble assembleItem in assemble.GetAssembleList())
            {
                showAssembles(assembleItem, list,repeat,newstep);
            }
            foreach (Part partItem in assemble.GetPartList())
            {
                string stepPartName = newstep + partItem.GetName();
                string quantityString = $"{Convert.ToString(partItem.GetTotalQuantiti())}" + $"({Convert.ToString(partItem.GetQuantiti())})";
                string totalForOneString = $"{Convert.ToString(partItem.GetTotal())}" + $"({Convert.ToString(partItem.GetTotalForOne())})";
                string tablePart= TableContent("Д", stepPartName, partItem.GetMaterial(), Convert.ToString(partItem.GetParam()), quantityString, totalForOneString);
                Console.WriteLine(tablePart);
                list.Add(tablePart);
            }
        }

        public static void ShowAllTable(Assemble assemble, Dictionary<string, double> finalList, int totalCount, List<string> list)
        {
            Console.Clear();
            string tableHead = TableHead("Тип", "Название", "Материал", "Параметр", "Количество", "Всего");
            Console.WriteLine(tableHead);
            list.Clear();
            list.Add(tableHead);
            string divider = "";
            for (int i = 0; i < tableHead.Length; i++)
            {
                divider += "_";
            }
            Console.WriteLine(divider);
            list.Add(divider);
            showAssembles(assemble,list);
            Console.WriteLine(divider);
            list.Add(divider);
            ShowFinalList(finalList, totalCount);
            Console.Write('\n');
        }


        public static void AssembleFill(Assemble assemble, Assemble assembleItem, Dictionary<string, double> finalList, int assembleTotalCount, List<string> list)
        {
            ShowAllTable(assemble, finalList, assembleTotalCount,list);

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

                ShowAllTable(assemble, finalList, assembleTotalCount, list);
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

                ShowAllTable(assemble, finalList, assembleTotalCount, list);
            }

            for (int i = 1; i <= assembleCount; i++)
            {
                AssembleFill(assemble, assembleItem.GetAssembleItem(i - 1), finalList, assembleTotalCount, list);
            }
            return;
        }
        public static void ShowFinalList(Dictionary<string, double> finalList, int totalCount)
        {
            foreach (var item in finalList)
            {
                Console.WriteLine($"{item.Key}............{item.Value}({item.Value / Convert.ToDouble(totalCount)})");
            }
        }

        public static void OpenFile(Assemble assemble, List<string> list)
        {
            Console.WriteLine("Выберите папку сохранения результатов");

            FolderBrowserDialog ofd = new FolderBrowserDialog();

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                Console.WriteLine($"Выбрана папка: {ofd.SelectedPath}");
                WriteFile(ofd, assemble,list);
                Console.ReadLine();
            }
        }

        public static void WriteFile(FolderBrowserDialog ofd, Assemble assemble, List<string> list)
        {
            try
            {
                StreamWriter sw = new StreamWriter($"{ofd.SelectedPath}\\Результат.txt");
                foreach(string row in list)
                {
                    sw.WriteLine(row);
                }
                
                sw.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Ошибка: " + e.Message);
            }
            finally
            {
                Console.WriteLine("Завершение работы...");
            }
        }
        [STAThread]
        static void Main(string[] args)
        {
            //List <Part> partList=new List<Part>();
            Console.Write("Название изделия:");
            string assembleName = Console.ReadLine();

            Console.Write("Кол-во изделий:");
            int totalCount = Convert.ToInt32(Console.ReadLine());

            Assemble assemble = new Assemble(assembleName, totalCount);
            var finalList = new Dictionary<string, double>();
            List<string> list = new List<string>();
            AssembleFill(assemble, assemble, finalList, totalCount, list);
            OpenFile(assemble, list);
        }
    }
}
