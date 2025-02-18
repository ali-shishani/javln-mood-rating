Good day :)

This is a message from me to help you keep tracking of the progress. The list below represents the planned tasks until reaching MVP.
- [x] Update Angular from 15 to 18.
- [x] Add front-end routing, layout, and home.
- [x] Implement front-end HttpClient/requests/url and wire it to configurations.
- [x] Restructure back-end solution to apply Domain Driven Design - DDD.
- [x] Implement App Config provider and organise DI.
- [x] Implement back-end MySql Database/ORM and wire it to configuration.
- [x] Implement .Net Core Identity/authentication/authorizattion - this to prepare for special permission of admin user.
- [x] Implement Mood Rating Mock-up Controller Actions, Service, Repository. Test connection between fron and back ends. - this is to prepare for the business requirement implementation.
- [x] Customise HttpResponses by wrapping it in a standard model to unify standards of error handling and friendly messages to the user.
- [x] Implement back-end end-point logic to provide the front-end the list of mood rating options. This is will be used to populate UI for the user to record how he/she feels.
- [x] Implement back-end unit test for the data initialisation end-point.
- [x] Implement UI Design to help users submit a mood rating. The user will fill an email address and select a rating of his/her mood + optional comment.
- [x] Implement back-end logic to store user's mood rating. Also add validation to prevent multiple submit actions in the same day.
- [x] Implement back-end unit test - Mood Rating Recording.
- [ ] Implement front-end unit test - Mood Rating Recording.
- [ ] Implement seeding for user,roles, claims...etc
- [ ] Implement Login - this is to prepare for the admin user access.
- [ ] Customise front-end routing for admin and create an initial landing page.
- [ ] Implement back-end Mood Rating Controller Action to retriev Mood Rating Records - sorted by most recent.
- [ ] Implement back-end unit test - admin should be able to retrieve the records sorted by most recent.
- [ ] Implement front-end admin view UI design.
- [ ] Implement front-end admin view unit test.
- [ ] TODO: TBD.

# Screenshots

Record your mood every day:
<img width="947" alt="image" src="https://github.com/user-attachments/assets/796c21a3-d9dd-434e-b8c0-5897b26bd1f7" />

<img width="952" alt="image" src="https://github.com/user-attachments/assets/78d3c319-a166-4500-8d59-1de610a02990" />

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
