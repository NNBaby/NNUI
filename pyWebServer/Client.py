import requests
from urllib import request
json_pack = dict()
json_pack['obj1'] = 'value1'
json_pack['obj2'] = {"usr":"yurui", "pwd":"12345"}

print('send json package with POST method (if you only need to transfer json package)', json_pack)
r = requests.post("http://127.0.0.1:5000/post",json=json_pack)
print('response from server(POST)', r.json())


print('\n\n','send json package with GET method (if you need to transfer other params, you can use this method)')
r = requests.get("http://127.0.0.1:5000/get/", params={"usr":"yurui", "pwd":"12345"}, json = json_pack)
print('response from server(GET)',r.json())

