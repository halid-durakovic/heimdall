<img align="left" src="https://avatars0.githubusercontent.com/u/7360948?v=3" />

&nbsp;Heimdall<br /><br />
=============

| Downloads | Version |
|-----------|---------|
| ![NuGet Total](https://img.shields.io/nuget/dt/Heimdall.svg) | ![NuGet Version](https://img.shields.io/nuget/v/Heimdall.svg) |

Easy to use HMAC Digest Authentication for WebAPI with various client implementations (C#, NodeJS and Browser versions)

##How it works

Let's first take a look at the component parts. Heimdall is broken up into two distinct logical parts, namely, `server` and `client`. 
Both create compatible signed messages using the the individual message representation for each request using certain key dimensions. 
So for example if we were to make a get request like the one below:

**Example GET Request**

Headers
    
    Accept: */*
    Content-Type: application/json
    
Path
    
    GET /api/mysecureresource/1
  
You would require a `username` and a `secret` to sign the message, this is already implemented for you in a Heimdall C# and JavaScript
client which we will cover in more detail later on. Once the message is sent to the server a delegating handler will then verify the 
message and then decide whether it is valid or not. So if our username was 'username' and the secret was 'secret' then our example
request would look like this: 

  

###Server

The server is implemented as a WebAPI delegating handler. This will check each request that comes in and then apply HMAC encryption
to check the integrity and authentication for each individual request message made to WebAPI. 

