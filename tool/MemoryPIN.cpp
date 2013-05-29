#include "pin.H"
#include <iostream>
#include <fstream>
#include <boost/icl/split_interval_map.hpp>
#include <string>

/*
 * Command line switches to define the memory region to monitor
 */
KNOB<int> KnobStartRegion(KNOB_MODE_OVERWRITE, "pintool", "startRegion", "0", "Indicates the start of the region to monitor");
KNOB<int> KnobEndRegion(KNOB_MODE_OVERWRITE, "pintool", "endRegion", "0", "Indicates the end of the region to monitor");

std::ostream * out = &cerr;

boost::icl::interval_map<int, std::string> address_lib_interval_map;

void instructionTrace(INS ins, void* v)
{
    printf("Instruction Address: 0x%08x : ", (int)INS_Address(ins));

    boost::icl::interval_map<int, std::string>::iterator it = address_lib_interval_map.begin();

    while(it != address_lib_interval_map.end())
    {
        boost::icl::discrete_interval<int> range = (*it).first;
        std::string name = (*it++).second; 
        if(boost::icl::contains(range, (int)INS_Address(ins)))
        {
            printf("%s", name.c_str());
            //std::cout << range << ":" << name << std::endl;
            break;
        }
    }
    printf("\n");
 }

void imageLoaded(IMG img, void* data)
{
    //printf("Image loaded\n");
    printf("------\n");
    printf("Loading Image: %s\n", IMG_Name(img).c_str());
    printf("\tLoading Location:\n");
    printf("\t0x%08x to 0x%08x\n", (int)IMG_LowAddress(img), (int)IMG_HighAddress(img));
    printf("\tFirst Instruction address: 0x%08x\n", (int)IMG_Entry(img));
    //printf("\tFirst Section: 0x%08x\n", (int)IMG_SecHead(img));
    //printf("\tLast Section: 0x%08x\n", (int)IMG_SecTail(img));
    printf("------\n");
    boost::icl::discrete_interval<int> itv = boost::icl::discrete_interval<int>::right_open(
        (int)IMG_LowAddress(img),
        (int)IMG_HighAddress(img)
    );

    address_lib_interval_map.add(
        make_pair(
            itv,
            std::string(IMG_Name(img))
        )
        );

}

/*!
 * The main procedure of the tool.
 * This function is called when the application image is loaded but not yet started.
 * @param[in]   argc            total number of elements in the argv array
 * @param[in]   argv            array of command line arguments, 
 *                              including pin -t <toolname> -- ...
 */
int main(int argc, char *argv[])
{
    // Initialize PIN library. Print help message if -h(elp) is specified
    // in the command line or the command line is invalid 
    PIN_Init(argc,argv);

    printf("Monitoring 0x%08x to 0x%08x\n",KnobStartRegion.Value(), KnobEndRegion.Value());
    
    IMG_AddInstrumentFunction(imageLoaded, 0);
    INS_AddInstrumentFunction(instructionTrace, 0);

    // Start the program, never returns
    PIN_StartProgram();
    
    return 0;
}

/* ===================================================================== */
/* eof */
/* ===================================================================== */
