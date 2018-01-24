import keras
from keras.datasets import mnist
(x_train, y_train), (x_test, y_test) = mnist.load_data()
rows, cols = 28, 28
num_classes = 10
# NHWC
x_train = x_train.reshape((x_train.shape[0], rows, cols, 1)) / 255.0
x_test = x_test.reshape((x_test.shape[0], rows, cols, 1)) / 255.0
y_train = keras.utils.np_utils.to_categorical(y_train, num_classes)
y_test = keras.utils.np_utils.to_categorical(y_test, num_classes)