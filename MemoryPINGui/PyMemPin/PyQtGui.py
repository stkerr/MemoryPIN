import sys
from PyQt4 import QtGui, QtCore, uic
from PyMemPin.QtInterface.InstructionModel import InstructionModel
from PyMemPin.QtInterface.LibraryModel import LibraryModel
from AlterLibraryDialog import AlterLibraryDialog

class PyQtGui(QtGui.QMainWindow):
    """description of class"""
    def __init__(self):
        # call our parent constructor
        QtGui.QMainWindow.__init__(self)

        
        self.instruction_data = []
        self.library_data = []

        # load the UI file
        self.ui = uic.loadUi('QtGuis/main_gui.ui')
        self.ui.show()

        # connect all the signals and slots now
        self.connect(self.ui.tabWidget, QtCore.SIGNAL("currentChanged(int)"), self.gui_tab_changed) # changing the tab
        self.ui.seen_library_view.setContextMenuPolicy(QtCore.Qt.CustomContextMenu)
        self.connect(self.ui.seen_library_view,QtCore.SIGNAL("customContextMenuRequested(QPoint)"), self.alter_library_loads) # right click on library list

    def alter_library_loads(self):
        
        # Load up the alter library dialog and pass along the library information
        alter_library_dialog = AlterLibraryDialog(self.library_data)

        # Update all the instructions with these new library offsets



    def gui_tab_changed(self):
        current_tab = self.ui.tabWidget.currentIndex()
        print "gui_tab_changed to " + str(current_tab)

        # Set up the instruction trace window
        self.instruction_data.sort(key=lambda x: x['count'])
        instruction_model = InstructionModel(self.instruction_data,['address','thread','library','count','time'],self)
        self.ui.instruction_trace_view.setModel(instruction_model)

        # Set up the library trace window
        library_model = LibraryModel(self.library_data,['Name','Start Address','End Address','Entry Address'],self)
        self.ui.seen_library_view.setModel(library_model)
        
        #self.ui.seen_thread_view.repaint()

if __name__ == "__main__":
	app = QtGui.QApplication(sys.argv)
	win = PyQtGui()
	sys.exit(app.exec_())