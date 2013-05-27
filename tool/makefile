# There is a lot of magic going on in this file. Be VERY careful when editing. You can probably
# use command line flags, rather than editing this, actually.
#
# If you do edit, all edits should be done after the 3 includes, since those are where a lot of
# PIN specific stuff happens.
#
# Also check build.sh (or build_win.sh) to see if you can change parameters in there.

# If the tool is built out of the kit, PIN_ROOT must be specified in the make invocation and point to the kit root.
ifdef PIN_ROOT
CONFIG_ROOT := $(PIN_ROOT)/source/tools/Config
else
CONFIG_ROOT := ../Config
endif

# All sorts of problems happen if you try to run a 64 bit tool on a 32 bit
# application. So... DONT'T DO THAT.
#
# If you don't specify a specific architecture, 32 bit (ia32) will be run 
# by default, which is probably what you want anyways (as of 2013 at least)
#
# You can define a 64 bit tool by specifying intel-64 on the command line
# in the ARCH variable. Otherwise, it will default to 32 bit.
ifdef ARCH
HOST_ARCH := $(ARCH)
else
HOST_ARCH := ia32
endif

include $(CONFIG_ROOT)/makefile.config
include makefile.rules
include $(TOOLS_ROOT)/Config/makefile.default.rules

# Set up Boost if it's manually specified, otherwise throw an error
# As of 5/27/2013, Boost 1.53.0 was the version in use. (It should be
# in source control as a ZIP file or something)
#
# Note this should point to the directory inside of boost/include, which
# contains boost/<assorted headers>. This can be confusing, especially on
# Windows systems. 
#
# On my Windows install, I have the Boost path, 
# C:\Boost\include\boost-1_53\boost\<headers>. For this path, I should pass
# the value of "BOOST_PATH=C:\\Boost\\include\\boost-1_53\\" to get things
# to work properly. (Remember to use \\ due to character path escaping!)
#
# On Linux, you can probably just use "BOOST_PATH=/usr/local/boost/include"
# or something similar
ifdef BOOST_PATH
BOOST_INCLUDE := $(BOOST_PATH)
else
$(error Please define BOOST_PATH when running this makefile)
endif

# Add the Boost library to the include path
COMP_OBJ := -I $(BOOST_INCLUDE) $(COMP_OBJ)