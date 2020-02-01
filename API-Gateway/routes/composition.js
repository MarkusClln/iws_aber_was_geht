const express = require('express');
const router = express.Router();
const bodyParser = require('body-parser');
router.use(bodyParser.json());
const requestPromise = require('request-promise');
const config = require('../config/default.json');


const basketURL= process.env.BASKET_IP + ":" +config.Nodes.basketPORT;
const marketingURL= process.env.MARKETING_IP + ":" + config.Nodes.marketingPORT;
const productURL= process.env.PRODUCT_IP + ":" + config.Nodes.productPORT;
const paymentURL= process.env.PAYMENT_IP + ":" + config.Nodes.paymentPORT;


//Warenkorb anzeigen: GET /basket/<customerId> + GET /product/<productId> + GET /marketing/<productId>
//Checkout: GET /basket/<customerId> + GET /product/<productId> + POST /payment
//????Produkt abfragen: GET /product/<productId> + GET /marketing/<productId>????

//Get-Request der alle Produkte samt Discount zurückgibt
router.get('/allProducts', async function(req, res, next) {
    res.setHeader("Content-Type", "application/json");
    //setze Request Optionen
    const productOptions = {
        method: 'GET',
        uri: "http://" + productURL
    };

    //gemergde Daten von Produktservice und Marketingservice
    //wird returned
    var newProducts = [];

    //Frage alle Produkte ab
    var allProducts = await requestPromise(productOptions)
        .then(function (response) {
            return JSON.parse(response);
        });

    //iteriere über alle zurückgegebene Produkte
    for (var i = 0; i < allProducts.length; i++) {
        var product = allProducts[i];

        //Set request options
        const marketingOptions = {
            method: 'GET',
            uri: "http://"+marketingURL+"/"+product.productId
        };

        //discount
        var discount = 0;

        //send request
        await requestPromise(marketingOptions)
            .then(function (response){
                var marketing = JSON.parse(response);
                for (var j = 0; j < marketing.length; j++) {
                    //check if discount of response is higher then current saved discount
                    if(discount < Number(marketing[j].productDiscount)){
                        discount = Number(marketing[j].productDiscount);
                    }
                }
                //add new attribute productDiscount to product and add it to the return collection
                product.productDiscount = discount;
                newProducts.push(product);
            });
    }
    res.send(newProducts);

});

module.exports = router;