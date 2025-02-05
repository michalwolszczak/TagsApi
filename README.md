# Tag API - Instrukcja uruchomienia aplikacji

Aby uruchomić aplikację, należy skorzystać z **Docker Compose** do zbudowania i uruchomienia kontenerów dla aplikacji oraz bazy danych MongoDB. Poniżej znajdują się kroki, które należy wykonać:

## Instrukcja uruchomienia

### 1. Zbudowanie obrazu

W głównym folderze aplikacji (tam, gdzie znajduje się plik `docker-compose.yml`), należy otworzyć terminal i wykonać następującą komendę:

```bash
docker-compose build
```
Komenda ta zbuduje obraz aplikacji API.

### 2. Podniesienie kontenerów

Po zakończeniu budowania obrazów, należy uruchomić je za pomocą komendy:

```bash
docker-compose up
```
To polecenie uruchomi dwa kontenery:

#### tags-api - aplikacja API
#### mongodbcontainer - kontener z bazą danych MongoDB

### 3. Dostęp do API

Po uruchomieniu kontenerów, dostęp do dokumentacji OpenAPI odbywa się przez Swagger UI. Aby go otworzyć, należy przejść do poniższego adresu w przeglądarce:

```bash
http://localhost:8080/swagger/v1/swagger.json
```
