const express = require('express');
const router = express.Router();
const bodyParser = require('body-parser');
router.use(bodyParser.json());
const requestPromise = require('request-promise');
const config = require('../config/default.json');

//urls for every service
const basketURL= "http://" +process.env.BASKET_IP + ":" +config.Nodes.basketPORT;
const marketingURL= "http://" +process.env.MARKETING_IP + ":" + config.Nodes.marketingPORT;
const productURL= "http://" +process.env.PRODUCT_IP + ":" + config.Nodes.productPORT;
const paymentURL= "http://" +process.env.PAYMENT_IP + ":" + config.Nodes.paymentPORT;

//POST-Request: Buy the stuff from the basket with given customerId
router.post('/checkout/:customerId', async function(req, res, next){
    res.setHeader("Content-Type", "application/json");

    //Set request options for basketservice
    const basketOptions = {
        method: 'GET',
        uri: basketURL + "/" + req.params.customerId
    };

    //Get basket by customerId
    var customerBasket = await requestPromise(basketOptions)
        .then(function (response) {
            return JSON.parse(response);
        });

    //field for calculating fullprice
    var fullPrice = 0;

    //Iterate over all items in basket
    for (var i = 0; i < customerBasket.items.length; i++) {
        var item = customerBasket.items[i];

        //Set request options for productservice
        const productOptions = {
            method: 'GET',
            uri: productURL + "/" + item.productId
        };

        //Get product by id
        var product = await requestPromise(productOptions)
            .then(function (response) {
                return JSON.parse(response);
            });

        //Set request options for marketingservice
        const marketingOptions = {
            method: 'GET',
            uri: marketingURL + "/" + product.productId
        };

        //get the highest discount
        var discount = 0;

        //Get marketingcampaigns by productid
        await requestPromise(marketingOptions)
            .then(function (response){
                //parse response to json
                var campaigns = JSON.parse(response);

                 //iterate over all campaigns
                for (var j = 0; j < campaigns.length; j++) {
                    //check if discount of response is higher then current saved discount
                    if(discount < Number(campaigns[j].productDiscount)){
                        discount = Number(campaigns[j].productDiscount);
                    }
                }
            });

        //Add value to fullPrice
        var calc = ((product.productPrice - discount) * item.count);
        fullPrice += calc;
    }

    //remove unneccessary field customerBasketId
    for (var j = 0; j < customerBasket.items.length; j++) {
        delete customerBasket.items[j].customerBasketId;
    }

    //Set request options for paymentservice
    const paymentOptions = {
        method: 'POST',
        uri: paymentURL + "/",
        headers: {
            'Content-Type': 'application/json; charset=utf-8'
        },
        json: true,
        body: {
            items : customerBasket.items,
            billNumber : Math.floor(Math.random() * Math.floor(9999999)),
            customerId : parseInt(req.params.customerId),
            price : fullPrice
        }
    };

    //Send request
    await requestPromise.post(paymentOptions)
        .then(function (response) {
            return response;
        });

    //Return success message
    res.send("Created payment");
});

//GET-Request: Get the basket by given customerId
router.get('/showBasket/:customerId', async function(req, res, next){
    res.setHeader("Content-Type", "application/json");

    //Set request options for basketservice
    const basketOptions = {
        method: 'GET',
        uri: basketURL + "/" + req.params.customerId
    };

    //Get the basket by customerId
    const customerBasket = await requestPromise(basketOptions)
        .then(function(response){
            return JSON.parse(response);
        });

    //Basket with additional informations in item collection
    var basketWithAdditionalData = customerBasket;
    
    //Iterate over all items in basket
    for(var i = 0; i<customerBasket.items.length; i++){
        var item = customerBasket.items[i];
        
        //Set request options for productservice
        const productOptions = {
            method: 'GET',
            uri: productURL + "/" + item.productId
        };

        //Get product by id
        var product = await requestPromise(productOptions)
            .then(function(response){
                return JSON.parse(response);
            });

        //Set request options for marketingservice
        const marketingOptions = {
            method: 'GET',
            uri: marketingURL + "/" + product.productId
        };

        //get the highest discount
        var discount = 0;

        //Get marketingcampaigns by productid
        await requestPromise(marketingOptions)
            .then(function (response){
                //parse response to json
                var campaigns = JSON.parse(response);

                 //iterate over all campaigns
                for (var j = 0; j < campaigns.length; j++) {
                    //check if discount of response is higher then current saved discount
                    if(discount < Number(campaigns[j].productDiscount)){
                        discount = Number(campaigns[j].productDiscount);
                    }
                }
            });

        //Set additional fields with desired values
        basketWithAdditionalData.items[i].productDiscount = discount;
        basketWithAdditionalData.items[i].productName = product.productName;
        basketWithAdditionalData.items[i].productDescription = product.productDescription;
        basketWithAdditionalData.items[i].productPrice = product.productPrice;
        basketWithAdditionalData.items[i].productQuantity = product.productQuantity;
    }

    //return basket with additional data
    res.send(basketWithAdditionalData);
});

//GET-Request: Get all products with their current discount
router.get('/allProducts', async function(req, res, next) {
    res.setHeader("Content-Type", "application/json");
    
    //Set request options for productservice
    const productOptions = {
        method: 'GET',
        uri: productURL
    };

    //Get all products
    var allProducts = await requestPromise(productOptions)
        .then(function (response) {
            return JSON.parse(response);
        });

    //Save product with additional discount-field in here
    var newProducts = [];

    //Iterate over all products
    for (var i = 0; i < allProducts.length; i++) {
        var product = allProducts[i];

        //Set request options for marketingservice
        const marketingOptions = {
            method: 'GET',
            uri: marketingURL+"/"+product.productId
        };

        //get the highest discount
        var discount = 0;

        //Get all marketing campaigns
        await requestPromise(marketingOptions)
            .then(function (response){
                //parse the repsonse to json
                var campaigns = JSON.parse(response);

                //iterate over all campaigns
                for (var j = 0; j < campaigns.length; j++) {
                    //check if discount of response is higher then current saved discount
                    if(discount < Number(campaigns[j].productDiscount)){
                        discount = Number(campaigns[j].productDiscount);
                    }
                }
                //add new attribute productDiscount to product and add it to the return collection
                product.productDiscount = discount;
                newProducts.push(product);
            });
    }
    res.send(newProducts);

});


router.get("/seedData", async function (req, res, next) {

    const productOptions = {
        method: 'POST',
        uri: productURL + "/",
        headers: {
            'Content-Type': 'application/json; charset=utf-8'
        },
        json: true,
        body: {
            productId: 1,
            productName: "Produkt_1",
            productDescription: "Ein tolles erstes Produkt",
            productPrice: 10,
            productQuantity: 50
        }
    };

    await requestPromise.post(productOptions)
        .then(function (response) {
           console.log(response);
        });


    productOptions.body = {
        productId: 2,
        productName: "Produkt_2",
        productDescription: "Ein tolles zweites Produkt",
        productPrice: 20,
        productQuantity: 30
    };


    await requestPromise.post(productOptions)
        .then(function (response) {
            console.log(response);
        });

       productOptions.body = {
           productId: 3,
           productName: "Produkt_3",
           productDescription: "Ein tolles drittes Produkt",
           productPrice: 30,
           productQuantity: 78
       };

    await requestPromise.post(productOptions)
           .then(function (response) {
               console.log(response);
           });

           productOptions.body = {
            productId: 4,
            productName: "Produkt_4",
            productDescription: "Ein tolles viertes Produkt",
            productPrice: 15,
            productQuantity: 124
        };
    
    
        await requestPromise.post(productOptions)
            .then(function (response) {
                console.log(response);
            });
            await requestPromise.post(productOptions)
            .then(function (response) {
                console.log(response);
            });
 
            productOptions.body = {
             productId: 5,
             productName: "Produkt_5",
             productDescription: "Ein tolles fünftes Produkt",
             productPrice: 150,
             productQuantity: 5
         };
     
     
         await requestPromise.post(productOptions)
             .then(function (response) {
                 console.log(response);
             });




       const marketingOptions = {
           method: 'POST',
           uri: marketingURL + "/",
           headers: {
               'Content-Type': 'application/json; charset=utf-8'
           },
           json: true,
           body: {
               marketingId: 1,
               productId: 1,
               productDiscount: 5
           }
       };

    await requestPromise.post(marketingOptions)
           .then(function (response) {
               console.log(response);
           });

       marketingOptions.body = {
           marketingId: 2,
           productId: 3,
           productDiscount: 1
       };

    await requestPromise.post(marketingOptions)
           .then(function (response) {
               console.log(response);
           });

           marketingOptions.body = {
            marketingId: 3,
            productId: 5,
            productDiscount: 25
        };
 
     await requestPromise.post(marketingOptions)
            .then(function (response) {
                console.log(response);
            });


    const basketOptions = {
        method: 'POST',
        uri: basketURL + "/1",
        headers: {
            'Content-Type': 'application/json; charset=utf-8'
        },
        json: true,
        body: {
            productId: 1
        }
    };

    await requestPromise.post(basketOptions)
        .then(function (response) {
            console.log(response);
        });

        basketOptions.body={
            productId: 1
        }
        await requestPromise.post(basketOptions)
        .then(function (response) {
            console.log(response);
        });
        basketOptions.body={
            productId: 5
        }
        await requestPromise.post(basketOptions)
        .then(function (response) {
            console.log(response);
        });
        basketOptions.body={
            productId: 5
        }
        await requestPromise.post(basketOptions)
        .then(function (response) {
            console.log(response);
        });
        basketOptions.body={
            productId: 2
        }
        await requestPromise.post(basketOptions)
        .then(function (response) {
            console.log(response);
        });
        

   res.send("Created Data")
});

module.exports = router;