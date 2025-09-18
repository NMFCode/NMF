import { Duplex } from "stream";

export class WebSocketStream extends Duplex {
	private socket: WebSocket;

	constructor(socket: WebSocket) {
		super();

		this.socket = socket;

		// Handle incoming WebSocket messages
		this.socket.onmessage = (event) => {
			const data = event.data;
			const length = Buffer.byteLength(data, "utf8");

			// Add Content-Length header
			const messageWithHeader = `Content-Length: ${length}\r\n\r\n${data}`;

			// Push as Buffer
			this.push(messageWithHeader);
		};

		this.socket.onclose = () => {
			this.push(null);
		};

		this.socket.onerror = (err) => {
			this.emit("error", new Error(`WebSocket error: ${err}`));
			this.destroy();
		};
	}

	override _read(size: number): void {}

	override _write(
		chunk: any,
		encoding: BufferEncoding,
		callback: (error?: Error | null) => void
	): void {
		if (this.socket.readyState !== WebSocket.OPEN) {
			callback(new Error("WebSocket is not open"));
			return;
		}

		try {
			const message = chunk.toString("utf8");

			// Skip Content Length
			if (message.startsWith("Content-Length:")) {
				callback();
				return;
			}

			this.socket.send(message);
			callback();
		} catch (err) {
			callback(
				err instanceof Error ? err : new Error("Failed to send message")
			);
		}
	}

	override _final(callback: (error?: Error | null) => void): void {
		if (this.socket.readyState === WebSocket.OPEN) {
			this.socket.close();
		}
		callback();
	}
}
