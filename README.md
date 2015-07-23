<img align="left" src="https://avatars0.githubusercontent.com/u/7360948?v=3" />

&nbsp;Heimdall<br /><br />
=============

| Downloads | Version |
|-----------|---------|
| ![NuGet Total](https://img.shields.io/nuget/dt/Heimdall.svg) | ![NuGet Version](https://img.shields.io/nuget/v/Heimdall.svg) |

Easy to use HMAC Digest Authentication for WebAPI with various client implementations (C#, NodeJS and Browser versions)

##How it works

Let's first take a look at the component parts. Heimdall is broken up into two distinct logical parts, namely, `server` and `client`. 

###Server

The server is implemented as a WebAPI delegating handler. This will check each request that comes in and then apply HMAC encryption
to check the integrity and authentication for each individual request message made to WebAPI. 