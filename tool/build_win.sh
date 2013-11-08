# This can be run on Windows inside a Cygwin/MinGW/Unix emulator environment.
# During development, I ran it inside of the Git Bash shell that is installed
# when installing Git for Windows.
#
# Actually, if you have Git Bash installed, you can just type 'build_win.sh' from a
# normal cmd.exe shell and Git Bash will be invoked automatically to run, which is probably
# the easiest/fastest way to do this.
#
# I typically define an environment variable BOOST_PATH, but you can pass it here as a command
# line argument if you like
#
make ARCH=ia32 PIN_ROOT=../pin-2.12-58423-msvc10-windows/ EXTRA_TOOL_LIBS=imagehlp.lib $1
