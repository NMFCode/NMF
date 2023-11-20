using Glsp.Test;
using NMF.Glsp.Language;
using NMF.Glsp.Server;
using NMF.Glsp.Server.Contracts;
using System.Net;
using System.Net.Sockets;

var fsm = new FsmLanguage();
var pn = new PetriNetLanguage();
var server = new GlspServer(fsm, pn);
var glspServer = new TcpGlspServer(server);
await glspServer.RunAsync();