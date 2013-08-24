using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
}
