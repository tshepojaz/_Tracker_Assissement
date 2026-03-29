# BeeToBee — Microservices Platform

A .NET 8 microservices solution comprising a **Catalog API** and an **Order API**, orchestrated via Docker Compose, secured with Keycloak, and backed by SQL Server.

---

## Prerequisites

Before getting started, ensure the following are installed on your machine:

- [Docker Desktop](https://www.docker.com/products/docker-desktop)
- [Git](https://git-scm.com)
- A database management client (e.g., [Azure Data Studio](https://azure.microsoft.com/en-us/products/data-studio) or [SSMS](https://learn.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms))
- [Postman](https://www.postman.com/downloads/)

---

## Getting Started

### Step 1 — Clone the Repository

```bash
git clone https://github.com/tshepojaz/_Tracker_Assissement.git _NewFolder
```

### Step 2 — Navigate to the Project Directory

```bash
cd _NewFolder
```

### Step 3 — Start the Services

> **Important:** Ensure Docker Desktop is running before executing this command.

```bash
docker-compose -f compose.yaml -f compose.debug.yaml up -d --build
```

### Step 4 — Wait for Initialisation to Complete

The initial build may take several minutes as Docker pulls and builds the required container images. Please allow time for all services to fully initialise before proceeding.

### Step 5 — Verify Running Containers

Open Docker Desktop and confirm that all containers are in a **running** state. If any container has stopped, restart it manually — some services have a startup dependency on the database container and may need a moment to become healthy.

---

## Database Verification

### Step 6 — Connect to SQL Server

Open your database management client and connect using the following credentials:

| Setting | Value |
|---|---|
| Server Name | `localhost,1400` |
| User ID | `sa` |
| Password | `beetobee@123` |

### Step 7 — Confirm Schema and Seed Data

Once connected, verify the following:

- **Keycloak tables** are present (created automatically by the Keycloak container)
- **Application tables** — `Products`, `Orders`, and `OrderItems` — are present and populated with seed data

---

## Keycloak Configuration

### Step 8 — Open the Keycloak Admin Portal

In Docker Desktop, click on port **9090** next to the Keycloak container to open the admin portal in your browser.

> **Note:** The Keycloak portal may take a few minutes to load on first access.

### Step 9 — Sign In to Keycloak

Use the following credentials on the login screen:

| Field | Value |
|---|---|
| Username | `admin` |
| Password | `admin` |

### Step 10 — Navigate to Realm Management

From the top-left panel, select **Manage Realms**.

### Step 11 — Create a New Realm

Click **Create Realm** and set the name to:

```
home_order
```

### Step 12 — Create Application Users

Create two users: **tshepom** and **justicem**.

For each user, navigate to their profile, select the **Credentials** tab, and set the password to:

```
1234
```

### Step 13 — Create a Client

From the left panel, navigate to **Clients** and create a new client with the following settings:

| Field | Value |
|---|---|
| Client ID | `home_app` |
| Client Type | `OpenID Connect` |

### Step 14 — Configure Client Capabilities

Under the **Capability Config** section, enable the following:

- ✅ Client Authentication
- ✅ Authorization
- ✅ Standard Flow
- ✅ Service Account Roles
- ✅ Direct Access Grants

### Step 15 — Set PKCE Method

Set the **PKCE Code Challenge Method** to `S256`, then click **Save**.

### Step 16 — Create Client Roles

Navigate to **home_app → Roles** and create the following two roles:

- `customer`
- `demoadmin`

### Step 17 — Import Authorisation Configuration

Navigate to **home_app → Authorization**, click the **Import** button, and select the following file included in this repository:

```
keycloak/home_app.json
```

### Step 18 — Authorisation Resources Confirmed

The import will automatically create the following within the client:

- **Resources**
- **Scopes**
- **Policies**
- **Permissions**

---

## Testing with Postman

### Step 19 — Confirm Service Ports

Before testing, ensure both APIs are running in Docker on the expected ports:

| Service | Port |
|---|---|
| Catalog API | `5001` |
| Order API | `5002` |

Import the provided Postman collection file included in this repository:

```
postman/TrackerB2B.postman_collection.json
```

### Step 20 — Run the Collection

Import the collection into Postman and follow the instructions in the accompanying walkthrough video for a guided tour of testing both services end-to-end.

---

## Architecture Overview

```
┌─────────────────┐     ┌─────────────────┐
│   Catalog API   │     │    Order API    │
│   (Port 5001)   │     │   (Port 5002)   │
└────────┬────────┘     └────────┬────────┘
         │                       │
         └──────────┬────────────┘
                    │
         ┌──────────▼────────────┐
         │      SQL Server       │
         │    (Port 1400)        │
         └───────────────────────┘
                    │
         ┌──────────▼────────────┐
         │       Keycloak        │
         │     (Port 9090)       │
         └───────────────────────┘
```

---

*Thank you for using the BeeToBee platform.*
