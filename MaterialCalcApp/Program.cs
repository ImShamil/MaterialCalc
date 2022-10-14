using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Windows.Forms;
using System.Security.Cryptography;

using OfficeOpenXml.Drawing.Chart;
using OfficeOpenXml.Style;
using OfficeOpenXml;
using static MaterialCalc.Program;


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

            public Part(string name, string material, double param, int quantity, int assembleQuantity)
            {
                if (name == "")
                {
                    name = "Деталь";
                }

                if (material == "")
                {
                    material = "Ст3";
                }
                
                this.name = name;
                this.material_name = material.ToLower();
                this.param = param;
                this.quantity = quantity;
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
            string assemlbeCountInput = Console.ReadLine();
            int assembleCount = TryParseInt(assemlbeCountInput);

            for (int i = 1; i <= assembleCount; i++)
            {
                Console.Write($"Название подсборки №{i}:");
                string assembleName = Console.ReadLine();

                Console.Write("Кол-во:");
                string subAssemlbeCountInput = Console.ReadLine();
                int totalCount= TryParseInt(subAssemlbeCountInput);
               
                Assemble subAssemble = new Assemble(assembleName, totalCount, assembleItem.GetTotalCount());

                assembleItem.AddAssemble(subAssemble);

                ShowAllTable(assemble, finalList, assembleTotalCount, list);
            }
            Console.Write("Кол-во деталей в узле:");
            string partCountInput = Console.ReadLine();
            int partCount = TryParseInt(partCountInput);

            for (int i = 1; i <= partCount; i++)
            {
                Console.Write($"Введите название детали №{i}:");
                string name = Console.ReadLine();
                Console.Write("Введите кол-во:");
                string quantityInput = Console.ReadLine();
                int quantity = TryParseInt(quantityInput);
                Console.Write("Введите название материала:");
                string material = Console.ReadLine();
                Console.Write("Введите контролируемый параметр:");
                string paramInput = Console.ReadLine();
                double param = TryParseDouble(paramInput);

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

        public static int TryParseInt(string input)
        {
            bool parseResult = int.TryParse(input, out var result);

            if (!parseResult)
            {

                while (!parseResult)
                {
                    Console.WriteLine("Невереный ввод! Повторите снова!");
                    Console.Write("Кол-во:");
                    input = Console.ReadLine();
                    parseResult = int.TryParse(input, out result);
                }
            }
            return result;
        }

        public static double TryParseDouble(string input)
        {
            if (input.Contains('.'))
            {
                input = input.Replace('.', ',');
            }

            bool parseResult = double.TryParse(input, out var result);

            if (!parseResult)
            {

                while (!parseResult)
                {
                    Console.WriteLine("Невереный ввод!Повторите снова!");
                    Console.Write("Введите контролируемый параметр:");
                    input = Console.ReadLine();
                    if (input.Contains('.'))
                    {
                        input = input.Replace('.', ',');
                    }
                    parseResult = double.TryParse(input, out result);
                }
            }
            return result;
        }



        public static void OpenFile(List<string> list)
        {
            Console.WriteLine("Выберите папку сохранения результатов");

            FolderBrowserDialog ofd = new FolderBrowserDialog();

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                Console.WriteLine($"Выбрана папка: {ofd.SelectedPath}");
                WriteFile(ofd,list);
                Console.ReadLine();
            }
        }

        public static void WriteFile(FolderBrowserDialog ofd, List<string> list)
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

        public static void GenerateEcxel(Assemble assemble)
        {
            
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var package = new ExcelPackage();

            ExcelWorksheet sheet = package.Workbook.Worksheets
                .Add("Результаты");

            sheet.Cells["A1"].Value = "Название";
            sheet.Cells["B1"].Value = "Материал";
            sheet.Cells["C1"].Value = "Кол-во на единицу";
            sheet.Cells["D1"].Value = "Общее кол-во";
            sheet.Cells["E1"].Value = "Площадь или длина на единицу";
            sheet.Cells["F1"].Value = "Сумма";
            ExcelGen(assemble, sheet);

            //sheet.cells[8, 2, 8, 4].loadfromarrays(new object[][] { new[] { "capitalization", "shareprice", "date" } });
            //var row = 9;
            //var column = 2;
            //foreach (var item in report.history)
            //{
            //    sheet.cells[row, column].value = item.capitalization;
            //    sheet.cells[row, column + 1].value = item.shareprice;
            //    sheet.cells[row, column + 2].value = item.date;
            //    row++;
            //}

            //sheet.cells[1, 1, row, column + 2].autofitcolumns();
            //sheet.column(2).width = 14;
            //sheet.column(3).width = 12;

            //sheet.cells[9, 4, 9 + report.history.length, 4].style.numberformat.format = "yyyy";
            //sheet.cells[9, 2, 9 + report.history.length, 2].style.numberformat.format = "### ### ### ##0";

            //sheet.column(2).style.horizontalalignment = excelhorizontalalignment.left;
            //sheet.cells[8, 3, 8 + report.history.length, 3].style.horizontalalignment = excelhorizontalalignment.center;
            //sheet.column(4).style.horizontalalignment = excelhorizontalalignment.right;

            //sheet.cells[8, 2, 8, 4].style.font.bold = true;
            //sheet.cells["b2:c4"].style.font.bold = true;

            //sheet.cells[8, 2, 8 + report.history.length, 4].style.border.borderaround(excelborderstyle.double);
            //sheet.cells[8, 2, 8, 4].style.border.bottom.style = excelborderstyle.thin;

            //var capitalizationchart = sheet.drawings.addchart("findingschart", officeopenxml.drawing.chart.echarttype.line);
            //capitalizationchart.title.text = "capitalization";
            //capitalizationchart.setposition(7, 0, 5, 0);
            //capitalizationchart.setsize(800, 400);
            //var capitalizationdata = (excelchartserie)(capitalizationchart.series.add(sheet.cells["b9:b28"], sheet.cells["d9:d28"]));
            //capitalizationdata.header = report.company.currency;

            //sheet.Protection.IsProtected = true;

            byte [] excel = package.GetAsByteArray();
            File.WriteAllBytes("..\\Результаты.xlsx", excel);
            //FileInfo fi = new FileInfo(filePath);
            //package.SaveAs(fi);
        }

        public static int ExcelGen(Assemble assemble, ExcelWorksheet sheet, int assembleLine = 0, int repeat = 0, int line = 2, string step = " ")
        {
            
            string stepAssambleName = step + assemble.GetName();
            repeat++;
            string newstep = new String(' ', repeat);
            sheet.Cells[$"A{line}"].Value = stepAssambleName;
            if (line == 2)
            {
                sheet.Cells[$"C{line}"].Value = 1;
            }
            else
            {
                sheet.Cells[$"C{line}"].Value = Convert.ToString(assemble.GetCount());
            }

            if (line != 2)
            {
                sheet.Cells[$"D{line}"].Formula = $"C{line}*D{assembleLine}";
            }
            else
            {
                sheet.Cells[$"D{line}"].Value = Convert.ToString(assemble.GetCount());
            }
            assembleLine = line;
            foreach (Assemble assembleItem in assemble.GetAssembleList())
            {
                line++;
                line = ExcelGen(assembleItem, sheet, assembleLine, repeat, line, newstep);
            }
            foreach (Part partItem in assemble.GetPartList())
            {
                line++;
                string stepPartName = newstep + partItem.GetName();
                sheet.Cells[$"A{line}"].Value = stepPartName;
                sheet.Cells[$"B{line}"].Value = partItem.GetMaterial();
                sheet.Cells[$"C{line}"].Value = Convert.ToString(partItem.GetTotalQuantiti());
                sheet.Cells[$"D{line}"].Formula = $"C{line}*D{assembleLine}";
                sheet.Cells[$"E{line}"].Value = Convert.ToString(partItem.GetParam());
                sheet.Cells[$"F{line}"].Formula = $"E{line}*D{line}";
            }
            return line;
        }


        [STAThread]
        static void Main(string[] args)
        {
            //List <Part> partList=new List<Part>();
            Console.Write("Название изделия:");
            string assembleName = Console.ReadLine();

            Console.Write("Кол-во изделий:");
            string totalCountInput = Console.ReadLine();
            int totalCount = TryParseInt(totalCountInput);

            Assemble assemble = new Assemble(assembleName, totalCount);
            var finalList = new Dictionary<string, double>();
            List<string> list = new List<string>();
            AssembleFill(assemble, assemble, finalList, totalCount, list);
            OpenFile(list);
            GenerateEcxel(assemble); 
        }
    }
}
