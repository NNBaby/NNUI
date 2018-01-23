import sys
import os
import custom_dataset

PATH = os.path.dirname(__file__)
sys.path.append(PATH)

def get_dataset(name, custom = False):
    if not custom:
        try:
            data = __import__(name)
            return data.x_train, data.y_train, data.x_test, data.y_test
        except Exception as e:
            raise RuntimeError("Build-In Dataset %s could not be found" % name)
    else:
        # custom
        try:
            return custom_dataset.get_custom_dataset(name)
        except Exception as e:
            print (e)
            raise RuntimeError("Custom Dataset %s could not be found" % name)
            
if __name__ == '__main__':
    data = get_dataset('mnist')
    data = get_dataset('./little_dog_cat/', custom = True)