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
    return [L.Input(shape = shape) for shape in argv["shapes"]]

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
    
def build_keras_model(info, topo, mode):

    # configure data operators
    name2op = dict()
    for op in topo:
        name2op[op["name"]] = op
    mode_info = info[mode]
    inputs = mode_info["inputs"]
    for op_name, data in inputs.items():
        op = name2op[op_name]
        op["shapes"] = data["shapes"]
        op["batch_size"] = data["batch_size"]
        
    xs = dict()
    for op in topo:
        inputs = [xs[name] for name in op["inputs"]]
        op_func = OP_MAP[op["type"]]
        outputs = op_func(*inputs, **op)
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
    inputs = [xs[name] for name in inputs_info]
    outputs = [xs[name] for name in outputs_info]
    
    model = Model(inputs = inputs, outputs = outputs)
    
    optimizer = mode_info.get("optimizer", "sgd")
    loss = [output["loss"] for output in outputs_info.values()]
    loss_weights = [output.get("loss_weight", 1.0) for output in outputs_info.values()]
    metrics = dict()
    for op_name, info in outputs_info.items():
        if "metrics" in info:
            metrics[op_name] = info["metrics"]
        
    model.compile(optimizer = optimizer, loss = loss, loss_weights = loss_weights, metrics = metrics)
    return model
    