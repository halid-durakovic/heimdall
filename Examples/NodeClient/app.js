var request = require('request'),
    crypto = require('crypto');

var req = {
    url: 'http://localhost:12345/api/values',
};

//send an unauth request
//sendRequest(req);

req.headers = {
    'Date': new Date()
}

/// HTTP PATH
/// HTTP METHOD\n +
/// Content-MD5\n +  
/// Timestamp\n +

var messageRepresentation = ['/api/values', 'GET', '', req.headers.date].join('/n');

var hmac_signature = crypto.createHmac("sha256", 'secret');
hmac_signature.update(messageRepresentation);

var hmac_signature_base64 = hmac_signature.digest("base64");

console.log(messageRepresentation);

//add auth header
req.headers['X-ApiAuth-Username'] = 'username';
req.headers['Authorization'] = "ApiAuth " + hmac_signature_base64;

sendRequest(req);

function sendRequest(r) {
    console.log('Sending Request: ');
    console.log(req);

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