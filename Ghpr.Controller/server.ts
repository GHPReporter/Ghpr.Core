var http = require("http");
var fs = require("fs");
var port = process.env.port || 1337;
fs.readFile("./index.html", (err, html) => {
    if (err) {
        throw err;
    }
    http.createServer((request, response) => {
        response.writeHeader(200, { "Content-Type": "text/html" });
        response.write(html);
        response.end();
    }).listen(port);
});