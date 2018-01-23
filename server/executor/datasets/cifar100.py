import keras
from keras.datasets import cifar100
(x_train, y_train), (x_test, y_test) = cifar100.load_data()

rows, cols = 32, 32
num_classes = 100

x_train = x_train.reshape((x_train.shape[0], rows, cols, 3)) / 255.0
x_test = x_test.reshape((x_test.shape[0], rows, cols, 3)) / 255.0
y_train = keras.utils.to_categorical(y_train, num_classes)
y_test = keras.utils.to_categorical(y_test, num_classes)