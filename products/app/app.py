from flask import Flask, request, Response
import pymongo
from bson.json_util import dumps
import os
import json
from flask_marshmallow import Marshmallow
from marshmallow import Schema, fields

app = Flask(__name__)
ma = Marshmallow(app)
DB_IP = os.getenv('DB_IP')
PORT = os.getenv('PORT')
myclient = pymongo.MongoClient("mongodb://"+DB_IP+":27017/")
mydb = myclient["productcatalog"]
mycol = mydb["product"]


class ProductSchema(Schema):
    """ /api/note - POST

    Parameters:
     - productId (int)
     - productName (str)
     - productDescription (str)
     - productPrice (int)
     - productQuantity (int)
    """
    # the 'required' argument ensures the field exists
    productId = fields.Int(required=True)
    productName = fields.Str(required=True)
    productDescription = fields.Str(required=True)
    productPrice = fields.Int(required=True)
    productQuantity = fields.Int(required=True)

product_schema = ProductSchema()


@app.route('/', methods=['GET', 'POST'])
def product():
    if request.method == 'POST':
        print(request.form)
        errors = product_schema.validate(request.form)
        if errors:
            return "wrong request form"
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



