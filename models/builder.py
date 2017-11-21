import keras.layers as L

def FC(x, **argv):
    return L.Dense(argv["dim_out"])(x)
    
def Conv(x, **argv):
    return L.Conv2D(filters = argv["dim_out"],
                    kernel_size = argv["kernel"],
                    strides = argv.get("strides", 1),
                    padding = argv.get("padding", 0))(x)
                    
def Pool(x, **argv):
    if argv["pool"] == "MAX":
        return L.MaxPooling2D(pool_size = argv["kernel"],
                              strides = argv.get("strides", 1))(x)
    elif argv["pool"] == "AVE":
        return L.AveragePooling2D(pool_size = argv["kernel"],
                              strides = argv.get("strides", 1))(x)
    raise ValueError("The pool operator must have pool type.")
    
def ReLU(x, **argv):
    return L.Activation("relu")    

def build_keras_model(topo):
    return None