<img align="left" src="https://avatars0.githubusercontent.com/u/7360948?v=3" />

&nbsp;Heimdall<br /><br />
=============

Heimdall is a library that can be used by 

 - Client applications wanting to sign outgoing messages
 - Server applications (ASP.net WebApi) wishing to verify signed messages

It is designed to be unobtrusive, and a drop in solution, both sides.

##Client##
Requests are signed with a signature composed as follows:

- Verb
- Path
- Content-Type (or empty string if no content)
- MD5 Hash of the content (or empty string if no content)
- Timestamp


**Timestamp** Should match the date used in the **Date** header of the request.  
If you are unable to set the **Date** header, a custom header of `X-ApiAuth-Date` should be used instead.

The above parameters should be concatenated together with a newline `/n`

Lastly, the resultant string should be hashed using **HMAC SHA256** [More info](http://en.wikipedia.org/wiki/hash-based_message_authentication_code "More Info")  
A `secret` should be used in the hash. This secret should be known only to client and server. (Can be hashed, as long as it is hashed both sides)

A header of `X-ApiAuth-Username` should be set, containing the username - this should be the username or tenant application id making the request.

The `Authorization` header should be set using `ApiAuth` Scheme, and calculated hash (as described above)

###Example Request###

**HEADERS**

Content-Type: application/x-www-form-urlencoded  
Content-Md5: NcUXHpFJj73ToZwuR7GVBQ==  
Date: Wed, 18 Sep 2013 16:00:58 GMT  
Host: requestb.in  
X-Apiauth-Username: myusername  
Authorization: ApiAuth pNxy+5vs0Zov5ENCXeoNieI+XvGrrg4+pQHxs9nBFjw=  
Content-Length: 29  

**Message Representation**  
POST\n/api/values\napplication/x-www-form-urlencoded\NcUXHpFJj73ToZwuR7GVBQ==\n09/27/2013 16:24:08

This then gets hashed, using a secret, and base64 encoded, to produce our signature:  
pNxy+5vs0Zov5ENCXeoNieI+XvGrrg4+pQHxs9nBFjw=


##Server##
 In order to configure the WebApi to authenticate incoming requests, we need to create an implementation of `IGetSecretFromUsername`  
This simple implementation should give you an idea of what is expected:

    class DummyGetSecretFromUsername : IGetSecretFromUsername
    {
        public string Secret(string username)
        {
            if(username=="iclp")
                return "password123";

            return string.Empty;
        }
    }

In a real-world scenario, this would likely talk to a repository to look up the secret from the username.

To configure the Authentication Handler across the whole API, you could set it up like this (in `Application_Start`)

            var authenticateRequest = new AuthenticateRequest(new DummyGetSecretFromUsername());
            GlobalConfiguration.Configuration.MessageHandlers.Add(new HmacAuthenticationHandler(authenticateRequest));