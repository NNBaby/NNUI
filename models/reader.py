import json

fin = open("LeNet5.json")
data = json.loads(fin.read())
print (data)