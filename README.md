# Blood Bank Management

## Overview

The Blood Bank Management System API is a RESTful web service designed for managing blood bank entries. It allows users to perform CRUD operations, paginate results, and search records based on blood type, donor name, or status.

---

## Features

- Create new blood bank entries.
- Retrieve all entries or specific entries by ID.
- Update existing records.
- Delete records by ID.
- Paginated listing of entries.
- Search functionality for blood type, donor name, and status.



---

## API Endpoints

### Base URL

```
http://localhost:<PORT>/
```

### Endpoints

1. **Create a New Entry**  
   **POST** `/`  
   Creates a new blood bank entry.  
   **Request Body**:
   ```json
   {
     "DonorName": "John Doe",
     "Age": 30,
     "BloodType": "A+",
     "ContactInfo": "1234567890",
     "Quantity": 300,
     "CollectionDate": "2024-11-01T00:00:00",
     "ExpirationDate": "2024-12-01T00:00:00",
     "Status": "Available"
   }
   ```
   **Response**:
   - `201 Created`: Entry created successfully.
   - `400 Bad Request`: Validation errors.

2. **Retrieve All Entries**  
   **GET** `/`  
   Fetches all blood bank entries.  
   **Response**:
   - `200 OK`: List of all entries.

3. **Retrieve an Entry by ID**  
   **GET** `/{id}`  
   Fetches a specific entry by its ID.  
   **Response**:
   - `200 OK`: Entry details.
   - `404 Not Found`: Entry not found.

4. **Update an Entry**  
   **PUT** `/{id}`  
   Updates an existing entry.  
   **Request Body**:
   ```json
   {
     "DonorName": "Jane Doe",
     "Age": 28,
     "BloodType": "O+",
     "ContactInfo": "0987654321",
     "Quantity": 250,
     "CollectionDate": "2024-11-10T00:00:00",
     "ExpirationDate": "2024-12-10T00:00:00",
     "Status": "Available"
   }
   ```
   **Response**:
   - `200 OK`: Updated entry details.
   - `400 Bad Request`: Validation errors.
   - `404 Not Found`: Entry not found.

5. **Delete an Entry**  
   **DELETE** `/{id}`  
   Deletes an entry by its ID.  
   **Response**:
   - `204 No Content`: Entry deleted successfully.
   - `404 Not Found`: Entry not found.

6. **Paginated Entries**  
   **GET** `/pagination?page={page}&size={size}`  
   Fetches paginated blood bank entries.  
   **Response**:
   - `200 OK`: Paginated list of entries.
   - `400 Bad Request`: Invalid page or size parameter.

7. **Search by Blood Type**  
   **GET** `/search/bloodtype?bloodType={bloodType}`  
   Searches entries by blood type.  
   **Response**:
   - `200 OK`: List of matching entries.
   - `400 Bad Request`: Invalid blood type.
   - `404 Not Found`: No entries found.

8. **Search by Donor Name**  
   **GET** `/search/donorname?donorName={donorName}`  
   Searches entries by donor name.  
   **Response**:
   - `200 OK`: List of matching entries.
   - `400 Bad Request`: Invalid donor name.
   - `404 Not Found`: No entries found.

9. **Search by Status**  
   **GET** `/search/status?status={status}`  
   Searches entries by status.  
   **Response**:
   - `200 OK`: List of matching entries.
   - `400 Bad Request`: Invalid status.
   - `404 Not Found`: No entries found.


10. **Search by Status**
    **GET** `/search/status`  
    Search for entries based on their status.

**Query Parameters**:
- `status` (string): The status of the blood entry. Valid statuses are: `Available`, `Requested`, `Expired`.

**Responses**:
- `200 OK`: Returns a list of matching entries.
- `400 Bad Request`: If the status is invalid.
- `404 Not Found`: If no entries match the provided status.



11. **Search by Donor Name**
    **GET** `/search/donorname`  
    Search for entries based on the donor's name.

**Query Parameters**:
- `donorName` (string): Full name of the donor (First name and Last name).

**Responses**:
- `200 OK`: Returns a list of matching entries.
- `400 Bad Request`: If the donor name is invalid.
- `404 Not Found`: If no entries match the provided donor name.



12. **Filter API**
**GET**: `/filter`  
Filter entries based on blood type, status, and donor name.

**Query Parameters**:
- `bloodType` (string): The blood type of the entry. Valid blood types are: `A+`, `A-`, `B+`, `B-`, `AB+`, `AB-`, `O+`, `O-`.
- `status` (string): The status of the blood entry. Valid statuses are: `Available`, `Requested`, `Expired`.
- `donorName` (string): Full name of the donor.

**Responses**:
- `200 OK`: Returns a list of matching entries.
- `400 Bad Request`: If any query parameter is invalid.
- `404 Not Found`: If no entries match the provided filters.



13. **Sorting API**
**GET** `/sort`  
Sort entries based on blood type or collection date.

**Query Parameters**:
- `sortBy` (string): The field to sort by. Supported values: `bloodtype`, `collectiondate`.
- `ascending` (boolean, default=true): Whether to sort in ascending order.

**Responses**:
- `200 OK`: Returns a list of sorted entries.
- `400 Bad Request`: If the `sortBy` parameter is missing or invalid.
- `404 Not Found`: If no entries are available for sorting.

---

## Validation Rules

- **Donor Name**: Between 3 and 100 characters, must consist of 1-2 words with letters only.
- **Age**: Must be between 18 and 65.
- **Blood Type**: Valid values are A+, A-, B+, B-, AB+, AB-, O+, O-.
- **Contact Info**: Must be a valid 10-digit phone number.
- **Quantity**: Must be between 1 and 500 ml.
- **Collection Date**: Must be a valid date not in the future.
- **Expiration Date**: Must be after the collection date.
- **Status**: Valid values are Available, Requested, Expired.

---

![Swagger Screenshot](./Screenshots/swagger.png)
