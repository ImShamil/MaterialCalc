﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaterialCalc
{
    internal class Program
    {
        public static string TableRow(string cellName, string cellMaterial, string cellParam, string cellQuantiti,string cellTotal)
        {
            string tableRow = string.Format("|{0,20}|{1,20}|{2,20}|{3,20}|{4,20}|", cellName, cellMaterial, cellParam,cellQuantiti,cellTotal);
            return tableRow;
        }

        public struct Assemble
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

            public int GetCount()
            {
                return this.count;
            }

            public void AddAssemble (Assemble assemble)
            {
                assemblesList.Add(assemble);
            }

            public Assemble GetAssembleItem(int index)
            {
                return assemblesList[index];
            }

             public void AddPart (Part part)
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

        public struct  Part
        {
            string name;
            string material_name;
            double param;
            int quantity;
            int totalQuantity;
            double total;

 
            public Part(string name, string material,string param,string quantity,int assembleQuantity)
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
                    param = "1";
                }
                if (quantity == "")
                {
                    quantity="1";
                }

                this.name = name;
                this.material_name = material.ToLower();
                this.param =Convert.ToDouble( param);
                this.quantity=Convert.ToInt32( quantity);
                this.totalQuantity=this.quantity*assembleQuantity;
                this.total=this.param*Convert.ToDouble(totalQuantity);
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
        };

     
        public static void showAssembles(Assemble assemble,int repeat=0,string step="")
        {   
            string stepAssambleName=step+assemble.GetName();
            Console.WriteLine($"|{stepAssambleName,-20}|{"",-20}|{"",-20}|{assemble.GetCount(),20}|" );
            repeat++;
            string Newstep=new String(' ', repeat);
           foreach( Assemble assembleItem in assemble.GetAssembleList())
            {  
                showAssembles(assembleItem,repeat,Newstep);
            }
           foreach (Part partItem in assemble.GetPartList())
           {    
                string stepPartName=Newstep+partItem.GetName();
                string quantityString = $"{Convert.ToString(partItem.GetTotalQuantiti())}"+$"({Convert.ToString(partItem.GetQuantiti())})";
                Console.WriteLine($"|{stepPartName,-20}|{partItem.GetMaterial(),-20}|{Convert.ToString(partItem.GetParam()),-20}|{quantityString,20}|");
                 
           }
        }

         public static void ShowAllTable(Assemble assemble)
        {
                Console.Clear();
                string tableRow=TableRow("Название", "Материал", "Параметр", "Количество","Всего");
                Console.WriteLine(tableRow);
                    for (int i=0; i < tableRow.Length; i++)
                    {
                        Console.Write('_');
                    }
                    Console.Write('\n');
                 showAssembles(assemble);
                    for (int i=0; i < tableRow.Length; i++)
                    {
                        Console.Write('_');
                    }
                    Console.Write('\n');
        }


        public static void SubAssembleFill(Assemble assemble, Assemble assembleItem)
        {   ShowAllTable(assemble);
            Console.WriteLine("Находимся в узле {0}",assembleItem.GetName());
            Console.Write("Кол-во подсборок в узле:");
            int assembleCount=Convert.ToInt32(Console.ReadLine());

            for (int i=1; i<=assembleCount;i++)
            {
                Console.Write($"Название подсборки №{i}:");
                string assembleName=Console.ReadLine();
            
                Console.Write("Кол-во:");
                int totalCount=Convert.ToInt32(Console.ReadLine());
                
                Assemble subAssemble=new Assemble(assembleName,totalCount);

                assembleItem.AddAssemble(subAssemble);
                ShowAllTable(assemble);
            }
            Console.Write("Кол-во деталей в узле:");
            int partCount=Convert.ToInt32(Console.ReadLine());

            for(int i=1; i <= partCount; i++)
            {
                 Console.Write($"Введите название детали №{i}:");
                string name = Console.ReadLine();
                Console.Write("Введите название материала:");
                string material = Console.ReadLine();
                Console.Write("Введите контролируемый параметр:");
                string param = Console.ReadLine();
                Console.Write("Введите кол-во:");
                string quantity = Console.ReadLine();
                
                if (param.Contains('.'))
                {
                   param = param.Replace('.', ',');
                }
                Part part = new Part(name, material,param,quantity,Convert.ToInt32(assembleItem.GetCount()));
                assembleItem.AddPart(part);
                ShowAllTable(assemble);
            }

            for(int i=1; i<=assembleCount;i++)
            {
                SubAssembleFill(assemble,assembleItem.GetAssembleItem(i-1));
            }
            return ;
        }

       

        static void Main(string[] args)
        {   
            //List <Part> partList=new List<Part>();
            Console.Write("Название изделия:");
            string assembleName=Console.ReadLine();
            
            Console.Write("Кол-во изделий:");
            int totalCount=Convert.ToInt32(Console.ReadLine());
           
            Assemble assemble=new Assemble(assembleName,totalCount);
            
           SubAssembleFill(assemble,assemble);

            
            
            
            
        }
    }
}
