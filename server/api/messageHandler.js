class MessageHandler {
	_io;
	_databaseConnector;

	constructor(io, databaseConnector) {
		this._io = io;
		this._databaseConnector = databaseConnector;
	}

	handleIncomingMessages(socket) {
		throw new Error("Method 'handleIncomingMessages()' must be implemented.");
	}
}

module.exports = MessageHandler;