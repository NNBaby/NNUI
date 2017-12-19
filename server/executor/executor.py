import multiprocessing
import threading
from .reader import *

def pipe_thread(pipe, model):
    while 1:
        data = pipe.recv()
        dtype = data["type"]
        if dtype == "get_loss":
            num_iter = data["iter"]
            if num_iter < len(model.logger):
                pipe.send(model.logger[num_iter]["loss"])
            else:
                pipe.send(-1)    
    
def worker(dict_info, pipe):
    model = Model(mode = "train")
    t = threading.Thread(target = pipe_thread, args = (pipe, model))
    t.start()
    model.read_dict(dict_info)
    model.train()


pipes = {}
PID = None
def new_model(dict_info):
    mg = multiprocessing.Manager()
    pipe = multiprocessing.Pipe()
    p = multiprocessing.Process(target = worker, args = (dict_info, pipe[0]))
    p.start()
    pipes[p.pid] = pipe[1]
    global PID
    PID = p.pid
    return p.pid
    
def get_loss(model_id, num_iter):
    model_id = PID
    pipe = pipes[model_id]
    pipe.send({"type": "get_loss", "iter": num_iter}) 
    s = pipe.recv()
    return float(s)