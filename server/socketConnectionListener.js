const MySqlDatabase = require("./framework/utils/mySqlDatabase");
const ConcreteMessageHandler = require("./api/concreteMessageHandler");

class SocketConnectionListener {
	#databaseConnector;

	#io = null;	
	#cryptoHelper = require("./framework/utils/cryptoHelper");
	#sessionStore = require("./framework/utils/sessionStore");

	#messageHandlers = [];

	#getRandomId = () => this.#cryptoHelper.generateHexId().toString("hex");

	async initializeServer(server) {
		this.#databaseConnector = new MySqlDatabase();
		this.#io = require("socket.io")(server);
		
		// add more message handlers here as the application grows.
		this.#messageHandlers.push(new ConcreteMessageHandler(this.#io, this.#databaseConnector));

		this.#handleSocketSession();
		this.#handleIncomingConnections(this.#io);
	}
	
	#handleSocketSession() {
		this.#io.use((socket, next) => {
			//get the session id from the connection attempt.
			const sessionId = socket.handshake.query.sessionId;

			//if the session id is present, find the session in the session store.
			const session = this.#sessionStore.findSession(sessionId);

			//if the session is found, then the user is reconnected.
			if (session) {
				//restore the session id and user id on the socket.
				socket.sessionId = sessionId;
				socket.userId = session.userId;
                console.log(socket.sessionId, "restored");
			} else {
				//If the session id is not present, create a new one.
				socket.sessionId = this.#getRandomId();
				socket.userId = this.#getRandomId();

				console.log(socket.sessionId, "connected");
			}
			socket.emit("session established", { sessionId: socket.sessionId, userId: socket.userId });
			return next();
		});
	}

	async #handleIncomingConnections() {
		this.#io.on('connection', (socket) => {
			//make sure that this socket can receive id-based messages based on the socket's user id.
			socket.join(socket.userId);
			this.#storeSession(socket);
			this.#handleDisconnect(socket);
			
			this.#messageHandlers.forEach(handler => handler.handleIncomingMessages(socket));

			socket.emit("session established", {
				sessionId: socket.sessionId,
				userId: socket.userId
			});
		});
	}

	#handleDisconnect(socket) { 
		socket.on("disconnect", async () => {
			console.log(socket.sessionId, "disconnected");

			//store the session, so that it can be restored if the connection is reestablished.
			this.#storeSession(socket);
		});
	}

	#storeSession(socket) {
		this.#sessionStore.saveSession(socket.sessionId, {
			userId: socket.userId,
			roomId: socket.roomId,
			playerName: socket.playerName
		});
	}

	getPlayerById(room, playerId) {
		if (room) {
			const index = Object.values(room.players).findIndex(player => player.userId === playerId);
			if (index !== -1) {
				return room.players[index];
			}
		}
	}
}

module.exports = SocketConnectionListener;