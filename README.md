![](https://raw.githubusercontent.com/badnickname/hema-dungeon/refs/heads/master/Frontend/src/assets/logo.png)
## Welcome to deep dark fantasy
- https://hema-dungeon.ru
- https://calc.hema-dungeon.ru

# Deployment
Pass `appsettings.Production.json` and `publish/wwwroot/images` to docker's volumes

## docker-compose.yaml
```yaml
version: '3.4'

services:
  hema:
    image: ghcr.io/badnickname/hema-dungeon/hema-dungeon:1.0
    ports:
      - 80:8080
    volumes:
      - /hema/appsettings.Production.json:/publish/appsettings.Production.json
      - /hema/images/:/publish/wwwroot/images/
      
```
