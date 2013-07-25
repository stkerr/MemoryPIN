class Library(object):
    """
    Represents a memory loaded into memory
    """

    def __init__(self, name="", address_start=-1, address_end=-1, address_entry=-1,):
        self.name = name
        self.address_start = address_start
        self.address_end = address_end
        self.address_entry = address_entry
        self.address_new_start = address_start

    def __repr__(self):
        return "%s\n0x%08X\n0x%08X\n0x%08X\n0x%0*X\n" % (self.name, self.address_start, self.address_end, self.address_entry, self.address_new_start)

    def __getitem__(self,index):
        if index == 0:
            return self.name
        elif index == 1:
            return self.address_start
        elif index == 2:
            return self.address_end
        elif index == 3:
            return self.address_entry
        elif index == 4:
            return self.address_entry
        else:
            return self.name