import json
import requests
import os
from urllib import request

ADDRESS = "http://127.0.0.1:5000"
PATH = os.path.dirname(__file__)

def test_model_compile():
    fname = os.path.join(PATH, "LeNet5-keras.json")
    fin = open(fname)
    json_data = json.loads(fin.read())
    json_data["request_type"] = "Compile"
    r = requests.post("%s/post" % ADDRESS, json = json_data)
    print('response from server(POST)', r.json())
    assert "id" in r.json()

def test_result_request():
    json_data = {
            "request_type": "ResultRequest",
            "curlossinfo_send" : {"itr" : 3}
    }
    r = requests.post("%s/post" % ADDRESS, json = json_data)
    j = r.json()
    itr = int(j["itr"])
    loss = float(j["loss"])

def test_connect():
    json_data = {
            "request_type": "Connect"
    }
    r = requests.post("%s/post" % ADDRESS, json = json_data)
    assert r.json() == "success"
