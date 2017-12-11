import keras.layers as L
from keras import backend as K
from keras.models import Model

OPERATOR_NO_INPUT = "There is no input in this operator"

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
    # the default shape is NHWC
    # if K.image_data_format() != 'channels_first':
    assert "shapes" in argv, ValueError(OPERATOR_NO_INPUT)
    return [L.Input(name = argv.get("name"), shape = shape) for shape in argv["shapes"]]

def FC(x, **argv):
    if len(x.get_shape()) > 2:
        x = L.Flatten()(x)
    return L.Dense(argv["dim_out"])(x)
    
def Conv(x, **argv):
    return L.Conv2D(name = argv.get("name"), 
                    filters = argv["dim_out"],
                    kernel_size = argv["kernel"],
                    strides = argv.get("stride", 1),
                    padding = argv.get("padding", "valid"))(x)
                    
def Pool(x, **argv):
    if argv["pool"] == "MAX":
        return L.MaxPooling2D(name = argv.get("name"), 
                              pool_size = argv["kernel"],
                              strides = argv.get("stride", 1))(x)
    elif argv["pool"] == "AVE":
        return L.AveragePooling2D(name = argv.get("name"), 
                                  pool_size = argv["kernel"],
                                  strides = argv.get("stride", 1))(x)
    raise ValueError("The pool operator must have pool type.")
    
def ReLU(x, **argv):
    return L.Activation(name = argv.get("name"), activation = "relu")(x) 

def Softmax(x, **argv):
    return L.Activation(name = argv.get("name"), activation = "softmax")(x)
    
    
OP_MAP = {
    "Data": Data,
    "FC": FC,
    "Conv": Conv,
    "Pool": Pool,
    "ReLU": ReLU,
    "Softmax": Softmax
}

def read_dataset(name):
    dataset = dict()
    if name == "mnist":
        from keras.datasets import mnist
        (x_train, y_train), (x_test, y_test) = mnist.load_data()
        rows, cols = 28, 28
        num_classes = 10
        # NHWC
        x_train = x_train.reshape((x_train.shape[0], rows, cols, 1))
        x_test = x_test.reshape((x_test.shape[0], rows, cols, 1))
        y_train = keras.utils.np_utils.to_categorical(y_train, num_classes)
        y_test = keras.utils.np_utils.to_categorical(y_test, num_classes)
        dataset["x_train"] = x_train
        dataset["y_train"] = y_train
        dataset["x_test"] = x_test
        dataset["y_test"] = y_test
    return dataset
    
def build_keras_model(info, topo, mode):
    # configure data operators
    name2op = dict()
    for op in topo:
        name2op[op["name"]] = op
    mode_info = info[mode]
    # inputs = mode_info["inputs"]

    xs = dict()
    for op in topo:
        inputs = [xs[name] for name in op["inputs"]]
        #op_func = OP_MAP[op["type"]]
        #outputs = op_func(*inputs, **op)
        op_func = L.__getattribute__(op["optype"])
        info = op.copy()
        for s in ["optype", "input", "inputs", "output", "outputs"]:
            if s in info:
                del info[s]
        outputs = op_func(**info)
        if len(inputs) > 0:
            outputs = outputs(*inputs)
        if type(outputs) is not list:
            outputs = [outputs]
        for name, output in zip(op["outputs"], outputs):
            xs[name] = output
    
    loss_ops = []
    for op in topo:
        if "loss" in op:
            loss_ops.append(op)
            if "loss_weight" not in op:
                op["loss_weight"] = 1.0
    
    # create model
    inputs_info = mode_info["inputs"]
    outputs_info = mode_info["outputs"]
    inputs = [xs[p["name"]] for p in inputs_info]
    outputs = [xs[p["name"]] for p in outputs_info]
    
    model = Model(inputs = inputs, outputs = outputs)
    
    optimizer = mode_info.get("optimizer", "sgd")
    loss = [output["loss"] for output in outputs_info]
    loss_weights = [output.get("loss_weight", 1.0) for output in outputs_info]
    metrics = dict()
    for info in outputs_info:
        op_name = info["name"]
        if "metrics" in info:
            metrics[op_name] = info["metrics"]
        
    model.compile(optimizer = optimizer, loss = loss, loss_weights = loss_weights, metrics = metrics)
    return model
    