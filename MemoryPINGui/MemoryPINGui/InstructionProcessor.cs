using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Drawing;
using System.Diagnostics;

namespace MemoryPINGui
{
    /*
     * Responsible for parsiong and managing information from an instruction trace log. 
     */
    class InstructionProcessor
    {        
        IList<Instruction> instructions;
        int minDepth = Int32.MaxValue, maxDepth = -1;
        Color[] colorBank;
        int lowFilterDepth = 0, highFilterDepth = Int32.MaxValue;

        public int LowFilterDepth
        {
            get { return lowFilterDepth; }
            set { lowFilterDepth = value; }
        }

        public int HighFilterDepth
        {
            get { return highFilterDepth; }
            set { highFilterDepth = value; }
        }

        public Color[] ColorBank
        {
            get { return colorBank; }
            set { colorBank = value; }
        }

        public int MaxDepth
        {
            get { return maxDepth; }
            set { maxDepth = value; }
        }

        public int MinDepth
        {
            get { return minDepth; }
            set { minDepth = value; }
        }

        IList<Library> libraries; // all libraries
        private List<string> includedLibraries; // a list of libraries to render 
        
        IDictionary<string, int> libraryOffsetDictionary;

        public IDictionary<string, int> LibraryOffsetDictionary
        {
            get { return libraryOffsetDictionary; }
            set { libraryOffsetDictionary = value; }
        }

        IList<int> threads;

        public IList<int> Threads
        {
            get { return threads; }
            set { threads = value; }
        }
        private List<int> includedThreads;

        public List<int> IncludedThreads
        {
            get { return includedThreads; }
            set { includedThreads = value; }
        }

        public List<string> IncludedLibraries
        {
            get { return includedLibraries; }
            set { includedLibraries = value; }
        }

        public IList<Library> Libraries
        {
            get { return libraries; }
            set { libraries = value; }
        }

        internal IList<Instruction> Instructions
        {
            get 
            {
                // remove any filtered libraries

                IList<Instruction> results = instructions.Where(
                                                            (x => 
                                                                (
                                                                    includedLibraries.Contains(x.LibraryName) 
                                                                    ||
                                                                    x.SystemCallName != ""
                                                                )
                                                                && 
                                                                includedThreads.Contains((int)x.Threadid)
                                                                &&
                                                                x.Depth <= HighFilterDepth
                                                                &&
                                                                x.Depth >= LowFilterDepth    
                                                            )
                                                            
                                                            ).ToList<Instruction>();
                
                // go through and adjust any instructions that have modified load addresses
                foreach(Instruction i in results)
                {
                    i.Address = i.Address_traced - i.Library.Loadaddress + i.Library.Originaladdress;
                }
                
                return results;
            }
            set { instructions = value; }
        }

        public InstructionProcessor(string filename, IList<Library> libraries, LibraryResultsProcessor processor)
        {
            this.libraries = libraries;
            instructions = new List<Instruction>();
            includedLibraries = new List<string>();
            threads = new List<int>();
            includedThreads = new List<int>();
            libraryOffsetDictionary = new Dictionary<string, int>();

            // initialise the color bank
            colorBank = new Color[16];
            colorBank[0] = Color.FromArgb(246, 150, 121);
            colorBank[1] = Color.FromArgb(249, 173, 129);
            colorBank[2] = Color.FromArgb(253, 198, 137);
            colorBank[3] = Color.FromArgb(255, 247, 153);
            colorBank[4] = Color.FromArgb(196, 223, 155);
            colorBank[5] = Color.FromArgb(163, 211, 156);
            colorBank[6] = Color.FromArgb(130, 202, 156);
            colorBank[7] = Color.FromArgb(122, 204, 200);
            colorBank[8] = Color.FromArgb(109, 207, 246);
            colorBank[9] = Color.FromArgb(125, 167, 217);
            colorBank[10] = Color.FromArgb(131, 147, 202);
            colorBank[11] = Color.FromArgb(135, 129, 189);
            colorBank[12] = Color.FromArgb(161, 134, 190);
            colorBank[13] = Color.FromArgb(189, 140, 191);
            colorBank[14] = Color.FromArgb(244, 154, 193);
            colorBank[15] = Color.FromArgb(245, 152, 157);

            Library strange = new Library();
            strange.Name = "INVALID";

            using (FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    while (!sr.EndOfStream)
                    {
                        string line = sr.ReadLine();
                        string[] entries = line.Split(new char[]{'|'});
                        Instruction instr = new Instruction();
                        foreach(string s in entries)
                        {
                            string[] keyvalue = s.Split(new char[] {':'});
                            if (keyvalue.Length < 2)
                                continue;
                            if (keyvalue.Length > 2)
                            {
                                // there is a colon besides the field delimeter
                                for (int i = 2; i < keyvalue.Length; i++)
                                {
                                    keyvalue[1] += ":" + keyvalue[i];
                                }
                            }

                            keyvalue[0] = keyvalue[0].Trim();
                            keyvalue[1] = keyvalue[1].Trim();

                            switch (keyvalue[0])
                            {
                                case "Thread ID":
                                    int id;
                                    if (Int32.TryParse(keyvalue[1], out id))
                                    {
                                        instr.Threadid = (uint)id;
                                        if (!Threads.Contains(id))
                                            Threads.Add(id);
                                        if (!IncludedThreads.Contains(id))
                                            IncludedThreads.Add(id);
                                    }
                                    else
                                        instr.Threadid = 0;
                                    break;
                                case "Instruction Address":
                                    int addr;
                                    if (int.TryParse(keyvalue[1], out addr))
                                    {
                                        instr.Address = instr.Address_traced = (uint)addr;
                                    }
                                    else
                                        instr.Address = instr.Address_traced = 0;
                                    break;
                                case "Library Name":
                                    if (libraries.Where(x => x.Name.Equals(keyvalue[1].Trim())).ToList().Count == 0)
                                    {
                                        instr.Library = strange;
                                        //throw new InvalidDataException("Somehow executed a non-loaded library!");
                                        if(!libraries.Contains(strange))
                                            libraries.Add(strange);
                                    }
                                    List<Library> lib = libraries.Where(x => x.Name.Equals(keyvalue[1].Trim())).ToList();
                                    if (lib.Count != 0)
                                    {
                                        instr.Library = lib.First();
                                    }
                                    else
                                    {
                                        Console.WriteLine("Found an instruction without a corresponding library!");
                                    }
                                    if (!IncludedLibraries.Contains(keyvalue[1]))
                                    {
                                        IncludedLibraries.Add(keyvalue[1]);
                                    }
                                    break;
                                case "Instruction Count":
                                    int count;
                                    if (Int32.TryParse(keyvalue[1], out count))
                                        instr.Instructionnumber = count;
                                    else
                                        instr.Instructionnumber = -1;
                                    break;
                                case "Time":
                                    int time;
                                    if (Int32.TryParse(keyvalue[1], out time))
                                        instr.Time = (uint)time;
                                    else
                                        instr.Instructionnumber = -1;
                                    break;
                                case "Depth":
                                    int depth;
                                    if (Int32.TryParse(keyvalue[1], out depth))
                                    {
                                        instr.Depth = (int)depth;
                                        instr.Color = ColorBank[instr.Depth % ColorBank.Length];
                                    }
                                    else
                                        instr.Depth = -1;
                                    break;
                                default:
                                    break;

                            }
                        }

                        if (MaxDepth < instr.Depth)
                            MaxDepth = instr.Depth;
                        if (MinDepth > instr.Depth)
                            MinDepth = instr.Depth;
                        instructions.Add(instr);
                    }
                }
            }

            
            

            foreach(Instruction instr in this.instructions)
            {
                if (instr.Library == strange)
                {
                    // This is an unresolved library, instruction address pairing. Let's patch it up.
                    instr.LibraryName = processor.GetLibraryName((int)instr.Address);
                    foreach(Library l in this.Libraries)
                    {
                        if (l.Name != null && l.Name.CompareTo(instr.LibraryName) == 0)
                        {
                            instr.Library = l;
                            break;
                        }
                    }
                    if (instr.LibraryName == null)
                    {
                        //Debug.WriteLine("Couldnt resolve address {0:X} to a library. Traced as {1:X}.", instr.Address, instr.Address_traced);
                        instr.Library = strange;
                        instr.LibraryName = "STRANGE MODE";
                    }
                }

                List<string> outValue = new List<string>();
                if (instr.Library.PeSupport != null && instr.Library.PeSupport.Exports.TryGetValue((int)instr.Address, out outValue))
                {
                    instr.SystemCallName = outValue[0];
                }
            }
        }
    }
}

