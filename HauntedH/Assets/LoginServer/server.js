const express = require('express');
const keys = require('./config/keys.js');
const app = express();
const bodyParser = require('body-parser');
//setup DB

app.use(bodyParser.urlencoded({ extended: false }))

const mongoose = require('mongoose');
mongoose.connect(keys.mongoURI,{useNewUrlParser: true,useUnifiedTopology: true});

//setup DB models
require('./model/Account');

// Setup the routes
require('./routes/authenticationRoutes')(app);

const port = 4000;
app.listen(keys.port, () => {
    console.log("Listening on " + port);
});