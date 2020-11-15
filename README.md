install ipset

add do follow commands

```
ipset create blacklist hash:ip hashsize 4096
iptables -I INPUT -m set --match-set blacklist src -j DROP
iptables -I FORWARD -m set --match-set blacklist src -j DROP
```