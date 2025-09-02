# Vis�o geral
Esta aplica��o trata-se apenas um exemplo do uso do Redis para fazer cache de resultados de requisi��es.

# Redis
- Banco de dados em mem�ria;
- Geralmente � usado para fazer cache de dados;
	- Trabalha com sistema de chave e valor.

# Pacotes
## Application
- StackExchange.Redis - v2.9.11

## API
- DotNetEnv - v3.1.1

# Comandos

Rodado redis via docker: sudo docker run -p 6379:6379 --name redis-master -e REDIS_REPLICATION_MODE=master -e REDIS_PASSWORD=minhaSenhaSegura123 bitnami/redis:latest