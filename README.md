Good day!

This is a message from me to help you keep tracking of the progress.
To organise the work and plan it properly, I prepared a list of sub-tasks ordered according to priority. The tasks I completed are marked by [x]. If you pull latest of the repo, the implementation will include a running front and back ends of all the ticked items.
I hope I could allocate more time during the 7 days of the task. I hope you like my solution :)
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
- [x] Implement front-end UI Design to initialise a mood rating. The user should see a page tp fill an email address and select a rating of his/her mood + optional comment.
- [x] Implement front-end unit test - initialise a mood rating.
- [x] Implement back-end logic to store user's mood rating. Also add validation to prevent multiple submit actions in the same day.
- [x] Implement back-end unit test - Mood Rating Recording.
- [x] Implement front-end unit test - Mood Rating Recording.
- [x] Implement seeding for user,roles, claims...etc
- [x] Implement Optional Authentication in back-end using .Net Core Identity
- [x] Implement Authentication in front-end
- [ ] Setup front-end app to start in https to allow admin login securely.
- [ ] Implement Login - this is to prepare for the admin user access.
- [ ] Customise front-end routing for admin and create an initial landing page - the routing will check if the user is logged-in, then it will direct to admin view-records component.
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


# Before you run the app locally
- When you run the app for the first time, this will creat seed data of the admin user. The admin user name is "admin" and the password is "Password123$".
- You may need to run migration. This will make sure that the database is up to date. You can do so by running the command of update-database in the console.
  
  <img width="577" alt="image" src="https://github.com/user-attachments/assets/30e7a977-3153-41df-b69b-fbeee91f5266" />

