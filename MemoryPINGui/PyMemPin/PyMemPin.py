import PyMemPin
import PyMemPin.DataImporter
from PyQtGui import PyQtGui

import sys
from PyQt4.QtGui import QApplication, QMainWindow


if __name__ == "__main__":

    # Load our data from disk
    data_importer = PyMemPin.DataImporter.DataImporter("D:\Documents\Code\MemoryPIN\MemoryPINGui")
    #print data_importer

    # Spawn a Qt application
    app = QApplication([])
  
    # Load the custom GUI into an object, which links it to th existing Qt application
    gui = PyQtGui()
    
    # Load our trace data into the GUI
    gui.instruction_data = data_importer.instruction_log.values()
    gui.library_data = data_importer.library_log.values()

    # open the GUI up. Note that this call blocks.
    return_code = app.exec_()

    # Do any cleanup

    # Exit with the system code from the GUI
    sys.exit(return_code)