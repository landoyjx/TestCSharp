#

test project in C#


## install dotnet


```
wget https://packages.microsoft.com/config/ubuntu/20.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
sudo dpkg -i packages-microsoft-prod.deb
```

```
sudo apt-get update; \
  sudo apt-get install -y apt-transport-https && \
  sudo apt-get update && \
  sudo apt-get install -y dotnet-sdk-3.1
```


## install ipset


add do follow commands

```
ipset create blacklist hash:ip hashsize 4096
iptables -I INPUT -m set --match-set blacklist src -j DROP
iptables -I FORWARD -m set --match-set blacklist src -j DROP
```