/**
 * Server application - contains all server config and api endpoints
 *
 * @author Pim Meijer & Jur van Oerle
 */
const corsConfig = require("./framework/utils/corsConfigHelper");
const express = require("express");
const bodyParser = require("body-parser");
const morgan = require("morgan");
const errorcodes = require("./framework/utils/httpErrorCodes");
const path = require("path");
const SocketConnectionListener = require("./socketConnectionListener");

const app = express();
//front-end as static directory
app.use(express.static(path.join(__dirname, '../src')));

//logger library  - 'short' is basic logging info
app.use(morgan("short"));

//helper libraries for parsing request bodies from json to javascript objects
app.use(bodyParser.urlencoded({extended: false}));
app.use(bodyParser.json());

//CORS config - Cross Origin Requests
app.use(corsConfig);

//always keep this route at the bottom! This is error handling for when a client calls a non existing route!
app.get("*", (req, res) => {
    res.status(errorcodes.ROUTE_NOT_FOUND_CODE).json({reason: "Not found, make sure the endpoint you are trying to call exists! :)"});
});

//------- END ROUTES -------

async function listen(port, callback) {
    const server = app.listen(port, callback);
	const socketConnectionListener = new SocketConnectionListener();
	socketConnectionListener.initializeServer(server);
}

module.exports = {
    listen: listen
};