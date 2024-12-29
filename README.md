# MagicVilla API

## Overview

MagicVilla API is a comprehensive backend service designed to manage villas and villa numbers, 
along with user authentication and authorization features. This API is consumed by an MVC application that provides a user-friendly interface for interacting with the villa data. 
The API and its consuming MVC application together offer functionality for listing, creating, editing, and deleting villas and villa numbers, 
as well as handling user login and registration.

## API Endpoints

### Villas Endpoints

GET /api/v1/VillaAPI: Get a list of all villas.

GET /api/v1/VillaAPI/{id}: Get details of a specific villa by Id.

POST /api/v1/VillaAPI: Create a new villa.

PUT /api/v1/VillaAPI/{id}: Update an existing villa by Id.

PATCH /api/v1/VillaAPI/{id}: Partialy Update an existing villa by Id.

DELETE /api/v1/VillaAPI/{id}: Delete a villa by Id.


### Villa Numbers Endpoints

GET /api/v1/VillaNumberAPI: Get a list of all villa numbers.

GET /api/v1/VillaNumberAPI/{villaNo}: Get details of a specific villa number by villa number.

POST /api/v1/VillaNumberAPI: Create a new villa number.

PUT /api/v1/VillaNumberAPI/{villaNo}: Update an existing villa number by ID.

PATCH /api/v1/VillaNumberAPI/{villaNo}: Partialy Update an existing villa number by villa number.

DELETE /api/v1/VillaNumberAPI/{villaNo}: Delete a villa number by villa number.

### Authentication Endpoints

POST /api/v1/UsersAuth/register: Register a new user.

POST /api/v1/UsersAuth/login: Authenticate a user and retrieve a token.


## MVC Application Features:

### Homepage:

Displays all villas with images for easy browsing.

![image](https://github.com/user-attachments/assets/d2a8030d-fd02-4a38-8d25-83057d14472f)


### Index Pages:

Separate pages for listing all villas and villa numbers.

![image](https://github.com/user-attachments/assets/0d8a14f2-4f34-47fc-86a5-d16b572a13b7)

![image](https://github.com/user-attachments/assets/8c397308-5909-451c-91b3-1c462516afa8)


### View Pages:

UI for editing and deleting villas and villa numbers.

![image](https://github.com/user-attachments/assets/228f5487-0972-4b1d-b532-3da5b6b6d1d5)

![image](https://github.com/user-attachments/assets/ed5cbf51-fcf8-4230-afcf-6a3a73bcc358)


### Authentication Pages:

UI for user registration.

![image](https://github.com/user-attachments/assets/2645d0e8-79aa-4bb5-8f3b-fe8e81bc14f9)

UI for user login.

![image](https://github.com/user-attachments/assets/3529c4e6-c1af-4fae-ba50-b118fa7ba3b0)
