Hi,

This is a message from me to help you keep tracking of the progress. The list below represents the planned tasks until reaching MVP.
- [ ] Update Angular from 15 to 18.
- [ ] Add front-end routing, layout, and home.
- [ ] Implement front-end HttpClient/requests/url and wire it to configurations.
- [ ] Restructure back-end solution to apply Domain Driven Design - DDD.
- [ ] Implement App Config provider and organise DI.
- [ ] Implement back-end MySql Database/ORM and wire it to configuration.
- [ ] Implement .Net Core Identity/authentication/authorizattion - this to prepare for special permission of admin user.
- [ ] Implement Mood Rating Mock-up Controller Actions, Service, Repository. Test connection between fron and back ends. - this is to prepare for the business requirement implementation.
- [ ] Customise HttpResponses by wrapping it in a standard model to unify standards of error handling and friendly messages to the user.
- [ ] Implement seeding for user,roles, claims...etc
- [ ] Implement UI Design to help users submit a mood rating. The user will fill an email address and select a rating of his/her mood.
- [ ] Implement back-end logic to store user's mood rating. Also add validation to prevent multiple submit actions in the same day.
- [ ] Implement back-end unit test - Mood Rating Recording.
- [ ] Implement front-end unit test - Mood Rating Recording.
- [ ] Implement Login - this is to prepare for the admin user access.
- [ ] Customise front-end routing for admin and create an initial landing page.
- [ ] TODO: TBD


# InterviewProjectTemplate

## Project Structure

The backend code is located in the `InterviewProjectTemplate` directory. The frontend is under `Client/web-client`.

## Running

Once the codebase has been cloned, it can be run using the following two commands (note that these commands should be run from the top level directory, where the `docker-compose.yml` file is located):

`docker compose build`

`docker compose up`

This will build and run the ASP.NET Core backend, the Angular frontend, and the MySQL database inside a docker container.

## Database

A blank MySQL database is included inside the container, and will start up when the container is run. The ASP.NET Core backend is already configured with a connection string to this database.

## Frontend

The frontend will be run on `http://localhost:4200`. When making API calls to the backend, please ensure that `environment.apiUrl` is used for the URL, rather than hardcoding the value. This will ensure that we can easily re-configure the URL if needed for deployment.
