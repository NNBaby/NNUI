import sys
import os

PATH = os.path.dirname(__file__)
sys.path.append(PATH)

def get_dataset(name):
    try:
        data = __import__(name)
        return data.x_train, data.y_train, data.x_test, data.y_test
    except Exception as e:
        raise RuntimeError("Dataset %s could not be found" % name)
    
if __name__ == '__main__':
    data = get_dataset('mnist')