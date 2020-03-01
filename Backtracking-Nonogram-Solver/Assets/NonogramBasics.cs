using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

public class NonogramBasics : MonoBehaviour
{
    class Nonogram
    {
        int[][] nonogramMatrix; //main structure, matrix filled with either 0, 1 or 2
                                //0 = empty space - 1 [ ] = filled space [■] - 2 = confirmed empty space [x]
        int xDim = 0;
        int yDim = 0;
        List<List<int>> rowValues = new List<List<int>>();
        List<List<int>> columnValues = new List<List<int>>();

        public void initialSpaceFilling(Nonogram inputNonogram)
        /*Fills up the most obvious spaces in the nonogram:
          1) Fills entire row or column if its value == corresponding dimension
          2) Fills up row or column with either filled or confirmed empty spaces, when the row or column value + spaces = corresponding dimension
          3) Also fills spaces where the exact position cant be determined, but there are some fixed spaces to be filled*/
        {
            for (int i = 0; i < inputNonogram.xDim; i++)
            {
                if (inputNonogram.rowValues[i].Count == 1 && inputNonogram.rowValues[i][0] == inputNonogram.xDim) //if condition 1 applies
                {
                    for (int j = 0; j < xDim; j++)
                    {
                        inputNonogram.nonogramMatrix[i][j] = 1;
                    }
                }
                else if (calculateLineValue(inputNonogram.rowValues[i]) == inputNonogram.xDim) //if condition 2 applies
                {
                    for (int j = 0; j < inputNonogram.rowValues[i].Count; j++)
                    {
                        int tempValue = inputNonogram.rowValues[i][j];
                        int index = 0;
                        while (tempValue > 0)
                        {
                            inputNonogram.nonogramMatrix[i][index] = 1;
                            tempValue--;
                            index++;
                        }
                    }
                }
                //part 3 pending
            }
        }

        public int calculateLineValue(List<int> pInputLine)
        {
            int spaces = (pInputLine.Count) - 1;
            int lineValue = spaces;
            for (int i = 0; i < pInputLine.Count; i++)
            {
                lineValue += pInputLine[i];
            }
            return lineValue;
        }

        public void readInputFile(string pTextFile) //reads the .txt file and intances a nonogram
        {
            if (File.Exists(pTextFile))
            {
                Console.WriteLine("File exists, initiating reading...");
                // Read a text file line by line.  
                int lineCount = 1; //helpful to set dimensions
                bool allRowsAdded = false;

                //Initializing an instance of Nonogram
                //Nonogram nonogram = new Nonogram();

                string[] lines = File.ReadAllLines(pTextFile);
                foreach (string line in lines)
                {


                    if (lineCount == 1) //checking if its the first line to add x,y values to the nonogram
                    {
                        //int[] rowColumnValues = { };
                        this.xDim = extractNumber(line)[0];
                        this.yDim = extractNumber(line)[1];
                        //this.xDim = rowColumnValues[1]; this.yDim = rowColumnValues[0];

                    }
                    else if (line == "COLUMNAS") //checks whether all row values have been added
                    {
                        allRowsAdded = true;
                    }
                    else if (lineCount > 2 && !allRowsAdded) //starts from 3 to avoid the line value of "FILAS"
                    {
                        Console.WriteLine("This is a ROW value: {0}", line);
                        this.rowValues.Add(extractNumber(line));

                    }
                    else if (lineCount > 2 && allRowsAdded) //as in, if all values left have to be assigned to the columns
                    {
                        Console.WriteLine("This is a COLUMN value: {0}", line);
                        this.columnValues.Add(extractNumber(line));
                    }
                    lineCount++;

                }

            }
            Console.WriteLine("xDim: {0}", this.xDim);
            Console.WriteLine("yDim: {0}", this.yDim);
            Console.WriteLine("Showing ROW values");
            Console.WriteLine("Size: {0}", this.rowValues.Count);
            for (int i = 0; i < this.rowValues.Count; i++)
            {
                for (int j = 0; j < this.rowValues[i].Count; j++)
                {
                    Console.Write("{0}  ", this.rowValues[i][j]);
                }
                Console.WriteLine();
            }
            Console.WriteLine("Showing COLUMN values");
            Console.WriteLine("Size: {0}", this.columnValues.Count);
            for (int i = 0; i < this.columnValues.Count; i++)
            {
                for (int j = 0; j < this.columnValues[i].Count; j++)
                {
                    Console.Write("{0}  ", this.columnValues[i][j]);
                }
                Console.WriteLine();
            }

        }
        List<int> extractNumber(string pInputString)
        {
            string[] numbers = Regex.Split(pInputString, @"\D+");
            List<int> resultingNumbers = new List<int>();

            foreach (string value in numbers)
            {
                if (!string.IsNullOrEmpty(value))
                {
                    int i = int.Parse(value);
                    //Console.WriteLine("Number: {0}", i);
                    resultingNumbers.Add(i);
                }
            }
            foreach (int element in resultingNumbers)
            {
                //Console.WriteLine("Number: {0}", element);
            }
            return resultingNumbers;
        }
    }


    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("8========D cummies");
            Nonogram nonogram = new Nonogram();
            string fileName = "C:/Users/Almudena/Desktop/testFile.txt";
            nonogram.readInputFile(fileName);
        }
    }
}

