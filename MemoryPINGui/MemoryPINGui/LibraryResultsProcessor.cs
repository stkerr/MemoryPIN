using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;
using System.Diagnostics;

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

    public class LibraryResultsProcessor
    {
        Dictionary<string, Interval> libraries_string_interval;
        Dictionary<Interval, string> libraries_interval_string;

        IList<Library> libraries;

        public IList<Library> Libraries
        {
            get { return libraries; }
            set { libraries = value; }
        }

        public LibraryResultsProcessor(IList<Library> libraries)
        {
            this.libraries = libraries;

            libraries_string_interval = new Dictionary<string, Interval>();
            libraries_interval_string = new Dictionary<Interval, string>();

            
            Library nullLibrary = new Library();
            nullLibrary.Name = "Select to view only system calls";
            nullLibrary.Originaladdress = 0;
            nullLibrary.PeSupport = null;
            this.libraries.Add(nullLibrary);
            
            foreach (Library l in this.libraries)
            {
                if(l.PeSupport != null)
                    AddLibraryRange(l.Name, l.PeSupport.ImageBase, l.PeSupport.ImageBase + l.PeSupport.ImageSize);
            }
        }

        public LibraryResultsProcessor(string filename)
        {
            
            libraries = new List<Library>();
            libraries_string_interval = new Dictionary<string,Interval>();
            libraries_interval_string = new Dictionary<Interval, string>();
            
            Library nullLibrary = new Library();
            nullLibrary.Name = "Select to view only system calls";
            nullLibrary.Originaladdress = 0;
            nullLibrary.PeSupport = null;
            libraries.Add(nullLibrary);
            
            using (FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    while (!sr.EndOfStream)
                    {
                        string line = sr.ReadLine();
                        string[] entries = line.Split(new char[] { '|' });
                        Library library = new Library();
                        foreach (string s in entries)
                        {
                            string[] keyvalue = s.Split(new char[] { ':' });
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
                                case "Library Name":
                                    library.Name = keyvalue[1].Trim();

                                    try
                                    {
                                        library.PeSupport = new PESupport(library.Name);
                                        library.Originaladdress = (uint)library.PeSupport.ImageBase;
                                    }
                                    catch (ApplicationException e)
                                    {
                                        library.PeSupport = new PESupport("C:\\Program Files\\Java\\jdk1.7.0_45\\bin\\jli.dll");
                                    }
                                    
                                    break;
                                case "Start Address":
                                    int addr;
                                    if (int.TryParse(keyvalue[1], out addr))
                                    {
                                        library.Loadaddress = (uint)addr;
                                    //    library.Originaladdress = (uint)addr;
                                    }
                                    else
                                    {
                                        library.Loadaddress = 0x1337BEEF;
                                    //    library.Originaladdress= 0;
                                    }
                                    break;
                                case "End Address":
                                    break;
                                case "Entry Address":
                                    break;
                                default:
                                    break;
                            }
                        }

                        Debug.WriteLine("Name: {0} Base: {1:X} End: {2:X}", library.Name, library.PeSupport.ImageBase, library.PeSupport.ImageBase + library.PeSupport.ImageSize);

                        Libraries.Add(library);
                    }
                }
            }

            foreach (Library l in this.libraries)
            {
                if (l.PeSupport != null)
                {
                    AddLibraryRange(l.Name, (int)l.Loadaddress, (int)(l.Loadaddress + l.PeSupport.ImageSize));
                }
            }   
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
                if (i.Contains(startAddress+1) || i.Contains(endAddress))
                {
                    return false;
                }
            }

            foreach (string s in libraries_interval_string.Values)
            {
                if (s.CompareTo(libraryname) == 0)
                {
                    return false;
                }
            }

            Debug.WriteLine("Adding: {0} to {1:X}-{2:X}", libraryname, startAddress, endAddress);
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
