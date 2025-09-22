import * as childProcess from "child_process";
import * as fs from "fs";
import * as vscode from "vscode";

export interface WebSocketServerLauncherOptions {
	/** Path to the .exe or .dll to run */
	readonly executable: string;
	/** Arguments passed to the process */
	readonly args?: string[];
	/** Enable console logging of stdout/stderr */
	readonly logging?: boolean;
	/** Optional working directory */
	readonly cwd?: string;
	/** If running a DLL, use dotnet to launch */
	readonly useDotnet?: boolean;
	/** Message or condition that indicates the server is ready */
	readonly waitForMessage?: string;
}

export class WebSocketServerLauncher implements vscode.Disposable {
	protected readonly options: Required<WebSocketServerLauncherOptions>;
	protected serverProcess?: childProcess.ChildProcess;
	protected resolvedPort?: number;

	constructor(options: WebSocketServerLauncherOptions) {
		this.options = {
			logging: false,
			args: [],
			cwd: process.cwd(),
			useDotnet: false,
			waitForMessage: "[AnyText-Extension-Server]:Startup completed on",
			...options,
		};
	}

	async start(): Promise<void> {
		const executable = this.options.executable;

		if (!executable || !fs.existsSync(executable)) {
			throw new Error(`Server executable not found: ${executable}`);
		}

		const process = this.startProcess();
		this.serverProcess = process;

		return this.configureProcess(process);
	}

	protected configureProcess(
		process: childProcess.ChildProcessWithoutNullStreams
	): Promise<void> {
		return new Promise((resolve, reject) => {
			process.stdout.on("data", (data) => {
				const message = data.toString();
				this.logStdout(message);

				if (message.includes(this.options.waitForMessage)) {
					const port = this.getPortFromStartupMessage(message);
					if (port) {
						this.resolvedPort = port;
						resolve();
					} else {
						reject(
							new Error(
								"Could not parse port from startup message"
							)
						);
					}
				}
			});

			process.stderr.on("data", (data) =>
				this.logStderr(data.toString())
			);
			process.on("error", (err) => {
				this.logStderr(err.message);
				reject(err);
			});

			process.on("exit", (code) => {
				if (code !== 0) {
					console.error(`[Server] exited with code ${code}`);
				}
			});
		});
	}

	protected startProcess(): childProcess.ChildProcessWithoutNullStreams {
		const { executable, args, cwd, useDotnet } = this.options;

		const finalArgs = [...(args ?? [])];

		if (useDotnet) {
			finalArgs.unshift(executable);
			return childProcess.spawn("dotnet", finalArgs, {
				cwd,
				shell: false,
			});
		} else {
			return childProcess.spawn(executable, finalArgs, {
				cwd,
				shell: false,
			});
		}
	}

	protected logStdout(message: string): void {
		if (this.options.logging) {
			console.log(`[Server] ${message}`);
		}
	}

	protected logStderr(message: string): void {
		if (this.options.logging) {
			console.error(`[Server] ${message}`);
		}
	}

	protected getPortFromStartupMessage(message: string): number | undefined {
		const regex = /http:\/\/(?:localhost|127\.0\.0\.1):(\d+)/;
		const match = message.match(regex);
		if (match && match[1]) {
			return parseInt(match[1], 10);
		}
		return undefined;
	}

	getResolvedPort(): number | undefined {
		return this.resolvedPort;
	}

	stop(): void {
		if (this.serverProcess && !this.serverProcess.killed) {
			this.serverProcess.kill("SIGINT");
		}
	}

	dispose(): void {
		this.stop();
	}

	getWebSocketUrl(path: string): string {
		const port = this.getResolvedPort();
		if (!port) throw new Error("Port not available yet");
		return `ws://localhost:${port}${path}`;
	}
}
