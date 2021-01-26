# Word-Frequency-API-Service
An API with an endpoint that accepts a URL to any website (e.g., Wikipedia or BBC News) in order to fetch the content of that URL and build a dictionary that contains the frequency of use of each word on that page. Each time a URL is being fetched, it saves the top 100 most frequent words to a database (MSSQL). In case of an existent word from previous fetches, the frequency is updated with the additional frequency calculated. The API has another endpoint which returns the top 100 words from its database, where the response is ordered by the most frequent word towards to the less frequent word.

**Prerequisites**

The API runs on .NET Core and it reads/writes to an MS SQL database on the local server (localdb)\mssqllocaldb
so you will need Visual Studio (and optionally SQL Server Management Studio).

**To run the API Service**

Everything should work out of the box using IIS Express in Visual Studio. The local database and table gets created automatically on start up if it doesn't exist and the app redirects straight to the Swagger UI which should be pretty self-explanatory. It also starts up using Docker Desktop (if you have it installed) without any issues, although I have not set up the local db for Docker yet so requests will fail, only IIS is supported for now. 

**TODO**

Unfortunately due to work and family commitments I have not been able to spend as much time with this project as I wanted to so I have not implemented authorisation yet. Will add token authorisation and token issuer endpoints shortly. I will also make the local sql server work with Docker if I have the time. 
