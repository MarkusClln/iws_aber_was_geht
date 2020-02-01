const express = require('express');
const httpProxy = require('express-http-proxy');
const config = require('./config/default.json');


// Constants
const PORT = config.Gateway.port;
const basketURL= process.env.BASKET_IP + ":" +config.Nodes.basketPORT;
const marketingURL= process.env.MARKETING_IP + ":" + config.Nodes.marketingPORT;
const productURL= process.env.PRODUCT_IP + ":" + config.Nodes.productPORT;
const paymentURL= process.env.PAYMENT_IP + ":" + config.Nodes.paymentPORT;

//Routes
const compositionRoute = require("./routes/composition");

// App
const app = express();

app.get('/', (req, res) => {
    res.send('Server is running');
});

//proxy
app.use('/marketing', httpProxy(marketingURL));
app.use('/basket', httpProxy(basketURL));
app.use('/product', httpProxy(productURL));
app.use('/payment', httpProxy(paymentURL));

app.use('/api', compositionRoute);

app.listen(PORT, () => console.log("Listen to port "+PORT));
