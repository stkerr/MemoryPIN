using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace MemoryPINGui
{
    public class Instruction : DataGridViewTextBoxColumn
    {
        int address;
        string library;
        int threadid;
        int tickcount;
        int instructionnumber;

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

        public string Library
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
            this.Library = library;
            this.Threadid = threadid;
            this.Instructionnumber = instructionnumber;
            this.Time = tickcount;

        }

        public Instruction()
        {
            this.Address = 0;
            this.Library = "";
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
        private List<string> filteredLibraries;
        private List<string> includedLibraries;

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
                IList<Instruction> results = instructions.Where(x => includedLibraries.Contains(x.Library)).ToList<Instruction>();
                return results;
            }
            set { instructions = value; }
        }

        public InstructionProcessor(string filename)
        {
            Libraries = new List<string>();
            filteredLibraries = new List<string>();
            instructions = new List<Instruction>();
            includedLibraries = new List<string>();

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
                                        instr.Threadid = id;
                                    else
                                        instr.Threadid = -1;
                                    break;
                                case "Instruction Address":
                                    int addr;
                                    if (int.TryParse(keyvalue[1], out addr))
                                        instr.Address = addr;
                                    else
                                        instr.Address = -1;
                                    break;
                                case "Library Name":
                                    instr.Library = keyvalue[1].Trim();
                                    if (!Libraries.Contains(keyvalue[1].Trim()))
                                    {
                                        Libraries.Add(keyvalue[1].Trim());
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
