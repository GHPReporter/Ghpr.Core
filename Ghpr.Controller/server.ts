const server = require("http").createServer((req, res) => {

    let filePath = require("path").resolve(__dirname + "./../../." + req.url);
    var fs = require("fs");
    fs.exists(filePath, (exists) => {
        if (!exists) {
            res.statusCode = 404;
            res.end(`File ${filePath} not found!`);
        }

        if (fs.statSync(filePath).isDirectory()) {
            filePath += "/index.html";
        }

        fs.readFile(filePath, (err, data) => {
            if (err) {
                res.statusCode = 500;
                res.end(`Error getting the file: ${err}.`);
            } else {
                res.end(data);
            }
        });
    });

}).listen(process.env.port || 1337, process.env.IP || "0.0.0.0", () => { });
