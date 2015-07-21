var request = require('request'),
    crypto = require('crypto');

var req = {
    url: 'http://localhost:12345/api/values',
    headers: { 'X-ApiAuth-Date': new Date().toUTCString() }
};

/// Http METHOD\n +
/// Http PATH\n +
/// Content-Type\n +  
/// Content-MD5\n +  
/// Timestamp\n +
var messageRepresentation = ['GET', '/api/values', '', '', req.headers['X-ApiAuth-Date']].join('\n');

var hmac_signature = crypto.createHmac("sha256", 'secret');
hmac_signature.update(messageRepresentation);

var hmac_signature_base64 = hmac_signature.digest("base64");

//add auth header
req.headers['X-ApiAuth-Username'] = 'username';
req.headers['Authorization'] = "ApiAuth " + hmac_signature_base64;

sendRequest(req);

function sendRequest(r) {
    console.log('Request: ');
    console.log(req);
    console.log();

    request(req, function (error, response, body) {
        if (!error) {
            console.log("Response:");
            console.log(response.statusCode);
            console.log(response.body);
        }
        else {
            console.log(error);
        }
    })
}
