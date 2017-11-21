import keras.layers as L
from keras import backend as K

def get_mnist():
    # mnist
    from keras.datasets import mnist
    (x_train, y_train), (x_test, y_test) = mnist.load_data()
    rows, cols = 28, 28
    if K.image_data_format() == 'channels_first':
        # NCHW
        x_train = x_train.reshape((x_train.shape[0], 1, rows, cols))
        x_test = x_test.reshape((x_test.shape[0], 1, rows, cols))
    else:
        # NHWC
        x_train = x_train.reshape((x_train.shape[0], rows, cols, 1))
        x_test = x_test.reshape((x_test.shape[0], rows, cols, 1))

def Data(x = None, **argv):
    if argv["dataset"] == "mnist":
        channels, rows, cols = 1, 28, 28
    else:
        raise ValueError("Unknown dataset :-(")
    shape = (channels, rows, cols) if K.image_data_format() == 'channels_first' else (rows, cols, channels)
    return [L.Input(shape = shape), L.Input(shape = (1,))]

def FC(x, **argv):
    if len(x.get_shape()) > 2:
        x = L.Flatten()(x)
    return L.Dense(argv["dim_out"])(x)
    
def Conv(x, **argv):
    return L.Conv2D(filters = argv["dim_out"],
                    kernel_size = argv["kernel"],
                    strides = argv.get("stride", 1),
                    padding = argv.get("padding", "valid"))(x)
                    
def Pool(x, **argv):
    if argv["pool"] == "MAX":
        return L.MaxPooling2D(pool_size = argv["kernel"],
                              strides = argv.get("stride", 1))(x)
    elif argv["pool"] == "AVE":
        return L.AveragePooling2D(pool_size = argv["kernel"],
                              strides = argv.get("stride", 1))(x)
    raise ValueError("The pool operator must have pool type.")
    
def ReLU(x, **argv):
    return L.Activation("relu")(x) 

def Softmax(x, **argv):
    return L.Activation("softmax")(x)
    
    
OP_MAP = {
    "Data": Data,
    "FC": FC,
    "Conv": Conv,
    "Pool": Pool,
    "ReLU": ReLU,
    "Softmax": Softmax
}
    
def build_keras_model(topo):
    xs = dict()
    for op in topo:
        inputs = [xs[name] for name in op["inputs"]]
        op_func = OP_MAP[op["type"]]
        outputs = op_func(*inputs, **op)
        if type(outputs) is not list:
            outputs = [outputs]
        for name, output in zip(op["outputs"], outputs):
            xs[name] = output
    return xs