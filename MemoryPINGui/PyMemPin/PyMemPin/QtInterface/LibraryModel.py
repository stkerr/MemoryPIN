from PyQt4.QtCore import *
from PyQt4.QtGui  import *
from PyMemPin     import Library

class LibraryModel(QAbstractTableModel):
    """
    Provides a Qt model for Library objects
    """

    def __init__(self, datain, headerdata, parent=None, *args):
        # Call our parent constructor
        QAbstractTableModel.__init__(self, parent, *args)

        # Initialize the data list. Add items to here to display them
        self.arraydata = datain
        self.headerdata = headerdata

        # Configure gui params

    def rowCount(self, parent): 
        return len(self.arraydata) 
 
    def columnCount(self, parent): 
        return len(self.headerdata)
 
    def data(self, index, role): 
        if not index.isValid(): 
            return QVariant() 
        elif role != Qt.DisplayRole: 
            return QVariant() 
        return QVariant(self.arraydata[index.row()][index.column()]) 

    def headerData(self, col, orientation, role):
        if orientation == Qt.Horizontal and role == Qt.DisplayRole:
            return QVariant(self.headerdata[col])
        return QVariant()


