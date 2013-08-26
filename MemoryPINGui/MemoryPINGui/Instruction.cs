using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace MemoryPINGui
{
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
        Color color;
        string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public Color Color
        {
          get { return color; }
          set { color = value; }
        }

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

        public Instruction(uint address, string library, uint threadid, int instructionnumber, uint tickcount, string name="", Color? color=null)
        {
            this.Address = this.Address_traced = address;
            this.Library = null;
            this.Threadid = threadid;
            this.Instructionnumber = instructionnumber;
            this.Time = tickcount;
            this.Color = color.HasValue ? color.Value : Color.White;
            this.Name = "";
        }

        public Instruction()
        {
            this.Address = this.Address_traced = 0;
            this.Library = new Library();
            this.Threadid = 0;
            this.Instructionnumber = -1;
            this.Time = 0;
            this.Color = Color.White;
            this.Name = "";
        }

    }
}
