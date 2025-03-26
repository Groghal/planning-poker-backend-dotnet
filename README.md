# Planning Poker Backend (.NET)

This is the .NET Core backend for the Planning Poker application. It provides a RESTful API for managing planning poker sessions.

## Prerequisites

- .NET 8.0 SDK
- Visual Studio 2022 or Visual Studio Code

## Getting Started

1. Clone the repository
2. Navigate to the project directory
3. Run the application:
   ```bash
   dotnet run
   ```

The API will be available at:
- HTTP: http://localhost:5000
- HTTPS: https://localhost:5001

Swagger documentation will be available at:
- http://localhost:5000/swagger
- https://localhost:5001/swagger

## API Endpoints

### Rooms

- `POST /api/rooms` - Create a new room
  - Request body:
    ```json
    {
      "roomId": "string (optional)",
      "voteOptions": ["1", "2", "3", "5", "8", "13"] (optional)
    }
    ```

- `POST /api/rooms/{roomId}/join` - Join a room
  - Request body:
    ```json
    {
      "username": "string"
    }
    ```

- `POST /api/rooms/{roomId}/vote` - Submit a vote
  - Request body:
    ```json
    {
      "username": "string",
      "vote": "string"
    }
    ```

- `POST /api/rooms/{roomId}/reveal` - Reveal votes in a room

- `POST /api/rooms/{roomId}/reset` - Reset votes in a room

- `GET /api/rooms/{roomId}` - Get room information

- `GET /api/rooms/{roomId}/vote-options` - Get vote options for a room

- `DELETE /api/rooms/{roomId}` - Delete a room

## Error Handling

The API uses standard HTTP status codes:
- 200: Success
- 201: Created
- 400: Bad Request
- 404: Not Found
- 500: Internal Server Error

Error responses include a message field with details about the error.

## CORS

The API is configured to allow requests from any origin. In a production environment, you should configure CORS to only allow requests from your frontend application. 