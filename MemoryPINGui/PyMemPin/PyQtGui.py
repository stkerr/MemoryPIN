import sys
from PyQt4 import QtGui, QtCore, uic
from PyMemPin.QtInterface.InstructionModel import InstructionModel
from PyMemPin.QtInterface.LibraryModel import LibraryModel
from PyMemPin.QtInterface.FlatListModel import FlatListModel
from AlterLibraryDialog import AlterLibraryDialog

class PyQtGui(QtGui.QMainWindow):
    """description of class"""
    def __init__(self):
        # call our parent constructor
        QtGui.QMainWindow.__init__(self)

        self.instruction_data = list()
        self.library_data = dict()
        self.selected_libraries = list()

        # load the UI file
        self.ui = uic.loadUi('QtGuis/main_gui.ui')

        # start the GUI
        self.ui.show()

    def setup(self):
        # connect all the signals and slots now
        self.connect(self.ui.tabWidget, QtCore.SIGNAL("currentChanged(int)"), self.gui_tab_changed) # changing the tab
        self.ui.seen_library_view.setContextMenuPolicy(QtCore.Qt.CustomContextMenu)
        self.connect(self.ui.seen_library_view,QtCore.SIGNAL("customContextMenuRequested(QPoint)"), self.alter_library_loads) # right click on library list
        
        # set up our visible library list
        model = FlatListModel(self.library_data,lambda x: x['name'])
        self.ui.visible_libraries_view.setModel(model)
        value = self.connect(self.ui.visible_libraries_view.selectionModel(),QtCore.SIGNAL("selectionChanged(QItemSelection, QItemSelection)"), self.visible_library_changed)
        #self.connect(self.ui.seen_library_list.selectionModel(),    QtCore.SIGNAL("selectionChanged(QItemSelection, QItemSelection)"), self.library_selection_changed) # Selected a new library view

    def alter_library_loads(self):
        
        # Load up the alter library dialog and pass along the library information
        alter_library_dialog = AlterLibraryDialog(self.library_data)

        print type(alter_library_dialog)

        # Update all the instructions with these new library offsets
        for i in self.instruction_data:

            # Check the library addresses
            for l in self.library_data:
                if i['library'] == l['name']:
                    i.address = i.original_address - l.address_start + l.address_new_start


    def visible_library_changed(self, selected, deselected):
        print "Selection changed!"
        self.selected_libraries = list()
        for i in self.ui.visible_libraries_view.selectedIndexes():
            selected_row = i.row()
            self.selected_libraries.append(self.library_data[selected_row]['name'])

    def gui_tab_changed(self):
        current_tab = self.ui.tabWidget.currentIndex()
        print "gui_tab_changed to " + str(current_tab)

        # Set up the instruction trace window
        self.instruction_data.sort(key=lambda x: x['count'])
        
        

        #filtered_data = filter(lambda x: x['library'] in selected_libraries, self.instruction_data)
        #filtered_data = [x for x in self.instruction_data if x['library'] in selected_libraries]
        filtered_data = [x for x in self.instruction_data if x['library'] in self.selected_libraries]
        instruction_model = InstructionModel(filtered_data,['address','thread','library','count','time'],self)
        self.ui.instruction_trace_view.setModel(instruction_model)

        # Set up the library trace window
        library_model = LibraryModel(self.library_data,['Name','Start Address','End Address','Entry Address'],self)
        self.ui.seen_library_view.setModel(library_model)
        
        # Set up the library filter view
        library_list_model = FlatListModel(filtered_data, lambda x: x['name'])
        self.ui.visible_libraries_view.setModel(library_list_model)

if __name__ == "__main__":
	app = QtGui.QApplication(sys.argv)
	win = PyQtGui()
	sys.exit(app.exec_())