# AsciiArtify: Containerized Full-Stack Image Processing Pipeline

**Live Deployment:** http://47.129.61.26/
*(Note: If the live server is spun down to conserve AWS Free Tier limits, reference the architecture and application flow below.)*

## Overview
AsciiArtify is a full-stack web application that converts uploaded images into ASCII art. The primary engineering focus of this project is the **infrastructure, containerization, and production deployment architecture**. 

It is deployed on an AWS EC2 (Ubuntu) instance using a multi-container Docker environment, orchestrated via Docker Compose, and fronted by an Nginx reverse proxy.

## Infrastructure & System Architecture
To overcome the resource constraints of an AWS Free Tier EC2 instance (1GB RAM) and avoid CORS configuration bloat, the deployment relies on a highly optimized, isolated architecture:

*   **Multi-Stage Docker Builds:** Reduces final production image sizes by separating the .NET SDK and Node.js build environments from the lightweight runtime containers.
*   **Nginx Reverse Proxy:** Acts as the single entry point for all traffic. It serves compiled static React assets on port 80 and securely routes `/api/*` traffic internally to the .NET backend on port 5225. This completely eliminates the need for cross-origin resource sharing (CORS) policies in the backend code.
*   **Constrained Resource Management:** Configured custom Linux swap partitions and managed EBS volume expansion to allow memory-intensive build processes (`npm install` and `dotnet publish`) to execute natively on a micro-instance without triggering Out-Of-Memory (OOM) kernel panics.



Frontend,"React, Vite, JavaScript"
Backend,"C#, .NET 9 Minimal API, ImageSharp 3.1.4"
Database,SQLite (Mounted as a persistent Docker volume)
DevOps & Hosting,"Docker, Docker Compose, Nginx, AWS EC2 (Ubuntu Linux)"

Prerequisites
Docker Engine / Docker Desktop
Build and Run
Clone the repository:
git clone [https://github.com/xiao00222/ImagetoAscii.git](https://github.com/xiao00222/ImagetoAscii.git)
cd ImagetoAscii
Start the infrastructure:
docker compose up --build
