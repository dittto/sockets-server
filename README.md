# Sockets - server layer

This is a Unity / C# application that uses WebSockets to communicate to our clients via the connection layer.

It creates an active server and then shows the position of all clients. It also handles moving the clients around from their current location, toward a target position.

## How to use it

After starting a connection layer, run this Unity project (taking care to make sure the WebSocket url in WebSocketServer.cs matches the connection layer url).

Start a few clients and then click around on the clients. If working correctly, you should see the server replicating the client's positions and move them towards their targets.
