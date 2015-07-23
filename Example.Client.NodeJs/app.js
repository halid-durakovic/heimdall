var request = require('request');
var crypto = require('crypto');

function encrypt(data, secret) {
    var hmacSignature = crypto.createHmac("sha256", secret || '');
    hmacSignature.update(messageRepresentation);
    return hmacSignature.digest("base64");
}

var httpMethod = 'GET';
var httpPath = '/api/values';
var contentType = 'application/json';
var contentMD5 = '';
var timestamp = new Date().toUTCString();
var messageRepresentation = [httpMethod, httpPath, contentType, contentMD5, timestamp].join('\n');

var req = {
    url: 'http://localhost:12345/api/values',
    headers: {
        'X-ApiAuth-Date': timestamp,
        'X-ApiAuth-Username': 'username',
        'Content-MD5': contentMD5,
        'Content-Type': 'application/json',
        'Authorization': 'ApiAuth ' + encrypt(messageRepresentation, 'secret')
    }
};

console.log('Request: ');
console.log(req);
console.log();

request(req, function (error, response, body) {
    if (!error) {
        console.log("Response:");
        console.log(response.statusCode);
        console.log(response.body);
    } else {
        console.log(error);
    }
});
