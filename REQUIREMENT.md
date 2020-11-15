# DoS Defense Coding Challenge

## Introduction

In this challenge, you will build a simple web server, serve it behind a reverse proxy with HTTPS, and protect it against DoS attack.

You may find the content of this challenge too much to be completed in one day. In that case, please complete as much as you can, and **make sure you have a working system deployed at submission time, even if it doesn't meet some of the objectives**.

## Goals

This coding challenge intends to test your:

- General understanding of web technologies
- Ability to deploy commonly used software
- Familiarity with C# and .NET
- Code quality (so please pay attention to coding styles & conventions)

## Resources

The following resources have been provided to you for this challenge:

### VM running Ubuntu 18.04 LTS

> **NOTE**
>
> In the real world you would probably deploy the components in this challenge on different machines for high availability. For this challenge though, please just deploy everything on the VM provided. You're free to choose the deployment model: container, systemd, etc.

    IP Address: 52.187.114.169
    Username: challenge
    Private Key: ./ssh/challenge

After you're done, please deploy and test on this VM. The VM already has port `22`, `80` and `443` exposed to the internet.

### A subdomain pointing to the VM IP

    Domain: challenge.xjonathan.me
    A Record: 514.169114.169

To secure your site with HTTPS, you need a domain DNS record that points to the IP address for obtaining a certificate. It has already been set up for you.

## Objectives

You should build/deploy the following components:

### 1. Web Server

The main component to be built is a web server that exposes 3 endpoints.

**For deployment, run two instances of the web server listening on different ports.**

Endpoints to be exposed:

**Registration**

    POST /api/register

    Request:
    {
        "username": "<USERNAME>",
        "password": "<PASSWORD>",
        "name": "<DISPLAY_NAME>"
    }

Please apply some usual checks on the format of `username` and `password` that you believe is reasonable. 

The server should fail the request if the user already exists.

`username` is case-insensitive. That is, you cannot register `admin` when `Admin` already exists.

**Login**

    POST /api/login

    Request:
    {
        "username": "<USERNAME>",
        "password": "<PASSWORD>"
    }

    Response:
        A string representing the JWT token

The login endpoint should return a `string` representing the JWT token

**Get Info**

    GET /api/info

    This endpoint requires authentication. Must send the following header or the request will fail with 401:

        Authorization: Bearer <THE_JWT_TOKEN>

    Response:
    {
        "userId": <USER_ID>,
        "name": "<DISPLAY_NAME>"
    }

### 2. Orleans Cluster

*You may believe in-memory databases like Redis can also serve this purpose, but unfortunately you're required to use Orleans here. It's part of the challenge.*

To protect the web server against DoS attacks, we need a way to identify attackers by their request frequency.

For deployment, please run two instances.

**Rate Limit**

The idea is to implement a **leaky bucket rate limit algorithm**. Instead of a window rate limit algo, the leaky bucket will continuously refill the quota. You **MUST** implement it as a leaky bucket algo.

Since we're deploying more than one instance of the web server, we can't just put the limiter in process memory. Instead, we will use a Orleans cluster for this purpose. Each `Grain` (aka. actor) should be responsible for rate limiting a single IP address.

When a request reaches the web server, it should first apply the rate limit consumption to the Orleans cluster to see if it overspends the limit. If yes, the server should fail the request immediately with 429.

You should set a rate limit of 30 requests per minute.

**Banning IP**

Failing the request with 429 will prevent legit users from sending more requests. However, for an attacker, the status code doesn't matter, as all he wants is to consume server resources, and having the request reached the server already does that.

To effectively defense against attacks, we need to go further and ban the attacker's IP address completely on the system level.

If an IP address rate limiter reaches -30 (i.e. he keeps sending requests even after getting status code 429), the IP address should be added to the blacklist (stored in MySql) and be **banned for 5 minutes**.

As for actually using the blacklist, please see the next component.

### 3. Ipset Synchronizer

To ban traffic on the system level, we need `iptables` and `ipset`. For reference see [this article](https://linux-audit.com/blocking-ip-addresses-in-linux-with-iptables/).

You should create an `ipset` and add it to relevant `iptables`.

To keep the `ipset` up to date, you should build a daemon that polls the blacklist table from database every 30 seconds for the latest banned list, and modify the `ipset` to match with the list.

### 4. MySql

Use MySql for user data & blacklist persistence. For deployment, a single-node deployment is sufficient for this challenge.

You **MUST** use EntityFramework Core for data access. You're free to choose code-first or database-first, whichever you like.

### 5. Reverse Proxy

You're free to choose whatever reverse proxy server you're familiar with, though `Nginx` is recommended for this challenge.

**HTTPS**

The website must be accessible with HTTPS via port `443`. You can get an HTTPS certificate at Let's Encrypt.

**HTTP to HTTPS redirection**

When accessing the website through HTTP via port `80`, the user should receive a 301 response and be redirected to the HTTPS counterpart.
