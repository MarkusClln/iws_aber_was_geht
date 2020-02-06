const express = require('express');
const router = express.Router();
const bodyParser = require('body-parser');
router.use(bodyParser.json());
const requestPromise = require('request-promise');
const config = require('../config/default.json');


const basketURL= "http://" +process.env.BASKET_IP + ":" +config.Nodes.basketPORT;
const marketingURL= "http://" +process.env.MARKETING_IP + ":" + config.Nodes.marketingPORT;
const productURL= "http://" +process.env.PRODUCT_IP + ":" + config.Nodes.productPORT;
const paymentURL= "http://" +process.env.PAYMENT_IP + ":" + config.Nodes.paymentPORT;


//Warenkorb anzeigen: GET /basket/<customerId> + GET /product/<productId> + GET /marketing/<productId>
//Checkout: GET /basket/<customerId> + GET /product/<productId> + POST /payment
//????Produkt abfragen: GET /product/<productId> + GET /marketing/<productId>????

router.post('/checkout', async function(req, res, next){
    res.setHeader("Content-Type", "application/json");

    const basketOptions = {
        method: 'GET',
        uri: basketURL + "/" + req.params.customerId
    };

    //GET basket by customerId
    var customerBasket = await requestPromise(basketOptions)
        .then(function (response) {
            return JSON.parse(response);
        });

    var fullPrice = 0;

    //Iterate over all items in basket
    for (var i = 0; i < customerBasket.Items.length; i++) {
        var item = customerBasket.Items[i];

        const productOptions = {
            method: 'GET',
            uri: productURL + "/" + item.productId
        };
        //GET product by id
        var product = await requestPromise(productOptions)
            .then(function (response) {
                return JSON.parse(response);
            });

        //discount
        var discount = getDiscountByProductId(item.productId);

        customerBasket.Items[i].productDiscount = discount;
        customerBasket.Items[i].productName = product.productName;
        customerBasket.Items[i].productDescription = product.productDescription;
        customerBasket.Items[i].productPrice = product.productPrice;
        customerBasket.Items[i].productQuantity = product.productQuantity;

        fullPrice += (product.productPrice - discount);
    }

    const paymentOptions = {
        method: 'POST',
        uri: paymentURL + "/",
        body: {
            items = customerBasket.Items,
            billNumber = "123456789",
            customerId = req.params.customerId,
            price = fullPrice
        }
    };

    await requestPromise.post(paymentOptions)
        .then(function (response) {
            return response;
        });
});



router.get('/showBasket/:customerId', async function(req, res, next){
    res.setHeader("Content-Type", "application/json");

    const basketOptions = {
        method: 'GET',
        uri: basketURL + "/" + req.params.customerId
    };

    //GET basket by customerId
    var customerBasket = await requestPromise(basketOptions)
        .then(function(response){
            return JSON.parse(response);
        });

    //Iterate over all items in basket
    for(var i = 0; i<customerBasket.Items.length; i++){
        var item = customerBasket.Items[i];

        const productOptions = {
            method: 'GET',
            uri: productURL + "/" + item.productId
        };
        //GET product by id
        var product = await requestPromise(productOptions)
            .then(function(response){
                return JSON.parse(response);
            });

            // const marketingOptions = {
            //     method: 'GET',
            //     uri: "http://" + marketingURL + "/" + item.productId
            // };

        //discount
        var discount = getDiscountByProductId(item.productId);
        customerBasket.Items[i].productDiscount = discount;
        customerBasket.Items[i].productName = product.productName;
        customerBasket.Items[i].productDescription = product.productDescription;
        customerBasket.Items[i].productPrice = product.productPrice;
        customerBasket.Items[i].productQuantity = product.productQuantity;
    }
    res.send(customerBasket);
});

//Get-Request der alle Produkte samt Discount zurückgibt
router.get('/allProducts', async function(req, res, next) {
    res.setHeader("Content-Type", "application/json");
    //setze Request Optionen
    const productOptions = {
        method: 'GET',
        uri: productURL
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
            uri: marketingURL+"/"+product.productId
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

function getDiscountByProductId(productId){
    const marketingOptions = {
        method: 'GET',
        uri: "http://" + marketingURL + "/" + productId
    };

    var discount = 0;
    //Get marketingcampaigns by productid
    await requestPromise(marketingOptions)
    .then(function (response){
        var marketing = JSON.parse(response);
        for (var j = 0; j < marketing.length; j++) {
            //check if discount of response is higher then current saved discount
            if(discount < Number(marketing[j].productDiscount)){
                discount = Number(marketing[j].productDiscount);
            }
        }
        
        return discount;
    });
}


module.exports = router;