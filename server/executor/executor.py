import multiprocessing
from .reader import *

def worker(dict_info):
    model = Model(mode = "train")
    model.read_dict(dict_info)
    model.train()
    
def new_model(dict_info):
    p = multiprocessing.Process(target = worker, args = (dict_info, ))
    p.start()
    return p.pid