# API de Noticias
## API RestFull com arquitetura Clean Code e autenticação JWT

## Indice

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
  "id": 0,
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
