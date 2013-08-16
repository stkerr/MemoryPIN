using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace MemoryPINGui
{
    public class HistogramEntry : IComparable<HistogramEntry>
    {
        uint address;

        public uint Address
        {
            get { return address; }
            set { address = value; }
        }
        uint count;

        public uint Count
        {
            get { return count; }
            set { count = value; }
        }

        public HistogramEntry(uint addr, uint count)
        {
            this.Address = addr;
            this.Count = count;
        }

        /*
        public int CompareTo(HistogramEntry other)
        {
            if (this.Address == other.Address) return 0;
            else if (this.Address < other.Address) return -1;
            else return 1;
        }
        */

        public int CompareTo(HistogramEntry other)
        {
            if (this.Count == other.Count) return 0;
            else if (this.Count < other.Count) return -1;
            else return 1;
        }
    }

    public class Library
    {
        string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        
        uint loadaddress;

        public uint Loadaddress
        {
            get { return loadaddress; }
            set { loadaddress = value; }
        }
        
        uint originaladdress;

        public uint Originaladdress
        {
            get { return originaladdress; }
            set { originaladdress = value; }
        }
    }

    public class Instruction
    {
        uint address_traced; // the address we got from the trace file

        public uint Address_traced
        {
            get { return address_traced; }
            set { address_traced = value; }
        }
        uint address; // the value calculated with rebasing
        Library library;
        uint threadid;
        uint tickcount;
        int instructionnumber;
        int depth;

        public int Depth
        {
            get { return depth; }
            set { depth = value; }
        }

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

        public uint Time
        {
            get { return tickcount; }
            set { tickcount = value; }
        }

        public uint Threadid
        {
            get { return threadid; }
            set { threadid = value; }
        }

        public Library Library
        {
            get { return library; }
            set { library = value; }
        }

        public uint Address
        {
            get { return address; }
            set { address = value; }
        }

        public Instruction(uint address, string library, uint threadid, int instructionnumber, uint tickcount)
        {
            this.Address = this.Address_traced = address;
            this.Library = null;
            this.Threadid = threadid;
            this.Instructionnumber = instructionnumber;
            this.Time = tickcount;

        }

        public Instruction()
        {
            this.Address = this.Address_traced = 0;
            this.Library = new Library();
            this.Threadid = 0;
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

                IList<Instruction> results = instructions.Where(x => includedLibraries.Contains(x.LibraryName) && includedThreads.Contains((int)x.Threadid)).ToList<Instruction>();
                
                // go through and adjust any instructions that have modified load addresses
                foreach(Instruction i in results)
                {
                    i.Address = i.Address_traced - i.Library.Loadaddress + i.Library.Originaladdress;
                }
                
                return results;
            }
            set { instructions = value; }
        }

        public InstructionProcessor(string filename, IList<Library> libraries)
        {
            this.libraries = libraries;
            instructions = new List<Instruction>();
            includedLibraries = new List<string>();
            threads = new List<int>();
            includedThreads = new List<int>();
            libraryOffsetDictionary = new Dictionary<string, int>();

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
                                        //throw new InvalidDataException("Somehow executed a non-loaded library!");
                                        Library strange = new Library();
                                        strange.Name = "(!) " + keyvalue[1].Trim();
                                        if(!libraries.Contains(strange))
                                            libraries.Add(strange);
                                    }
                                    List<Library> lib = libraries.Where(x => x.Name.Equals(keyvalue[1].Trim())).ToList();
                                    if(lib.Count != 0)
                                        instr.Library = lib.First();
                                    if (!IncludedLibraries.Contains(keyvalue[1]))
                                    {
                                        IncludedLibraries.Add(keyvalue[1]);
                                    }
                                    /*
                                    instr.Library.Loadaddress = lib.First().Loadaddress;
                                    instr.Library.Originaladdress = lib.First().Originaladdress;
                                    instr.Library.Name = keyvalue[1].Trim();
                                     */
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
                                        instr.Depth = (int)depth;
                                    else
                                        instr.Depth = -1;
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
