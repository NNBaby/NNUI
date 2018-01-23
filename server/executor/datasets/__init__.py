import sys
import os

PATH = os.path.dirname(__file__)
sys.path.append(PATH)

import custom_dataset

BUILDIN_DATASETS = {
    'MNIST' : 'mnist',
    'CIFAR-10' : 'cifar10',
    'CIFAR-100' : 'cifar100'
}

def get_dataset(name, custom = False):
    if not custom:
        try:
            data = __import__(name)
            return data.x_train, data.y_train, data.x_test, data.y_test
        except Exception as e:
            print (e)
            raise RuntimeError("Build-In Dataset %s could not be found" % name)
    else:
        # custom
        try:
            return custom_dataset.get_custom_dataset(name)
        except Exception as e:
            print (e)
            raise RuntimeError("Custom Dataset %s could not be found" % name)
            
if __name__ == '__main__':
    for name in BUILDIN_DATASETS.values():
        print ("Load %s" % name)
        data = get_dataset(name)
    print ("Load Custom Dataset")
    data = get_dataset('./little_dog_cat/', custom = True)