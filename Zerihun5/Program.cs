/********************************************************************
*** NAME : Kaleab Zerihun
*** CLASS : CSc 354
*** ASSIGNMENT : Assignment 
*** DUE DATE : 12-4-24
*** INSTRUCTOR : Hamer
*********************************************************************
*** DESCRIPTION : Write
the linker/loader as a separate program.****
********************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Markup;
using System.Xml.Linq;
using System.Linq.Expressions;
using System.Diagnostics.CodeAnalysis;
using System.Data;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Collections;
using System.Reflection.Emit;
using static System.Net.Mime.MediaTypeNames;
using System.Text.RegularExpressions;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Reflection.PortableExecutable;

namespace Zerihun5
{
    public class Program
    {
        /********************************************************************
        *** METHOD: main
        *********************************************************************
        *** DESCRIPTION : main function
        *** INPUT ARGS : args
        **** OUTPUT ARGS : N/A
        *** IN/OUT ARGS : 
        *** RETURN : void
        ********************************************************************/
        static void Main(string[] args)
        {
            string fileName = string.Empty;

            // If the expression file name has not been provided in the command line ask the user for a expression file name
            do
            {
                if (args.Length < 1)
                {
                    Console.Write($"Please enter the program file name: ");
                    fileName = Console.ReadLine();
                }
                // If the experssion file name has been provided in the command the pass it to the searchSymbol function 
                else
                {
                    foreach(var val in args)
                    {
                        fileName += val + " ";
                    }
                }
            } while (System.String.IsNullOrEmpty(fileName));

            string[] splitFileNmaes = fileName.Trim().Split(' ');
            List<List<string>> separetFileLines = new();
            foreach (string s in splitFileNmaes)
            {
                string fNmae = Path.Combine(Directory.GetCurrentDirectory(), s);
                List<string> strings = new List<string>();
                if (File.Exists(fNmae))
                {
                    //Read the file and for each line
                    string[] lines = File.ReadAllLines(fNmae);
                    foreach (string line in lines)
                    {
                        strings.Add(line);
                    }
                    separetFileLines.Add(strings);

                }
                else
                {
                    Console.WriteLine($"ERROR - The file {s} is not found.");
                    Environment.Exit(0);
                }
            }

            executeFunction(separetFileLines);



        }
        /********************************************************************
        *** METHOD: passOne
        *********************************************************************
        *** DESCRIPTION : executes everything and then prints the output to the file and console
        *** INPUT ARGS : List<List<string>> separetFileLines
        **** OUTPUT ARGS : N/A
        *** IN/OUT ARGS : 
        *** RETURN : void
        ********************************************************************/
        static void executeFunction(List<List<string>> separetFileLines)
        {
            
            //get the total numbers to be printed
            //declare varibales to be used later
            string totalLength = "000000";
            List<string> final = new List<string>();
            string lines = "03300";
            List<ESTAB> estab = new List<ESTAB>();
            List<string> CSADDR = new List<string>();
            List<string> CSLTH = new List<string>();
            string csaddr = "00000";
            string cslth = "00000";
            List<string> tRecord = new List<string>();
            int k = 0;
            List<string> mRecord = new List<string>();
            int ind = 0;
            //for each code part
            foreach (var code in separetFileLines)
            {
                int a = Convert.ToInt32(lines, 16);
                int b = 0;
                //if the e record is not expty
                if (code[code.Count() - 1].Substring(1) != "")
                {
                    b = Convert.ToInt32(code[code.Count() - 1].Substring(1), 16);
                }
                //assign the varibles with values
                int c = Convert.ToInt32(cslth, 16);
                csaddr = (a + b + c).ToString("X").PadLeft(5, '0');
                cslth = code[0].Substring(code[0].Length - 6);
                CSADDR.Add(csaddr);
                CSLTH.Add(cslth);
                //for each line in the code
                foreach (var line in code)
                {
                    //if it is a header record get the appropet values
                    if (line.StartsWith("H"))
                    {
                        //if the record does not exisit 
                        if (!estab.Any(e =>

                        e.CSECT == line.Substring(1, 4) &&
                        e.CSADDR == csaddr &&
                        e.LENGTH == cslth))
                        {
                            estab.Add(new ESTAB(line.Substring(1, 4), "", "", csaddr, "", cslth));
                        }

                    }
                    //if it is d record get the approporet values
                    if (line.StartsWith("D"))
                    {
                        string content = line.Substring(1);
                        int val = content.Length / 12;
                        int i = 0;
                        int count = 0;
                        string addr = string.Empty;
                        string symbol = string.Empty;
                        string laddr = string.Empty;
                        //if there are more than one d record values
                        while (count <= val && i + 4 < content.Length)
                        {
                            if (i + 4 <= content.Length)
                            {
                                symbol = content.Substring(i, 4); // First 4 chars
                            }
                            if (i + 5 < content.Length)
                            {
                                addr = content.Substring(i + 4, 6).Substring(1); // Next 5 chars
                            }
                            //get the laddr
                            laddr = (Convert.ToInt32(addr, 16) + Convert.ToInt32(csaddr, 16)).ToString("X").PadLeft(5, '0');
                            if (!estab.Any(e =>

                           e.SYMBOL == symbol &&
                           e.ADDR == addr &&
                           e.LDADDR == laddr))
                            {
                                estab.Add(new ESTAB("", symbol, addr, "", laddr, ""));
                            }
                            i = i + 9;
                            count++;
                        }

                    }
                    //if it is t record keep it in a list
                    if (line.StartsWith("T"))
                    {
                        tRecord.Add(line.Substring(1,6) + "^" + line.Substring(9));
                    }
                    //if it is m record keep it in a list
                    if (line.StartsWith("M"))
                    {
                        //if there are more than one files that have m record
                        if (ind == 0)
                        {
                            string kb = (0x03300 + Convert.ToInt32(line.Substring(1, 6), 16)).ToString("X").PadLeft(5, '0');
                            mRecord.Add(kb + line.Substring(7));
                        }
                        else
                        {
                            string kb = (0x03300 + Convert.ToInt32(CSLTH[ind - 1], 16) + Convert.ToInt32(line.Substring(1, 6), 16)).ToString("X").PadLeft(5, '0');
                            mRecord.Add(kb + line.Substring(7));

                        }

                    }


                }
                k++;
                ind++;
            }
            //print out the symbol table for testing purposes
            int len = 0;
            string l = string.Empty;
            Console.WriteLine("{0, -10} {1, -10} {2, -10} {3, -10} {4, -10} {5, -10}", "CSECT", "SYMBOL", "ADDR", "CSADDR", "LDADDR", "LENGTH");
            foreach (var val in estab)
            {
                Console.WriteLine("{0, -10} {1, -10} {2, -10} {3, -10} {4, -10} {5, -10}", val.CSECT, val.SYMBOL, val.ADDR, val.CSADDR, val.LDADDR, val.LENGTH);
                if (val.LENGTH != "")
                {
                    len = len + Convert.ToInt32(val.LENGTH, 16);
                }
            }
            Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
            string t = "";
            for (int j = 0; j < tRecord.Count; j++)
            {

                string tstart = (Convert.ToInt32(tRecord[j].Split("^")[0], 16) + 0x03300).ToString("X5");
                int op = t.Length/2;
                if (j > 0)
                {
                    while ((0x03300 + op).ToString("X5") != tstart)
                    {
                        t += "?";
                        op++;
                    }
                }
                t = t + tRecord[j].Split("^")[1]; // Assume tRecord[0] is a string

                int kal = t.Length;
                
                
                

            }
            for (int i = 0; i < len; i++)
            {

                // Calculate the start index for the 2-character substring
                int startIndex = i * 2;
                string tobeAdded = string.Empty;


                // Ensure we don't exceed the length of the string
                if (startIndex + 5 <= t.Length)
                {
                    tobeAdded = t.Substring(startIndex, 6);
                    //string a = added.Substring(startIndex, 2);
                    ////////////////////
                    ////
                    /// update the 1000 to 03300

                    string add = (0x03300 + i).ToString("X").PadLeft(5, '0');

                    if (mRecord.Any(item => item.StartsWith(add)))
                    {
                        final.Add(add.PadLeft(5, '0') + "^" + tobeAdded);

                    }
                }
                else
                {
                    string add = (0x03300 + i).ToString("X");

                }
            }

            int beti = 0;
            string added = string.Empty;
            //foreach mrecord
            foreach (var vale in mRecord)
            {
                //update them if they need updating
                int startIndex = beti * 2;
                if (startIndex + 5 <= t.Length)
                {

                    string chunk = t.Substring(startIndex, 2);

                    /////////
                    string add = vale.Substring(0, 5);

                    //if the final list has the items
                    if (final.Any(item => item.Contains(add)))
                    {
                        string element = final.FirstOrDefault(item => item.StartsWith(add));
                        string tobeAdded = element.Split("^")[1];
                        string symbolval = "00000";
                        foreach (var val in estab)
                        {
                            if (vale.Substring(vale.Length - 4) == val.SYMBOL)
                            {
                                symbolval = val.LDADDR;
                            }
                            else if (vale.Substring(vale.Length - 4) == val.CSECT)
                            {
                                symbolval = val.CSADDR;
                            }
                        }
                        //if it is plus
                        string va = string.Empty;
                        if (vale[7] == '+')
                        {
                            va = (Convert.ToInt32(tobeAdded, 16) + (Convert.ToInt32(symbolval, 16))).ToString("X").PadLeft(6, '0');
                        }
                        //if it is minus
                        else if (vale[7] == '-')
                        {
                            va = (Convert.ToInt32(tobeAdded, 16) - (Convert.ToInt32(symbolval, 16))).ToString("X").PadLeft(6, '0');
                        }
                        //update the file list
                        added += va;
                        int index = final.FindIndex(item => item.StartsWith(add));
                        if (index != -1)
                        {
                            final[index] = element.Split("^")[0] + "^" + va; // Update the specific line in the list
                        }
                    }
                }
                else
                {
                    string add = (0x03300 + beti).ToString("X");
                }
                beti++;
            }


            //int vida = 0;
            for (int i = 0; i < len; i++)
            {
                // Calculate the address based on the current line number
                string currentAddress = (0x03300 + i).ToString("X").PadLeft(5, '0');

                // Ensure we don't exceed the length of the string
                int startIndex = i * 2;
                if (startIndex + 2 <= t.Length)
                {
                    // Extract the current chunk from `t`
                    string chunk = t.Substring(startIndex, 2);

                    // Extract the address from `final`

                    // Check if `mRecord` contains the current address
                    if (final.Any(item => item.StartsWith(currentAddress)))
                    {
                        // Find the matching `mRecord` and replace the next 6 characters in `t`
                        string matchingRecord = final.First(item => item.Contains(currentAddress));
                        string replacement = matchingRecord.Split("^")[1]; // Extract 6 chars from the match

                        // Replace the next 6 characters in `t`
                        if (startIndex + 6 <= t.Length)
                        {
                            t = t.Substring(0, startIndex) + replacement + t.Substring(startIndex + 6);
                        }
                        else
                        {
                            t = t.Substring(0, startIndex) + replacement.Substring(0, t.Length - startIndex);
                        }
                    }
                }
                else
                {

                }
            }

            Console.WriteLine("          0  1  2  3  4  5  6  7  8  9  A  B  C  D  E  F"); // Header row

            for (int i = 0; i < len; i += 16)  // i increments by 16 to handle a full line of pairs
            {
                // Print the address for the current line
                Console.Write((0x03300 + i).ToString("X") + "     ");

                for (int j = 0; j < 16; j++)
                {
                    int startIndex = (i + j) * 2;

                    // Check if we still have at least 2 chars left in the string
                    if (startIndex + 2 <= t.Length)
                    {
                        string chunk = t.Substring(startIndex, 2);
                        Console.Write(chunk + " ");
                    }
                    else
                    {
                        // Not enough chars left for a complete pair
                        Console.Write("?? ");
                    }
                }

                // Move to the next line after printing 16 pairs (or ?? for missing pairs)
                Console.WriteLine();
            }
            Console.WriteLine($"Execution begins at address {03300}");
            using (StreamWriter writer = new StreamWriter(new FileStream("MEMORY.DAT", FileMode.Create, FileAccess.Write)))
            {
                writer.AutoFlush = true;
                writer.WriteLine("          0  1  2  3  4  5  6  7  8  9  A  B  C  D  E  F"); // Header row

                for (int i = 0; i < len; i += 16)  // i increments by 16 to handle a full line of pairs
                {
                    // Print the address for the current line
                    writer.Write((0x03300 + i).ToString("X") + "     ");

                    for (int j = 0; j < 16; j++)
                    {
                        int startIndex = (i + j) * 2;

                        // Check if we still have at least 2 chars left in the string
                        if (startIndex + 2 <= t.Length)
                        {
                            string chunk = t.Substring(startIndex, 2);
                            writer.Write(chunk + " ");
                        }
                        else
                        {
                            // Not enough chars left for a complete pair
                            writer.Write("?? ");
                        }
                    }

                    
                    writer.WriteLine();
                }
                writer.WriteLine($"Execution begins at address {03300}");

            }

        }

    }
}