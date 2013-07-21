from PyQt4.QtCore import *
from PyQt4.QtGui import *

class FlatListModel(QAbstractListModel): 
    '''
    Provides a Qt model for a flat, 1-D list
    Adapted from http://www.saltycrane.com/blog/2008/01/pyqt-43-simple-qabstractlistmodel/
    '''
    def __init__(self, datain, renderroutine=str, parent=None, *args): 
        """ datain: a list where each item is a row
        """
        QAbstractListModel.__init__(self, parent, *args) 
        self.listdata = datain
        self.renderroutine = renderroutine # Used to provide custom rendering. Defaults to str()

    def rowCount(self, parent=QModelIndex()): 
        return len(self.listdata) 
 
    def data(self, index, role): 
        if index.isValid() and role == Qt.DisplayRole:
            return QVariant(self.renderroutine(self.listdata[index.row()]))
        else: 
            return QVariant()