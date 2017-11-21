import json
try:
    import Queue
except:
    import queue as Queue
import builder

OPERATOR_FIRST_INPUT_NOT_EXISTS = "The input of the first operator must exist"
OPERATOR_INPUT_ERROR = "The key 'input' and 'inputs' cannot exist at the same time"
OPERATOR_OUTPUT_ERROR = "The key 'output' and 'outputs' cannot exist at the same time"
OPERATOR_NOT_EXISTS = "Operator %s doesn't exist"
OPERATOR_TOPO_ERROR = "Operator topology errors"
OPERATOR_LOOP_ERROR = "Error:-( There is a cycle in the graph."

# auto name if name is None    
def get_op_name(op, names):
    if "name" not in op:
        # auto name
        type_name = op["type"]
        # Add, Add_1, Add_2
        if type_name not in names:
            op["name"] = type_name
        else:
            i = 1
            while True:
                name = "%s_%d" % (type_name, i)
                if name not in names:
                    break
                i += 1
            op["name"] = name
    return op["name"]

def get_model_topo(filename):
    fin = open(filename)
    data = json.loads(fin.read())
    ops = data["operators"]
    # graph topology analysis
    in_degrees = dict()
    outputs = dict() # output(s) -> operator
    name2op = dict() # op_name -> op
    
    # init degrees to 0
    for op in ops: # reference
        name = get_op_name(op, in_degrees)
        in_degrees[name] = 0
        outputs[name] = name
        if "output" in op:
            assert "outputs" not in op, ValueError(OPERATOR_OUTPUT_ERROR)
            op["outputs"] = [op["output"]]
            outputs[op["output"]] = name
        elif "outputs" in op:
            for o in op["outputs"]:
                outputs[o] = name
        else:
            # no output
            op["outputs"] = [name]
        name2op[name] = op
        
    # compute in-degrees
    last_op = None
    for op in ops:
        if op["type"] not in ["Data"]:
            if "input" not in op and "inputs" not in op:
                assert last_op is not None, ValueError(OPERATOR_FIRST_INPUT_NOT_EXISTS)
                op["inputs"] = [last_op["name"]]
        else:
            op["inputs"] = []
            
        # unite 'input' to 'inputs'
        if "input" in op:
            assert "inputs" not in op, ValueError(OPERATOR_INPUT_ERROR)
            op["inputs"] = [op["input"]]        
        
        if "inputs" in op:
            for ip in op["inputs"]:
                assert ip in outputs, ValueError(OPERATOR_NOT_EXISTS % ip)
                in_degrees[outputs[ip]] += 1
        last_op = op

    # compute topology graph
    q = Queue.Queue()
    for op_name, degree in in_degrees.items():
        if degree == 0:
            q.put(op_name)
    topo_inv = []
    vis = set()
    while not q.empty():
        op_name = q.get()
        assert op_name not in vis, ValueError(OPERATOR_LOOP_ERROR)
        vis.add(op_name) # set visit flag for avoiding cycle in the graph
        topo_inv.append(op_name)
        for ip_name in name2op[op_name]["inputs"]:
            op_name = outputs[ip_name]
            in_degrees[op_name] -= 1
            if in_degrees[op_name] == 0:
                q.put(ip_name)
    assert len(topo_inv) == len(in_degrees), ValueError(OPERATOR_TOPO_ERROR)
    topo = [name2op[name] for name in topo_inv[::-1]]
    return topo
    
def get_model_symbol(topo):
    return builder.build_keras_model(topo)
    
def read_model(filename):
    topo = get_model_topo(filename)
    sym = get_model_symbol(topo)
    return sym
        
sym = read_model("LeNet5.json")
print (sym)