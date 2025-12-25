# dbs.blog

Personal technical blog built with **ASP.NET Core**, designed with a strong focus on **performance and clean architecture**.

This project goes beyond a simple CRUD blog and explores real-world concerns such as CDN caching, telemetry, rate limiting, and efficient data modeling.

---

## ✨ Features

- ASP.NET Core Web Application
- PostgreSQL database
- Daily page view tracking with atomic UPSERTs
- Client-side pageview beacon using `navigator.sendBeacon`
- Server-side deduplication and rate limiting
- Azure Blob Storage for media files
- CDN-friendly cache headers (`immutable`, long `max-age`)
- Clean Architecture + CQRS (Mediator pattern)
- Docker ready
- Designed to work efficiently behind CDN / Front Door

---

## 🏗️ Architecture Overview

- **Web**: ASP.NET Core MVC
- **Application**: Commands + validations (CQRS)
- **Domain**: Entities and business rules
- **Infrastructure**:
  - PostgreSQL
  - Raw SQL for hot paths (page views)
  - Azure Blob Storage for images
- **Cross-cutting concerns**:
  - Rate limiting
  - In-memory deduplication
  - Memory-seeded hashing for visitor identification

---

## 📊 Page View Strategy

Page views are tracked with the following goals:
- Accurate counting
- High performance
- CDN compatibility
- No cookies required

### How it works:
1. Client sends a beacon on page load
2. Server applies:
   - Rate limiting
   - In-memory deduplication
3. Data is stored per page **per day**
4. PostgreSQL UPSERT guarantees atomic increments

This allows future analytics such as:
- Views per day
- Views per post
- Traffic trends

---

## 🖼️ Media Handling

- Images are stored in **Azure Blob Storage**
- Served through CDN / Front Door
- Uploaded with cache headers:
