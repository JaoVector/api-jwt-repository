# API de Noticias
## API RestFull e autenticação JWT
#### O projeto tem a finalidade de apresentar o funcionamento do JWT e também a implantação de uma aplicação em um ambiente produtivo usando o Azure VM, ACR, ACI e AKS.

# Funcionalidades do Projeto

## 1 - Gerenciando Usuários
#### Efetue o Cadastro do Usuário:
```
POST: https://localhost:7037/Usuario
```
#### Corpo da Requisição:
```json
{
  "email": "string",
  "name": "string",
  "password": "string",
  "role": "string"
}
```
OBS: O atributo "role" é utilizado para especificar se o usuário é um "manager" ou "employee" neste projeto.

#### Consultando Usuários:
```
GET: https://localhost:7037/Usuario
```
OBS: Para controlar a quantidade de dados no retorno, utilize o endpoint abaixo:
```
GET: https://localhost:7037/Usuario?skip=0&take=5
```
skip ira determinar o ponto de partida da consulta e o take a quantidades de items retornados.
```json
[
  {
    "id": 0,
    "email": "string",
    "name": "string",
    "role": "string"
  }
]
```

#### Consultando Usuário por Id:
```
GET: https://localhost:7037/Usuario/{id}
```
#### Corpo do Response:
```json
{
  "id": 0,
  "email": "string",
  "name": "string",
  "role": "string"
}
```

#### Atualizando Usuário:
```
PUT: https://localhost:7037/Usuario/{id}
```
#### Corpo da Requisição:
```json
{
  "email": "string",
  "name": "string",
  "password": "string",
  "role": "string"
}
```

#### Deletando Usuário
```
DELETE: https://localhost:7037/Usuario/{id}
```

## 2 - Login.
#### Efeutando o Login de Acordo com Usuário cadastrado:
```
POST: https://localhost:7037/Login/GeraToken?email=joao@gmail&password=1234
```
OBS: Substituir os valores de email= e password= de acordo com o usuário que deseja logar.
#### Após efetuar o Login será retornado um Token para autenticação e autorização:
```json
{
  "token": "string"
}
```
### Endpoints que Usuários com role=employee podem utilizar:
- Noticia
  ```
  GET: https://localhost:7037/Noticia
  GET: https://localhost:7037/Noticia/{id}
  GET: https://localhost:7037/Noticia?skip=0&take=5
  POST: https://localhost:7037/Noticia
  ```
- Usuario
  ```
  POST: https://localhost:7037/Usuario
  GET: https://localhost:7037/Usuario
  GET: https://localhost:7037/Usuario?skip=0&take=5
  GET: https://localhost:7037/Usuario/{id}
  ```
### Endpoints que Usuários com role=manager podem utilizar:
- Noticia
  ```
  PUT: https://localhost:7037/Noticia/{id}
  DELETE: https://localhost:7037/Noticia/{id}
  ```
- Usuario
  ```
  PUT: https://localhost:7037/Usuario/{id}
  DELETE: https://localhost:7037/Usuario/{id}
  ```
  
## 3 - Administrando as Notícias.
#### Efetue o Cadastro da Noticia:
```
POST: https://localhost:7037/Noticia
```
#### Corpo da Requisição:
```json
{
  "titulo": "string",
  "descricao": "string",
  "conteudo": "string",
  "autor": "string"
}
```

#### Efetue a Consulta de todas as Noticias:
```
GET: https://localhost:7037/Noticia
```
OBS: Para controlar a quantidade de dados no retorno, utilize o endpoint abaixo:
```
GET: https://localhost:7037/Noticia?skip=0&take=5
```
skip ira determinar o ponto de partida da consulta e o take a quantidades de items retornados.
#### Corpo de Response
```json
[
  {
    "id": 0,
    "titulo": "string",
    "descricao": "string",
    "conteudo": "string",
    "dataPublicacao": "string",
    "autor": "string"
  }
]
```
OBS: A data foi convertida de DateTime para string utilizando Map no momento do retorno.
```C#
CreateMap<Noticia, ReadNoticiaDTO>().ForMember(x => x.DataPublicacao, opt => opt.MapFrom(src => ((DateTime)src.DataPublicacao).ToString("dd/MM/yyyy")));
```

#### Efetue a Consulta da Noticia por Id:
```
GET: https://localhost:7037/Noticia/{id}
```
#### Corpo da Consulta
```json
{
  "id": 0,
  "titulo": "string",
  "descricao": "string",
  "conteudo": "string",
  "dataPublicacao": "string",
  "autor": "string"
}
```

#### Efetua a Edição da Noticia:
```
PUT: https://localhost:7037/Noticia/{id}
```
#### Corpo do Requisição para atualizar a Noticia
```json
{
  "titulo": "string",
  "descricao": "string",
  "conteudo": "string",
  "dataPublicacao": "string",
  "autor": "string"
}
```

#### Efetue o Delete da Noticia
```
DELETE: https://localhost:7037/Noticia/{id}
```

# Implantando API e Banco SQL Server em um Cluster Kubernetes (AKS) com GitHub - Actions

### 1 - Crie um Cluster Kubernetes no AKS seguindo o tutorial abaixo:
```
https://learn.microsoft.com/pt-br/azure/aks/learn/quick-kubernetes-deploy-portal?tabs=azure-cli
```
### 2 - Após criar o cluster faça o login no Azure utilizando o comando "az login" no Azure Cli.
### 3 - Conecte no Cluster AKS utilizando os comandos abaixo:
```
az account set --subscription <sua-subscription>
az aks get-credentials --resource-group <seu-resource-group> --name <seu-cluster-name>
```
### OBS: Execute o comando kubectl get nodes para verificar se esta funcionando corretamente a conexão.
### 4 - É necessário criar um Secret para o usuário SA, que será a senha para nossa instância do SQL Server, use o comando abaixo:
```
kubectl create secret generic mssql --from-literal=MSSQL_SA_PASSWORD="<sua-senha>"
```
### 5 - Entre no repositório do GitHub onde se encontra o projeto, acesse a aba Actions e crie um novo Workflow em branco, e adicione o código yml abaixo:
```yaml
name: CI-CD
on:
  push:
    branches: ["main"]
  workflow_dispatch:

jobs:
  CI:
    runs-on: ubuntu-latest
    steps:
      - name: Obter Código Fonte
        uses: actions/checkout@v3.6.0

      - name: Docker Login
        uses: docker/login-action@v2.2.0
        with:
          username: ${{ secrets.DOCKERHUB_USR }} # Secrets para o usuário do Docker Hub
          password: ${{ secrets.DOCKERHUB_PWD }} # Secrets para a senha do usuário do Docker Hub
            
      - name: Docker Build
        uses: docker/build-push-action@v4.1.1
        with:
          context: ./NoticiarioAPI
          file: ./NoticiarioAPI/Dockerfile
          push: true
          tags: |
            <sua-imagem>:v${{ github.run_number }}
            <sua-imagem>:latest

  CD:
    runs-on: ubuntu-latest
    needs: [CI]
    steps:
      - name: Obter Código Fonte
        uses: actions/checkout@v3.6.0
      - name: Azure Login
        uses: Azure/login@v1.4.6
        with:
          creds: '{"clientId":"${{ secrets.CLIENT_ID }}","clientSecret":"${{ secrets.CLIENT_SECRET }}","subscriptionId":"${{ secrets.SUBSCRIPTION_ID }}","tenantId":"${{ secrets.TENANT_ID }}"}'
      - name: Azure Kubernetes set context
        uses: Azure/aks-set-context@v3
        with:
          resource-group: <seu-resource-group>
          cluster-name: <nome-do-cluster-aks>
          
      - name: Deploy to Kubernetes cluster
        uses: Azure/k8s-deploy@v4.9
        with:
          manifests: |
            k8s/pvc.yml
            k8s/sql-server-deploy.yml
            k8s/api-deploy.yml
          images: |
            <sua-imagem>:v${{ github.run_number }}
```
#### 6 - Faça o dockerfile que irá gerar a imagem do container da Aplicação ASP Net Core. Use o código abaixo:
```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:6.0-focal AS base
WORKDIR /app
EXPOSE 80
FROM mcr.microsoft.com/dotnet/sdk:6.0-focal AS build
WORKDIR /src
COPY ["NoticiarioAPI.csproj", "./"]
RUN dotnet restore "./NoticiarioAPI.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "NoticiarioAPI.csproj" -c Release -o /app/build
FROM build AS publish
RUN dotnet publish "NoticiarioAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NoticiarioAPI.dll"]
```
#### Execute o docker build . -t <nome-da-imagem:tag_imagem> para construir a imagem.
#### 7 - Gere as Secrets que serão utilizadas para que o GitHub Actions se conecte com o Azure e possa executar os manifests dentro do ambiente.
```
az ad sp create-for-rbac -n pipeline --role Owner --scopes /subscriptions/<sua-subscription>
```
#### Com as informações que irão retornar será possível criar novas secrets.
```json
{
  "appId": "---",
  "displayName": "pipeline",
  "password": "---",
  "tenant": "---"
}
```
#### Secrets:
```
CLIENT_ID = appId
CLIENT_SECRET = password
TENANT_ID = tenant
SUBSCRIPTION_ID = <sua-subscription>
```
#### 8 - Vá até o VSCode e faça commit e push das alterações, o CI-CD irá iniciar automaticamente. quando terminar verifique se os Pods estão funcionando com o comando kubectl get pods.
#### 9 - Visualize os serviços expostos com o comando kubectl get svc, depois pegue o EXTERNAL-IP do serviço mssql-deployment junto da porta 1433 e tente se conectar utilizando um aplicativo externo como o DBeaver.
#### 10 - Tendo sucesso na conexão do banco, entre no appsettings.js da aplicação e altere a string de conexão para o formato abaixo:
```
"Server=<External-IP>, 1433;Database=Noticia;User Id=sa;Password=<senha-secret-mssql>"
```
#### Execute o migrations e o database update para atualizar o banco de dados dentro do Pod do Cluster.
```
dotnet ef migrations add "Deploy"
dotnet ef database update
```
#### 11 - Após esses passos basta efetuar o commit e push novamente para atualizar a string de conexão possibilitando a aplicação que estiver no Pod noticia-api-*** possa acessar o banco do pod mssql-deployment-***.

### Arquivo yml para criar o persistent volume claim onde irão ser gravadas as informações geradas pelas requisições HTTP.
```yaml
kind: StorageClass
apiVersion: storage.k8s.io/v1
metadata:
     name: azure-disk
provisioner: kubernetes.io/azure-disk
parameters:
  storageaccounttype: Standard_LRS
  kind: Managed
---
kind: PersistentVolumeClaim
apiVersion: v1
metadata:
  name: mssql-data
  annotations:
    volume.beta.kubernetes.io/storage-class: azure-disk
spec:
  accessModes:
  - ReadWriteOnce
  resources:
    requests:
      storage: 2Gi

```
### Arquivo yml para criar o deploy do SQL Server utilizando a imagem da última versão de 2019, e gerando um serviço para expor o Pod.
```yaml
apiVersion: apps/v1
kind: Deployment
metadata:
  name: mssql-deployment
spec:
  replicas: 1
  selector:
     matchLabels:
       app: mssql
  template:
    metadata:
      labels:
        app: mssql
    spec:
      terminationGracePeriodSeconds: 30
      hostname: mssqlinst
      securityContext:
        fsGroup: 10001
      containers:
      - name: mssql
        image: mcr.microsoft.com/mssql/server:2019-latest
        resources:
          requests:
            memory: "2G"
            cpu: "2000m"
          limits:
            memory: "2G"
            cpu: "2000m"
        ports:
        - containerPort: 1433
        env:
        - name: MSSQL_PID
          value: "Express"
        - name: ACCEPT_EULA
          value: "Y"
        - name: MSSQL_SA_PASSWORD
          valueFrom:
            secretKeyRef:
              name: mssql
              key: MSSQL_SA_PASSWORD
        volumeMounts:
        - name: mssqldb
          mountPath: /var/opt/mssql
      volumes:
      - name: mssqldb
        persistentVolumeClaim:
          claimName: mssql-data
---
apiVersion: v1
kind: Service
metadata:
  name: mssql-deployment
spec:
  selector:
    app: mssql
  ports:
    - protocol: TCP
      port: 1433
      targetPort: 1433
  type: LoadBalancer
```
### Arquivo yml para gerar o Deploy da API e também gerando um serviço para expor a aplicação.
```yaml
apiVersion: apps/v1
kind: Deployment
metadata:
  name: noticia-api
  labels:
    app: api-crud
spec:
  replicas: 1
  selector:
    matchLabels:
      app: crud
  template:
    metadata:
      labels:
        app: crud
    spec:
      containers:
        - name: api-noticia-container
          image: <sua-imagem>
          imagePullPolicy: IfNotPresent
          ports:
            - containerPort: 80
---
apiVersion: v1
kind: Service
metadata:
  name: api-noticia-service
spec:
  selector:
    app: crud
  ports:
    - protocol: TCP
      port: 8080
      targetPort: 80
  type: LoadBalancer
```
