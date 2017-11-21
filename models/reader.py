import json
try:
    import Queue
except:
    import queue as Queue

OPERATOR_FIRST_INPUT_NOT_EXISTS = "The input of the first operator must exist"
OPERATOR_INPUT_ERROR = "The key 'input' and 'inputs' cannot exist at the same time"
OPERATOR_NOT_EXISTS = "Operator %s doesn't exist"

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

def read_model(filename):
    fin = open(filename)
    data = json.loads(fin.read())
    ops = data["operators"]
    # graph topology analysis
    in_degrees = dict()
    outputs = dict() # output(s) -> operator
    
    # init degrees to 0
    for op in ops: # reference
        name = get_op_name(op, in_degrees)
        in_degrees[name] = 0
        outputs[name] = name
        if "output" in op:
            outputs[op["output"]] = name
        elif "outputs" in op:
            for o in op["outputs"]:
                outputs[o] = name
        
    # compute in-degrees
    last_op = None
    for op in ops:
        if "input" not in op and "inputs" not in op and op["type"] not in ["Data"]:
            assert last_op is not None, ValueError(OPERATOR_FIRST_INPUT_NOT_EXISTS)
            op["input"] = last_op["name"]
            
        if "input" in op:
            assert "inputs" not in op, ValueError(OPERATOR_INPUT_ERROR)
            ip = op["input"]
            assert ip in outputs, ValueError(OPERATOR_NOT_EXISTS % ip)
            in_degrees[outputs[ip]] += 1
        elif "inputs" in op:
            for ip in op["inputs"]:
                assert ip in outputs, ValueError(OPERATOR_NOT_EXISTS % ip)
                in_degrees[outputs[ip]] += 1
        last_op = op

read_model("LeNet5.json")