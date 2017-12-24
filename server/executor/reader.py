import json
try:
    import Queue
except:
    import queue as Queue
    
import keras
import sys
import socket
from . import builder

OPERATOR_FIRST_INPUT_NOT_EXISTS = "The input of the first operator must exist"
OPERATOR_INPUT_ERROR = "The key 'input' and 'inputs' cannot exist at the same time"
OPERATOR_OUTPUT_ERROR = "The key 'output' and 'outputs' cannot exist at the same time"
OPERATOR_NOT_EXISTS = "Operator %s doesn't exist"
OPERATOR_TOPO_ERROR = "Operator topology errors"
OPERATOR_LOOP_ERROR = "Error:-( There is a cycle in the graph."

'''
class RedirectStdOut:
    ADDRESS = ("127.0.0.1", 3939)
    def __init__(self):
        self.socket = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
        self.buffer = ""
    def write(self, text):
        self.buffer += text
    def flush(self):
        self.socket.sendto(self.buffer.encode("UTF-8"), self.ADDRESS)
        self.buffer = ""
        
sys.stdout = RedirectStdOut()
'''

class LogRedirectCallback(keras.callbacks.Callback):

    def __init__(self, nnbaby_model):
        super(LogRedirectCallback, self).__init__()
        self.nnbaby_model = nnbaby_model

    def on_train_begin(self, logs={}):
        pass

    def on_batch_end(self, batch, logs={}):
        print (logs)
        self.nnbaby_model.write_logs(logs)

class Model:
    def __init__(self, mode = "train"):
        self.mode = mode
        self.info = None
        self.topo = None
        self.model = None
        self.logger = []
    def open(self, filename):
        self.info = self.read_json(filename)
        self.read_dict(self.info)
    def read_json(self, string):
        self.info = json.loads(string)
        self.read_dict(self.info)
    def read_csjs(self, string): 
        def read_json_from_cs(self, s):
            js = s.strip("\" ").replace("\\\"", "\"")
            return json.loads(js)
        self.info = self.read_json_from_cs(string)  
        self.read_dict(self.info)
    def read_dict(self, dict_data):
        self.info = dict_data
        self.topo = self.get_model_topo(self.info)
        self.model = builder.build_keras_model(self.info, self.topo, self.mode)
    def open_json(self, filename):
        fin = open(filename)
        return json.loads(fin.read())

    def get_model_topo(self, info):
        ops = info["operators"]
        # graph topology analysis
        in_degrees = dict()
        outputs = dict() # output(s) -> operator
        name2op = dict() # op_name -> op
        
        # init degrees to 0
        for op in ops: # reference
            name = self.get_op_name(op, in_degrees)
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
            if op["optype"] not in ["Data", "Input"]:
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
        
    # auto name if name is None    
    def get_op_name(self, op, names):
        if "name" not in op:
            # auto name
            type_name = op["optype"]
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
    def write_logs(self, logs):
        self.logger.append(logs)
    def train(self):
        import keras
        from keras.datasets import mnist
        mode_info = self.info[self.mode]
        (x_train, y_train), (x_test, y_test) = mnist.load_data()
        rows, cols = 28, 28
        num_classes = 10
        # NHWC
        x_train = x_train.reshape((x_train.shape[0], rows, cols, 1)) / 255.0
        x_test = x_test.reshape((x_test.shape[0], rows, cols, 1)) / 255.0
        y_train = keras.utils.np_utils.to_categorical(y_train, num_classes)
        y_test = keras.utils.np_utils.to_categorical(y_test, num_classes)
        self.model.fit(x = {"data": x_train}, y = {"pred": y_train},
                       batch_size = mode_info["batch_size"], epochs = mode_info["epochs"],
                       callbacks = [LogRedirectCallback(nnbaby_model = self)], verbose = 0)
        
    def test(self):
        pass
        # preds = model.model.evaluate(x=X_test,y=Y_test)

if __name__ == "__main__":    
    model = Model(mode = "train")
    mdoel.open("LeNet5-keras.json")
    #fin = open("LeNetCS.json")
    #buf = fin.read()
    #model = Model(mode = "train", string = buf)
    model.train()
