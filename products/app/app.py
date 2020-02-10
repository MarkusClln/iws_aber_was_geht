from flask import Flask, request, Response
import pymongo
from bson.json_util import dumps
import os
import json

app = Flask(__name__)
DB_IP = os.getenv('DB_IP')
PORT = os.getenv('PORT')
myclient = pymongo.MongoClient("mongodb://"+DB_IP+":27017/")
mydb = myclient["productcatalog"]
mycol = mydb["product"]


@app.route('/', methods=['GET', 'POST'])
def product():
    if request.method == 'POST':
        req_data = request.get_json()

        if 'productId' not in req_data:
            return "productId is missing"
        if 'productName' not in req_data:
            return "productName is missing"
        if 'productDescription' not in req_data:
            return "productDescription is missing"
        if 'productPrice' not in req_data:
            return "productPrice is missing"
        if 'productQuantity' not in req_data:
            return "productQuantity is missing"

        mycol.insert_one(request.json)
        return "done"
        
    if request.method == 'GET':
        js = dumps(mycol.find())
        resp = Response(js, status=200, mimetype='application/json')
        return resp


@app.route('/<id>', methods=['GET'])
def getId(id):
    if request.method == 'GET':
        js = dumps(mycol.find_one({"productId": int(id)}))
        resp = Response(js, status=200, mimetype='application/json')
        return resp


if __name__ == '__main__':
    app.run(host='0.0.0.0', port=PORT)



