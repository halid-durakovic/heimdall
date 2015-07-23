function Heimdall(uri, username, secret) {

    if (!$)
        throw 'Could not find JQuery, please install it. See https://jquery.com/ for more ... ';

    if (!CryptoJS)
        throw 'Could not find CryptoJS, please install it. See https://code.google.com/p/crypto-js/ for more ... ';

    var self = this;
    self.contentType = 'application/json';

    // Private Functions 

    function encrypt(data, secret) {
        var hash = CryptoJS.HmacSHA256(data, secret || '');
        var hashInBase64 = CryptoJS.enc.Base64.stringify(hash);
        return hashInBase64;
    }

    // Public Functions

    self.get = function (path, onComplete) {

        var currentDate = new Date().toUTCString();
        var messageRepresentation = ['GET', path, self.contentType, '', currentDate].join('\n');
        var authKey = encrypt(messageRepresentation, secret);

        $.ajax({
            url: uri + path,
            headers: {
                'X-ApiAuth-Date': currentDate,
                'X-ApiAuth-Username': username,
                'Authorization': 'ApiAuth ' + authKey,
                'Content-Type': self.contentType
            },
            type: "GET",
            success: function (res) {
                if (onComplete)
                    onComplete(null, res);
            },
            error: function (err) {
                if (onComplete)
                    onComplete(err);
            }
        });

    };

    self.post = function (path, body, onComplete) {

        var contentMd5 = encrypt(JSON.stringify(body));
        var currentDate = new Date().toUTCString();
        var messageRepresentation = ['POST', path, self.contentType, contentMd5, currentDate].join('\n');
        var authKey = encrypt(messageRepresentation, secret);

        $.ajax({
            url: uri + path,
            headers: {
                'X-ApiAuth-Date': currentDate,
                'X-ApiAuth-Username': username,
                'Authorization': 'ApiAuth ' + authKey,
                'Content-Type': self.contentType,
                'Content-MD5': contentMd5
            },
            type: "POST",
            data: JSON.stringify(body),
            success: function (res) {
                if (onComplete)
                    onComplete(null, res);
            },
            error: function (err) {
                if (onComplete)
                    onComplete(err);
            }
        });

    };

    self.put = function(path, body, onComplete) {

        var contentMd5 = encrypt(JSON.stringify(body));
        var currentDate = new Date().toUTCString();
        var messageRepresentation = ['PUT', path, self.contentType, contentMd5, currentDate].join('\n');
        var authKey = encrypt(messageRepresentation, secret);

        $.ajax({
            url: uri + path,
            headers: {
                'X-ApiAuth-Date': currentDate,
                'X-ApiAuth-Username': username,
                'Authorization': 'ApiAuth ' + authKey,
                'Content-Type': self.contentType,
                'Content-MD5': contentMd5
            },
            type: "PUT",
            data: JSON.stringify(body),
            success: function (res) {
                if (onComplete)
                    onComplete(null, res);
            },
            error: function (err) {
                if (onComplete)
                    onComplete(err);
            }
        });

    };

    self.delete = function (path, onComplete) {

        var currentDate = new Date().toUTCString();
        var messageRepresentation = ['DELETE', path, self.contentType, '', currentDate].join('\n');
        var authKey = encrypt(messageRepresentation, secret);

        $.ajax({
            url: uri + path,
            headers: {
                'X-ApiAuth-Date': currentDate,
                'X-ApiAuth-Username': username,
                'Authorization': 'ApiAuth ' + authKey,
                'Content-Type': self.contentType
            },
            type: "DELETE",
            success: function (res) {
                if (onComplete)
                    onComplete(null, res);
            },
            error: function (err) {
                if (onComplete)
                    onComplete(err);
            }
        });

    };

}