# This can be run on Windows inside a Cygwin/MinGW/Unix emulator environment.
# During development, I ran it inside of the Git Bash shell that is installed
# when installing Git for Windows.
#
# Actually, if you have Git Bash installed, you can just type 'build_win.sh' from a
# normal cmd.exe shell and Git Bash will be invoked automatically to run, which is probably
# the easiest/fastest way to do this.
make ARCH=ia32 PIN_ROOT=../pin-2.12-58423-msvc10-windows/ BOOST_PATH=C:/Boost/include/boost-1_53/ $1