var http = require("http");
var port = process.env.port || 1337;
http.createServer((req, res) => {
    res.writeHead(200, { 'Content-Type': "text/plain" });
    res.end("Hello World\n");
}).listen(port);
//# sourceMappingURL=ghpr.controller.js.map