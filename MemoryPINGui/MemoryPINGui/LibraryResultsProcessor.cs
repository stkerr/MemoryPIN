using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace MemoryPINGui
{
    class Interval
    {
        int start;

        public int Start
        {
            get { return start; }
            set { start = value; }
        }

        int end;

        public int End
        {
            get { return end; }
            set { end = value; }
        }

        public Interval(int start, int end)
        {
            this.start = start;
            this.end = end;
        }

        public bool Contains(int address)
        {
            if (address >= start && address < end)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    class LibraryResultsProcessor
    {
        string filename;
        FileStream filestream;

        Dictionary<string, Interval> libraries_string_interval;
        Dictionary<Interval, string> libraries_interval_string;

        public LibraryResultsProcessor(string filename)
        {
            this.filename = filename;
            this.filestream = new FileStream(filename, FileMode.Open);
            libraries_string_interval = new Dictionary<string,Interval>();
            libraries_interval_string = new Dictionary<Interval, string>();
        }

        public bool AddLibraryRange(string libraryname, int startAddress, int endAddress)
        {
            if (startAddress < 0 || endAddress < 0 || endAddress <= startAddress)
            {
                return false;
            }

            /*
             * O(N) lookup is incorrect, but whatever. There probably won't be that many libraries_string_interval. 
             * (I'm sure in 5 years when I come back to this code, I'll hate myself for that comment.)
             */
            foreach(Interval i in libraries_string_interval.Values)
            {  
                // don't insert a new library if we arlready have something here
                if (i.Contains(startAddress) || i.Contains(endAddress))
                {
                    return false;
                }
            }

            // insert the new interval
            Interval interval = new Interval(startAddress, endAddress);

            libraries_string_interval[libraryname] = interval;
            libraries_interval_string[interval] = libraryname;

            return true;
        }

        public string GetLibraryName(int address)
        {
            foreach (Interval i in libraries_string_interval.Values)
            {
                if (i.Contains(address))
                {
                    return libraries_interval_string[i];
                }
            }

            return null;
        }
    }
}
