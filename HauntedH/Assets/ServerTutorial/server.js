const express = require('express');
const keys = require('./config/keys.js');
const app = express();
const bodyParser = require('body-parser');

app.use(bodyParser.urlencoded({ extended: false }))
//setup DB
const mongoose = require('mongoose');
mongoose.connect(keys.mongoURI,{useNewUrlParser: true,useUnifiedTopology: true});
//setup DB models
require('./model/Account');

// Setup the routes
require('./routes/authenticationRoutes')(app);
const port = 3000;
app.listen(keys.port, () => {
    console.log("Listening on " + port);
})