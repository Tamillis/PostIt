# PostIt
A .NET Entity Framework using Web App demonstration.

A very simple twitter clone where users can post short message with a title and a body, see everyone's posts organised by relevance (a simple recency + likes formula) and leave comment posts, i.e. posts that are anchored to some other post Id, which are displayed in a post's details web page.

## Current Goal
Complete the initial basic project (purely focusing on back-end functionality), in order to then enter a "second sprint" as a proper Agile-Scrum processes with a Kanban board demonstration (Found Here: PROVIDE LINK) and the goal of making the project testable with a service layer, so will be employing a Test-First Dev approach: User Stories -> Tests -> Make Tests Pass.

## Feature List
Simple Cookie-based Authentication system, usernames (Author "Handles") and passwords (hashed) stored on the database directly.

Ability to Register, Sign In and Sign Out.

Ability to view all posts.

Ability to make posts anonymously if not logged in

Can Edit and Delete your posts, but not others'

## TODO

In-line editing?* (push feature)

Pop-up warning to delete?* (push feature)

Fixup front end (not in initial sprint)

Add comments to posts, where each comment is just a post

Add like system to posts, where each user can like a post only once

Add post ordering by 'the formula'