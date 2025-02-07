const MessageHandler = require("./messageHandler");

class ConcreteMessageHandler extends MessageHandler {

    constructor(io, databaseConnector) {
        super(io, databaseConnector);
    }

    handleIncomingMessages(socket) {
        //listen to specific incoming messages by their name.
        //more information can be found on https://socket.io/docs/v4/listening-to-events/ and https://socket.io/docs/v4/emitting-events/
        this.#handleMessage(socket);
	}

    async #handleMessage(socket) {
        // handle the message here
        socket.on("message name", (data) => {
            console.log("some data received", data);
            // more information about emitting messages can be found here https://socket.io/docs/v4/emitting-events/
            // send a message to all concurrent users.
            this._io.emit("message name", { data: "data" });

            // send a message to all concurrent users except the sender.
            socket.broadcast.emit("message name", { data: "data" });

            // send a message to the sender.
            socket.emit("message name", { data: "data" });

            // execute a prepared query.
            //this._databaseConnector.executePreparedQuery("SELECT * FROM tabel WHERE id = ?", [1]);
        });
    }
}

module.exports = ConcreteMessageHandler;