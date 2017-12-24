import json
import requests
from urllib import request

ADDRESS = "http://127.0.0.1:5000"

def test_login():
    json_pack = dict()
    json_pack['obj1'] = 'value1'
    json_pack['obj2'] = {"usr":"NNBaby", "pwd":"12345"}

    print('send json package with POST method (if you only need to transfer json package)', json_pack)
    r = requests.post("%s/post" % ADDRESS ,json=json_pack)
    print('response from server(POST)', r.json())


    print('\n\n','send json package with GET method (if you need to transfer other params, you can use this method)')
    r = requests.get("%s/get/" % ADDRESS, params={"usr":"yurui", "pwd":"12345"}, json = json_pack)
    print('response from server(GET)',r.json())

def test_model():
    fin = open("LeNet5-keras.json")
    json_data = json.loads(fin.read())
    json_data["request_type"] = "Compile"
    r = requests.post("%s/post" % ADDRESS, json = json_data)
    print('response from server(POST)', r.json())
    
if __name__ == "__main__":
    test_model()