# PostIt
A C# .NET Entity Framework MVC Web App demonstration.

<img src="./PostItDemo/wwwroot/Assets//PostItsLogo.png" style="margin:0px 0px auto:" width="200px"/>

A very simple twitter clone where users can register & log in, post short messages with a title and a body, see everyone's posts organised by relevance (a simple recency + likes formula) and leave comment posts, i.e. posts that are anchored to some other post.

- [PostIt](#postit)
  - [Feature List](#feature-list)
    - [Wanted Feature List](#wanted-feature-list)
  - [Current State of the Project](#current-state-of-the-project)
    - [Login Page](#login-page)
    - [User Registration](#user-registration)
    - [Wrong Password](#wrong-password)
    - [Postboard](#postboard)
    - [New Post](#new-post)
  - [QA Demonstration](#qa-demonstration)
  - [User Journey Demonstration](#user-journey-demonstration)
    - [#1 User Story](#1-user-story)
    - [#2 SpecFlow Feature file](#2-specflow-feature-file)
    - [#3 Tests](#3-tests)
    - [#4 Implementation](#4-implementation)
    - [#5 Product](#5-product)
  - [KANBAN Board](#kanban-board)
  - [Figma Design](#figma-design)
  - [Metroretro Retrospective](#metroretro-retrospective)
  - [Installation](#installation)
    - [NuGet Dependencies](#nuget-dependencies)

## Feature List
- Cookie-based Registration, with and Sign In and Sign Out.
  - Usernames (Handles) and Passwords (hashed) stored directly on database
- Ability to make posts anonymously if not logged in
- A main postboard view where all posts can be seen and interacted with
- A per-post view where comment posts can be left, other comment posts seen
- Can Edit and Delete your own posts, but not others'

### Wanted Feature List
- Pagination
- API endpoints

## Current State of the Project
Sprint 1 is complete as of this push. Ready to start sprint 2, which doubles as a demonstration of the entire sprint process. There will be links to a kanban board, User Stories, Figma design board, a metro retrospective and a redone README suitable for the project. The README will contain an example of a User Story, its breakdown into SpecFlow Unit Tests, the result of those tests (passing) and then a short section on the code that makes it pass, all together functioning as a demonstration of TDD.

### Login Page
![Login Page](./Screenshots/LoginPage.jpg)

### User Registration
![Registration](./Screenshots/DavidRegistered.jpg)

### Wrong Password
![Wrong password](./Screenshots/WrongPassword.jpg)

### Postboard
![Poastboard](./Screenshots/PostBoard.jpg)

### New Post
![New Post](./Screenshots/NewPost.jpg)

## QA Demonstration
A demonstration of Unit testing using NUnit can be found in the code [here](./PostItsTests)

In Memory database use can be found with the PostItServiceShould tests.

![PostItServiceShouldTests](./Screenshots/PostItServiceShouldTests.jpg)

For a demonstration of SpecFlow use, see [User Journey Demonstration](#user-journey-demonstration)

## User Journey Demonstration
Here is a demonstration of one user story, and its implementation throughout the project in a step by step manner; a workflow known as Test Driven Development.
### #1 User Story 

![User Story](./Screenshots/UserStory.jpg)

### #2 SpecFlow Feature file

TODO (Feature File screenshot)
### #3 Tests 
TODO (Unit testing project code screenshot)
### #4 Implementation 
TODO (Code implementation screenshot)

### #5 Product 
TODO (website screenshot/s)

## KANBAN Board

https://github.com/users/Tamillis/projects/2

## Figma Design
[Home Page Design](https://www.figma.com/file/1JO25scDlwgyS5pMxvU0Cb/PostIts-Homepage?type=design&node-id=0%3A1&mode=design&t=wMqsCHv867fsVw1f-1)

I am not a designer, but this will do as a demonstration of "this is what I want you to make", and then the real site being how close I can get to that.

## Metroretro Retrospective
TODO

## Installation

Under releases, download the provided .zip, unzip and open the .sln using Visual Studio (with ASP.NET and web development & SQL Server installed), install dependencies through NuGet (see screenshot below), click run.

For the latest development iteration: clone the repo and load the .sln using Visual Studio (with ASP.NET and web development & SQL Server installed), install dependencies through NuGet (see screenshot below), and click run.

### NuGet Dependencies
![Nuget Dependencies](./Screenshots/NugetDependencies.jpg)