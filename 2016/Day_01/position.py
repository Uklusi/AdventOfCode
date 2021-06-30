from copy import copy

class Position:
    def __init__(self, x=0, y=0, orientation=0):
        self.x = x
        self.y = y
        self.orientation = orientation

    def turnRight(self):
        self.orientation = (self.orientation + 1) % 4
    
    def turnLeft(self):
        self.orientation = (self.orientation - 1) % 4
    
    def turn(self, direction):
        if direction in ["R", "r", "1", 1]:
            self.turnRight()
        elif direction in ["L", "l", "-1", -1]:
            self.turnLeft()
        elif direction in ["0", 0]:
            pass
        else:
            raise("DirectionError")
    
    def move(self, n, direction=None):
        if direction is None:
            direction = self.orientation
        if direction == 0:
            self.y += n
        elif direction == 1:
            self.x += n
        elif direction == 2:
            self.y -= n
        elif direction == 3:
            self.x -= n
        else:
            raise("DirectionError")

    def __add__(self, other):
        return Position(self.x + other.x, self.y + other.y, self.orientation)
    
    def current(self):
        return (self.x, self.y)

    def __hash__(self):
        return hash((self.x, self.y))
    
    def __eq__(self, other):
        return (self.x, self.y) == (other.x, other.y)

    def copy(self):
        return copy(self)