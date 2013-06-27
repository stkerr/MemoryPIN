using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace MemoryPINGui
{
    public class Library
    {
        string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        int loadaddress;

        public int Loadaddress
        {
            get { return loadaddress; }
            set { loadaddress = value; }
        }
        int originaladdress;

        public int Originaladdress
        {
            get { return originaladdress; }
            set { originaladdress = value; }
        }
    }

    public class Instruction : DataGridViewTextBoxColumn
    {
        int address;
        Library library;
        int threadid;
        int tickcount;
        int instructionnumber;
        string libraryName;

        public string LibraryName
        {
            get { return library.Name; }
            set { library.Name = value; }
        }

        public int Instructionnumber
        {
            get { return instructionnumber; }
            set { instructionnumber = value; }
        }

        public int Time
        {
            get { return tickcount; }
            set { tickcount = value; }
        }

        public int Threadid
        {
            get { return threadid; }
            set { threadid = value; }
        }

        public Library Library
        {
            get { return library; }
            set { library = value; }
        }

        public int Address
        {
            get { return address; }
            set { address = value; }
        }

        public Instruction(int address, string library, int threadid, int instructionnumber, int tickcount)
        {
            this.Address = address;
            this.Library = null;
            this.Threadid = threadid;
            this.Instructionnumber = instructionnumber;
            this.Time = tickcount;

        }

        public Instruction()
        {
            this.Address = 0;
            this.Library = new Library();
            this.Threadid = -1;
            this.Instructionnumber = -1;
            this.Time = 0;
        }

    }

    /*
     * Responsible for parsiong and managing information from an instruction trace log. 
     */
    class InstructionProcessor
    {
        IList<Instruction> instructions;
        IList<string> libraries;
        private List<string> includedLibraries;

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

        public IList<string> Libraries
        {
            get { return libraries; }
            set { libraries = value; }
        }

        internal IList<Instruction> Instructions
        {
            get 
            {
                // remove any filtered libraries

                IList<Instruction> results = instructions.Where(x => includedLibraries.Contains(x.LibraryName) && includedThreads.Contains(x.Threadid)).ToList<Instruction>();
                /*
                // go through and adjust any instructions that have modified load addresses
                foreach(Instruction i in results)
                {
                    if(libraryOffsetDictionary.ContainsKey(i.Library.Name))
                    {
                        i.Address = i.Address - i.Library.Loadaddress + i.Library.Originaladdress;
                    }
                }
                */
                return results;
            }
            set { instructions = value; }
        }

        public InstructionProcessor(string filename)
        {
            Libraries = new List<string>();
            instructions = new List<Instruction>();
            includedLibraries = new List<string>();
            threads = new List<int>();
            includedThreads = new List<int>();
            libraryOffsetDictionary = new Dictionary<string, int>();

            StreamReader instructionfile = new StreamReader(filename);

            using (FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read))
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
                                    keyvalue[1] += keyvalue[i];
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
                                        instr.Threadid = id;
                                        if (!Threads.Contains(id))
                                            Threads.Add(id);
                                    }
                                    else
                                        instr.Threadid = -1;
                                    break;
                                case "Instruction Address":
                                    int addr;
                                    if (int.TryParse(keyvalue[1], out addr))
                                    {
                                        instr.Address = addr;
                                        instr.Library.Loadaddress = instr.Library.Originaladdress = addr;
                                    }
                                    else
                                        instr.Address = -1;
                                    break;
                                case "Library Name":
                                    if(!libraries.Contains(keyvalue[1].Trim()))
                                        libraries.Add(keyvalue[1].Trim());
                                    instr.Library.Name = keyvalue[1].Trim();
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
                                        instr.Time = time;
                                    else
                                        instr.Instructionnumber = -1;
                                    break;
                                default:
                                    break;

                            }

                            
                        }
                        instructions.Add(instr);

                    }
                }
            }
        }
    }
}
