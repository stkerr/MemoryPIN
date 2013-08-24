using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
}
