# PostIt
A .NET Entity Framework using Web App demonstration.

A very simple twitter clone where users can post short message with a title and a body, see everyone's posts organised by relevance (a simple recency + likes formula) and leave comment posts, i.e. posts that are anchored to some other post Id, which are displayed in a post's details web page.

## Current Goal
Complete the initial basic project (purely focusing on back-end functionality), in order to then enter a "second sprint" with proper Agile-Scrum processes and the goal of making the project testable with a service layer, so will be employing a Test-First Dev approach: User Stories -> Tests -> Make Tests Pass.

## Feature List
Simple Cookie-based Authentication system, usernames (Author "Handles") and passwords (hashed) stored on the database directly.

Ability to Register and Sign In.

Ability to view all posts.

## TODO

Password hashing.

Conditional access to delete / edit

In-line editing?* (push feature)

Pop-up warning to delete?* (push feature)

Fixup front end to not be the default

Add comments to posts, with user controlled comments