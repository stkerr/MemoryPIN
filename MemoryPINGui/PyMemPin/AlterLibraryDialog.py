import sys
from PyQt4 import QtGui, QtCore, uic
from PyMemPin.QtInterface.FlatListModel import FlatListModel

class AlterLibraryDialog(QtGui.QDialog):
    """description of class"""
    def __init__(self,library_data,parent=None, title="", label='',text=''):
        QtGui.QWidget.__init__(self,parent)

        # Load the UI file
        self.ui = uic.loadUi('QtGuis/library_alter_gui.ui')
        
        # Load the libraries into the GUI
        model = FlatListModel(library_data,lambda x: x['name'])
        self.ui.seen_library_list.setModel(model)

        # Connect signals and slots
        self.connect(self.ui.update_button,QtCore.SIGNAL("clicked()"), self.update_library_button_clicked) # right click on library list
        print self.ui.seen_library_list.selectionModel()
        print type(self.ui.seen_library_list.selectionModel)
        self.connect(self.ui.seen_library_list.selectionModel(), QtCore.SIGNAL("selectionChanged(QItemSelection, QItemSelection)"), self.library_selection_changed) # Selected a new library view

        # Start the dialog
        self.ui.exec_()

    def library_selection_changed(self, selected, deselected):
        selected_row = selected.indexes()[0].row()

    def update_library_button_clicked(self):
        # See which library is selected
        selected_index = self.ui.seen_library_list.selectedIndexes()

        # Set the library's address to the currently entred one
