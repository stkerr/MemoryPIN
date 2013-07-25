import sys
from PyQt4 import QtGui, QtCore, uic
from PyMemPin.QtInterface.FlatListModel import FlatListModel

class AlterLibraryDialog(QtGui.QDialog):
    """description of class"""
    def __init__(self,library_data,parent=None, title="", label='',text=''):
        QtGui.QWidget.__init__(self,parent)

        # Save the library data
        self.library_data = library_data

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
        
        selected_library = self.library_data[selected_row]
        self.ui.original_address_textedit.clear()
        self.ui.original_address_textedit.insertPlainText(hex(selected_library.address_start))
        self.ui.new_address_textedit.clear()
        self.ui.new_address_textedit.insertPlainText(hex(selected_library.address_new_start))

        
    def update_library_button_clicked(self):
        # See which library is selected
        selected_index = self.ui.seen_library_list.selectedIndexes()[0]

        # Get the address value
        try:
            input = str(self.ui.new_address_textedit.toPlainText())
            if input[0] == '0' and input[1] == 'x':
                new_address = int(input,16)
            else:
                new_address = int(input)
        except TypeError:
            return

        # Set the library's address to the currently entred one
        selected_row = selected_index.row()
        selected_library = self.library_data[selected_row]

        selected_library.address_new_start = new_address