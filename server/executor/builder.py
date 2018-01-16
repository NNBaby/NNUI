from keras import layers as L
from keras.models import Model
BABYNET_BACKEND = "keras"

print ("Using %s backend" % BABYNET_BACKEND)
    
OPERATOR_NO_INPUT = "There is no input in this operator"

def build_keras_model(info, topo, mode):
    mode_info = info[mode]

    xs = dict()
    for op in topo:
        inputs = [xs[name] for name in op["inputs"]]
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

BUILDING_BACKEND = {
    "keras" : build_keras_model,
}

def build_model(info, topo, mode):
    assert BABYNET_BACKEND in BUILDING_BACKEND, "Unknown Backend: %s" % BABYNET_BACKEND
    return BUILDING_BACKEND[BABYNET_BACKEND](info, topo, mode)
