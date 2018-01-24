from flask import Flask, request, jsonify
import json
from executor import new_model, get_loss
app = Flask(__name__)

@app.route('/get/')
def get_res():
    username = request.args.get('usr')
    if(username != ''):
        print('received parameter usr:',username)

    password = request.args.get('pwd')
    if(password != ''):
        print('received parameter pwd:',password)

    jsonpack = request.json
    print('This is json message :', jsonpack,'\n\n')
    return jsonify(jsonpack)
    
def post_compile(jsonpack):
    print (jsonpack)
    mid = new_model(jsonpack)
    result = {"id" : mid}
    return result
    
def post_result_request(jsonpack):
    num_iter = jsonpack["curlossinfo_send"]["itr"]
    result = {
        "itr" : num_iter,
        "loss" : get_loss(0, num_iter)
    }
    return result
    
def post_connect(jsonpack):
    return "success"
    
POST_FUNCS = {
    "Compile": post_compile,
    "ResultRequest": post_result_request,
    "Connect": post_connect
}    
    
@app.route('/post', methods = ['POST'])
def post():
    jsonpack = request.json

    rtype = jsonpack["request_type"]
    
    if rtype in POST_FUNCS:
        return jsonify(POST_FUNCS[rtype](jsonpack))
    return jsonify("")

if __name__ == "__main__":
    app.run(host = '0.0.0.0', port = 5000, debug = True)