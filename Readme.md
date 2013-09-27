#WebApi Authentication#

WebApiAuthentication is a library that can be used by 

 - Client applications wanting to sign outgoing messages
 - Server applications (ASP.net WebApi) wishing to verify signed messages

It is designed to be unobtrusive, and a drop in solution, both sides.

##Client##
Requests are signed with a signature composed as follows:

- Verb
- Path
- Content-Type
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

##Server##
 
 
### Authenticate attribute ###
