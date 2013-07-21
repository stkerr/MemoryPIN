from PyMemPin.Instruction import Instruction
from PyMemPin.Library import Library

class DataImporter(object):
    """
    Imports data files produced by the Memory PIN tool and does some initial processing on them
    """

    def __init__(self,path,instructionlogfile="instructionTrace.txt",librarylogfile="memorypin.txt"):

        # Save the parameters for later
        self.path = path
        self.instructionlogfile = instructionlogfile
        self.librarylogfile = librarylogfile

        # Initialize some data structores
        self.seen_libraries = set()
        self.seen_threads = set()
        self.instruction_log = dict()
        self.library_log = dict()

        # Load the files
        self.load_instruction_file()
        self.load_library_file()

        pass

    def load_instruction_file(self):
        
        # Grab the lines from the trace file
        lines = [line.strip() for line in open(self.path + "\\" + self.instructionlogfile)]

        for l in lines:
            # Tokenize each line
            tokens = l.split('|')

            # Make a new instruction object
            instruction = Instruction()

            for t in tokens:
                # Split each token on the colon character
                it = iter(t.split(':'))
                key = it.next()
                value = ''.join(list(it)).strip() # This is needed in case the user provided a colon (such as in a file path) and to strip the white space

                # Process each value and record it
                if key == 'Thread ID':
                    self.seen_threads.add(int(value))
                    instruction.thread = int(value)
                elif key == 'Instruction Address':
                    instruction.address = int(value)
                elif key == 'Library Name':
                    self.seen_libraries.add(value)
                    instruction.library = value
                elif key == 'Instruction Count':
                    instruction.count = int(value)
                elif key == 'Time':
                    instruction.time = int(value)
                else:
                    pass

            # Record the instruction to our log
            self.instruction_log[instruction.address] = instruction
        pass

    def load_library_file(self):
        # Grab the lines from the library file
        lines = [line.strip() for line in open(self.path + "\\" + self.librarylogfile)]

        for l in lines:
            # Tokenize each line
            tokens = l.split('|')

            # Make a new Library object
            library = Library()

            for t in tokens:
                # Split each token on the colon character
                it = iter(t.split(':'))
                key = it.next()
                value = ''.join(list(it)).strip() # This is needed in case the user provided a colon (such as in a file path) and to strip the white space

                # Process each value and record it
                if key == 'Library Name':
                    self.seen_libraries.add(value)
                    library.name = value
                elif key == "Start Address":
                    library.address_start = int(value)
                elif key == "End Address":
                    library.address_end = int(value)
                elif key == "Entry Address":
                    library.address_entry = int(value)
                else:
                    pass

            # Record the library to the log now
            self.library_log[library.name] = library
        pass
