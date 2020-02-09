from flask import Flask, request, Response
import pymongo
from bson.json_util import dumps
import os

app = Flask(__name__)
DB_IP = os.getenv('DB_IP')
PORT = os.getenv('PORT')
myclient = pymongo.MongoClient("mongodb://"+DB_IP+":27017/")
mydb = myclient["payment"]
mycol = mydb["payments"]


@app.route('/', methods=['GET', 'POST'])
def payment():
    if request.method == 'POST':

        req_data = request.get_json()

        if 'items' not in req_data:
            return "items is missing"
        if 'billNumber' not in req_data:
            return "billNumber is missing"
        if 'customerId' not in req_data:
            return "customerId is missing"
        if 'price' not in req_data:
            return "price is missing"

        mycol.insert_one(request.json)
        return "done"
    if request.method == 'GET':
        js = dumps(mycol.find())
        resp = Response(js, status=200, mimetype='application/json')
        return resp

@app.route('/<id>', methods=['GET'])
def getId(id):
    if request.method == 'GET':
        js = dumps(mycol.find_one({"customerId": int(id)}))
        resp = Response(js, status=200, mimetype='application/json')
        return resp


if __name__ == '__main__':
    app.run(host='0.0.0.0', port=PORT)