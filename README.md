# Tech 205 - Final Group Project

## About This Repo
This project, completed by the Tech 205 group, is an ASP.NET web application and API. The web application allows an admin user to manage (ie create and assign roles to) other users, trainees to create and edit trackers of their progress and personal portfolios, and trainers to view and comment on trainees' progress. The API allows admins to manager users, their portfolios and their trackers.

## Project Overview

### Web App
The personal tracker site is a different implementation of the personal tracker system used during Sparta Global training. A trainee's personal tracker, which should be updated regularly, contains details of what they should start doing, stop doing, and habits they should continue. Their personal profiles display information about their background such as education history. A trainee user has access to their previous trackers, which allows them to review their progress during their training. After they have created a tracker they can select it just to read, or edit or delete it.
Trainers are able to view lists and details of all trainees, profiles and trackers, which they can add comments to.
Admins can sign in, view lists of all trainees and their profiles and trackers, users, add a particular type of user. 

### API
The API was built for the use of an internal adminstrator to manage all users and entries, thus they can perform CRUD operations on the Users, Profiles and PersonalTrackers tables. JWT authentication is implemented for data security, though the concept is that the API would only be available internally via intranet.

## API Use
When the application runs the URL will begin with `https://localhost:7141/` followed by the endpoint:

#### Spartans
  ```js
  // request body, SpartanDTO
     {
        "userName": string,
        "firstName": string,
        "lastName": string,
        "email": string,
        "phoneNumber": string,
        "emailConfirmed": bool,
        "passwordHash": string
     } 
  ```
- GET: `api/Spartans` - serves an array of user objects and their roles
- GET: `api/Spartan/{id}` - serves the user object of the spartan with id = `id`
- PUT: `api/Spartan/{id}` - takes a SpartanDTO and returns a No Content result on successful update
- POST: `api/Spartan` - takes a SpartanDTO and serves the newly created Spartan object
- DELETE: `api/Suppliers/{id}` - deletes Spartan with id = `id`

#### TraineeProfiles
```js
  // request body, TraineeProfileDTO
     {
        "id": int,
        "title": string,
        "pictureURL": string,
        "aboutMe": string,
        "workExperience": string,
        "complete": bool,
        "spartanId": string
     } 
```

- GET: `api/Profiles` - serves an array of trainee profile objects
- GET: `api/Prolies/{id}` - serves the TraineeProfile object with id = `id`
- PUT: `api/Profiles/{id}` - takes a TraineeProfileDTO and returns a No Content result on successful update
- POST: `api/Profiles` - takes a TraineeProfileDTO and serves the newly created TraineeProfile object
- DELETE: `api/Profiles/{id}` - deletes the TraineeProfile with id = `id`

#### PersonalTrackers
```js
  // request body, PersonalTrackerDTO
     {
        "id": int,
        "title": string,
        "stopSelfFeedback": string,
        "startSelfFeedback": string,
        "continueSelfFeedack": string,
        "trainerComments": string,
        "technicalSkills": string, // "Unskilled", "Low Skilled", "Partially Skilled" or "Skilled"
        "spartanId": string
     } 
```

- GET: `api/PersonalTrackers` - serves an array of PersonalTracker objects
- GET: `api/Prolies/{id}` - serves the PersonalTracker object with id = `id`
- PUT: `api/Profiles/{id}` - takes a PersonalTrackerDTO and returns a No Content result on successful update
- POST: `api/Profiles` - takes a PersonalTrackerDTO and serves the newly created PersonalTracker object
- DELETE: `api/Profiles/{id}` - deletes the PersonalTracker with id = `id`

## Using this Repo
Clone the repo using 
```
git clone https://github.com/ConnorJamesDawson/Final_Group_Project.git
```

To interact with the api you will need to generate a jwt.
From the root of the repo:

```
cd .\Final_Project\Final_Project\
dotnet user-jwts create
```

This will output a jwt that you can add as a bearer token in requests to the API.

## Libraries
- Microsoft.AspNetCore.Authentication.JwtBearer v7.0.5
- Microsoft.AspNetCore.Identity.EntityFrameworkCore v7.0.5
- Microsoft.AspNetCore.Identity.UI v7.0.5
- Microsoft.AspNetCore.Mvc.NewtonsoftJson v7.0.5
- Microsoft.EntityFrameworkCore.SqlServer v7.0.5
- Microsoft.EntityFrameworkCore.Tools v7.0.5
- Microsoft.VisualStudio.Web.CodeGeneration.Design v7.0.6
- Microsoft.EntityFrameworkCore.InMemory v7.0.5
- Moq v4.18.4
- Nunit.Analyzers 3.6.1
